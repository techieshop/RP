using AutoMapper;
using RP.DAL.Repository;
using RP.Model;
using RP.Platform.Context;
using RP.Platform.Manager;
using RP.Platform.ViewModel;
using System.Web.Mvc;

namespace RP.Platform.Controller
{
	public class RoleController : BaseController
	{
		private readonly IRoleRepository _roleRepository;

		public RoleController(
			IRoleRepository roleRepository
			)
		{
			_roleRepository = roleRepository;
		}

		[Auth(Create = new[] { Dom.EntityType.Role.Id })]
		[HttpGet]
		public ActionResult Add()
		{
			return View(Mvc.View.Role.Add);
		}

		[Auth(Create = new[] { Dom.EntityType.Role.Id })]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Add(RoleAddViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				Role role = new Role
				{
					EntityInfo = EntityInfo.Empty(Dom.EntityType.Role.Id),
					Name = viewModel.Name,
					Description = viewModel.Description
				};
				EntityContext.AddEntityProgress(role.EntityInfo, new EntityProgress
				{
					OrganizationId = Dom.Common.OrganizationId,
					EntityStateAfterId = Dom.EntityType.Role.State.Created
				});
				EntityContext.AddEntityProgress(role.EntityInfo, new EntityProgress
				{
					OrganizationId = Dom.Common.OrganizationId,
					EntityStateBeforeId = Dom.EntityType.Role.State.Created,
					EntityStateAfterId = Dom.EntityType.Role.State.Active
				});
				EntityContext.AddEntityOrganization(role.EntityInfo, Dom.Common.OrganizationId, Dom.EntityType.Role.State.Active);
				_roleRepository.AddOrUpdate(role);
				_roleRepository.UnitOfWork.SaveChanges();

				return RedirectToAction(Mvc.Controller.Role.List, Mvc.Controller.Role.Name);
			}
			return View(Mvc.View.Role.Add, viewModel);
		}

		[Auth(Read = new[] { Dom.EntityType.Role.Id })]
		[HttpGet]
		public ActionResult Details(int id)
		{
			RoleDetails details = _roleRepository.GetRoleDetails(UserContext.User.Id, UserContext.User.OrganizationId, id);
			if (details.Detail == null)
				return NotFoundResult();

			RoleDetailsViewModel viewModel = Mapper.Map<RoleDetailsViewModel>(details);
			InitDetailsViewModel(viewModel, details);

			return View(Mvc.View.Role.Details, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Role.Id })]
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

			ViewData[Mvc.ViewData.Controller] = Mvc.Controller.Role.Name;
			return PartialView(Mvc.View.Entity.EntityStateChange, entityStateChangeViewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Role.Id })]
		[HttpGet]
		public ActionResult Edit(int id)
		{
			var details = _roleRepository.GetRoleDetails(UserContext.User.Id, UserContext.User.OrganizationId, id);
			if (details.Detail == null)
				return NotFoundResult();

			var viewModel = Mapper.Map<RoleDetailsViewModel>(details);

			return View(Mvc.View.Role.Edit, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Role.Id })]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(RoleDetailsViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var role = _roleRepository.Get<Role>(viewModel.Detail.Id);
				role.Name = viewModel.Detail.Name;
				role.Description = viewModel.Detail.Description;
				_roleRepository.UnitOfWork.SaveChanges();

				return RedirectToAction(Mvc.Controller.Role.Details, Mvc.Controller.Role.Name, new { id = viewModel.Detail.Id });
			}

			return View(Mvc.View.Role.Edit, viewModel);
		}

		[Auth(Read = new[] { Dom.EntityType.Role.Id })]
		[HttpGet]
		public ActionResult List(RoleFilterViewModel filterViewModel)
		{
			InitRoleFilterViewModel(ref filterViewModel, true);
			var filter = Mapper.Map<RoleFilter>(filterViewModel);
			RoleItems list = _roleRepository.GetRoleItems(UserContext.User.Id, UserContext.User.OrganizationId, filter);
			RoleItemsViewModel viewModel = Mapper.Map<RoleItemsViewModel>(list);
			viewModel.Filter = filterViewModel;
			InitListViewModel(viewModel, list);

			return View(Mvc.View.Role.List, viewModel);
		}

		private void InitDetailsViewModel(RoleDetailsViewModel viewModel, RoleDetails details)
		{
			InitBaseDetailViewModel(details.Detail, viewModel.Detail);
			InitBaseDetailEntityStateChange(viewModel.Detail);
			ViewData[Mvc.ViewData.Controller] = Mvc.Controller.Role.Name;
		}

		private void InitListViewModel(RoleItemsViewModel viewModel, RoleItems list)
		{
			foreach (var item in viewModel.Items)
			{
				InitBaseItemViewModel(item);
			}
		}

		private void InitRoleFilterViewModel(ref RoleFilterViewModel filterViewModel, bool isList)
		{
			if (isList)
				InitBaseFilterViewModel(filterViewModel);
		}
	}
}