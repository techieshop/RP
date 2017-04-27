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
	public class MemberController : BaseController
	{
		private readonly IMemberRepository _memberRepository;
		private readonly IOrganizationRepository _organizationRepository;

		public MemberController(
			IMemberRepository memberRepository,
			IOrganizationRepository organizationRepository)
		{
			_memberRepository = memberRepository;
			_organizationRepository = organizationRepository;
		}

		[Auth(Create = new[] { Dom.EntityType.Pigeon.Id })]
		[HttpGet]
		public ActionResult Add()
		{
			ICollection<SelectListItemCount> organizationItems = _organizationRepository.GetOrganizations(
				UserContext.User.Id,
				UserContext.User.OrganizationId,
				new List<int> { Dom.OrganizationType.Club }
			);
			var viewModel = new MemberAddViewModel
			{
				GenderItems = InitGenderSelectListItems(),
				OrganizationItems = Mapper.Map<ICollection<SelectListItem>>(organizationItems)
			};
			if (organizationItems.FirstOrDefault(m => m.Value == UserContext.User.OrganizationId) != null)
			{
				viewModel.OrganizationId = UserContext.User.OrganizationId;
			}

			return View(Mvc.View.Member.Add, viewModel);
		}

		[Auth(Create = new[] { Dom.EntityType.Member.Id })]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Add(MemberAddViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				Member member = Member.Empty();
				member.FirstName = viewModel.FirstName;
				member.LastName = viewModel.LastName;
				member.MiddleName = viewModel.MiddleName;
				member.DateOfBirth = viewModel.DateOfBirth;
				member.GenderId = viewModel.GenderId;
				member.Email = viewModel.Email;
				member.Phone = viewModel.Phone;
				member.Mobile = viewModel.Mobile;

				if (!string.IsNullOrWhiteSpace(viewModel.Address?.FormattedAddress))
				{
					member.Address = new Address
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

				if (!string.IsNullOrWhiteSpace(viewModel.Website?.Url))
				{
					member.Website = new Website
					{
						Url = viewModel.Website.Url,
					};
				}

				EntityContext.AddEntityProgress(
					member.EntityInfo,
					new EntityProgress
					{
						OrganizationId = viewModel.OrganizationId,
						EntityStateAfterId = Dom.EntityType.Member.State.Created
					}
				);
				EntityContext.AddEntityOrganization(
					member.EntityInfo,
					viewModel.OrganizationId,
					Dom.EntityType.Member.State.Created
				);
				_memberRepository.AddOrUpdate(member);
				_memberRepository.UnitOfWork.SaveChanges();

				return RedirectToAction(Mvc.Controller.Member.List, Mvc.Controller.Member.Name);
			}

			ICollection<SelectListItemCount> organizationItems = _organizationRepository.GetOrganizations(
				UserContext.User.Id,
				UserContext.User.OrganizationId,
				new List<int> { Dom.OrganizationType.Club }
			);

			viewModel.GenderItems = InitGenderSelectListItems();
			viewModel.OrganizationItems = Mapper.Map<ICollection<SelectListItem>>(organizationItems);
			return View(Mvc.View.Member.Add, viewModel);
		}

		[Auth(Read = new[] { Dom.EntityType.Member.Id })]
		[HttpGet]
		public ActionResult Details(int id)
		{
			MemberDetails details = _memberRepository.GetMemberDetails(UserContext.User.Id, UserContext.User.OrganizationId, id);
			if (details.Detail == null)
				return NotFoundResult();

			MemberDetailsViewModel viewModel = Mapper.Map<MemberDetailsViewModel>(details);
			InitDetailsViewModel(viewModel, details);

			return View(Mvc.View.Member.Details, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Member.Id })]
		[HttpGet]
		public ActionResult Edit(int id)
		{
			MemberDetails details = _memberRepository.GetMemberDetails(UserContext.User.Id, UserContext.User.OrganizationId, id);
			if (details.Detail == null)
				return NotFoundResult();

			MemberDetailsViewModel viewModel = Mapper.Map<MemberDetailsViewModel>(details);
			viewModel.Detail.GenderItems = InitGenderSelectListItems();

			return View(Mvc.View.Member.Edit, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Member.Id })]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(MemberDetailsViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				Member member = _memberRepository.Get<Member>(viewModel.Detail.Id);
				member.FirstName = viewModel.Detail.FirstName;
				member.LastName = viewModel.Detail.LastName;
				member.MiddleName = viewModel.Detail.MiddleName;
				member.DateOfBirth = viewModel.Detail.DateOfBirth;
				member.GenderId = viewModel.Detail.GenderId;
				member.Email = viewModel.Detail.Email;
				member.Phone = viewModel.Detail.Phone;
				member.Mobile = viewModel.Detail.Mobile;

				if (!string.IsNullOrWhiteSpace(viewModel.Address?.FormattedAddress))
				{
					if (member.AddressId != null)
					{
						member.Address.City = viewModel.Address.City;
						member.Address.PostalCode = viewModel.Address.PostalCode;
						member.Address.Street = viewModel.Address.Street;
						member.Address.Number = viewModel.Address.Number;
						member.Address.Latitude = double.Parse(viewModel.Address.Latitude.Replace(".", ","));
						member.Address.Longitude = double.Parse(viewModel.Address.Longitude.Replace(".", ","));
						member.Address.FormattedAddress = viewModel.Address.FormattedAddress;
					}
					else
					{
						member.Address = new Address
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
					if (member.AddressId != null)
					{
						_memberRepository.Delete(member.Address);
						member.AddressId = null;
					}
				}
				if (!string.IsNullOrWhiteSpace(viewModel.Website?.Url))
				{
					if (member.WebsiteId != null)
					{
						member.Website.Url = viewModel.Website.Url;
					}
					else
					{
						member.Website = new Website
						{
							Url = viewModel.Website.Url,
						};
					}
				}
				else
				{
					if (member.WebsiteId != null)
					{
						_memberRepository.Delete(member.Website);
						member.WebsiteId = null;
					}
				}
				_memberRepository.UnitOfWork.SaveChanges();
				return RedirectToAction(Mvc.Controller.Member.Details, Mvc.Controller.Member.Name, new { id = viewModel.Detail.Id });
			}
			viewModel.Detail.GenderItems = InitGenderSelectListItems();

			return View(Mvc.View.Member.Edit, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.Member.Id })]
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

			ViewData[Mvc.ViewData.Controller] = Mvc.Controller.Member.Name;
			return PartialView(Mvc.View.Entity.EntityStateChange, entityStateChangeViewModel);
		}

		[Auth(Read = new[] { Dom.EntityType.Member.Id })]
		[HttpGet]
		public ActionResult List(MemberFilterViewModel filterViewModel)
		{
			InitMemberFilterViewModel(ref filterViewModel, true);
			var filter = Mapper.Map<MemberFilter>(filterViewModel);
			MemberItems list = _memberRepository.GetMemberItems(UserContext.User.Id, UserContext.User.OrganizationId, filter);
			MemberItemsViewModel viewModel = Mapper.Map<MemberItemsViewModel>(list);
			viewModel.Filter = filterViewModel;
			InitListViewModel(viewModel, list);

			return View(Mvc.View.Member.List, viewModel);
		}

		private void InitDetailsViewModel(MemberDetailsViewModel viewModel, MemberDetails details)
		{
			InitBaseDetailViewModel(details.Detail, viewModel.Detail);
			InitBaseDetailEntityStateChange(viewModel.Detail);
			ViewData[Mvc.ViewData.Controller] = Mvc.Controller.Member.Name;
			if (viewModel.OrganizationMemberTypes != null)
			{
				foreach (var organizationMemberType in viewModel.OrganizationMemberTypes)
				{
					switch (organizationMemberType.MemberTypeId)
					{
						case Dom.MemberType.Head:
							viewModel.Detail.MemberType = Format.Join(", ", viewModel.Detail.MemberType, StyleContext.GetTranslation(Dom.Translation.MemberType.Head));
							break;

						case Dom.MemberType.Deputy:
							viewModel.Detail.MemberType = Format.Join(", ", viewModel.Detail.MemberType, StyleContext.GetTranslation(Dom.Translation.MemberType.Deputy));
							break;

						case Dom.MemberType.MainJudge:
							viewModel.Detail.MemberType = Format.Join(", ", viewModel.Detail.MemberType, StyleContext.GetTranslation(Dom.Translation.MemberType.MainJudge));
							break;

						case Dom.MemberType.Judge:
							viewModel.Detail.MemberType = Format.Join(", ", viewModel.Detail.MemberType, StyleContext.GetTranslation(Dom.Translation.MemberType.Judge));
							break;

						case Dom.MemberType.Secretary:
							viewModel.Detail.MemberType = Format.Join(", ", viewModel.Detail.MemberType, StyleContext.GetTranslation(Dom.Translation.MemberType.Secretary));
							break;

						case Dom.MemberType.Cashier:
							viewModel.Detail.MemberType = Format.Join(", ", viewModel.Detail.MemberType, StyleContext.GetTranslation(Dom.Translation.MemberType.Cashier));
							break;

						default:
							viewModel.Detail.MemberType = Format.Join(", ", viewModel.Detail.MemberType, StyleContext.GetTranslation(Dom.Translation.MemberType.Member));
							break;
					}
				}
			}

			viewModel.Detail.Gender = GetGenderName(viewModel.Detail.GenderId);
		}

		private void InitListViewModel(MemberItemsViewModel viewModel, MemberItems list)
		{
			foreach (var item in viewModel.Items)
			{
				InitBaseItemViewModel(item);
				item.FormattedName = $"{item.LastName} {item.FirstName} {item.MiddleName}";
			}
		}

		private void InitMemberFilterViewModel(ref MemberFilterViewModel filterViewModel, bool isList)
		{
			if (isList)
				InitBaseFilterViewModel(filterViewModel);
		}
	}
}