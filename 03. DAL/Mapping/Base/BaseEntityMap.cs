using RP.Model;

namespace RP.DAL.Mapping
{
	public class BaseEntityMap<TEntity> : BaseModelMap<TEntity>
		where TEntity : BaseEntity
	{
		public BaseEntityMap()
		{
			Property(t => t.EntityInfoId).IsRequired();
			HasRequired(t => t.EntityInfo).WithMany().HasForeignKey(t => t.EntityInfoId);
		}
	}
}