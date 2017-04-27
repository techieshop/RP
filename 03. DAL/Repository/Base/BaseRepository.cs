using RP.Common.Extension;
using RP.DAL.DBContext;
using RP.DAL.UnitOfWork;
using RP.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace RP.DAL.Repository
{
	public class BaseRepository<TContext> : IRepository<TContext>
		where TContext : BaseDbContext<TContext>
	{
		protected IUnitOfWork<TContext> _unitOfWork;

		public BaseRepository(IUnitOfWork<TContext> unitOfWork)
		{
			if (unitOfWork == null)
				throw new ArgumentNullException(nameof(unitOfWork));

			_unitOfWork = unitOfWork;
		}

		public TEntity Get<TEntity>(int id) where TEntity : BaseModel
		{
			return GetQuery<TEntity>().FirstOrDefault(t => t.Id == id);
		}

		public TEntity GetEntity<TEntity>(int id, params Expression<Func<TEntity, object>>[] includes)
			where TEntity : BaseEntity
		{
			var entity = GetQuery<TEntity>().Where(t => t.Id == id);

			if (!includes.IsNullOrEmpty())
				entity = includes.Aggregate(entity, (query, include) => query.Include(include));

			return entity.SingleOrDefault();
		}

		public TEntity GetEntityByEntityInfoId<TEntity>(int entityInfoId, params Expression<Func<TEntity, object>>[] includes)
			where TEntity : BaseEntity
		{
			var entity = GetQuery<TEntity>().Where(t => t.EntityInfoId == entityInfoId);

			if (!includes.IsNullOrEmpty())
				entity = includes.Aggregate(entity, (query, include) => query.Include(include));

			return entity.SingleOrDefault();
		}

		public virtual void Add<TEntity>(TEntity entity) where TEntity : class
		{
			if (entity == null)
				throw new ArgumentNullException(nameof(entity));

			_unitOfWork.DbContext.Set<TEntity>().Add(entity);
		}

		public virtual void AddOrUpdate<TEntity>(TEntity entity) where TEntity : BaseModel
		{
			if (entity == null)
				throw new ArgumentNullException(nameof(entity));

			if (entity.Id == default(int))
			{
				_unitOfWork.DbContext.Set<TEntity>().Add(entity);
			}
			else
			{
				var entityName = GetEntityName<TEntity>();
				object originalItem;
				EntityKey key = _unitOfWork.DbContext.ObjectContext.CreateEntityKey(entityName, entity);
				if (_unitOfWork.DbContext.ObjectContext.TryGetObjectByKey(key, out originalItem))
				{
					_unitOfWork.DbContext.ObjectContext.ApplyCurrentValues(key.EntitySetName, entity);
				}
			}
		}

		public void Delete<TEntity>(TEntity entity)
			where TEntity : class
		{
			_unitOfWork.DbContext.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
		}

		public IUnitOfWork<TContext> UnitOfWork => _unitOfWork;

		protected virtual EntityKey GetEntityKey<TEntity>(object keyValue) where TEntity : BaseModel
		{
			var entityName = GetEntityName<TEntity>();
			var objectSet = _unitOfWork.DbContext.ObjectContext.CreateObjectSet<TEntity>();
			var keyPropertyName = objectSet.EntitySet.ElementType.KeyMembers[0].ToString();
			var entityKey = new EntityKey(entityName, new[] { new EntityKeyMember(keyPropertyName, keyValue) });
			return entityKey;
		}

		protected virtual string GetEntityName<TEntity>() where TEntity : BaseModel
		{
			string containerName = _unitOfWork.DbContext.ObjectContext.DefaultContainerName;
			string setName = _unitOfWork.DbContext.ObjectContext.CreateObjectSet<TEntity>().EntitySet.Name;
			return $"{containerName}.{setName}";
		}

		protected virtual IQueryable GetQuery(Type type)
		{
			return _unitOfWork.DbContext.Set(type);
		}

		protected virtual IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class
		{
			return _unitOfWork.DbContext.Set<TEntity>();
		}

		protected virtual ICollection<TEntity> ExecuteSp<TEntity>(string spName)
		{
			return ExecuteSp<TEntity>(spName, null);
		}

		protected virtual IList<TEntity> ExecuteSp<TEntity>(string spName, object parameters)
		{
			string sql;
			object[] sqlParameters = GetSqlQueryParams(spName, parameters, out sql);
			return _unitOfWork.DbContext.Database.SqlQuery<TEntity>(sql, sqlParameters).ToList();
		}

		protected virtual void ExecuteSp(string spName, object parameters)
		{
			string sql;
			object[] sqlParameters = GetSqlQueryParams(spName, parameters, out sql);
			_unitOfWork.DbContext.Database.ExecuteSqlCommand(sql, sqlParameters);
		}

		protected virtual IQueryable<TEntity> ExecuteQuerySp<TEntity>(string spName, object parameters)
			where TEntity : BaseModel
		{
			string sql;
			object[] sqlParameters = GetSqlQueryParams(spName, parameters, out sql);
			return _unitOfWork.DbContext.Set<TEntity>().SqlQuery(sql, sqlParameters).AsQueryable();
		}

		protected virtual TResult ExecuteMultiResultSetSp<TResult>(string spName)
			where TResult : class, new()
		{
			return ExecuteMultiResultSetSp<TResult>(spName, null);
		}

		protected virtual TResult ExecuteMultiResultSetSp<TResult>(string spName, object parameters)
			where TResult : class, new()
		{
			TResult result = new TResult();
			EntityConnection entityConnection = (EntityConnection)_unitOfWork.DbContext.ObjectContext.Connection;
			SqlConnection sqlConnection = (SqlConnection)entityConnection.StoreConnection;

			using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
			{
				string sql;
				SqlParameter[] sqlParameters = GetSqlQueryParams(spName, parameters, out sql);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = spName;
				if (sqlParameters != null && sqlParameters.Any())
					sqlCommand.Parameters.AddRange(sqlParameters);

				sqlConnection.Open();
				try
				{
					using (SqlDataReader reader = sqlCommand.ExecuteReader())
					{
						IEnumerable<PropertyInfo> properties = typeof(TResult)
							.GetProperties(BindingFlags.Public | BindingFlags.Instance)
							.Where(t => t.CanWrite);
						var propertyInfos = properties as IList<PropertyInfo> ?? properties.ToList();
						if (propertyInfos.Any())
						{
							properties = propertyInfos.OrderBy(t => t.GetMetadata().Order);
							foreach (var property in properties)
							{
								Type type = property.PropertyType;
								if (property.IsCollection())
									type = property.PropertyType.GetGenericArguments().SingleOrDefault();

								if (type != null)
								{
									var items = GetType()
										.GetMethod("Translate", BindingFlags.Instance | BindingFlags.NonPublic)
										.MakeGenericMethod(type)
										.Invoke(this, new object[] { reader })
										as IEnumerable;
									if (items != null)
									{
										property.SetValue(result, property.IsCollection() ? items : items.First(), null);
									}
								}

								if (!reader.NextResult())
									break;
							}
						}
					}
				}
				finally
				{
					sqlConnection.Close();
				}
			}
			return result;
		}

		protected ICollection<T> Translate<T>(SqlDataReader dataReader)
		{
			List<T> items = new List<T>();
			Type type = typeof(T);
			while (dataReader.Read())
			{
				T item;
				if (dataReader.FieldCount == 1
					&& (type.IsPrimitive
					|| type == typeof(string)
					|| (type.IsGenericType
					&& type.GetGenericTypeDefinition() == typeof(Nullable<>)
					&& type.GetGenericArguments()[0].IsPrimitive)))
				{
					object value = dataReader.GetValue(0);
					if (value is DBNull)
						value = null;
					item = (T)value;
				}
				else
				{
					item = Activator.CreateInstance<T>();
					for (int i = 0; i < dataReader.FieldCount; i++)
					{
						string propertyName = dataReader.GetName(i);
						PropertyInfo property = type.GetProperty(propertyName);
						if (property != null)
						{
							object value = dataReader.GetValue(i);
							if (value is DBNull)
								value = null;
							property.SetValue(item, value, null);
						}
					}
				}
				items.Add(item);
			}
			return items;
		}

		protected virtual SqlParameter[] GetSqlQueryParams(
			string spName, object parameters, out string sql)
		{
			if (string.IsNullOrEmpty(spName))
				throw new ArgumentException("Is null or empty", nameof(spName));

			StringBuilder query = new StringBuilder(spName);
			var sqlParameters = new List<SqlParameter>();
			PropertyInfo[] properties = parameters?.GetType().GetProperties();
			if (properties != null)
			{
				foreach (var property in properties)
				{
					object value = property.GetValue(parameters, null);
					if (property.IsCollection())
					{
						Type genericType = property.PropertyType.GetGenericArguments().SingleOrDefault();
						PropertyInfo[] genericProperties = genericType?.GetProperties();
						if (genericProperties != null && genericProperties.Any())
						{
							var dataTable = new DataTable();
							foreach (var genericProperty in genericProperties)
							{
								if (!genericProperty.IsCollection())
									dataTable.Columns.Add(genericProperty.Name);
							}
							IEnumerable tableProperty = value as IEnumerable;
							if (tableProperty != null)
							{
								foreach (var item in tableProperty)
								{
									dataTable.Rows.Add((from genericProperty in genericProperties where !genericProperty.IsCollection() select genericProperty.GetValue(item, null)).ToArray());
								}
							}
							query.AppendFormat(" @{0},", property.Name);
							var sqlParameter = new SqlParameter(property.Name, SqlDbType.Structured)
							{
								TypeName = $"udt.{genericType.Name}",
								Value = dataTable
							};
							sqlParameters.Add(sqlParameter);
						}
					}
					else
					{
						value = value ?? DBNull.Value;
						query.AppendFormat(" @{0},", property.Name);
						sqlParameters.Add(new SqlParameter(
							property.Name,
							value
							));
					}
				}
			}
			sql = query.ToString().TrimEnd(',');
			return sqlParameters.ToArray();
		}
	}
}