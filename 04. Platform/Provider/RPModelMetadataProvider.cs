using RP.Platform.DataAnnotations;
using RP.Platform.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RP.Platform.Provider
{
	public class RPModelMetadataProvider : DataAnnotationsModelMetadataProvider
	{
		protected override ModelMetadata CreateMetadata(
			IEnumerable<System.Attribute> attributes,
			Type containerType,
			Func<object> modelAccessor,
			Type modelType,
			string propertyName)
		{
			var enumerable = attributes as System.Attribute[] ?? attributes.ToArray();
			var property = base.CreateMetadata(
				enumerable,
				containerType,
				modelAccessor,
				modelType,
				propertyName
			);

			if (attributes != null && enumerable.Any())
			{
				property.IsRequired = false;

				var rpRequiredAttribute = enumerable
					.SingleOrDefault(a => typeof(RPRequiredAttribute) == a.GetType() || typeof(RPDependentRequiredAttribute) == a.GetType());

				if (rpRequiredAttribute != null)
					property.IsRequired = true;

				var rpDisplayAttribute = enumerable
					.SingleOrDefault(a => typeof(RPDisplayAttribute) == a.GetType());

				var rpDisplay = rpDisplayAttribute as RPDisplayAttribute;
				if (rpDisplay != null)
				{
					property.AdditionalValues.Add(Mvc.ModelMetadata.LabelMessage, rpDisplay.LabelMessage);
					property.AdditionalValues.Add(Mvc.ModelMetadata.PlaceholderMessage, rpDisplay.PlaceholderMessage);
					property.AdditionalValues.Add(Mvc.ModelMetadata.HelpTitleMessage, rpDisplay.HelpTitleMessage);
					property.AdditionalValues.Add(Mvc.ModelMetadata.HelpDescriptionMessage, rpDisplay.HelpDescriptionMessage);
					property.AdditionalValues.Add(Mvc.ModelMetadata.ShowValidationMessage, rpDisplay.ShowValidationMessage);
				}
			}

			return property;
		}
	}
}