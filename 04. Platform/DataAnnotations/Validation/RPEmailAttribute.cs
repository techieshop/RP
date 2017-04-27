using RP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Web.Mvc;

namespace RP.Platform.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public class RPEmailAttribute : BaseValidationAttribute, IClientValidatable
	{
		public RPEmailAttribute() : this(Dom.Translation.Validation.WrongEmail)
		{
		}

		public RPEmailAttribute(int errorMessageCode)
		{
			ErrorMessageCode = errorMessageCode;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			bool isValid = true;
			string stringValue = null;

			if (value != null)
				stringValue = value.ToString();

			if (!string.IsNullOrEmpty(stringValue))
			{
				try
				{
					new MailAddress(stringValue);
				}
				catch
				{
					isValid = false;
				}
			}

			return GetValidationResult(isValid);
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			yield return new RPEmailRule(GetErrorMessage());
		}
	}

	public class RPEmailRule : ModelClientValidationRule
	{
		private const string ValidationTypeKey = "rpemail";

		public RPEmailRule(string errorMessage)
		{
			ErrorMessage = errorMessage;
			ValidationType = ValidationTypeKey;
		}
	}
}