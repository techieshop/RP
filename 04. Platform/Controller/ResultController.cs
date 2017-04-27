using AutoMapper;
using RP.DAL.Repository;
using RP.Model;
using RP.Platform.Context;
using RP.Platform.Manager;
using RP.Platform.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace RP.Platform.Controller
{
	public class ResultController : BaseController
	{
		private readonly IRaceRepository _raceRepository;

		public ResultController(
			IRaceRepository raceRepository)
		{
			_raceRepository = raceRepository;
		}

		[Auth(Read = new[] { Dom.EntityType.Result.Id })]
		[HttpGet]
		public ActionResult Details(int id)
		{
			ResultDetail details = _raceRepository.GetResultDetails(UserContext.User.Id, UserContext.User.OrganizationId, id);
			if (details == null)
				return NotFoundResult();

			ResultDetailViewModel viewModel = Mapper.Map<ResultDetailViewModel>(details);
			viewModel.SeasonName = $"{GetSeasonTypeName(viewModel.SeasonTypeId)}-{viewModel.Year}";
			viewModel.AverageDistance = viewModel.AverageDistance / 1000;
			return View(Mvc.View.Result.Details, viewModel);
		}

		[Auth(Read = new[] { Dom.EntityType.Result.Id })]
		[HttpGet]
		public ActionResult DownloadCommandPdfFile(int id)
		{
			ResultCommandItems model = _raceRepository.GetResultCommand(UserContext.User.Id, UserContext.User.OrganizationId, id);
			if (model.Detail == null)
				return NotFoundResult();

			model.Detail.AverageDistance = model.Detail.AverageDistance / 1000;
			model.Detail.DurationOfCompetitionTimeSpan = TimeSpan.FromSeconds(model.Detail.DurationOfCompetition);
			model.Detail.SeasonName = $"{GetSeasonTypeName(model.Detail.SeasonTypeId)}-{model.Detail.Year}";

			int position = 1;
			model.Items = model.Items.OrderByDescending(m => m.MarkTotal).ToList();
			model.Items.ForEach(
				m =>
				{
					m.Position = position++;
					m.Member = Common.Extension.Format.FormattedInitials(m.LastName, m.FirstName, m.MiddleName);
				});

			return PdfView(
				$"{model.Detail.Name}_{StyleContext.GetTranslation(Dom.Translation.Result.Command)}",
				Mvc.View.Result.Command,
				Mvc.View.Shared.EmptyLayout,
				model
			);
		}

		[Auth(Read = new[] { Dom.EntityType.Result.Id })]
		[HttpGet]
		public ActionResult DownloadMasterPdfFile(int id)
		{
			ResultMasterItems model = _raceRepository.GetResultMaster(UserContext.User.Id, UserContext.User.OrganizationId, id);
			if (model.Detail == null)
				return NotFoundResult();

			model.Detail.AverageDistance = model.Detail.AverageDistance / 1000;
			model.Detail.DurationOfCompetitionTimeSpan = TimeSpan.FromSeconds(model.Detail.DurationOfCompetition);
			model.Detail.SeasonName = $"{GetSeasonTypeName(model.Detail.SeasonTypeId)}-{model.Detail.Year}";

			int position = 1;
			model.Items = model.Items.OrderBy(m => m.CoefficientTotal).ToList();
			model.Items.ForEach(
				m =>
				{
					m.Position = position++;
					m.Success = m.PrizeCount * 100.0 / m.StatementCount;
					m.Member = Common.Extension.Format.FormattedInitials(m.LastName, m.FirstName, m.MiddleName);
				});

			return PdfView(
				$"{model.Detail.Name}_{StyleContext.GetTranslation(Dom.Translation.Result.Master)}",
				Mvc.View.Result.Master,
				Mvc.View.Shared.EmptyLayout,
				model
			);
		}

		[Auth(Read = new[] { Dom.EntityType.Result.Id })]
		[HttpGet]
		public ActionResult DownloadTimePdfFile(int id)
		{
			ResultTimeItems model = _raceRepository.GetResultTime(UserContext.User.Id, UserContext.User.OrganizationId, id);
			if (model.Detail == null)
				return NotFoundResult();

			model.Detail.SeasonName = $"{GetSeasonTypeName(model.Detail.SeasonTypeId)}-{model.Detail.Year}";

			model.Items.ForEach(
				m =>
				{
					m.Distance = m.Distance / 1000;
					m.FlyTimeSpan = TimeSpan.FromSeconds(m.FlyTime);
					m.Member = Common.Extension.Format.FormattedInitials(m.LastName, m.FirstName, m.MiddleName);
					m.Sex = GetSexNameNumber(m.SexId);
				});

			return PdfView(
				$"{model.Detail.Name}_{StyleContext.GetTranslation(Dom.Translation.Result.Time)}",
				Mvc.View.Result.Time,
				Mvc.View.Shared.EmptyLayout,
				model
			);
		}

		[Auth(Read = new[] { Dom.EntityType.Result.Id })]
		[HttpGet]
		public ActionResult DownloadTitlePdfFile(int id)
		{
			ResultDetail details = _raceRepository.GetResultDetails(UserContext.User.Id, UserContext.User.OrganizationId, id);
			if (details == null)
				return NotFoundResult();

			ResultDetailViewModel model = Mapper.Map<ResultDetailViewModel>(details);
			model.AverageDistance = model.AverageDistance / 1000;
			model.SeasonName = $"{GetSeasonTypeName(model.SeasonTypeId)}-{model.Year}";

			return PdfView(
				$"{model.Name}_{StyleContext.GetTranslation(Dom.Translation.Result.Title)}",
				Mvc.View.Result.Title,
				Mvc.View.Shared.EmptyLayout,
				model
			);
		}

		[Auth(Read = new[] { Dom.EntityType.Result.Id })]
		[HttpGet]
		public ActionResult List()
		{
			ICollection<ResultItem> list = _raceRepository.GetResultItems(UserContext.User.Id, UserContext.User.OrganizationId);

			var years = list.GroupBy(m => m.Year);
			var level1Items = new List<ResultItemViewModel>();
			foreach (var year in years)
			{
				var organizations = year.GroupBy(m => m.OrganizationName);
				var level2Items = new List<ResultItemViewModel>();
				foreach (var organization in organizations)
				{
					var seasons = organization.GroupBy(m => m.SeasonTypeId);
					var level3Items = new List<ResultItemViewModel>();
					foreach (var season in seasons)
					{
						var races = season.ToList();
						var level4Items = new List<ResultItemViewModel>();
						foreach (var race in races)
						{
							level4Items.Add(new ResultItemViewModel
							{
								Level = 4,
								Name = race.Name,
								Items = new List<ResultItemViewModel>(),
								RaceId = race.Id
							});
						}
						level3Items.Add(new ResultItemViewModel
						{
							Level = 3,
							Name = GetSeasonTypeName(season.Key),
							Items = level4Items
						});
					}
					level2Items.Add(new ResultItemViewModel
					{
						Level = 2,
						Name = organization.Key,
						Items = level3Items
					});
				}
				level1Items.Add(new ResultItemViewModel
				{
					Level = 1,
					Name = year.Key.ToString(),
					Items = level2Items
				});
			}

			return View(Mvc.View.Result.List, level1Items);
		}
	}
}