using AutoMapper;
using Marketplace.Dtos;
using Marketplace.Interfaces;
using Marketplace.Models;
using Marketplace.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IMarketRepository _marketRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, IMapper mapper, 
            IMarketRepository marketRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _marketRepository = marketRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
        public IActionResult GeProducts()
        {
            var Products = _mapper.Map<List<ProductDto>>(_productRepository.GetProducts());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(Products);
        }
        
        [HttpGet("{productId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetProduct(int productId)
        {
            if (!_productRepository.ProductExists(productId))
                return NotFound();
            var user = _mapper.Map<UserDto>(_productRepository.GetProduct(productId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }
        
        [HttpGet("category/{cateId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
        public IActionResult GetProductByCategory(int cateId)
        {

            if (!_productRepository.ProductExists(cateId))
                return NotFound();
            var product = _mapper.Map<List<ProductDto>>(_productRepository.GetProductByCategory(cateId));

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            return Ok(product);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateProduct([FromQuery]int marketId, [FromQuery]int categoryId,[FromBody] ProductDto CreateProduct)
        {
            if (CreateProduct == null) 
                return BadRequest(ModelState);

            var isExisting = _productRepository.GetProducts()
                .Where(c => c.Name.Trim() == CreateProduct.Name.TrimEnd()).FirstOrDefault();

            if (isExisting != null)
            {
                ModelState.AddModelError("", "Product Already Exists");
                return StatusCode(422, ModelState);
            }

            var productMap = _mapper.Map<Product>(CreateProduct);

            productMap.Market = _marketRepository.GetMarket(marketId);
            productMap.Category = _categoryRepository.GetCategory(categoryId);

            if (!_productRepository.CreateProduct(productMap))
            {
                ModelState.AddModelError("", "Something went wrong while creating...");
                return StatusCode(500, ModelState);
            }

            return Ok(productMap);
        }

        [HttpPut("{productId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProduct(int productId, [FromBody] ProductDto updateProduct)
        {
            if (updateProduct == null) return BadRequest(ModelState);
            if (productId == 0) return BadRequest(ModelState);
            if (productId != updateProduct.Id) return BadRequest(ModelState);

            if (!_productRepository.ProductExists(productId))
                return NotFound();

            var productMap = _mapper.Map<Product>(updateProduct);

            if (!_productRepository.UpdateProduct(productMap))
            {
                ModelState.AddModelError("", "Somthing went wrong while updating...");
                return StatusCode(500, ModelState);
            }

            return Ok(productMap);
        }

        [HttpDelete("{productId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProduct(int productId)
        {
            if (!_productRepository.ProductExists(productId)) return NotFound();

            var productToDelete = _productRepository.GetProduct(productId);

            if (productToDelete == null)
                return NotFound();

            if (!_productRepository.DeleteProduct(productToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting....");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
