using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SarojWebApp.Models;
using SarojWebApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SarojWebApp.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private IUserService _userService;
        public HomeController(IUserService userService)
        {
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginModel model)
        {
            var user = await _userService.Login(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
        [Authorize]
        [HttpGet]
        [Route("home/GetAllNotes/{id}")]
        async Task<IActionResult> GetAllNotes(int userId)
        {
            return Ok(_userService.GetAllNotes(userId));
        }
        [Authorize]
        [HttpPut]
        async Task<IActionResult> UpdateNote(UserNote note)
        {
            return Ok(_userService.UpdateNote(note));
        }
        [Authorize]
        [HttpPost]
        async Task<IActionResult> CreateNote(UserNote note)
        {
            return Ok(_userService.CreateNote(note));
        }
        [Authorize]
        [HttpDelete]
        async Task<IActionResult> DeleteNote(int noteId)
        {
            return Ok(_userService.DeleteNote(noteId));
        }
    }
}

