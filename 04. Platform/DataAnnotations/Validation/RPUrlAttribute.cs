using RP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web.Mvc;

namespace RP.Platform.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public class RPUrlAttribute : BaseValidationAttribute, IClientValidatable
	{
		public RPUrlAttribute() : this(Dom.Translation.Validation.WrongUrl)
		{
		}

		public RPUrlAttribute(int errorMessageCode)
		{
			ErrorMessageCode = errorMessageCode;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			bool isValid = false;
			string stringValue = null;

			if (value != null)
				stringValue = value.ToString();

			if (!string.IsNullOrWhiteSpace(stringValue))
			{
				try
				{
					if (!string.IsNullOrEmpty(stringValue))
					{
						if (!stringValue.StartsWith("http"))
							stringValue = "http://" + stringValue;

						HttpWebRequest request = WebRequest.Create(stringValue) as HttpWebRequest;
						if (request != null)
						{
							request.Timeout = 2000;
							//Get only the header information -- no need to download any content
							request.Method = WebRequestMethods.Http.Head;

							HttpWebResponse response = request.GetResponse() as HttpWebResponse;
							if (response != null)
								isValid = response.StatusCode == HttpStatusCode.OK;
						}
					}
				}
				catch (Exception)
				{
					isValid = false;
				}
			}
			else
			{
				//Url is required field, but when it's empty we don't validate
				isValid = true;
			}

			return GetValidationResult(isValid);
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			yield return new RPUrlRule(GetErrorMessage());
		}
	}

	public class RPUrlRule : ModelClientValidationRule
	{
		private const string ValidationTypeKey = "rpurl";

		public RPUrlRule(string errorMessage)
		{
			ErrorMessage = errorMessage;
			ValidationType = ValidationTypeKey;
		}
	}
}