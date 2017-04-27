using AutoMapper;
using RP.Common.Extension;
using RP.Common.Manager;
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
	public class UserController : BaseController
	{
		private readonly IOrganizationRepository _organizationRepository;
		private readonly IRoleRepository _roleRepository;
		private readonly IUserRepository _userRepository;

		public UserController(
			IOrganizationRepository organizationRepository,
			IRoleRepository roleRepository,
			IUserRepository userRepository
			)
		{
			_organizationRepository = organizationRepository;
			_roleRepository = roleRepository;
			_userRepository = userRepository;
		}

		[Auth(Create = new[] { Dom.EntityType.User.Id })]
		[HttpGet]
		public ActionResult Add()
		{
			ICollection<SelectListItemCount> organizationItems = _organizationRepository.GetOrganizations(
				UserContext.User.Id,
				UserContext.User.OrganizationId
			);
			ICollection<SelectListItemCount> roleItems = _roleRepository.GetRoles(
				UserContext.User.Id,
				UserContext.User.OrganizationId
			);
			UserAddViewModel viewModel = new UserAddViewModel
			{
				OrganizationItems = Mapper.Map<ICollection<SelectListItem>>(organizationItems),
				RoleItems = Mapper.Map<ICollection<SelectListItem>>(roleItems),
				GenderItems = InitGenderSelectListItems(),
				LanguageItems = InitLanguageSelectListItems()
			};

			return View(Mvc.View.User.Add, viewModel);
		}

		[Auth(Create = new[] { Dom.EntityType.User.Id })]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Add(UserAddViewModel viewModel)
		{
			if (_userRepository.Exists(viewModel.Email))
			{
				AddModelError(viewModel, m => m.Email, Dom.Translation.User.DuplicateEmail);
			}
			if (ModelState.IsValid)
			{
				var user = new User
				{
					EntityInfo = EntityInfo.Empty(Dom.EntityType.User.Id),
					FirstName = viewModel.FirstName,
					LastName = viewModel.LastName,
					MiddleName = viewModel.MiddleName,
					Email = viewModel.Email,
					Password = EncryptionManager.Encrypt(viewModel.Password),
					DateOfBirth = viewModel.DateOfBirth,
					GenderId = viewModel.GenderId,
					Phone = viewModel.Phone,
					Mobile = viewModel.Mobile,
					LanguageId = viewModel.LanguageId != 0 ? viewModel.LanguageId : Dom.Language.System,
					Salutation = viewModel.Salutation
				};
				if (!string.IsNullOrWhiteSpace(viewModel.Address?.FormattedAddress))
				{
					user.Address.City = viewModel.Address.City;
					user.Address.PostalCode = viewModel.Address.PostalCode;
					user.Address.Street = viewModel.Address.Street;
					user.Address.Number = viewModel.Address.Number;
					user.Address.Latitude = double.Parse(viewModel.Address.Latitude.Replace(".", ","));
					user.Address.Longitude = double.Parse(viewModel.Address.Longitude.Replace(".", ","));
					user.Address.FormattedAddress = viewModel.Address.FormattedAddress;
				}
				user.UserRoles = new List<UserRole>();
				var organizationId = UserContext.User.OrganizationId;
				if (viewModel.RoleOrganizationId != 0)
				{
					viewModel.RoleIds?.ToList().ForEach(r =>
					{
						user.UserRoles.Add(new UserRole
						{
							OrganizationId = viewModel.RoleOrganizationId,
							RoleId = r
						});
					});
					organizationId = viewModel.RoleOrganizationId;
				}

				EntityContext.AddEntityProgress(user.EntityInfo, new EntityProgress
				{
					OrganizationId = organizationId,
					EntityStateAfterId = Dom.EntityType.User.State.Created
				});
				EntityContext.AddEntityProgress(user.EntityInfo, new EntityProgress
				{
					OrganizationId = organizationId,
					EntityStateBeforeId = Dom.EntityType.User.State.Created,
					EntityStateAfterId = Dom.EntityType.User.State.Active
				});
				EntityContext.AddEntityOrganization(user.EntityInfo, organizationId, Dom.EntityType.User.State.Active);
				_userRepository.AddOrUpdate(user);
				_userRepository.UnitOfWork.SaveChanges();

				return RedirectToAction(Mvc.Controller.User.List, Mvc.Controller.User.Name);
			}
			ICollection<SelectListItemCount> organizationItems = _organizationRepository.GetOrganizations(
				UserContext.User.Id,
				UserContext.User.OrganizationId
			);
			viewModel.OrganizationItems = Mapper.Map<ICollection<SelectListItem>>(organizationItems);
			ICollection<SelectListItemCount> roleItems = _roleRepository.GetRoles(
				UserContext.User.Id,
				UserContext.User.OrganizationId
			);
			viewModel.RoleItems = Mapper.Map<ICollection<SelectListItem>>(roleItems);
			viewModel.GenderItems = InitGenderSelectListItems();
			viewModel.LanguageItems = InitLanguageSelectListItems();

			return View(Mvc.View.User.Add, viewModel);
		}

		[Auth(Read = new[] { Dom.EntityType.User.Id })]
		[HttpGet]
		public ActionResult Details(int id)
		{
			UserDetails details = _userRepository.GetUserDetails(UserContext.User.Id, UserContext.User.OrganizationId, id);
			if (details.Detail == null)
				return NotFoundResult();

			UserDetailsViewModel viewModel = Mapper.Map<UserDetailsViewModel>(details);
			InitDetailsViewModel(viewModel, details);

			return View(Mvc.View.User.Details, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.User.Id })]
		[HttpGet]
		public ActionResult Edit(int id)
		{
			var details = _userRepository.GetUserDetails(UserContext.User.Id, UserContext.User.OrganizationId, id);
			if (details.Detail == null)
				return NotFoundResult();

			var viewModel = Mapper.Map<UserDetailsViewModel>(details);
			viewModel.Detail.Password = EncryptionManager.Decrypt(viewModel.Detail.Password);
			ICollection<SelectListItemCount> organizationItems = _organizationRepository.GetOrganizations(
				UserContext.User.Id,
				UserContext.User.OrganizationId
			);
			viewModel.Detail.OrganizationItems = Mapper.Map<ICollection<SelectListItem>>(organizationItems);
			ICollection<SelectListItemCount> roleItems = _roleRepository.GetRoles(
				UserContext.User.Id,
				UserContext.User.OrganizationId
			);
			viewModel.Detail.RoleItems = Mapper.Map<ICollection<SelectListItem>>(roleItems);
			viewModel.Detail.RoleIds = details.UserRoles.Select(m => m.RoleId).Distinct().ToList();
			viewModel.Detail.RoleOrganizationId = details.UserRoles.Select(m => m.OrganizationId).FirstOrDefault();
			viewModel.Detail.GenderItems = InitGenderSelectListItems();
			viewModel.Detail.LanguageItems = InitLanguageSelectListItems();

			return View(Mvc.View.User.Edit, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.User.Id })]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(UserDetailsViewModel viewModel)
		{
			if (_userRepository.Exists(viewModel.Detail.Email, viewModel.Detail.Id))
			{
				AddModelError(viewModel, m => m.Detail.Email, Dom.Translation.User.DuplicateEmail);
			}
			if (ModelState.IsValid)
			{
				var user = _userRepository.Get<User>(viewModel.Detail.Id);
				user.FirstName = viewModel.Detail.FirstName;
				user.LastName = viewModel.Detail.LastName;
				user.MiddleName = viewModel.Detail.MiddleName;
				user.Email = viewModel.Detail.Email;
				user.Password = EncryptionManager.Encrypt(viewModel.Detail.Password);
				user.DateOfBirth = viewModel.Detail.DateOfBirth;
				user.GenderId = viewModel.Detail.GenderId;
				user.Phone = viewModel.Detail.Phone;
				user.Mobile = viewModel.Detail.Mobile;
				user.LanguageId = viewModel.Detail.LanguageId != 0 ? viewModel.Detail.LanguageId : Dom.Language.System;
				user.Salutation = viewModel.Detail.Salutation;
				if (!string.IsNullOrWhiteSpace(viewModel.Address?.FormattedAddress))
				{
					if (user.AddressId != null)
					{
						user.Address.City = viewModel.Address.City;
						user.Address.PostalCode = viewModel.Address.PostalCode;
						user.Address.Street = viewModel.Address.Street;
						user.Address.Number = viewModel.Address.Number;
						user.Address.Latitude = double.Parse(viewModel.Address.Latitude.Replace(".", ","));
						user.Address.Longitude = double.Parse(viewModel.Address.Longitude.Replace(".", ","));
						user.Address.FormattedAddress = viewModel.Address.FormattedAddress;
					}
					else
					{
						user.Address = new Address
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
					if (user.AddressId != null)
					{
						_userRepository.Delete(user.Address);
						user.AddressId = null;
					}
				}
				if (user.UserRoles != null)
				{
					user.UserRoles.ToList().ForEach(x =>
					{
						_userRepository.Delete(x);
					});
				}
				else
				{
					user.UserRoles = new List<UserRole>();
				}
				if (viewModel.Detail.RoleOrganizationId != 0)
				{
					viewModel.Detail.RoleIds?.ToList().ForEach(r =>
					{
						user.UserRoles.Add(new UserRole
						{
							OrganizationId = viewModel.Detail.RoleOrganizationId,
							RoleId = r
						});
					});
					//todo check this in future
					user.EntityInfo.EntityOrganizations.First().OrganizationId = viewModel.Detail.RoleOrganizationId;
				}
				_userRepository.UnitOfWork.SaveChanges();
				return RedirectToAction(Mvc.Controller.User.Details, Mvc.Controller.User.Name, new { id = viewModel.Detail.Id });
			}

			ICollection<SelectListItemCount> organizationItems = _organizationRepository.GetOrganizations(
				UserContext.User.Id,
				UserContext.User.OrganizationId
			);
			viewModel.Detail.OrganizationItems = Mapper.Map<ICollection<SelectListItem>>(organizationItems);
			ICollection<SelectListItemCount> roleItems = _roleRepository.GetRoles(
				UserContext.User.Id,
				UserContext.User.OrganizationId
			);
			viewModel.Detail.RoleItems = Mapper.Map<ICollection<SelectListItem>>(roleItems);
			viewModel.Detail.GenderItems = InitGenderSelectListItems();
			viewModel.Detail.LanguageItems = InitLanguageSelectListItems();
			return View(Mvc.View.User.Edit, viewModel);
		}

		[Auth(Write = new[] { Dom.EntityType.User.Id })]
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

			ViewData[Mvc.ViewData.Controller] = Mvc.Controller.User.Name;
			return PartialView(Mvc.View.Entity.EntityStateChange, entityStateChangeViewModel);
		}

		[Auth(Read = new[] { Dom.EntityType.User.Id })]
		[HttpGet]
		public ActionResult List(UserFilterViewModel filterViewModel)
		{
			InitUserFilterViewModel(ref filterViewModel, true);
			var filter = Mapper.Map<UserFilter>(filterViewModel);
			UserItems list = _userRepository.GetUserItems(UserContext.User.Id, UserContext.User.OrganizationId, filter);
			UserItemsViewModel viewModel = Mapper.Map<UserItemsViewModel>(list);
			viewModel.Filter = filterViewModel;
			InitListViewModel(viewModel, list);

			return View(Mvc.View.User.List, viewModel);
		}

		[HttpGet]
		public ActionResult Login(string redirectUrl)
		{
			if (UserContext.IsAuthenticated)
			{
				if (!string.IsNullOrEmpty(redirectUrl))
				{
					return LogInActionResult(UserContext.User.Id, redirectUrl);
				}
				else
				{
					return RedirectHome();
				}
			}

			var viewModel = new UserLoginViewModel { RedirectUrl = redirectUrl };

			return View(Mvc.View.User.Login, viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Login(UserLoginViewModel viewModel)
		{
			if (ModelState.IsValid && !UserContext.IsAuthenticated)
			{
				var password = EncryptionManager.Encrypt(viewModel.Password);
				var user = _userRepository.GetUser(viewModel.Email, password);
				if (user != null && _userRepository.HasUserLoginAccess(user.Id))
				{
					var userAuth = _userRepository.GetUserAuth(user.Id);
					AuthManager.SetAuthCookie(userAuth.UserData.Email, userAuth.UserData.Id, userAuth.UserData.OrganizationId, true);

					return LogInActionResult(userAuth.UserData.Id, viewModel.RedirectUrl);
				}
				else
				{
					AddModelError(viewModel, m => m.Password, Dom.Translation.User.LogInError);
				}
			}

			return View(Mvc.View.User.Login, viewModel);
		}

		public ActionResult LogInActionResult(int userId, string redirectUrl)
		{
			if (!string.IsNullOrEmpty(redirectUrl))
			{
				return Redirect(redirectUrl);
			}
			return RedirectHome();
		}

		public ActionResult Logout()
		{
			AuthManager.SignOut();

			return RedirectHome();
		}

		[Auth]
		[HttpGet]
		public ActionResult Profile()
		{
			var userProfile = _userRepository.GetUserProfile(UserContext.User.Id);
			UserProfileViewModel viewModel = Mapper.Map<UserProfileViewModel>(userProfile);
			viewModel.FormattedName = Format.FormattedFullName(viewModel.LastName, viewModel.FirstName, viewModel.MiddleName);
			viewModel.Gender = GetGenderName(viewModel.GenderId);
			viewModel.Language = GetLanguageName(viewModel.LanguageId);

			return View(Mvc.View.User.Profile, viewModel);
		}

		[HttpGet]
		[Auth]
		public ActionResult ProfileEdit()
		{
			var userProfile = _userRepository.GetUserProfile(UserContext.User.Id);
			UserProfileViewModel viewModel = Mapper.Map<UserProfileViewModel>(userProfile);
			viewModel.Password = EncryptionManager.Decrypt(viewModel.Password);
			viewModel.ConfirmPassword = viewModel.Password;
			viewModel.FormattedName = Format.FormattedFullName(viewModel.LastName, viewModel.FirstName, viewModel.MiddleName);
			viewModel.Gender = GetGenderName(viewModel.GenderId);
			viewModel.Language = GetLanguageName(viewModel.LanguageId);
			viewModel.GenderItems = InitGenderSelectListItems();
			viewModel.LanguageItems = InitLanguageSelectListItems();

			return View(Mvc.View.User.ProfileEdit, viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ProfileEdit(UserProfileViewModel viewModel)
		{
			if (_userRepository.Exists(viewModel.Email, viewModel.Id))
			{
				AddModelError(viewModel, m => m.Email, Dom.Translation.User.DuplicateEmail);
			}

			if (ModelState.IsValid && viewModel.Password == viewModel.ConfirmPassword)
			{
				User user = _userRepository.Get<User>(viewModel.Id);
				user.Id = viewModel.Id;
				user.FirstName = viewModel.FirstName;
				user.LastName = viewModel.LastName;
				user.MiddleName = viewModel.MiddleName;
				user.Email = viewModel.Email;
				user.Password = EncryptionManager.Encrypt(viewModel.Password);
				user.DateOfBirth = viewModel.DateOfBirth;
				user.GenderId = viewModel.GenderId;
				user.LanguageId = viewModel.LanguageId != 0 ? viewModel.LanguageId : Dom.Language.System;
				user.Phone = viewModel.Phone;
				user.Mobile = viewModel.Mobile;
				user.Salutation = viewModel.Salutation;

				if (!string.IsNullOrWhiteSpace(viewModel.Address?.FormattedAddress))
				{
					if (user.AddressId != null)
					{
						user.Address.City = viewModel.Address.City;
						user.Address.PostalCode = viewModel.Address.PostalCode;
						user.Address.Street = viewModel.Address.Street;
						user.Address.Number = viewModel.Address.Number;
						user.Address.Latitude = double.Parse(viewModel.Address.Latitude.Replace(".",","));
						user.Address.Longitude = double.Parse(viewModel.Address.Longitude.Replace(".", ","));
						user.Address.FormattedAddress = viewModel.Address.FormattedAddress;
					}
					else
					{
						user.Address = new Address
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
					if (user.AddressId != null)
					{
						_userRepository.Delete(user.Address);
						user.AddressId = null;
					}
				}
				_userRepository.UnitOfWork.SaveChanges();
			}
			else
			{
				viewModel.GenderItems = InitGenderSelectListItems();
				viewModel.LanguageItems = InitLanguageSelectListItems();

				return View(Mvc.View.User.ProfileEdit, viewModel);
			}

			return RedirectToAction(Mvc.Controller.User.Profile, Mvc.Controller.User.Name);
		}

		private void InitDetailsViewModel(UserDetailsViewModel viewModel, UserDetails details)
		{
			InitBaseDetailViewModel(details.Detail, viewModel.Detail);
			InitBaseDetailEntityStateChange(viewModel.Detail);
			ViewData[Mvc.ViewData.Controller] = Mvc.Controller.User.Name;
			viewModel.Detail.FormattedName = $"{details.Detail.LastName} {details.Detail.FirstName} {details.Detail.MiddleName}";
			viewModel.Detail.Gender = GetGenderName(viewModel.Detail.GenderId);
			viewModel.Detail.Language = GetLanguageName(viewModel.Detail.LanguageId);
		}

		private void InitListViewModel(UserItemsViewModel viewModel, UserItems list)
		{
			foreach (var item in viewModel.Items)
			{
				InitBaseItemViewModel(item);
			}
		}

		private void InitUserFilterViewModel(ref UserFilterViewModel filterViewModel, bool isList)
		{
			if (isList)
				InitBaseFilterViewModel(filterViewModel);
		}
	}
}