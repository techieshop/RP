using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RP.Platform.Binder
{
	public class IntCollectionModelBinder : DefaultModelBinder
	{
		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
			IList<int> result = null;
			if (value != null && !string.IsNullOrEmpty(value.AttemptedValue))
			{
				var stringValues = value.AttemptedValue.Split(',');
				if (stringValues != null && stringValues.Any())
				{
					result = new List<int>();
					foreach (var stringValue in stringValues)
					{
						int intValue;
						if (int.TryParse(stringValue, out intValue))
							result.Add(intValue);
					}
				}
			}
			return result;
		}
	}
}