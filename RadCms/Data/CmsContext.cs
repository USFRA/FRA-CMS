using System;
using System.Data.Entity;
using System.Linq;
using RadCms.Entities;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace RadCms.Data
{
    public class CmsContext : DbContext, IDbContext
    {
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Add entities to context
            //var referencedAssemblyNames = Assembly.Load("RadCms.Web").GetReferencedAssemblies();
            var allAssemblyNames = AppDomain.CurrentDomain.GetAssemblies().Select(e => e.GetName().Name);
            var appAssemblyNames = allAssemblyNames.Where(e => e.StartsWith("RadCms"));
            foreach(var assemblyName in appAssemblyNames)
            {
                var types = Assembly.Load(assemblyName).GetTypes().Where(t => typeof(IEntity).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract).ToList();
                foreach(var type in types)
                {
                    var method = modelBuilder.GetType().GetMethod("Entity");
                    method = method.MakeGenericMethod(new Type[] { type });
                    method.Invoke(modelBuilder, null);
                }
            }

            foreach(var assemblyName in appAssemblyNames)
            {
                var types = Assembly.Load(assemblyName).GetTypes().Where(t => typeof(IEntityRelation).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract).ToList();
                foreach(var type in types)
                {
                    var relationBuilder = (IEntityRelation)Activator.CreateInstance(type);
                    relationBuilder.OnModelCreating(modelBuilder);
                }
            }

            modelBuilder.Entity<NavItem>().HasRequired(i => i.Group)
                                        .WithMany(g => g.Items)
                                        .WillCascadeOnDelete();

            modelBuilder.Entity<CmsPage>().HasOptional(u => u.Html)
                                       .WithRequired()
                                       .WillCascadeOnDelete();

            modelBuilder.Entity<CmsPage>().HasOptional(e => e.Type)
                                        .WithMany()
                                        .Map(m => m.MapKey("Type_Id"));

            modelBuilder.Entity<PubPage>().HasOptional(u => u.Html)
                                        .WithRequired()
                                        .WillCascadeOnDelete();

            modelBuilder.Entity<PubPage>().HasOptional(e => e.Type)
                                        .WithMany()
                                        .Map(m => m.MapKey("Type_Id"));

            modelBuilder.Entity<VerPage>().HasOptional(u => u.Html)
                                        .WithRequired()
                                        .WillCascadeOnDelete();

            modelBuilder.Entity<VerPage>().HasOptional(e => e.Type)
                                        .WithMany()
                                        .Map(m => m.MapKey("Type_Id"));

            modelBuilder.Entity<NaviNode>().HasOptional(e => e.Type)
                                        .WithMany()
                                        .Map(m => m.MapKey("Type_Id"));

            modelBuilder.Entity<PageLayout>().HasOptional(e => e.Type)
                                        .WithMany()
                                        .Map(m => m.MapKey("Type_Id"));

            modelBuilder.Entity<Favorite>().HasOptional(u => u.Page)
                                       .WithMany()
                                       .WillCascadeOnDelete();

            modelBuilder.Entity<FooterItem>().HasRequired(i => i.Section)
                    .WithMany(g => g.Items)
                    .WillCascadeOnDelete();

            modelBuilder.Entity<PagePermission>().HasRequired(p => p.User)
                    .WithMany()
                    .WillCascadeOnDelete();

            modelBuilder.Entity<PagePermission>().HasRequired(p => p.Page)
                    .WithMany()
                    .WillCascadeOnDelete();

            modelBuilder.Entity<NaviPermission>().HasRequired(p => p.User)
                    .WithMany()
                    .WillCascadeOnDelete();

            modelBuilder.Entity<NaviPermission>().HasRequired(p => p.Section)
                    .WithMany()
                    .WillCascadeOnDelete();

            modelBuilder.Entity<Permission>().HasRequired(p => p.User)
                    .WithMany()
                    .WillCascadeOnDelete();

            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<RadCms.Entities.ContentType> ContentTypes { get; set; }
    } 
}
/*


            /*
             * one to one/zero between library 
             *
modelBuilder.Entity<LibEntity>().HasOptional(u => u.HtmlText)
                           .WithRequired()
                           .WillCascadeOnDelete();


modelBuilder.Entity<PubLibEntity>().HasOptional(u => u.HtmlText)
                           .WithRequired()
                           .WillCascadeOnDelete();


modelBuilder.Entity<VerLibEntity>().HasOptional(u => u.HtmlText)
                           .WithRequired()
                           .WillCascadeOnDelete();


modelBuilder.Entity<LibAttachment>().HasOptional(u => u.FileData)
                           .WithRequired()
                           .WillCascadeOnDelete();


modelBuilder.Entity<PubLibAttachment>().HasOptional(u => u.FileData)
                           .WithRequired()
                           .WillCascadeOnDelete();


modelBuilder.Entity<VerLibAttachment>().HasOptional(u => u.FileData)
                           .WithRequired()
                           .WillCascadeOnDelete();



//one to many - Library
modelBuilder.Entity<LibAttachment>().HasRequired(t => t.LibItem)
                            .WithMany(t => t.Attachments)
                            .WillCascadeOnDelete();


modelBuilder.Entity<PubLibAttachment>().HasRequired(t => t.LibItem)
                            .WithMany(t => t.Attachments)
                            .WillCascadeOnDelete();


modelBuilder.Entity<VerLibAttachment>().HasRequired(t => t.LibItem)
                            .WithMany(t => t.Attachments)
                            .WillCascadeOnDelete();

modelBuilder.Entity<SeriesRole>().HasRequired(e => e.Series)
                    .WithMany()
                    .WillCascadeOnDelete();

modelBuilder.Entity<SeriesRole>().HasRequired(e => e.Role)
                    .WithMany()
                    .WillCascadeOnDelete();

modelBuilder.Entity<LibEntity>().HasMany(u => u.Subjects)
                .WithMany();


modelBuilder.Entity<PubLibEntity>().HasMany(u => u.Subjects)
                .WithMany();

modelBuilder.Entity<LibEntity>().HasMany(u => u.Roles)
                .WithMany();

modelBuilder.Entity<PubLibEntity>().HasMany(u => u.Roles)
                .WithMany();





*/
