using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repository;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;


namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/Users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("")]

        public async Task<IActionResult> GetUsers()
        {
            var result = await _userRepository.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                return BadRequest("Invalid ID format");
            var user = await _userRepository.GetByIdAsync(id);
            return User == null ? NotFound() : Ok(user);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (string.IsNullOrEmpty(user.name))
                return BadRequest("name is required");

            if (string.IsNullOrEmpty(user.email))
                return BadRequest("email is required");

            user.userId = null; 


            await _userRepository.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.userId }, user);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id,User user)
        {
            if (!ObjectId.TryParse(id, out _))
                return BadRequest("Invalid ID format");
            if (string.IsNullOrEmpty(user.name))
                return BadRequest("name is Required");
            if (string.IsNullOrEmpty(user.email))
                return BadRequest("email is Required");
            var existingUser = await _userRepository.GetByIdAsync(id);
            if(existingUser==null)
                return NotFound();
            user.userId = existingUser.userId;
            await _userRepository.UpdateUserAsync(id, user);
            return Ok(user);


        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                return BadRequest("Invalid ID format");

            var result = await _userRepository.DeleteUserAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
