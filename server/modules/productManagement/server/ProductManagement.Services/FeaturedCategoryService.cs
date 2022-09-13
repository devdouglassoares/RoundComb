using Core.Database;
using Core.ObjectMapping;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Repositories;
using ProductManagement.Core.Services;

namespace ProductManagement.Services
{
    public class FeaturedCategoryService : BaseService<FeaturedCategory, FeaturedCategoryDto>, IFeaturedCategoryService
    {
        public FeaturedCategoryService(IMappingService mappingService, IRepository repository)
            : base(mappingService, repository)
        {
        }

        public override FeaturedCategory Create(FeaturedCategoryDto model)
        {
            var diplayOrder = Repository.Count<FeaturedCategory>();
            model.DisplayOrder = diplayOrder;
            return base.Create(model);
        }
    }
}