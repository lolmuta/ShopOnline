using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
        {
            try
            {
                var products = await this.productRepository.GetItems();
                var productCatetories = await this.productRepository.GetCategories();
                if(products == null || productCatetories == null)
                {
                    return NotFound();
                }

                var productDtos = products.ConvertToDto(productCatetories);
                return Ok(productDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Error retrive data from database");
                throw;
            }
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetItem(int id)
        {
            try
            {
                var product = await productRepository.GetItem(id);
                if(product == null)
                {
                    return BadRequest();
                }
                var category = await productRepository.GetCategory(product.CategoryId);
                var productDto = product.ConvertToDto(category);
                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrive data from database");
            };

        }
        //public async Task<ActionResult<ProductCategory>> GetCategroy(int id)
        //{
        //    try
        //    {
        //        var Categroy = await productRepository.GetCategory(id);
        //        if (Categroy == null)
        //        {
        //            return BadRequest();
        //        }

        //        return Ok(Categroy);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            "Error retrive data from database");
        //    };
        //}
        [HttpGet]
        [Route(nameof(GetCategories))]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetCategories()
        {
            try
            {
                var productCategories = await productRepository.GetCategories();
                var productCategoryDtos = productCategories.ConverToDto();
                return Ok(productCategoryDtos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrive data from database");
            }

        }
    }
}
