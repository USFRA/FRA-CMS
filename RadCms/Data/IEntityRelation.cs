using System.Data.Entity;

namespace RadCms.Data
{
    public interface IEntityRelation
    {
        void OnModelCreating(DbModelBuilder modelBuilder);
    }
}
