using AutoMapper;
using RP.DAL.Repository;
using RP.Model;
using RP.Platform.Context;
using RP.Platform.Manager;
using RP.Platform.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RP.Platform.Controller
{
	public class PointController : BaseController
	{
		private readonly IOrganizationRepository _organizationRepository;
		private readonly IPointRepository _pointRepository;

		public PointController(
			IPointRepository pointRepository,
			IOrganizationRepository organizationRepository)
		{
			_pointRepository = pointRepository;
			_organizationRepository = organizationRepository;
		}

		[Auth(Create = new[] { Dom.EntityType.Point.Id })]
		[HttpGet]
		public ActionResult Add()
		{
			ICollection<SelectListItemCount> organizationItems = _organizationRepository.GetOrganizations(
				UserContext.User.Id,
				UserContext.User.OrganizationId,
				new List<int> { Dom.OrganizationType.Club }
			);
			var viewModel = new PointAddViewModel
			{
				OrganizationItems = Mapper.Map<ICollection<SelectListItem>>(organizationItems)
			};
			if (organizationItems.FirstOrDefault(m => m.Value == UserContext.User.OrganizationId) != null)
			{
				viewModel.OrganizationId = UserContext.User.OrganizationId;
			}

			return View(Mvc.View.Point.Add, viewModel);
		}

		[Auth(Create = new[] { Dom.EntityType.Point.Id })]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Add(PointAddViewModel viewModel)
		{
			if (string.IsNullOrWhiteSpace(viewModel.Address?.FormattedAddress))
			{
				AddModelError(viewModel, m => m.Address.City, StyleContext.GetTranslation(Dom.Translation.Validation.Required));
			}
			if (ModelState.IsValid)
			{
				Point point = Point.Empty();
				point.Name = viewModel.Name;

				if (!string.IsNullOrWhiteSpace(viewModel.Address?.FormattedAddress))
				{
					point.Address = new Address
					{
						CountryId = Dom.Country.Ukraine,
						City = viewModel.Address.City,
						PostalCode = viewModel.Address.PostalCode,
						Street = viewModel.Address.Street,
						Number = viewModel.Address.Number,
						Latitude = double.Parse(viewModel.Address.Latitude.Replace(".", ",")),
						Longitude = double.Parse(viewModel.Address.Longitude.Replace(".", ",")),
						FormattedAddress = viewModel.Address.FormattedAddress
					};
				}

				EntityContext.AddEntityProgress(
					point.EntityInfo,
					new EntityProgress
					{
						OrganizationId = viewModel.OrganizationId,
						EntityStateAfterId = Dom.EntityType.Point.State.Created
					}
				);
				EntityContext.AddEntityOrganization(
					point.EntityInfo,
					viewModel.OrganizationId,
					Dom.EntityType.Point.State.Created
				);
				_pointRepository.AddOrUpdate(point);
				_pointRepository.UnitOfWork.SaveChanges();

				return RedirectToAction(Mvc.Controller.Point.List, Mvc.Controller.Point.Name);
			}

			ICollection<SelectListItemCount> organizationItems = _organizationRepository.GetOrganizations(
				UserContext.User.Id,
				UserContext.User.OrganizationId,
				new List<int> { Dom.OrganizationType.Club }
			);

			viewModel.OrganizationItems = Mapper.Map<ICollection<SelectListItem>>(organizationItems);
			return View(Mvc.View.Point.Add, viewModel);
		}

		[Auth(Read = new[] { Dom.EntityType.Point.Id })]
		[HttpGet]
		public ActionResult Details(int id)
		{
			PointDetails details = _pointRepository.GetPointDetails(UserContext.User.Id, UserContext.User.OrganizationId, id);
			if (details.Detail == null)
				return NotFoundResult();

			PointDetailsViewModel viewModel = Mapper.Map<PointDetailsViewModel>(details);
			InitDetailsViewModel(viewModel, details);

			return View(Mvc.View.Point.Details, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Point.Id })]
		[HttpGet]
		public ActionResult Edit(int id)
		{
			PointDetails details = _pointRepository.GetPointDetails(UserContext.User.Id, UserContext.User.OrganizationId, id);
			if (details.Detail == null)
				return NotFoundResult();

			PointDetailsViewModel viewModel = Mapper.Map<PointDetailsViewModel>(details);

			return View(Mvc.View.Point.Edit, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Point.Id })]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(PointDetailsViewModel viewModel)
		{
			if (string.IsNullOrWhiteSpace(viewModel.Address?.FormattedAddress))
			{
				AddModelError(viewModel, m => m.Address.City, StyleContext.GetTranslation(Dom.Translation.Validation.Required));
			}
			if (ModelState.IsValid)
			{
				Point point = _pointRepository.Get<Point>(viewModel.Detail.Id);
				point.Name = viewModel.Detail.Name;

				if (!string.IsNullOrWhiteSpace(viewModel.Address?.FormattedAddress))
				{
					point.Address.City = viewModel.Address.City;
					point.Address.PostalCode = viewModel.Address.PostalCode;
					point.Address.Street = viewModel.Address.Street;
					point.Address.Number = viewModel.Address.Number;
					point.Address.Latitude = double.Parse(viewModel.Address.Latitude.Replace(".", ","));
					point.Address.Longitude = double.Parse(viewModel.Address.Longitude.Replace(".", ","));
					point.Address.FormattedAddress = viewModel.Address.FormattedAddress;
				}

				_pointRepository.UnitOfWork.SaveChanges();
				return RedirectToAction(Mvc.Controller.Point.Details, Mvc.Controller.Point.Name, new { id = viewModel.Detail.Id });
			}

			return View(Mvc.View.Point.Edit, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Point.Id })]
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

			ViewData[Mvc.ViewData.Controller] = Mvc.Controller.Point.Name;
			return PartialView(Mvc.View.Entity.EntityStateChange, entityStateChangeViewModel);
		}

		[Auth(Read = new[] { Dom.EntityType.Point.Id })]
		[HttpGet]
		public ActionResult List(PointFilterViewModel filterViewModel)
		{
			InitPointFilterViewModel(ref filterViewModel, true);
			var filter = Mapper.Map<PointFilter>(filterViewModel);
			PointItems list = _pointRepository.GetPointItems(UserContext.User.Id, UserContext.User.OrganizationId, filter);
			PointItemsViewModel viewModel = Mapper.Map<PointItemsViewModel>(list);
			viewModel.Filter = filterViewModel;
			InitListViewModel(viewModel, list);

			return View(Mvc.View.Point.List, viewModel);
		}

		private void InitDetailsViewModel(PointDetailsViewModel viewModel, PointDetails details)
		{
			InitBaseDetailViewModel(details.Detail, viewModel.Detail);
			InitBaseDetailEntityStateChange(viewModel.Detail);
			ViewData[Mvc.ViewData.Controller] = Mvc.Controller.Point.Name;
		}

		private void InitListViewModel(PointItemsViewModel viewModel, PointItems list)
		{
			foreach (var item in viewModel.Items)
			{
				InitBaseItemViewModel(item);
			}
		}

		private void InitPointFilterViewModel(ref PointFilterViewModel filterViewModel, bool isList)
		{
			if (isList)
				InitBaseFilterViewModel(filterViewModel);
		}
	}
}