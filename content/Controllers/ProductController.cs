using System;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using FurnitureBuildingSolution.Services;

namespace FurnitureBuildingSolution.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMapper _autoMapper;
        private readonly IProductService _productService;

        public ProductController(IMapper autoMapper, IProductService productService)
        {
            _autoMapper = autoMapper;
            _productService = productService;
        }

        [HttpGet("getCategories")]
        public IActionResult GetCategories()
        {
            try
            {
                var categories = _productService.GetCategories();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("getStandardModels")]
        public IActionResult GetStandardModels(int? categoryId = null)
        {
            try
            {
                var products = _productService.GetStandardModels(categoryId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("getStandardModel")]
        public IActionResult GetStandardModel(int id)
        {
            try
            {
                var product = _productService.GetStandardModel(id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
