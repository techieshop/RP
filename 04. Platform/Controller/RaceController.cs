using AutoMapper;
using RP.Common.Extension;
using RP.Common.Manager;
using RP.DAL.Repository;
using RP.Model;
using RP.Platform.Context;
using RP.Platform.Manager;
using RP.Platform.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RP.Platform.Controller
{
	public class RaceController : BaseController
	{
		private readonly IOrganizationRepository _organizationRepository;
		private readonly IPointRepository _pointRepository;
		private readonly IRaceRepository _raceRepository;
		private readonly ISeasonRepository _seasonRepository;

		public RaceController(
			IPointRepository pointRepository,
			IRaceRepository raceRepository,
			ISeasonRepository seasonRepository,
			IOrganizationRepository organizationRepository)
		{
			_pointRepository = pointRepository;
			_raceRepository = raceRepository;
			_seasonRepository = seasonRepository;
			_organizationRepository = organizationRepository;
		}

		[Auth(Create = new[] { Dom.EntityType.Race.Id })]
		[HttpGet]
		public ActionResult Add()
		{
			ICollection<SelectListItemCount> organizationItems = _organizationRepository.GetOrganizations(
				UserContext.User.Id,
				UserContext.User.OrganizationId,
				new List<int> { Dom.OrganizationType.Club }
			);
			var viewModel = new RaceAddViewModel
			{
				OrganizationItems = Mapper.Map<ICollection<SelectListItem>>(organizationItems),
				RaceTypeItems = InitRaceTypeSelectListItems(),
				RaceTypeId = Dom.RaceType.Single
			};
			if (organizationItems.FirstOrDefault(m => m.Value == UserContext.User.OrganizationId) != null)
			{
				viewModel.OrganizationId = UserContext.User.OrganizationId;
				var points = _pointRepository.GetPoints(UserContext.User.Id, UserContext.User.OrganizationId, viewModel.OrganizationId);
				viewModel.PointItems = Mapper.Map<ICollection<SelectListItem>>(points);
				var seasons = _seasonRepository.GetSeasons(UserContext.User.Id, UserContext.User.OrganizationId, viewModel.OrganizationId);
				viewModel.SeasonItems = Mapper.Map<ICollection<SelectListItem>>(seasons);
			}

			return View(Mvc.View.Race.Add, viewModel);
		}

		[Auth(Create = new[] { Dom.EntityType.Race.Id })]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Add(RaceAddViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				Race race = Race.Empty();
				race.Name = viewModel.Name;
				race.RaceTypeId = viewModel.RaceTypeId;
				race.PointId = viewModel.PointId;
				race.SeasonId = viewModel.SeasonId;

				EntityContext.AddEntityProgress(
					race.EntityInfo,
					new EntityProgress
					{
						OrganizationId = viewModel.OrganizationId,
						EntityStateAfterId = Dom.EntityType.Race.State.Created
					}
				);
				EntityContext.AddEntityOrganization(
					race.EntityInfo,
					viewModel.OrganizationId,
					Dom.EntityType.Race.State.Created
				);
				_raceRepository.AddOrUpdate(race);
				_raceRepository.UnitOfWork.SaveChanges();

				return RedirectToAction(Mvc.Controller.Race.List, Mvc.Controller.Race.Name);
			}

			ICollection<SelectListItemCount> organizationItems = _organizationRepository.GetOrganizations(
				UserContext.User.Id,
				UserContext.User.OrganizationId,
				new List<int> { Dom.OrganizationType.Club }
			);

			if (viewModel.OrganizationId != 0)
			{
				var points = _pointRepository.GetPoints(UserContext.User.Id, UserContext.User.OrganizationId, viewModel.OrganizationId);
				viewModel.PointItems = Mapper.Map<ICollection<SelectListItem>>(points);
				var seasons = _seasonRepository.GetSeasons(UserContext.User.Id, UserContext.User.OrganizationId, viewModel.OrganizationId);
				viewModel.SeasonItems = Mapper.Map<ICollection<SelectListItem>>(seasons);
			}

			viewModel.OrganizationItems = Mapper.Map<ICollection<SelectListItem>>(organizationItems);
			viewModel.RaceTypeItems = InitRaceTypeSelectListItems();

			return View(Mvc.View.Race.Add, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Race.Id })]
		[HttpGet]
		public ActionResult CalculateDistances(int id)
		{
			RaceDetails details = _raceRepository.GetRaceDetails(UserContext.User.Id, UserContext.User.OrganizationId, id);
			if (details.Detail == null || details.Detail.EntityStateId != Dom.EntityType.Race.State.Open)
				return NotFoundResult();

			Race race = _raceRepository.Get<Race>(id);
			race.RaceDistances.ToList().ForEach(x => _raceRepository.Delete(x));
			var members = race.RacePigeons.Select(m => m.Pigeon.Member).Distinct().ToList();
			foreach (var member in members)
			{
				race.RaceDistances.Add(new RaceDistance
				{
					Distance = member.Address?.Latitude == null
						? 0
						: CoordinatesManager.CalculateDistance(member.Address.Latitude, member.Address.Longitude,
							race.Point.Address.Latitude, race.Point.Address.Longitude),
					MemberId = member.Id
				});
			}
			_raceRepository.UnitOfWork.SaveChanges();

			return RedirectToAction(Mvc.Controller.Race.Details, Mvc.Controller.Race.Name, new { id });
		}

		private double FlyTime(DateTime startRaceDateTime, DateTime pigeonReturnDateTime)
		{
			//todo calculate according sun time
			//Нічний час.Якщо тривалість гонки продовжується на другу і подальшу доби,
			//від часу польоту голуба віднімається нічний час за кожну ніч, 
			//яку голуб провів під час гонки до моменту констатування. 
			//Нічний час встановлюється тривалістю 6 год.з 2300 год.до 0500 год.
			//Якщо голуб констатований у нічний час до 2400 год., йому встановлюється час 2300 год.завершеної доби. 
			//Якщо голуб констатований у нічний час після 2400 год., йому встановлюється час 500 год.поточної доби.

			DateTime pigeon = pigeonReturnDateTime, race = startRaceDateTime;

			//set time 23:00:00
			if (pigeonReturnDateTime.Hour == 23)
			{
				pigeon = pigeon.AddMinutes(-pigeon.Minute);
				pigeon = pigeon.AddSeconds(-pigeon.Second);
				pigeon = pigeon.AddMilliseconds(-pigeon.Millisecond);
			}

			//set time 05:00:00
			if (pigeonReturnDateTime.Hour < 5)
			{
				pigeon = pigeon.AddMinutes(-pigeon.Minute);
				pigeon = pigeon.AddSeconds(-pigeon.Second);
				pigeon = pigeon.AddMilliseconds(-pigeon.Millisecond);
				pigeon = pigeon.AddHours(5 - pigeon.Hour);
			}

			var totalHours = (pigeon - race).TotalHours;
			int days = (int)totalHours / 24;

			return (pigeon.AddHours(-days * 6) - race).TotalSeconds;
		}

		[Auth(Write = new[] { Dom.EntityType.Race.Id, Dom.EntityType.Result.Id })]
		[HttpGet]
		public ActionResult CalculateResult(int id)
		{
			RaceDetails details = _raceRepository.GetRaceDetails(UserContext.User.Id, UserContext.User.OrganizationId, id);
			if (details.Detail == null)
				return NotFoundResult();

			if (details.Detail.EntityStateId != Dom.EntityType.Race.State.Open ||
				details.Detail.StartRaceTime == null ||
				details.Detail.DarknessBeginTime == null ||
				details.Detail.DarknessEndTime == null ||
				details.StatementPigeonIds == null ||
				(details.StatementPigeonIds != null && details.Detail.ReturnPigeonCount * 1.0 / details.StatementPigeonIds.Count <= 0.2))
				return NotFoundResult();

			Race race = _raceRepository.Get<Race>(id);
			race.RaceResults.ToList().ForEach(
				x =>
				{
					x.RaceResultCategories.ToList().ForEach(c => _raceRepository.Delete(c));
					_raceRepository.Delete(x);
				});

			var members = race.RacePigeons.Select(m => m.Pigeon.Member).Distinct().ToList();
			int memberCount = members.Count;
			int pigeonCount = race.RacePigeons.Count;
			ICollection<RaceResult> raceResults = new List<RaceResult>();
			foreach (var pigeonReturnTime in race.PigeonReturnTimes)
			{
				double? distance = race.RaceDistances.First(m => m.MemberId == pigeonReturnTime.Pigeon.MemberId)?.Distance;
				if (distance > 0)
				{
					var totalTime = FlyTime(race.StartRaceTime.Value, pigeonReturnTime.ReturnTime);
					var speed = distance.Value / totalTime;
					raceResults.Add(new RaceResult
					{
						Pigeon = pigeonReturnTime.Pigeon,
						RaceId = id,
						Distance = distance.Value,
						FlyTime = totalTime,
						Speed = speed * 60
					});
				}
			}

			List<RaceResult> raceResultSortered = raceResults
				.OrderByDescending(m => m.Speed) //
				.ThenByDescending(m => m.Distance) // якщо швидкість однакова, вище стоїть голуб із більшою дистанцією
				.ThenBy(m => m.Pigeon.Number) // якщо швидкість і дистанція однакові, вище стоїть голуб із меншим порядковим номером кільця
				.ToList();
			//todo
			//Якщо середня швидкість першого голуба становить менше 600 м / хв.,
			//місця у гонці повинні визначатися за фактичним часом констатування голубів 
			//у гонці без врахування швидкості та відстані від точки випуску до учасника. 
			//Якщо зафіксований час співпадає для двох і більше голубів, розстановка місць здійснюється від більшої подоланої дистанції до меншої.

			int numberOfPrizes = pigeonCount / 5;
			// заокруглення в більшу сторону
			if (numberOfPrizes * 5 < pigeonCount)
				numberOfPrizes++;
			for (int i = 0; i < raceResultSortered.Count; i++)
			{
				raceResultSortered[i].Position = i + 1;
				if (i < numberOfPrizes)
				{
					raceResultSortered[i].Ac = true;
					var raceResultCategories = RaceManager.GetCategory(raceResultSortered[i].Pigeon.Year, raceResultSortered[i].Distance, memberCount, pigeonCount);
					raceResultSortered[i].RaceResultCategories = new List<RaceResultCategory>();
					foreach (var raceResultCategory in raceResultCategories)
					{
						raceResultSortered[i].RaceResultCategories.Add(
							new RaceResultCategory
							{
								RaceResult = raceResultSortered[i],
								CategoryId = raceResultCategory.Key,
								IsOlymp = raceResultCategory.Value,
								Coefficient = RaceManager.CalculateCoefficient(pigeonCount, i + 1),
								Mark = RaceManager.CalculateMark(numberOfPrizes, i + 1)
							});
					}
				}
				race.RaceResults.Add(raceResultSortered[i]);
			}

			race.RaceStatistics.ToList().ForEach(x => _raceRepository.Delete(x));

			foreach (var member in members)
			{
				int memberId = member.Id;
				int statedPigeonCount = race.RacePigeons.Count(m => m.Pigeon.MemberId == memberId);
				int prizePigeonCount = race.RaceResults.Count(m => m.Pigeon.MemberId == memberId && m.Ac);
				double mark = race.RaceResults.Where(m => m.Pigeon.MemberId == memberId && m.Ac).OrderBy(m => m.Position).Take(5).Sum(m => m.RaceResultCategories.First().Mark);
				race.RaceStatistics.Add(new RaceStatistic
				{
					MemberId = member.Id,
					StatedPigeonCount = statedPigeonCount,
					PrizePigeonCount = prizePigeonCount,
					Mark = mark,
					Success = prizePigeonCount * 100.0 / statedPigeonCount
				});
			}

			var firstPigeon = raceResultSortered.FirstOrDefault(m => m.Position == 1);
			var lastPigeon = raceResultSortered.FirstOrDefault(m => m.Position == numberOfPrizes);
			race.PigeonCount = race.RacePigeons.Count;
			race.MemberCount = memberCount;
			race.AverageDistance = race.RaceDistances.Where(m => m.Distance > 0).Average(m => m.Distance);
			race.TimeOfFirst = race.StartRaceTime.Value.AddSeconds(firstPigeon.FlyTime);
			race.TimeOfLast = race.StartRaceTime.Value.AddSeconds(lastPigeon.FlyTime);
			race.SpeedOfFirst = firstPigeon.Speed;
			race.SpeedOfLast = lastPigeon.Speed;
			race.TwentyPercent = race.PigeonCount / 5;
			race.CockCount = race.RacePigeons.Count(m => m.Pigeon.SexId == Dom.Sex.Cock);
			race.HenCount = race.RacePigeons.Count(m => m.Pigeon.SexId == Dom.Sex.Hen);
			int currentYear = DateTime.UtcNow.Year;
			race.YoungCount = race.RacePigeons.Count(m => currentYear - m.Pigeon.Year == 0);
			race.YearlyCount = race.RacePigeons.Count(m => currentYear - m.Pigeon.Year == 1);
			race.AdultsCount = race.RacePigeons.Count(m => currentYear - m.Pigeon.Year > 1);
			race.StartСompetitionTime = race.PigeonReturnTimes.Min(m => m.ReturnTime);

			//todo
			race.EndСompetitionTime = race.StartRaceTime.Value.AddSeconds(lastPigeon.FlyTime);

			race.DurationOfCompetition = race.EndСompetitionTime.Value.Subtract(race.StartСompetitionTime.Value).TotalSeconds;
			race.PigeonTwentyPercentAFact = race.RaceResults.Count(m => m.FlyTime < race.DurationOfCompetition.Value);
			race.MemberTwentyPercentAFact = race.RaceResults.Where(m => m.FlyTime < race.DurationOfCompetition.Value).Select(m => m.Pigeon.MemberId).Distinct().Count();
			race.InFactAbidedPercent = race.PigeonTwentyPercentAFact * 100.0 / race.PigeonCount;
			_raceRepository.UnitOfWork.SaveChanges();

			return RedirectToAction(Mvc.Controller.Race.Details, Mvc.Controller.Race.Name, new { id });
		}

		[Auth(Read = new[] { Dom.EntityType.Race.Id })]
		[HttpGet]
		public ActionResult Details(int id)
		{
			RaceDetails details = _raceRepository.GetRaceDetails(UserContext.User.Id, UserContext.User.OrganizationId, id);
			if (details.Detail == null)
				return NotFoundResult();

			RaceDetailsViewModel viewModel = Mapper.Map<RaceDetailsViewModel>(details);
			InitDetailsViewModel(viewModel, details);

			return View(Mvc.View.Race.Details, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Race.Id })]
		[HttpGet]
		public ActionResult Edit(int id)
		{
			RaceDetails details = _raceRepository.GetRaceDetails(UserContext.User.Id, UserContext.User.OrganizationId, id);
			if (details.Detail == null || details.Detail?.EntityStateId != Dom.EntityType.Race.State.Open)
				return NotFoundResult();

			RaceDetailsViewModel viewModel = Mapper.Map<RaceDetailsViewModel>(details);
			var points = _pointRepository.GetPoints(UserContext.User.Id, UserContext.User.OrganizationId, viewModel.Detail.OrganizationId);
			viewModel.Detail.PointItems = Mapper.Map<ICollection<SelectListItem>>(points);
			foreach (var pigeon in details.Pigeons)
			{
				pigeon.Ring = Format.FormattedRing(pigeon.Year, pigeon.Code, pigeon.Number);
				pigeon.InStatement = details.StatementPigeonIds.Contains(pigeon.Id);
			}
			viewModel.StatementIds = details.StatementPigeonIds.ToList();
			viewModel.StatementItems = new Dictionary<string, ICollection<SelectListItem>>();
			foreach (var member in details.Members)
			{
				viewModel.StatementItems.Add(
					Format.FormattedInitials(member.LastName, member.FirstName, member.MiddleName) + $" (#{member.Id})",
					details.Pigeons
						.Where(m => m.MemberId == member.Id)
						.Select(g => new SelectListItem
						{
							Value = g.Id.ToString(),
							Text = g.Ring,
							Selected = g.InStatement
						}).ToList()
				);
			}

			return View(Mvc.View.Race.Edit, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Race.Id })]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(RaceDetailsViewModel viewModel)
		{
			if (viewModel.Detail.DarknessEndTime <= viewModel.Detail.DarknessBeginTime)
			{
				AddModelError(viewModel, m => m.Detail.DarknessEndTime, StyleContext.GetTranslation(Dom.Translation.Race.DarknessEndTimeLessDarknessBeginTime));
			}
			if (viewModel.Detail.StartRaceTime < viewModel.Detail.DarknessBeginTime)
			{
				AddModelError(viewModel, m => m.Detail.StartRaceTime, StyleContext.GetTranslation(Dom.Translation.Race.StartRaceTimeLessDarknessBeginTime));
			}

			if (ModelState.IsValid)
			{
				Race race = _raceRepository.Get<Race>(viewModel.Detail.Id);
				race.Name = viewModel.Detail.Name;
				race.PointId = viewModel.Detail.PointId;
				race.StartRaceTime = viewModel.Detail.StartRaceTime;
				race.DarknessBeginTime = viewModel.Detail.DarknessBeginTime;
				race.DarknessEndTime = viewModel.Detail.DarknessEndTime;
				race.Weather = viewModel.Detail.Weather;
				race.RacePigeons.ToList().ForEach(x => _raceRepository.Delete(x));
				if (viewModel.StatementIds != null)
				{
					foreach (var pigeon in viewModel.StatementIds)
					{
						race.RacePigeons.Add(
							new RacePigeon
							{
								PigeonId = pigeon,
								RaceId = race.Id
							});
					}
				}
				_raceRepository.UnitOfWork.SaveChanges();
				return RedirectToAction(Mvc.Controller.Race.Details, Mvc.Controller.Race.Name, new { id = viewModel.Detail.Id });
			}

			RaceDetails details = _raceRepository.GetRaceDetails(UserContext.User.Id, UserContext.User.OrganizationId, viewModel.Detail.Id);
			var points = _pointRepository.GetPoints(UserContext.User.Id, UserContext.User.OrganizationId, viewModel.Detail.OrganizationId);
			viewModel.Detail.PointItems = Mapper.Map<ICollection<SelectListItem>>(points);
			foreach (var pigeon in details.Pigeons)
			{
				pigeon.Ring = Format.FormattedRing(pigeon.Year, pigeon.Code, pigeon.Number);
				pigeon.InStatement = details.StatementPigeonIds.Contains(pigeon.Id);
			}
			viewModel.StatementIds = details.StatementPigeonIds.ToList();
			viewModel.StatementItems = new Dictionary<string, ICollection<SelectListItem>>();
			foreach (var member in details.Members)
			{
				viewModel.StatementItems.Add(
					Format.FormattedInitials(member.LastName, member.FirstName, member.MiddleName) + $" (#{member.Id})",
					details.Pigeons
						.Where(m => m.MemberId == member.Id)
						.Select(g => new SelectListItem
						{
							Value = g.Id.ToString(),
							Text = g.Ring,
							Selected = g.InStatement
						}).ToList()
				);
			}

			return View(Mvc.View.Race.Edit, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Race.Id })]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EntityStateChange(EntityTransitionViewModel entityTransitionViewModel)
		{
			EntityStateChangeViewModel entityStateChangeViewModel;
			if (!EntityContext.TryChangeEntityState(
					entityTransitionViewModel.EntityInfoId,
					entityTransitionViewModel.OrganizationId,
					entityTransitionViewModel.EntityTransitionId,
					out entityStateChangeViewModel))
				return ForbiddenResult();

			ViewData[Mvc.ViewData.Controller] = Mvc.Controller.Race.Name;
			return PartialView(Mvc.View.Entity.EntityStateChange, entityStateChangeViewModel);
		}

		[Auth(Read = new[] { Dom.EntityType.Race.Id })]
		[HttpGet]
		public ActionResult List(RaceFilterViewModel filterViewModel)
		{
			InitRaceFilterViewModel(ref filterViewModel, true);
			var filter = Mapper.Map<RaceFilter>(filterViewModel);
			RaceItems list = _raceRepository.GetRaceItems(UserContext.User.Id, UserContext.User.OrganizationId, filter);
			RaceItemsViewModel viewModel = Mapper.Map<RaceItemsViewModel>(list);
			viewModel.Filter = filterViewModel;
			InitListViewModel(viewModel, list);

			return View(Mvc.View.Race.List, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Race.Id })]
		[HttpGet]
		public ActionResult Points(int id)
		{
			var points = _pointRepository.GetPoints(UserContext.User.Id, UserContext.User.OrganizationId, id);
			return Json(Mapper.Map<ICollection<SelectListItem>>(points), JsonRequestBehavior.AllowGet);
		}

		[Auth(Write = new[] { Dom.EntityType.Race.Id })]
		[HttpGet]
		public ActionResult ReturnTime(int id)
		{
			RaceResultReturnTimes resultReturnTimes = _raceRepository.GetRaceResultReturnTimes(UserContext.User.Id, UserContext.User.OrganizationId, id);
			if (resultReturnTimes.Detail == null || resultReturnTimes.Detail?.EntityStateId != Dom.EntityType.Race.State.Open)
				return NotFoundResult();

			RaceResultReturnTimesViewModel viewModel = Mapper.Map<RaceResultReturnTimesViewModel>(resultReturnTimes);
			viewModel.Detail.RaceTypeName = GetRaceTypeName(viewModel.Detail.RaceTypeId);
			viewModel.Detail.SeasonTypeName = GetSeasonTypeName(viewModel.Detail.SeasonTypeId);
			viewModel.Detail.SeasonName = $"{viewModel.Detail.SeasonTypeName}-{viewModel.Detail.Year}";
			foreach (var pigeon in resultReturnTimes.Pigeons)
			{
				pigeon.Ring = Format.FormattedRing(pigeon.Year, pigeon.Code, pigeon.Number);
				pigeon.InStatement = resultReturnTimes.PigeonReturnTimes.Any(m => m.Id == pigeon.Id);
			}

			viewModel.PigeonReturnItems = new Dictionary<string, ICollection<DateTimeSelectListItem>>();
			foreach (var member in resultReturnTimes.Members)
			{
				viewModel.PigeonReturnItems.Add(
					Format.FormattedInitials(member.LastName, member.FirstName, member.MiddleName) + $" (#{member.Id})",
					resultReturnTimes.Pigeons
						.Where(m => m.MemberId == member.Id)
						.Select(g => new DateTimeSelectListItem
						{
							Value = g.Id.ToString(),
							Text = g.Ring,
							Selected = g.InStatement,
							DateTime = resultReturnTimes.PigeonReturnTimes.Any() ? resultReturnTimes.PigeonReturnTimes.FirstOrDefault(m => m.Id == g.Id)?.ReturnTime : null
						}).ToList()
				);
			}

			return View(Mvc.View.Race.ReturnTime, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Race.Id })]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ReturnTime(RaceResultReturnTimesViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				ICollection<PigeonReturnTimeRef> pigeonReturnTimes =
					string.IsNullOrWhiteSpace(viewModel.PigeonReturnTimesJson) ?
					new List<PigeonReturnTimeRef>() :
					JsonHelper.JsonDeserialize<ICollection<PigeonReturnTimeRef>>(viewModel.PigeonReturnTimesJson);
				Race race = _raceRepository.Get<Race>(viewModel.Detail.Id);
				race.Name = viewModel.Detail.Name;
				race.PointId = viewModel.Detail.PointId;
				race.StartRaceTime = viewModel.Detail.StartRaceTime;
				race.DarknessBeginTime = viewModel.Detail.DarknessBeginTime;
				race.DarknessEndTime = viewModel.Detail.DarknessEndTime;
				race.Weather = viewModel.Detail.Weather;
				race.PigeonReturnTimes.ToList().ForEach(x => _raceRepository.Delete(x));
				if (pigeonReturnTimes != null)
				{
					foreach (var time in pigeonReturnTimes)
					{
						race.PigeonReturnTimes.Add(
							new PigeonReturnTime
							{
								PigeonId = time.Id,
								ReturnTime = time.ReturnTime
							});
					}
				}
				_raceRepository.UnitOfWork.SaveChanges();
				return RedirectToAction(Mvc.Controller.Race.Details, Mvc.Controller.Race.Name, new { id = viewModel.Detail.Id });
			}

			RaceResultReturnTimes resultReturnTimes = _raceRepository.GetRaceResultReturnTimes(UserContext.User.Id, UserContext.User.OrganizationId, viewModel.Detail.Id);
			viewModel.Detail.RaceTypeName = GetRaceTypeName(viewModel.Detail.RaceTypeId);
			viewModel.Detail.SeasonTypeName = GetSeasonTypeName(viewModel.Detail.SeasonTypeId);
			viewModel.Detail.SeasonName = $"{viewModel.Detail.SeasonTypeName}-{viewModel.Detail.Year}";
			foreach (var pigeon in resultReturnTimes.Pigeons)
			{
				pigeon.Ring = Format.FormattedRing(pigeon.Year, pigeon.Code, pigeon.Number);
				pigeon.InStatement = resultReturnTimes.PigeonReturnTimes.Any(m => m.Id == pigeon.Id);
			}

			viewModel.PigeonReturnItems = new Dictionary<string, ICollection<DateTimeSelectListItem>>();
			foreach (var member in resultReturnTimes.Members)
			{
				viewModel.PigeonReturnItems.Add(
					Format.FormattedInitials(member.LastName, member.FirstName, member.MiddleName) + $" (#{member.Id})",
					resultReturnTimes.Pigeons
						.Where(m => m.MemberId == member.Id)
						.Select(g => new DateTimeSelectListItem
						{
							Value = g.Id.ToString(),
							Text = g.Ring,
							Selected = g.InStatement,
							DateTime = resultReturnTimes.PigeonReturnTimes.Any() ? resultReturnTimes.PigeonReturnTimes.FirstOrDefault(m => m.Id == g.Id)?.ReturnTime : null
						}).ToList()
				);
			}

			return View(Mvc.View.Race.ReturnTime, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Race.Id })]
		[HttpGet]
		public ActionResult Seasons(int id)
		{
			var seasons = _seasonRepository.GetSeasons(UserContext.User.Id, UserContext.User.OrganizationId, id);
			return Json(Mapper.Map<ICollection<SelectListItem>>(seasons), JsonRequestBehavior.AllowGet);
		}

		private void InitDetailsViewModel(RaceDetailsViewModel viewModel, RaceDetails details)
		{
			InitBaseDetailViewModel(details.Detail, viewModel.Detail);
			InitBaseDetailEntityStateChange(viewModel.Detail);

			if (details.Detail.EntityStateId == Dom.EntityType.Race.State.Open &&
				details.Detail.StartRaceTime != null &&
				details.Detail.DarknessBeginTime != null &&
				details.Detail.DarknessEndTime != null &&
				details.Detail.HasCalculatedDistances &&
				details.StatementPigeonIds != null && details.StatementPigeonIds.Any() &&
				details.Detail.ReturnPigeonCount * 1.0 / details.StatementPigeonIds.Count >= 0.2
			)
			{
				viewModel.CanCalculateResults = true;
			}

			viewModel.Detail.RaceTypeName = GetRaceTypeName(viewModel.Detail.RaceTypeId);
			viewModel.Detail.SeasonTypeName = GetSeasonTypeName(viewModel.Detail.SeasonTypeId);
			viewModel.Detail.SeasonName = $"{viewModel.Detail.SeasonTypeName}-{viewModel.Detail.Year}";
			ViewData[Mvc.ViewData.Controller] = Mvc.Controller.Race.Name;
		}

		private void InitListViewModel(RaceItemsViewModel viewModel, RaceItems list)
		{
			foreach (var item in viewModel.Items)
			{
				InitBaseItemViewModel(item);
				item.RaceTypeName = GetRaceTypeName(item.RaceTypeId);
				item.SeasonTypeName = GetSeasonTypeName(item.SeasonTypeId);
				item.SeasonName = $"{item.SeasonTypeName}-{item.Year}";
			}
		}

		private void InitRaceFilterViewModel(ref RaceFilterViewModel filterViewModel, bool isList)
		{
			if (isList)
				InitBaseFilterViewModel(filterViewModel);
		}
	}
}