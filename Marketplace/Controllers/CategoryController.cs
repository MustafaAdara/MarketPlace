using AutoMapper;
using Marketplace.Dtos;
using Marketplace.Interfaces;
using Marketplace.Models;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(categories);
        }

        [HttpGet("{productId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        public IActionResult GetCategoryOfAProduct(int productId)
        {
            
            if (!_categoryRepository.CategoriesExists(productId)) 
                return NotFound();
            var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategoryOfAProduct(productId));

            if(!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(category);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody]CategoryDto CreateCategory) 
        {
            if(CreateCategory == null) return BadRequest(ModelState);

            var isExisting = _categoryRepository.GetCategories().Where(c=>c.Name.Trim() == CreateCategory.Name.TrimEnd()).FirstOrDefault();

            if(isExisting != null)
            {
                ModelState.AddModelError("", "Category Already Exists");
                return StatusCode(422, ModelState);
            }

            var categoryMap = _mapper.Map<Category>(CreateCategory);

            if (!_categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while creating...");
                return StatusCode(500, ModelState);
            }

            return Ok(categoryMap);
        }

        [HttpPut("{cateId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int cateId, [FromBody] CategoryDto updateCategory)
        {
            if (updateCategory == null) return BadRequest(ModelState);
            if (cateId == 0) return BadRequest(ModelState);
            if (cateId != updateCategory.Id) return BadRequest(ModelState);

            if(!_categoryRepository.CategoriesExists(cateId)) 
                return NotFound();

            var categoryMap = _mapper.Map<Category>(updateCategory);

            if(!_categoryRepository.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Somthing went wrong while updating...");
                return StatusCode(500, ModelState);
            }

            return Ok(categoryMap);
        }

        [HttpDelete("{cateId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int cateId)
        {
            if(!_categoryRepository.CategoriesExists(cateId)) return NotFound();

            var CategoryToDelete = _categoryRepository.GetCategory(cateId);

            if (CategoryToDelete == null) return NotFound();

            if (!_categoryRepository.DeleteCategory(CategoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting....");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
