using Microsoft.Practices.Unity;
using RP.Common.Extension;
using RP.Common.Manager;
using RP.DAL.Repository;
using RP.Model;
using RP.Platform.Context;
using RP.Platform.Manager;
using RP.Platform.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;

namespace RP.Platform.Controller
{
	public class BaseController : System.Web.Mvc.Controller
	{
		protected IDomainValueRepository DomainValueRepository;
		protected IEntityCacheRepository EntityCacheRepository;

		public IEntityContext EntityContext { get; private set; }

		public IStyleContext StyleContext { get; private set; }

		public IUserContext UserContext { get; private set; }

		[NonAction]
		public ActionResult ForbiddenResult()
		{
			Response.TrySkipIisCustomErrors = true;
			Response.StatusCode = (int)HttpStatusCode.Forbidden;

			return View(Mvc.View.Error.ErrorPage, new ErrorViewModel
			{
				Title = StyleContext.GetTranslation(Dom.Translation.Error.Forbidden),
				Description = StyleContext.GetTranslation(Dom.Translation.Error.Forbidden)
			});
		}

		[InjectionMethod]
		public void InitContext(
			IDomainValueRepository domainValueRepository,
			IEntityCacheRepository entityCacheRepository,
			IEntityContext entityContext,
			IStyleContext styleContext,
			IUserContext userContext)
		{
			DomainValueRepository = domainValueRepository;
			EntityCacheRepository = entityCacheRepository;
			EntityContext = entityContext;
			StyleContext = styleContext;
			UserContext = userContext;
		}

		[NonAction]
		public ActionResult LoginWithReturnResult()
		{
			Response.TrySkipIisCustomErrors = true;
			Response.StatusCode = (int)HttpStatusCode.Forbidden;

			var routes = new RouteValueDictionary
			{
				{"redirectUrl", Request.Url.GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped)}
			};

			return RedirectToAction(Mvc.Controller.User.LogIn, Mvc.Controller.User.Name, routes);
		}

		public ActionResult NotFoundResult()
		{
			Response.TrySkipIisCustomErrors = true;
			Response.StatusCode = (int)HttpStatusCode.NotFound;
			return View(Mvc.View.Error.ErrorPage, new ErrorViewModel
			{
				Title = StyleContext.GetTranslation(Dom.Translation.Error.PageNotFound),
				Description = StyleContext.GetTranslation(Dom.Translation.Error.ErrorMessage)
			});
		}

		public ActionResult RedirectHome()
		{
			return RedirectToAction(Mvc.Controller.Home.Dashboard, Mvc.Controller.Home.Name);
		}

		public ActionResult RedirectResponseResult(string url)
		{
			return Redirect(url);
		}

