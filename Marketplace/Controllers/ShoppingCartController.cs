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
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IMapper mapper,
            IUserRepository userRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ShoppingCart>))]
        public IActionResult GetShoppingCarts()
        {
            var carts = _mapper.Map<List<ShoppingCartDto>>(_shoppingCartRepository.GetCarts());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(carts);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(ShoppingCart))]
        public IActionResult GetCartByUser(int userId)
        {

            if (!_shoppingCartRepository.CartExists(userId))
                return NotFound();
            var cart = _mapper.Map<ShoppingCartDto>(_shoppingCartRepository.GetCartByUser(userId));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(cart);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCart(int userId, [FromBody] ShoppingCartDto CreateCart)
        {
            if (CreateCart == null) return BadRequest(ModelState);


            var cartMap = _mapper.Map<ShoppingCart>(CreateCart);

            cartMap.UserId = userId;

            if (!_shoppingCartRepository.CreateCart(cartMap))
            {
                ModelState.AddModelError("", "Something went wrong while creating...");
                return StatusCode(500, ModelState);
            }

            return Ok(cartMap);
        }

        [HttpPut("{cartId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCart(int cartId, [FromBody] ShoppingCartDto updateCart)
        {
            if (updateCart == null) return BadRequest(ModelState);
            if (cartId == 0) return BadRequest(ModelState);
            if (cartId != updateCart.Id) return BadRequest(ModelState);

            if (!_shoppingCartRepository.CartExists(cartId))
                return NotFound();

            var cartMap = _mapper.Map<ShoppingCart>(updateCart);

            if (!_shoppingCartRepository.UpdateCart(cartMap))
            {
                ModelState.AddModelError("", "Somthing went wrong while updating...");
                return StatusCode(500, ModelState);
            }

            return Ok(cartMap);
        }

        [HttpDelete("{cartId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCart(int cartId)
        {
            if (!_shoppingCartRepository.CartExists(cartId)) return NotFound();

            var CartToDelete = _shoppingCartRepository.GetCart(cartId);

            if (CartToDelete == null) return NotFound();

            if (!_shoppingCartRepository.DeleteCart(CartToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting....");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
