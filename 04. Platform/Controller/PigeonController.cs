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

namespace RP.Platform.Controller
{
	public class PigeonController : BaseController
	{
		private readonly IMemberRepository _memberRepository;
		private readonly IPigeonRepository _pigeonRepository;

		public PigeonController(
			IMemberRepository memberRepository,
			IPigeonRepository pigeonRepository)
		{
			_memberRepository = memberRepository;
			_pigeonRepository = pigeonRepository;
		}

		[Auth(Create = new[] { Dom.EntityType.Pigeon.Id })]
		[HttpGet]
		public ActionResult Add()
		{
			ICollection<SelectListItemCount> owners = _memberRepository.GetMembers(
				UserContext.User.Id,
				UserContext.User.OrganizationId
			);
			var viewModel = new PigeonAddViewModel
			{
				SexItems = InitSexSelectListItems(),
				OwnerItems = Mapper.Map<ICollection<SelectListItem>>(owners)
			};

			return View(Mvc.View.Pigeon.Add, viewModel);
		}

		[Auth(Create = new[] { Dom.EntityType.Pigeon.Id })]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Add(PigeonAddViewModel viewModel)
		{
			int year;
			if (!int.TryParse(viewModel.Year, out year))
			{
				AddModelError(viewModel, m => m.Year, StyleContext.GetTranslation(Dom.Translation.Pigeon.IncorrectYear));
			}
			else
			{
				if (year < DateTime.Now.AddYears(-25).Year || year > DateTime.Now.Year)
				{
					AddModelError(viewModel, m => m.Year, StyleContext.GetTranslation(Dom.Translation.Pigeon.IncorrectYear));
				}
			}

			if (ModelState.IsValid)
			{
				Pigeon pigeon = Pigeon.Empty();
				pigeon.MemberId = viewModel.OwnerId.Value;
				pigeon.SexId = viewModel.SexId;
				pigeon.Year = year;
				pigeon.Code = viewModel.Code;
				pigeon.Number = viewModel.Number;

				var member = _memberRepository.Get<Member>(pigeon.MemberId);
				var organizationId = member.EntityInfo.EntityOrganizations.First().OrganizationId;
				EntityContext.AddEntityProgress(
					pigeon.EntityInfo,
					new EntityProgress
					{
						OrganizationId = organizationId,
						EntityStateAfterId = Dom.EntityType.Pigeon.State.Created
					}
				);
				EntityContext.AddEntityOrganization(
					pigeon.EntityInfo,
					organizationId,
					Dom.EntityType.Pigeon.State.Created
				);
				_pigeonRepository.AddOrUpdate(pigeon);
				_pigeonRepository.UnitOfWork.SaveChanges();

				return RedirectToAction(Mvc.Controller.Pigeon.List, Mvc.Controller.Pigeon.Name);
			}

			ICollection<SelectListItemCount> owners = _memberRepository.GetMembers(
				UserContext.User.Id,
				UserContext.User.OrganizationId
			);
			viewModel.SexItems = InitSexSelectListItems();
			viewModel.OwnerItems = Mapper.Map<ICollection<SelectListItem>>(owners);
			return View(Mvc.View.Pigeon.Add, viewModel);
		}

		[Auth(Read = new[] { Dom.EntityType.Pigeon.Id })]
		[HttpGet]
		public ActionResult Details(int id)
		{
			PigeonDetails details = _pigeonRepository.GetPigeonDetails(UserContext.User.Id, UserContext.User.OrganizationId, id);
			if (details.Detail == null)
				return NotFoundResult();

			PigeonDetailsViewModel viewModel = Mapper.Map<PigeonDetailsViewModel>(details);
			InitDetailsViewModel(viewModel, details);

			return View(Mvc.View.Pigeon.Details, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Pigeon.Id })]
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

			ViewData[Mvc.ViewData.Controller] = Mvc.Controller.Pigeon.Name;
			return PartialView(Mvc.View.Entity.EntityStateChange, entityStateChangeViewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Pigeon.Id })]
		[HttpGet]
		public ActionResult Edit(int id)
		{
			PigeonDetails details = _pigeonRepository.GetPigeonDetails(UserContext.User.Id, UserContext.User.OrganizationId, id);
			if (details.Detail == null)
				return NotFoundResult();

			PigeonDetailsViewModel viewModel = Mapper.Map<PigeonDetailsViewModel>(details);
			viewModel.Detail.SexItems = InitSexSelectListItems();

			return View(Mvc.View.Pigeon.Edit, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Pigeon.Id })]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(PigeonDetailsViewModel viewModel)
		{
			int year;
			if (!int.TryParse(viewModel.Detail.Year, out year))
			{
				AddModelError(viewModel, m => m.Detail.Year, StyleContext.GetTranslation(Dom.Translation.Pigeon.IncorrectYear));
			}
			else
			{
				if (year < DateTime.Now.AddYears(-25).Year || year > DateTime.Now.Year)
				{
					AddModelError(viewModel, m => m.Detail.Year, StyleContext.GetTranslation(Dom.Translation.Pigeon.IncorrectYear));
				}
			}
			if (ModelState.IsValid)
			{
				Pigeon pigeon = _pigeonRepository.Get<Pigeon>(viewModel.Detail.Id);
				pigeon.Year = year;
				pigeon.Code = viewModel.Detail.Code;
				pigeon.Number = viewModel.Detail.Number;
				pigeon.SexId = viewModel.Detail.SexId;
				_pigeonRepository.UnitOfWork.SaveChanges();
				return RedirectToAction(Mvc.Controller.Pigeon.Details, Mvc.Controller.Pigeon.Name, new { id = viewModel.Detail.Id });
			}
			viewModel.Detail.SexItems = InitSexSelectListItems();

			return View(Mvc.View.Pigeon.Edit, viewModel);
		}

		[Auth(Read = new[] { Dom.EntityType.Pigeon.Id })]
		[HttpGet]
		public ActionResult List(PigeonFilterViewModel filterViewModel)
		{
			InitPigeonFilterViewModel(ref filterViewModel, true);
			var filter = Mapper.Map<PigeonFilter>(filterViewModel);
			PigeonItems list = _pigeonRepository.GetPigeonItems(UserContext.User.Id, UserContext.User.OrganizationId, filter);
			PigeonItemsViewModel viewModel = Mapper.Map<PigeonItemsViewModel>(list);
			viewModel.Filter = filterViewModel;
			InitListViewModel(viewModel, list);

			return View(Mvc.View.Pigeon.List, viewModel);
		}

		private void InitDetailsViewModel(PigeonDetailsViewModel viewModel, PigeonDetails details)
		{
			InitBaseDetailViewModel(details.Detail, viewModel.Detail);
			InitBaseDetailEntityStateChange(viewModel.Detail);
			ViewData[Mvc.ViewData.Controller] = Mvc.Controller.Pigeon.Name;
			viewModel.Detail.Sex = GetSexName(viewModel.Detail.SexId);
		}

		private void InitListViewModel(PigeonItemsViewModel viewModel, PigeonItems list)
		{
			foreach (var item in viewModel.Items)
			{
				InitBaseItemViewModel(item);
				item.Sex = GetSexName(item.SexId);
			}
		}

		private void InitPigeonFilterViewModel(ref PigeonFilterViewModel filter, bool isList)
		{
			if (isList)
				InitBaseFilterViewModel(filter);
		}
	}
}