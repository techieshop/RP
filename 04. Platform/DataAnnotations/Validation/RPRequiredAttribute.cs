using RP.Common.Extension;
using RP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace RP.Platform.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public class RPRequiredAttribute : BaseValidationAttribute, IClientValidatable
	{
		public RPRequiredAttribute()
			: this(Dom.Translation.Validation.Required) { }

		public RPRequiredAttribute(int errorMessageCode)
		{
			ErrorMessageCode = errorMessageCode;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			bool isValid = (value != null && !value.Equals(value.GetType().GetDefaultValue()));

			if (isValid && value is string)
			{
				string stringValue = (string)value;
				if (string.IsNullOrWhiteSpace(stringValue))
				{
					isValid = false;
				}
			}

			return GetValidationResult(isValid);
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			yield return new RPRequiredRule(GetErrorMessage());
		}
	}

	public class RPRequiredRule : ModelClientValidationRule
	{
		private const string ValidationTypeKey = "rprequired";

		public RPRequiredRule(string errorMessage)
		{
			ErrorMessage = errorMessage;
			ValidationType = ValidationTypeKey;
		}
	}
}