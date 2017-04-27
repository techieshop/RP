using RP.Model;
using RP.Platform.Manager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace RP.Platform.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public class RPMaxLengthAttribute : BaseValidationAttribute, IClientValidatable, IMetadataAware
	{
		private readonly int _maxLength;

		public RPMaxLengthAttribute(int maxLength)
			: this(maxLength, Dom.Translation.Validation.MaxLength) { }

		public RPMaxLengthAttribute(int maxLength, int errorMessageCode)
		{
			_maxLength = maxLength;
			ErrorMessageCode = errorMessageCode;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			bool isValid = !(value != null && value.ToString().Length > _maxLength);

			return GetValidationResult(isValid);
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			yield return new RPMaxLengthRule(_maxLength, GetErrorMessage());
		}

		public void OnMetadataCreated(ModelMetadata metadata)
		{
			metadata.AdditionalValues[Mvc.ModelMetadata.MaxLength] = _maxLength;
		}
	}

	public class RPMaxLengthRule : ModelClientValidationRule
	{
		private const string ValidationTypeKey = "rpmaxlength";
		private const string MaxLengthKey = "maxlength";

		public RPMaxLengthRule(int maxLength, string errorMessage)
		{
			ErrorMessage = errorMessage;
			ValidationType = ValidationTypeKey;

			ValidationParameters[MaxLengthKey] = maxLength;
		}
	}
}