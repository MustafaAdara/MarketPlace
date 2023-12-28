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
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers()
        {
            var users = _mapper.Map<List<UserDto>>(_userRepository.GetUsers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(users);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUser(int userId)
        {
            if (!_userRepository.UserExists(userId))
                return NotFound();
            var user = _mapper.Map<UserDto>(_userRepository.GetUser(userId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] UserDto CreateUser)
        {
            if (CreateUser == null) return BadRequest(ModelState);

            var isExisting = _userRepository.GetUsers().Where(c => c.Name.Trim() == CreateUser.Name.TrimEnd()).FirstOrDefault();

            if (isExisting != null)
            {
                ModelState.AddModelError("", "User Already Exists");
                return StatusCode(422, ModelState);
            }

            var userMap = _mapper.Map<User>(CreateUser);

            if (!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while creating...");
                return StatusCode(500, ModelState);
            }

            return Ok(userMap);
        }

        [HttpPut("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(int userId, [FromBody] UserDto updateUser)
        {
            if (updateUser == null) return BadRequest(ModelState);
            if (userId == 0) return BadRequest(ModelState);
            if (userId != updateUser.Id) return BadRequest(ModelState);

            if (!_userRepository.UserExists(userId))
                return NotFound();

            var userMap = _mapper.Map<User>(updateUser);

            if (!_userRepository.UpdateUser(userMap))
            {
                ModelState.AddModelError("", "Somthing went wrong while updating...");
                return StatusCode(500, ModelState);
            }

            return Ok(userMap);
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(int userId)
        {
            if (!_userRepository.UserExists(userId)) return NotFound();

            var UserToDelete = _userRepository.GetUser(userId);

            if (UserToDelete == null) return NotFound();

            if (!_userRepository.DeleteUser(UserToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting....");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
