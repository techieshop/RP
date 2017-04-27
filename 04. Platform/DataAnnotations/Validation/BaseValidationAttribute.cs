using RP.Platform.Context;
using System.ComponentModel.DataAnnotations;

namespace RP.Platform.DataAnnotations
{
	public class BaseValidationAttribute : ValidationAttribute
	{
		public int ErrorMessageCode { get; set; }

		public virtual string GetErrorMessage()
		{
			return StyleContext.Current.GetTranslation(ErrorMessageCode);
		}

		protected ValidationResult GetValidationResult(bool isValid)
		{
			return isValid
				? ValidationResult.Success
				: new ValidationResult(GetErrorMessage());
		}
	}
}