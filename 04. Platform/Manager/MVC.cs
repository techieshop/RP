namespace RP.Platform.Manager
{
	public static class Mvc
	{
		public static class Controller
		{
			public static class Entity
			{
				public const string EntityStateChange = "entitystatechange";
				public const string Name = "entity";
			}

			public static class Home
			{
				public const string Dashboard = "dashboard";
				public const string Name = "home";
			}

			public static class Member
			{
				public const string Add = "add";
				public const string Details = "details";
				public const string Edit = "edit";
				public const string List = "list";
				public const string Name = "member";
			}

			public static class Organization
			{
				public const string Add = "add";
				public const string Details = "details";
				public const string Edit = "edit";
				public const string List = "list";
				public const string Name = "organization";
				public const string Organizations = "organizations";
			}

			public static class Pigeon
			{
				public const string Add = "add";
				public const string Details = "details";
				public const string Edit = "edit";
				public const string List = "list";
				public const string Name = "pigeon";
			}

			public static class Point
			{
				public const string Add = "add";
				public const string Details = "details";
				public const string Edit = "edit";
				public const string List = "list";
				public const string Name = "point";
			}

			public static class Public
			{
				public const string MemberDetail = "memberdetails";
				public const string MemberList = "memberlist";
				public const string Name = "public";
				public const string OrganizationDetail = "organizationdetails";
				public const string OrganizationList = "organizationlist";
				public const string PigeonDetail = "pigeondetails";
				public const string PigeonList = "pigeonlist";
			}

			public static class Race
			{
				public const string Add = "add";
				public const string CalculateDistances = "calculatedistances";
				public const string CalculateResult = "calculateresult";
				public const string Details = "details";
				public const string Edit = "edit";
				public const string List = "list";
				public const string Name = "race";
				public const string Points = "points";
				public const string ReturnTime = "returntime";
				public const string Seasons = "seasons";
			}

			public static class Registration
			{
				public const string Add = "add";
				public const string ChangeIsRead = "changeisread";
				public const string Delete = "delete";
				public const string Details = "details";
				public const string Edit = "edit";
				public const string List = "list";
				public const string Name = "registration";
			}

			public static class Result
			{
				public const string Details = "details";
				public const string DownloadCommandPdfFile = "downloadcommandpdffile";
				public const string DownloadMasterPdfFile = "downloadmasterpdffile";
				public const string DownloadTimePdfFile = "downloadtimepdffile";
				public const string DownloadTitlePdfFile = "downloadtitlepdffile";
				public const string List = "list";
				public const string Name = "result";
			}

			public static class Role
			{
				public const string Add = "add";
				public const string Details = "details";
				public const string Edit = "edit";
				public const string List = "list";
				public const string Name = "role";
			}

			public static class Season
			{
				public const string Add = "add";
				public const string Details = "details";
				public const string Edit = "edit";
				public const string List = "list";
				public const string Name = "season";
			}

			public static class Setting
			{
				public const string List = "list";
				public const string Name = "setting";
			}

			public static class User
			{
				public const string Add = "add";
				public const string Details = "details";
				public const string Edit = "edit";
				public const string List = "list";
				public const string LogIn = "login";
				public const string LogOut = "logout";
				public const string Name = "user";
				public const string Profile = "profile";
				public const string ProfileEdit = "profileedit";
			}
		}

		public static class ModelMetadata
		{
			public const string HelpDescriptionMessage = "HelpDescriptionMessage";
			public const string HelpTitleMessage = "HelpTitleMessage";
			public const string LabelMessage = "LabelMessage";
			public const string MaxLength = "MaxLength";
			public const string PlaceholderMessage = "PlaceholderMessage";
			public const string ShowValidationMessage = "ShowValidationMessage";
			public const string ValidationMessage = "ValidationMessage";
		}

		public static class View
		{
			public static class Alerts
			{
				public const string Alert = "Alert/Alert";
			}

			public static class Entity
			{
				public const string EntityStateChange = "Entity/_EntityStateChange";
			}

			public static class Error
			{
				public const string ErrorPage = "Error/Error";
			}

			public static class Home
			{
				public const string Dashboard = "Dashboard";
			}

			public static class Member
			{
				public const string Add = "Add";
				public const string Details = "Details";
				public const string Edit = "Edit";
				public const string List = "List";
				public const string ListItems = "_List";
			}

			public static class Organization
			{
				public const string Add = "Add";
				public const string Details = "Details";
				public const string Edit = "Edit";
				public const string List = "List";
				public const string ListItems = "_List";
			}

			public static class Pigeon
			{
				public const string Add = "Add";
				public const string Details = "Details";
				public const string Edit = "Edit";
				public const string List = "List";
				public const string ListItems = "_List";
			}

			public static class Point
			{
				public const string Add = "Add";
				public const string Details = "Details";
				public const string Edit = "Edit";
				public const string List = "List";
				public const string ListItems = "_List";
			}

			public static class Race
			{
				public const string Add = "Add";
				public const string Details = "Details";
				public const string Edit = "Edit";
				public const string List = "List";
				public const string ListItems = "_List";
				public const string ReturnTime = "ReturnTime";
			}

			public static class Registration
			{
				public const string Add = "Registration/Add";
				public const string List = "List";
				public const string ListItems = "_List";
			}

			public static class Result
			{
				public const string Command = "Command";
				public const string Details = "Details";
				public const string List = "List";
				public const string Master = "Master";
				public const string Time = "Time";
				public const string Title = "Title";
			}

			public static class Role
			{
				public const string Add = "Add";
				public const string Details = "Details";
				public const string Edit = "Edit";
				public const string List = "List";
				public const string ListItems = "_List";
			}

			public static class Season
			{
				public const string Add = "Add";
				public const string Details = "Details";
				public const string Edit = "Edit";
				public const string List = "List";
				public const string ListItems = "_List";
			}

			public static class Setting
			{
				public const string List = "Setting/List";
				public const string ListItems = "Setting/_List";
			}

			public static class Shared
			{
				public const string EmptyLayout = "_EmptyLayout";
				public const string Filter = "_Filter";
				public const string Footer = "_Footer";
				public const string Header = "_Header";
				public const string Meta = "_Meta";
			}

			public static class UI
			{
				public const string DateTime = "UI/_DateTime";
				public const string DropDown = "UI/_DropDown";
				public const string GoogleMap = "UI/_GoogleMap";
				public const string RadioButton = "UI/_RadioButton";
				public const string TabsWithDateTime = "UI/_TabsWithDateTime";
				public const string TabsWithDrag = "UI/_TabsWithDrag";
				public const string TextArea = "UI/_TextArea";
				public const string TextBox = "UI/_TextBox";
			}

			public static class User
			{
				public const string Add = "Add";
				public const string Details = "Details";
				public const string Edit = "Edit";
				public const string List = "List";
				public const string ListItems = "_List";
				public const string Login = "Login";
				public const string Profile = "Profile";
				public const string ProfileEdit = "ProfileEdit";
			}
		}

		public static class ViewData
		{
			public const string Action = "action";
			public const string Controller = "controller";
		}
	}
}