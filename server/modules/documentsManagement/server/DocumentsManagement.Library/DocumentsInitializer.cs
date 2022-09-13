using DocumentsManagement.Library.Entities;
using System.Data.Entity;

namespace DocumentsManagement.Library
{
    public class DocumentsInitializer
    {
        public static void InitializeContext<TEntity>(DbModelBuilder modelBuilder) where TEntity : BaseDocumentEntity
        {
            modelBuilder.Entity<TEntity>();
            var entityType = typeof(TEntity).Name;
            modelBuilder.Entity<FileRecord>().ToTable(entityType + "_FileRecord");
        }
    }
}
