using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FurnitureBuildingSolution.Dtos;
using FurnitureBuildingSolution.Repositories;

namespace FurnitureBuildingSolution.Services
{
    public interface IProductService
    {
        List<ProductCategoryDto> GetCategories();
        List<StandardModelDto> GetStandardModels(int? categoryId);
        StandardModelDto GetStandardModel(int id);
    }

    public class ProductService : IProductService
    {
        private IProductRepository _productRepository;
        private IBookcaseService _bookcaseService;
        private readonly IMapper _autoMapper;

        public ProductService(IMapper mapper, IProductRepository productRepository, IBookcaseService bookcaseService)
        {
            _autoMapper = mapper;
            _productRepository = productRepository;
            _bookcaseService = bookcaseService;
        }

        public List<ProductCategoryDto> GetCategories()
        {
            var categories = _productRepository.GetCategories();
            var categoryDtos = categories.Select(c => _autoMapper.Map<ProductCategoryDto>(c));
            return categoryDtos.ToList();
        }

        public List<StandardModelDto> GetStandardModels(int? categoryId)
        {
            var standardModels = _productRepository.GetStandardModels(categoryId);
            var standardModelDtos = standardModels.Select(sm =>
            {
                var standardModel = _autoMapper.Map<StandardModelDto>(sm);
                AddPrice(ref standardModel);

                return standardModel;
            });
            return standardModelDtos.ToList();
        }

        private void AddPrice(ref StandardModelDto standardModel)
        {
            var bookcase = _bookcaseService.Get(standardModel.BookcaseId);
            standardModel.Price = bookcase.SalesPrice.Value;
        }

        public StandardModelDto GetStandardModel(int id)
        {
            var standardModel = _autoMapper.Map<StandardModelDto>(_productRepository.GetStandardModel(id));
            AddPrice(ref standardModel);

            return standardModel;
        }
    }
}
