using RP.Platform.Context;
using RP.Platform.Manager;
using System.Web.Mvc;

namespace RP.Platform.ViewModel
{
	public class BaseInputViewModel
	{
		protected string _helpDescriptionMessage;
		protected string _helpTitleMessage;
		protected bool _isRequired;
		protected string _labelMessage;
		protected ModelMetadata _modelMetadata;
		protected string _placeholderMessage;
		protected bool _showValidationMessage;
		protected MvcHtmlString _validationMessage;
		protected IStyleContext StyleContext;

		public BaseInputViewModel(ModelMetadata metadata)
		{
			StyleContext = Context.StyleContext.Current;
			if (metadata != null)
			{
				_modelMetadata = metadata;
				_isRequired = ModelMetadata.IsRequired;

				if (metadata.AdditionalValues.ContainsKey(Mvc.ModelMetadata.LabelMessage))
					LabelMessage = metadata.AdditionalValues[Mvc.ModelMetadata.LabelMessage] as string;

				if (metadata.AdditionalValues.ContainsKey(Mvc.ModelMetadata.PlaceholderMessage))
					PlaceholderMessage = metadata.AdditionalValues[Mvc.ModelMetadata.PlaceholderMessage] as string;

				if (metadata.AdditionalValues.ContainsKey(Mvc.ModelMetadata.HelpTitleMessage))
					HelpTitleMessage = metadata.AdditionalValues[Mvc.ModelMetadata.HelpTitleMessage] as string;

				if (metadata.AdditionalValues.ContainsKey(Mvc.ModelMetadata.HelpDescriptionMessage))
					HelpDescriptionMessage = metadata.AdditionalValues[Mvc.ModelMetadata.HelpDescriptionMessage] as string;

				if (metadata.AdditionalValues.ContainsKey(Mvc.ModelMetadata.ValidationMessage))
					_validationMessage = metadata.AdditionalValues[Mvc.ModelMetadata.ValidationMessage] as MvcHtmlString;

				_showValidationMessage = true;
				if (metadata.AdditionalValues.ContainsKey(Mvc.ModelMetadata.ShowValidationMessage))
					_showValidationMessage = (bool)metadata.AdditionalValues[Mvc.ModelMetadata.ShowValidationMessage];
			}
		}

		public string HelpDescriptionMessage
		{
			get
			{
				return _helpDescriptionMessage;
			}
			set
			{
				_helpDescriptionMessage = value;
			}
		}

		public string HelpTitleMessage
		{
			get
			{
				return _helpTitleMessage;
			}
			set
			{
				_helpTitleMessage = value;
			}
		}

		public MvcHtmlString Input { get; set; }

		public bool IsRequired => _isRequired;

		public string LabelMessage
		{
			get
			{
				return _labelMessage;
			}
			set
			{
				_labelMessage = value;
			}
		}

		public object Model
		{
			get
			{
				object model = null;
				if (_modelMetadata != null)
					model = _modelMetadata.Model;

				return model;
			}
		}

		public ModelMetadata ModelMetadata => _modelMetadata;

		public string PlaceholderMessage
		{
			get
			{
				return _placeholderMessage;
			}
			set
			{
				_placeholderMessage = value;
			}
		}

		public bool ShowValidationMessage => _showValidationMessage;
		public MvcHtmlString ValidationMessage => _validationMessage;
	}
}