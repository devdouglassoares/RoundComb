using Core;
using Core.Database.Repositories;
using CustomForm.Data.DbContexts;

namespace CustomForm.Data.Repositories
{
    public interface IRepository: IBaseRepository, IDependency
    {
         
    }

    public class CustomFormRepository: Repository, IRepository {
        public CustomFormRepository(CustomFormDataContext dbContext) : base(dbContext)
        {
        }
    }
}