		protected void AddModelError<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression, string message)
		{
			ModelState.AddModelError(ExpressionHelper.GetExpressionText(expression), message);
		}

		protected void AddModelError<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression, int nameCode)
		{
			AddModelError(model, expression, StyleContext.GetTranslation(nameCode));
		}

		protected string GetCountryTranslation(int? countryId)
		{
			return countryId == null
				? string.Empty
				: StyleContext.GetTranslation(DomainValueRepository.GetCountry(countryId.Value).NameCode);
		}

		protected string GetGenderName(int? genderId)
		{
			switch (genderId)
			{
				case Dom.Gender.Male:
					return StyleContext.GetTranslation(Dom.Translation.Gender.Male);

				case Dom.Gender.Female:
					return StyleContext.GetTranslation(Dom.Translation.Gender.Female);

				default:
					return StyleContext.GetTranslation(Dom.Translation.Gender.NotSpecified);
			}
		}

		protected string GetLanguageName(int? languageId)
		{
			switch (languageId)
			{
				case Dom.Language.Ukraine:
					return StyleContext.GetTranslation(Dom.Translation.Language.Ukraine);

				case Dom.Language.English:
					return StyleContext.GetTranslation(Dom.Translation.Language.English);

				default:
					return StyleContext.GetTranslation(Dom.Translation.Language.System);
			}
		}

		protected string GetPdfHtml(string viewName, string masterName, object model)
		{
			string result;
			ViewData.Model = model;
			using (var stringWriter = new StringWriter())
			{
				ViewEngineResult viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, masterName);
				var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, stringWriter);
				viewResult.View.Render(viewContext, stringWriter);
				result = stringWriter.GetStringBuilder().ToString();
			}
			return result;
		}

		protected string GetRaceTypeName(int? raceTypeId)
		{
			switch (raceTypeId)
			{
				case Dom.RaceType.Single:
					return StyleContext.GetTranslation(Dom.Translation.RaceType.Single);

				case Dom.RaceType.Common:
					return StyleContext.GetTranslation(Dom.Translation.RaceType.Common);

				default:
					return "";
			}
		}

		protected string GetSeasonTypeName(int? seasonTypeId)
		{
			switch (seasonTypeId)
			{
				case Dom.SeasonType.Adults:
					return StyleContext.GetTranslation(Dom.Translation.SeasonType.Adults);

				case Dom.SeasonType.Youth:
					return StyleContext.GetTranslation(Dom.Translation.SeasonType.Youth);

				default:
					return "";
			}
		}

		protected string GetSexName(int? sexId)
		{
			switch (sexId)
			{
				case Dom.Sex.Cock:
					return StyleContext.GetTranslation(Dom.Translation.Sex.Cock);

				case Dom.Sex.Hen:
					return StyleContext.GetTranslation(Dom.Translation.Sex.Hen);

				default:
					return StyleContext.GetTranslation(Dom.Translation.Sex.NotIdentified);
			}
		}

		protected string GetSexNameNumber(int? sexId)
		{
			switch (sexId)
			{
				case Dom.Sex.Cock:
					return "1";

				case Dom.Sex.Hen:
					return "0";

				default:
					return "";
			}
		}

		protected void InitBaseDetailEntityStateChange(BaseDetailViewModel detailViewModel)
		{
			detailViewModel.HasEntityStateChangeAccess = false;
			if (detailViewModel.AccessTypeId == Dom.AccessType.ReadWrite)
			{
				IDictionary<int, string> entityTransitions = EntityContext.GetAccesibleEntityTransitionsFrom(detailViewModel.EntityStateId, detailViewModel.EntityTypeId, detailViewModel.OrganizationId);
				if (!entityTransitions.IsNullOrEmpty())
				{
					detailViewModel.EntityStateChange = new EntityStateChangeViewModel
					{
						EntityInfoId = detailViewModel.EntityInfoId,
						OrganizationId = detailViewModel.OrganizationId,
						EntityTransitions = entityTransitions
					};
					detailViewModel.HasEntityStateChangeAccess = true;
				}
			}
		}

		protected void InitBaseDetailViewModel(BaseDetail model, BaseDetailViewModel viewModel)
		{
			viewModel.EntityStateNameCode = EntityCacheRepository.GetEntityStateNameCode(model.EntityStateId);
		}

		protected void InitBaseFilterViewModel(BaseFilterViewModel filterViewModel)
		{
			if (filterViewModel.Skip == 0)
				filterViewModel.Skip = 0;
			if (filterViewModel.Take == 0)
				filterViewModel.Take = 30;
		}

		protected void InitBaseItemViewModel(BaseItemViewModel viewModel)
		{
			viewModel.EntityStateNameCode = EntityCacheRepository.GetEntityStateNameCode(viewModel.EntityStateId);
		}

		protected ICollection<SelectListItem> InitGenderSelectListItems()
		{
			return new List<SelectListItem>
			{
				new SelectListItem {
					Text = StyleContext.GetTranslation(Dom.Translation.Gender.Male),
					Value = Dom.Gender.Male.ToString()
				},
				new SelectListItem {
					Text = StyleContext.GetTranslation(Dom.Translation.Gender.Female),
					Value = Dom.Gender.Female.ToString()
				}
			};
		}

		protected ICollection<SelectListItem> InitLanguageSelectListItems()
		{
			return new List<SelectListItem>
			{
				new SelectListItem
				{
					Text = StyleContext.GetTranslation(Dom.Translation.Language.Ukraine),
					Value = Dom.Language.Ukraine.ToString()
				},
				new SelectListItem
				{
					Text = StyleContext.GetTranslation(Dom.Translation.Language.English),
					Value = Dom.Language.English.ToString()
				}
			};
		}

		protected ICollection<SelectListItem> InitOrganizationTypeSelectedListItems()
		{
			return new List<SelectListItem>
			{
				new SelectListItem
				{
					Value = Dom.OrganizationType.Country.ToString(),
					Text = StyleContext.GetTranslation(Dom.Translation.OrganizationType.Country)
				},
				new SelectListItem
				{
					Value = Dom.OrganizationType.Region.ToString(),
					Text = StyleContext.GetTranslation(Dom.Translation.OrganizationType.Region)
				},
				new SelectListItem
				{
					Value = Dom.OrganizationType.Club.ToString(),
					Text = StyleContext.GetTranslation(Dom.Translation.OrganizationType.Club)
				}
			};
		}

		protected ICollection<SelectListItem> InitRaceTypeSelectListItems()
		{
			return new List<SelectListItem>
			{
				new SelectListItem {
					Text = StyleContext.GetTranslation(Dom.Translation.RaceType.Single),
					Value = Dom.RaceType.Single.ToString()
				},
				new SelectListItem {
					Text = StyleContext.GetTranslation(Dom.Translation.RaceType.Common),
					Value = Dom.RaceType.Common.ToString()
				}
			};
		}

		protected ICollection<SelectListItem> InitSeasonTypeSelectListItems()
		{
			return new List<SelectListItem>
			{
				new SelectListItem {
					Text = StyleContext.GetTranslation(Dom.Translation.SeasonType.Adults),
					Value = Dom.SeasonType.Adults.ToString()
				},
				new SelectListItem {
					Text = StyleContext.GetTranslation(Dom.Translation.SeasonType.Youth),
					Value = Dom.SeasonType.Youth.ToString()
				}
			};
		}

		protected ICollection<SelectListItem> InitSexSelectListItems()
		{
			return new List<SelectListItem>
			{
				new SelectListItem {
					Text = StyleContext.GetTranslation(Dom.Translation.Sex.Cock),
					Value = Dom.Sex.Cock.ToString()
				},
				new SelectListItem {
					Text = StyleContext.GetTranslation(Dom.Translation.Sex.Hen),
					Value = Dom.Sex.Hen.ToString()
				}
			};
		}

		protected ActionResult PdfView(string fileName, string viewName, string masterName, object model)
		{
			string html = GetPdfHtml(viewName, masterName, model);
			byte[] file = FileManager.ConvertHtmlToPdf(html);
			return File(file, Dom.ContentType.PdfContentType, Path.ChangeExtension(fileName, Dom.FileFormat.Pdf));
		}
	}
}