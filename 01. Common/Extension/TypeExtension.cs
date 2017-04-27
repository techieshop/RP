using RP.Common.Attribute;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RP.Common.Extension
{
	public static class TypeExtension
	{
		public static object GetDefaultValue(this Type type)
		{
			object defaultValue = null;
			if (type.IsValueType)
				defaultValue = Activator.CreateInstance(type);

			return defaultValue;
		}

		public static MetadataAttribute GetMetadata(this Enum en)
		{
			FieldInfo fieldInfo = en.GetType().GetField(en.ToString());
			object[] attributes = fieldInfo.GetCustomAttributes(typeof(MetadataAttribute), false);

			MetadataAttribute metadata;
			if (!attributes.IsNullOrEmpty())
				metadata = (MetadataAttribute)attributes[0];
			else
				metadata = new MetadataAttribute { Value = fieldInfo.ToString() };

			return metadata;
		}

		public static MetadataAttribute GetMetadata(this PropertyInfo propertyInfo)
		{
			object[] attributes = propertyInfo.GetCustomAttributes(typeof(MetadataAttribute), false);

			MetadataAttribute metadata;
			if (!attributes.IsNullOrEmpty())
				metadata = (MetadataAttribute)attributes[0];
			else
				metadata = new MetadataAttribute { Value = propertyInfo.Name };

			return metadata;
		}

		public static string GetPropertyName<TModel, TProperty>(this TModel obj, Expression<Func<TModel, TProperty>> expression)
		{
			return ((MemberExpression)expression.Body).Member.Name;
		}

		public static string GetTypeName(this object obj)
		{
			Type objectType = obj.GetType();
			string typeName = objectType.Name;
			if (objectType.IsGenericType)
			{
				typeName = $"{objectType.Name} {objectType.GetGenericArguments().Select(item => item.Name).Join(",")}";
			}
			return typeName;
		}

		public static bool IsEmpty(this object obj, bool useMetadata = false)
		{
			bool isDefault = true;
			PropertyInfo[] properties = obj.GetType().GetProperties();
			if (!properties.IsNullOrEmpty())
			{
				foreach (var property in properties)
				{
					object value = property.GetValue(obj, null);
					bool isIgnored = false;
					if (useMetadata)
					{
						MetadataAttribute metadata = GetMetadata(property);
						isIgnored = metadata.IsIgnored;
					}
					if (!isIgnored && value != null && !value.Equals(property.PropertyType.GetDefaultValue()))
					{
						if (property.IsCollection())
						{
							var collection = (IEnumerable)value;
							if (!collection.Any())
								continue;
						}
						if (property.PropertyType == typeof(string))
						{
							var str = (string)value;
							if (string.IsNullOrEmpty(str))
								continue;
						}
						isDefault = false;
						break;
					}
				}
			}
			return isDefault;
		}
	}
}