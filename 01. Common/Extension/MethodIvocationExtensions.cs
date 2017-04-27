using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Text;

namespace RP.Common.Extension
{
	public static class MethodIvocationExtensions
	{
		public static string GetDescription(this IMethodInvocation input)
		{
			return $"{input.GetName()}<{string.Join<Type>(",", input.MethodBase.GetGenericArguments())}>({input.GetParams()})";
		}

		public static string GetName(this IMethodInvocation input)
		{
			return $"{input.Target.GetType().FullName}.{input.MethodBase.Name}";
		}

		public static string GetParams(this IMethodInvocation input)
		{
			StringBuilder paramsString = new StringBuilder();
			for (int i = 0; i < input.Arguments.Count; i++)
			{
				paramsString.Append(input.Arguments.ParameterName(i));
				paramsString.Append(':');
				paramsString.Append(input.Arguments[i]);
				if (i != input.Arguments.Count - 1)
					paramsString.Append(',');
			}
			return paramsString.ToString();
		}
	}
}