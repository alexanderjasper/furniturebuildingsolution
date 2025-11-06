using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FurnitureBuildingSolution.Database;
using FurnitureBuildingSolution.Entities;

namespace FurnitureBuildingSolution.Repositories
{
    public interface IProductRepository
    {
        List<ProductCategory> GetCategories();
        List<StandardModel> GetStandardModels(int? categoryId);
        StandardModel GetStandardModel(int id);
    }

    public class ProductRepository : IProductRepository
    {
        private DataContext _context;
        private readonly IMapper _autoMapper;

        public ProductRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _autoMapper = mapper;
        }

        public List<ProductCategory> GetCategories()
        {
            return _context.ProductCategories.Select(pc => _autoMapper.Map<ProductCategory>(pc)).ToList();
        }

        public List<StandardModel> GetStandardModels(int? categoryId)
        {
            if (categoryId == null)
            {
                return _context.StandardModels.Select(sm => _autoMapper.Map<StandardModel>(sm)).ToList();
            }
            else
            {
                return _context.StandardModels.Where(sm => sm.ProductCategoryId == categoryId.Value).Select(sm => _autoMapper.Map<StandardModel>(sm)).ToList();
            }
        }

        public StandardModel GetStandardModel(int id)
        {
            var dbStandardModel = _context.StandardModels.Single(sm => sm.Id == id);
            return _autoMapper.Map<StandardModel>(dbStandardModel);
        }
    }
}
