using RP.Common.Extension;
using RP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Mvc;

namespace RP.Platform.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public class RPDependentRequiredAttribute : BaseValidationAttribute, IClientValidatable
	{
		protected string _dependentProperty;

		public RPDependentRequiredAttribute(string dependentProperty)
			: this(dependentProperty, Dom.Translation.Validation.Required) { }

		public RPDependentRequiredAttribute(string dependentProperty, int errorMessageCode)
		{
			ErrorMessageCode = errorMessageCode;
			_dependentProperty = dependentProperty;
		}

		public string DependentProperty => _dependentProperty;

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			bool isValid = true;

			object dependentValue = null;
			PropertyInfo dependentProperty = validationContext.ObjectType.GetProperty(DependentProperty);
			if (dependentProperty != null)
				dependentValue = dependentProperty.GetValue(validationContext.ObjectInstance, null);

			if ((dependentValue != null)
				&& (value == null || value.Equals(value.GetType().GetDefaultValue())))
			{
				isValid = false;
			}

			return GetValidationResult(isValid);
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			yield return new RPDependentRequiredRule(DependentProperty, GetErrorMessage());
		}
	}

	public class RPDependentRequiredRule : ModelClientValidationRule
	{
		private const string ValidationTypeKey = "rpdependentrequired";
		private const string DependentPropertyKey = "dependentproperty";

		public RPDependentRequiredRule(string dependentProperty, string errorMessage)
		{
			ErrorMessage = errorMessage;
			ValidationType = ValidationTypeKey;

			ValidationParameters[DependentPropertyKey] = dependentProperty;
		}
	}
}