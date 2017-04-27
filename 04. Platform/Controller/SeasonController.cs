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
using RP.Common.Extension;

namespace RP.Platform.Controller
{
	public class SeasonController : BaseController
	{
		private readonly ISeasonRepository _seasonRepository;
		private readonly IOrganizationRepository _organizationRepository;

		public SeasonController(
			ISeasonRepository seasonRepository,
			IOrganizationRepository organizationRepository)
		{
			_seasonRepository = seasonRepository;
			_organizationRepository = organizationRepository;
		}

		[Auth(Create = new[] { Dom.EntityType.Season.Id })]
		[HttpGet]
		public ActionResult Add()
		{
			ICollection<SelectListItemCount> organizationItems = _organizationRepository.GetOrganizations(
				UserContext.User.Id,
				UserContext.User.OrganizationId,
				new List<int> { Dom.OrganizationType.Club }
			);
			var viewModel = new SeasonAddViewModel
			{
				SeasonTypeItems = InitSeasonTypeSelectListItems(),
				OrganizationItems = Mapper.Map<ICollection<SelectListItem>>(organizationItems),
				Year = DateTime.UtcNow.Year
			};
			if (organizationItems.FirstOrDefault(m => m.Value == UserContext.User.OrganizationId) != null)
			{
				viewModel.OrganizationId = UserContext.User.OrganizationId;
			}

			return View(Mvc.View.Season.Add, viewModel);
		}

		[Auth(Create = new[] { Dom.EntityType.Season.Id })]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Add(SeasonAddViewModel viewModel)
		{
			if (viewModel.Year < DateTime.Now.Year)
			{
				AddModelError(viewModel, m => m.Year, Dom.Translation.Common.YearLessCurrentYear);
			}
			if (ModelState.IsValid)
			{
				Season season = Season.Empty();
				season.Year = viewModel.Year;
				season.SeasonTypeId = viewModel.SeasonTypeId;

				EntityContext.AddEntityProgress(
					season.EntityInfo,
					new EntityProgress
					{
						OrganizationId = viewModel.OrganizationId,
						EntityStateAfterId = Dom.EntityType.Season.State.Created
					}
				);
				EntityContext.AddEntityOrganization(
					season.EntityInfo,
					viewModel.OrganizationId,
					Dom.EntityType.Season.State.Created
				);
				_seasonRepository.AddOrUpdate(season);
				_seasonRepository.UnitOfWork.SaveChanges();

				return RedirectToAction(Mvc.Controller.Season.List, Mvc.Controller.Season.Name);
			}

			ICollection<SelectListItemCount> organizationItems = _organizationRepository.GetOrganizations(
				UserContext.User.Id,
				UserContext.User.OrganizationId,
				new List<int> { Dom.OrganizationType.Club }
			);

			viewModel.OrganizationItems = Mapper.Map<ICollection<SelectListItem>>(organizationItems);
			viewModel.SeasonTypeItems = InitSeasonTypeSelectListItems();
			return View(Mvc.View.Season.Add, viewModel);
		}

		[Auth(Read = new[] { Dom.EntityType.Season.Id })]
		[HttpGet]
		public ActionResult Details(int id)
		{
			SeasonDetails details = _seasonRepository.GetSeasonDetails(UserContext.User.Id, UserContext.User.OrganizationId, id);
			if (details.Detail == null)
				return NotFoundResult();

			SeasonDetailsViewModel viewModel = Mapper.Map<SeasonDetailsViewModel>(details);
			InitDetailsViewModel(viewModel, details);

			return View(Mvc.View.Season.Details, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Season.Id })]
		[HttpGet]
		public ActionResult Edit(int id)
		{
			SeasonDetails details = _seasonRepository.GetSeasonDetails(UserContext.User.Id, UserContext.User.OrganizationId, id);
			if (details.Detail == null)
				return NotFoundResult();

			SeasonDetailsViewModel viewModel = Mapper.Map<SeasonDetailsViewModel>(details);
			viewModel.Detail.SeasonTypeItems = InitSeasonTypeSelectListItems();
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

			return View(Mvc.View.Season.Edit, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Season.Id })]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(SeasonDetailsViewModel viewModel)
		{
			if (viewModel.Detail.Year < DateTime.Now.Year)
			{
				AddModelError(viewModel, m => m.Detail.Year, Dom.Translation.Common.YearLessCurrentYear);
			}
			if (ModelState.IsValid)
			{
				Season season = _seasonRepository.Get<Season>(viewModel.Detail.Id);
				season.Year = viewModel.Detail.Year;
				season.SeasonTypeId = viewModel.Detail.SeasonTypeId;
				season.Statements.ToList().ForEach(x => _seasonRepository.Delete(x));
				if (viewModel.StatementIds != null)
				{
					foreach (var pigeon in viewModel.StatementIds)
					{
						season.Statements.Add(
							new Statement
							{
								PigeonId = pigeon,
								SeasonId = season.Id
							});
					}
				}
				_seasonRepository.UnitOfWork.SaveChanges();
				return RedirectToAction(Mvc.Controller.Season.Details, Mvc.Controller.Season.Name, new { id = viewModel.Detail.Id });
			}

			SeasonDetails details = _seasonRepository.GetSeasonDetails(UserContext.User.Id, UserContext.User.OrganizationId, viewModel.Detail.Id);
			viewModel.Detail.SeasonTypeItems = InitSeasonTypeSelectListItems();
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

			return View(Mvc.View.Season.Edit, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Season.Id })]
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

			ViewData[Mvc.ViewData.Controller] = Mvc.Controller.Season.Name;
			return PartialView(Mvc.View.Entity.EntityStateChange, entityStateChangeViewModel);
		}

		[Auth(Read = new[] { Dom.EntityType.Season.Id })]
		[HttpGet]
		public ActionResult List(SeasonFilterViewModel filterViewModel)
		{
			InitSeasonFilterViewModel(ref filterViewModel, true);
			var filter = Mapper.Map<SeasonFilter>(filterViewModel);
			SeasonItems list = _seasonRepository.GetSeasonItems(UserContext.User.Id, UserContext.User.OrganizationId, filter);
			SeasonItemsViewModel viewModel = Mapper.Map<SeasonItemsViewModel>(list);
			viewModel.Filter = filterViewModel;
			InitListViewModel(viewModel, list);

			return View(Mvc.View.Season.List, viewModel);
		}

		private void InitDetailsViewModel(SeasonDetailsViewModel viewModel, SeasonDetails details)
		{
			InitBaseDetailViewModel(details.Detail, viewModel.Detail);
			InitBaseDetailEntityStateChange(viewModel.Detail);
			viewModel.Detail.SeasonTypeName = GetSeasonTypeName(details.Detail.SeasonTypeId);
			ViewData[Mvc.ViewData.Controller] = Mvc.Controller.Season.Name;
		}

		private void InitListViewModel(SeasonItemsViewModel viewModel, SeasonItems list)
		{
			foreach (var item in viewModel.Items)
			{
				InitBaseItemViewModel(item);
				item.SeasonTypeName = GetSeasonTypeName(item.SeasonTypeId);
			}
		}

		private void InitSeasonFilterViewModel(ref SeasonFilterViewModel filterViewModel, bool isList)
		{
			if (isList)
				InitBaseFilterViewModel(filterViewModel);
		}
	}
}