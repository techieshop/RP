using AutoMapper;
using RP.DAL.Repository;
using RP.Model;
using RP.Platform.Context;
using RP.Platform.Manager;
using RP.Platform.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RP.Platform.Controller
{
	public class RegistrationController : BaseController
	{
		private readonly IRegistrationRepository _registrationRepository;

		public RegistrationController(
			IRegistrationRepository registrationRepository)
		{
			_registrationRepository = registrationRepository;
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Add(RegistrationViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var registration = Mapper.Map<Registration>(viewModel);
				registration.DateTimeRequest = DateTime.Now;
				_registrationRepository.Add(registration);
				_registrationRepository.UnitOfWork.SaveChanges();
				return PartialView(Mvc.View.Alerts.Alert, new BaseAlertViewModel {
					AlertType = AlertType.Success,
					CanClose = true,
					MessageCode = Dom.Translation.Registration.ApplicationAccepted
				});
			} else {
				return PartialView(Mvc.View.Registration.Add, viewModel);
			}
			
		}

		[HttpGet]
		[Auth]
		public ActionResult ChangeIsRead(int id)
		{
			if (UserContext.User.Id == Dom.Common.UserId)
			{
				Registration registration = _registrationRepository.Get<Registration>(id);
				registration.IsRead = !registration.IsRead;
				_registrationRepository.UnitOfWork.SaveChanges();

				return RedirectToAction(Mvc.Controller.Registration.List, Mvc.Controller.Registration.Name);
			}
			return RedirectHome();
		}

		[HttpGet]
		[Auth]
		public ActionResult Delete(int id)
		{
			if (UserContext.User.Id == Dom.Common.UserId)
			{
				Registration registration = _registrationRepository.Get<Registration>(id);
				_registrationRepository.Delete(registration);
				_registrationRepository.UnitOfWork.SaveChanges();

				return RedirectToAction(Mvc.Controller.Registration.List, Mvc.Controller.Registration.Name);
			}
			return RedirectHome();
		}

		[HttpGet]
		[Auth]
		public ActionResult List(bool onlyNew = false)
		{
			if (UserContext.User.Id == Dom.Common.UserId)
			{
				ICollection<Registration> registrations = _registrationRepository.GetRegistrations(onlyNew);

				return View(Mvc.View.Registration.List, registrations);
			}
			return RedirectHome();
		}
	}
}