using AutoMapper;
using RP.Common.Extension;
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
	public class OrganizationController : BaseController
	{
		private readonly IOrganizationRepository _organizationRepository;

		public OrganizationController(
			IOrganizationRepository organizationRepository)
		{
			_organizationRepository = organizationRepository;
		}

		[Auth(Create = new[] { Dom.EntityType.Organization.Id })]
		[HttpGet]
		public ActionResult Add()
		{
			ICollection<SelectListItemCount> userOrganizations = _organizationRepository.GetOrganizations(
				UserContext.User.Id,
				UserContext.User.OrganizationId,
				new List<int> { Dom.OrganizationType.Region }
			);
			var viewModel = new OrganizationAddViewModel
			{
				OrganizationTypeId = Dom.OrganizationType.Club,
				OrganizationTypeItems = InitOrganizationTypeSelectedListItems(),
				OrganizationItems = Mapper.Map<ICollection<SelectListItem>>(userOrganizations)
			};

			return View(Mvc.View.Organization.Add, viewModel);
		}

		[Auth(Create = new[] { Dom.EntityType.Organization.Id })]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Add(OrganizationAddViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				Organization organization = Organization.Empty(viewModel.Name, viewModel.OrganizationTypeId);
				var organizationRelations = _organizationRepository.GetOrganizationRelation(viewModel.OrganizationId);
				foreach (var organizationRelation in organizationRelations)
				{
					organization.OrganizationRelations.Add(
						new OrganizationRelation
						{
							Level = (byte)(organizationRelation.Level + 1),
							Order = (byte)(organizationRelation.Order + 1),
							RelatedOrganizationId = organizationRelation.RelatedOrganizationId
						}
					);
				}
				organization.OrganizationRelations.Add(new OrganizationRelation { Level = 1, Order = 1, RelatedOrganization = organization });
				EntityContext.AddEntityProgress(
					organization.EntityInfo,
					new EntityProgress
					{
						Organization = organization,
						EntityStateAfterId = Dom.EntityType.Organization.State.Created
					}
				);
				EntityContext.AddEntityOrganization(
					organization.EntityInfo,
					organization,
					Dom.EntityType.Organization.State.Created);
				_organizationRepository.AddOrUpdate(organization);
				_organizationRepository.UnitOfWork.SaveChanges();

				return RedirectToAction(Mvc.Controller.Organization.List, Mvc.Controller.Organization.Name);
			}

			ICollection<SelectListItemCount> userOrganizations = _organizationRepository.GetOrganizations(
				UserContext.User.Id,
				UserContext.User.OrganizationId,
				new List<int> { viewModel.OrganizationTypeId - 1 }
			);
			viewModel.OrganizationTypeItems = InitOrganizationTypeSelectedListItems();
			viewModel.OrganizationItems = Mapper.Map<ICollection<SelectListItem>>(userOrganizations);
			return View(Mvc.View.Organization.Add, viewModel);
		}

		[Auth(Read = new[] { Dom.EntityType.Organization.Id })]
		[HttpGet]
		public ActionResult Details(int id)
		{
			OrganizationDetails details = _organizationRepository.GetOrganizationDetails(UserContext.User.Id, UserContext.User.OrganizationId, id);
			if (details.Detail == null)
				return NotFoundResult();

			OrganizationDetailsViewModel viewModel = Mapper.Map<OrganizationDetailsViewModel>(details);
			InitDetailsViewModel(viewModel, details);

			return View(Mvc.View.Organization.Details, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Organization.Id })]
		[HttpGet]
		public ActionResult Edit(int id)
		{
			OrganizationDetails details = _organizationRepository.GetOrganizationDetails(UserContext.User.Id, UserContext.User.OrganizationId, id);
			if (details.Detail == null)
				return NotFoundResult();

			OrganizationDetailsViewModel viewModel = Mapper.Map<OrganizationDetailsViewModel>(details);
			InitDetailsViewModel(viewModel, details);
			viewModel.HeadIds = viewModel.Heads.Select(m => m.Key).ToList();
			viewModel.DeputyIds = viewModel.Deputies.Select(m => m.Key).ToList();
			viewModel.MainJudgeIds = viewModel.MainJudges.Select(m => m.Key).ToList();
			viewModel.JudgeIds = viewModel.Judges.Select(m => m.Key).ToList();
			viewModel.SecretaryIds = viewModel.Secretaries.Select(m => m.Key).ToList();
			viewModel.CashierIds = viewModel.Cashiers.Select(m => m.Key).ToList();

			return View(Mvc.View.Organization.Edit, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Organization.Id })]
		[HttpPost, ValidateInput(false)]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(OrganizationDetailsViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var organization = _organizationRepository.Get<Organization>(viewModel.Detail.Id);
				organization.Name = viewModel.Detail.Name;
				organization.Description = viewModel.Detail.Description;
				organization.CreateDate = viewModel.Detail.CreateDate;
				if (!string.IsNullOrWhiteSpace(viewModel.Address?.FormattedAddress))
				{
					if (organization.AddressId != null)
					{
						organization.Address.City = viewModel.Address.City;
						organization.Address.PostalCode = viewModel.Address.PostalCode;
						organization.Address.Street = viewModel.Address.Street;
						organization.Address.Number = viewModel.Address.Number;
						organization.Address.Latitude = double.Parse(viewModel.Address.Latitude.Replace(".", ","));
						organization.Address.Longitude = double.Parse(viewModel.Address.Longitude.Replace(".", ","));
						organization.Address.FormattedAddress = viewModel.Address.FormattedAddress;
					}
					else
					{
						organization.Address = new Address
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
				}
				else
				{
					if (organization.AddressId != null)
					{
						_organizationRepository.Delete(organization.Address);
						organization.AddressId = null;
					}
				}
				if (!string.IsNullOrWhiteSpace(viewModel.Website?.Url))
				{
					if (organization.WebsiteId != null)
					{
						organization.Website.Url = viewModel.Website.Url;
					}
					else
					{
						organization.Website = new Website
						{
							Url = viewModel.Website.Url,
						};
					}
				}
				else
				{
					if (organization.WebsiteId != null)
					{
						_organizationRepository.Delete(organization.Website);
						organization.WebsiteId = null;
					}
				}

				if (organization.OrganizationMemberTypes != null)
				{
					organization.OrganizationMemberTypes.ToList().ForEach(x => _organizationRepository.Delete(x));
				}
				else
				{
					organization.OrganizationMemberTypes = new List<OrganizationMemberType>();
				}

				viewModel.HeadIds?.ForEach(m => organization.OrganizationMemberTypes.Add(new OrganizationMemberType { MemberId = m, MemberTypeId = Dom.MemberType.Head }));
				viewModel.DeputyIds?.ForEach(m => organization.OrganizationMemberTypes.Add(new OrganizationMemberType { MemberId = m, MemberTypeId = Dom.MemberType.Deputy }));
				viewModel.MainJudgeIds?.ForEach(m => organization.OrganizationMemberTypes.Add(new OrganizationMemberType { MemberId = m, MemberTypeId = Dom.MemberType.MainJudge }));
				viewModel.JudgeIds?.ForEach(m => organization.OrganizationMemberTypes.Add(new OrganizationMemberType { MemberId = m, MemberTypeId = Dom.MemberType.Judge }));
				viewModel.SecretaryIds?.ForEach(m => organization.OrganizationMemberTypes.Add(new OrganizationMemberType { MemberId = m, MemberTypeId = Dom.MemberType.Secretary }));
				viewModel.CashierIds?.ForEach(m => organization.OrganizationMemberTypes.Add(new OrganizationMemberType { MemberId = m, MemberTypeId = Dom.MemberType.Cashier }));

				_organizationRepository.UnitOfWork.SaveChanges();
				return RedirectToAction(Mvc.Controller.Organization.Details, Mvc.Controller.Organization.Name, new { id = viewModel.Detail.Id });
			}

			OrganizationDetails details = _organizationRepository.GetOrganizationDetails(UserContext.User.Id, UserContext.User.OrganizationId, viewModel.Detail.Id);
			InitDetailsViewModel(viewModel, details);

			return View(Mvc.View.Organization.Edit, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Organization.Id })]
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

			ViewData[Mvc.ViewData.Controller] = Mvc.Controller.Organization.Name;
			return PartialView(Mvc.View.Entity.EntityStateChange, entityStateChangeViewModel);
		}

		[Auth(Read = new[] { Dom.EntityType.Organization.Id })]
		[HttpGet]
		public ActionResult List(OrganizationFilterViewModel filterViewModel)
		{
			InitOrganizationFilterViewModel(ref filterViewModel, true);
			var filter = Mapper.Map<OrganizationFilter>(filterViewModel);
			OrganizationItems list = _organizationRepository.GetOrganizationItems(UserContext.User.Id, UserContext.User.OrganizationId, filter);
			OrganizationItemsViewModel viewModel = Mapper.Map<OrganizationItemsViewModel>(list);
			viewModel.Filter = filterViewModel;
			InitListViewModel(viewModel, list);

			return View(Mvc.View.Organization.List, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Organization.Id })]
		[HttpGet]
		public ActionResult Organizations(int organizationTypeId)
		{
			var organizations = _organizationRepository.GetOrganizations(
				UserContext.User.Id,
				UserContext.User.OrganizationId,
				new List<int> { organizationTypeId - 1 }
			);
			return Json(Mapper.Map<ICollection<SelectListItem>>(organizations), JsonRequestBehavior.AllowGet);
		}

		private void InitDetailsViewModel(OrganizationDetailsViewModel viewModel, OrganizationDetails details)
		{
			InitBaseDetailViewModel(details.Detail, viewModel.Detail);
			InitBaseDetailEntityStateChange(viewModel.Detail);
			ViewData[Mvc.ViewData.Controller] = Mvc.Controller.Organization.Name;

			viewModel.Heads = details
				.OrganizationMemberTypes
				.Where(om => om.OrganizationId == details.Detail.Id && om.MemberTypeId == Dom.MemberType.Head)
				.ToDictionary(t => t.MemberId, t => Format.FormattedInitials(t.LastName, t.FirstName, t.MiddleName));
			viewModel.Deputies = details
					.OrganizationMemberTypes
					.Where(om => om.OrganizationId == details.Detail.Id && om.MemberTypeId == Dom.MemberType.Deputy)
					.ToDictionary(t => t.MemberId, t => Format.FormattedInitials(t.LastName, t.FirstName, t.MiddleName));
			viewModel.MainJudges = details
					.OrganizationMemberTypes
					.Where(om => om.OrganizationId == details.Detail.Id && om.MemberTypeId == Dom.MemberType.MainJudge)
					.ToDictionary(t => t.MemberId, t => Format.FormattedInitials(t.LastName, t.FirstName, t.MiddleName));
			viewModel.Judges = details
					.OrganizationMemberTypes
					.Where(om => om.OrganizationId == details.Detail.Id && om.MemberTypeId == Dom.MemberType.Judge)
					.ToDictionary(t => t.MemberId, t => Format.FormattedInitials(t.LastName, t.FirstName, t.MiddleName));
			viewModel.Secretaries = details
					.OrganizationMemberTypes
					.Where(om => om.OrganizationId == details.Detail.Id && om.MemberTypeId == Dom.MemberType.Secretary)
					.ToDictionary(t => t.MemberId, t => Format.FormattedInitials(t.LastName, t.FirstName, t.MiddleName));
			viewModel.Cashiers = details
					.OrganizationMemberTypes
					.Where(om => om.OrganizationId == details.Detail.Id && om.MemberTypeId == Dom.MemberType.Cashier)
					.ToDictionary(t => t.MemberId, t => Format.FormattedInitials(t.LastName, t.FirstName, t.MiddleName));

			viewModel.HeadItems = Mapper.Map<ICollection<SelectListItem>>(details.Members);
			viewModel.DeputyItems = Mapper.Map<ICollection<SelectListItem>>(details.Members);
			viewModel.MainJudgeItems = Mapper.Map<ICollection<SelectListItem>>(details.Members);
			viewModel.JudgeItems = Mapper.Map<ICollection<SelectListItem>>(details.Members);
			viewModel.SecretaryItems = Mapper.Map<ICollection<SelectListItem>>(details.Members);
			viewModel.CashierItems = Mapper.Map<ICollection<SelectListItem>>(details.Members);
		}

		private void InitListViewModel(OrganizationItemsViewModel viewModel, OrganizationItems list)
		{
			foreach (var item in viewModel.Items)
			{
				InitBaseItemViewModel(item);
				item.Heads = list
					.Members
					.Where(om => om.OrganizationId == item.Id && om.MemberTypeId == Dom.MemberType.Head)
					.ToDictionary(t => t.MemberId, t => Format.FormattedInitials(t.LastName, t.FirstName, t.MiddleName));
				item.Judges = list
					.Members
					.Where(om => om.OrganizationId == item.Id && (om.MemberTypeId == Dom.MemberType.MainJudge || om.MemberTypeId == Dom.MemberType.Judge))
					.Select(m => new
					{
						Key = m.MemberId,
						Value = Format.FormattedInitials(m.LastName, m.FirstName, m.MiddleName)
					})
					.Distinct()
					.ToDictionary(t => t.Key, t => t.Value);
			}
		}

		private void InitOrganizationFilterViewModel(ref OrganizationFilterViewModel filterViewModel, bool isList)
		{
			if (isList)
				InitBaseFilterViewModel(filterViewModel);
		}
	}
}