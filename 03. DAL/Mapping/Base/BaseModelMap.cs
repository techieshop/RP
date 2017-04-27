using RP.Model;
using System.Data.Entity.ModelConfiguration;

namespace RP.DAL.Mapping
{
	public class BaseModelMap<TEntity> : EntityTypeConfiguration<TEntity>
		where TEntity : BaseModel
	{
		public BaseModelMap()
		{
			HasKey(t => t.Id);
		}
	}
}