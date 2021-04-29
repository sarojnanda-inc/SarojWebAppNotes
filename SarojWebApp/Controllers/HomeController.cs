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
        [Route("GetAllNotes/{id}")]
        public async Task<IActionResult> GetAllNotes(int id)
        {
            var notes = await _userService.GetAllNotes(id);
            return Ok(notes);
        }
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateNote(UserNote note)
        {
            var result = await _userService.UpdateNote(note);
            return Ok(result);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateNote(UserNote note)
        {
            return Ok(await _userService.CreateNote(note));
        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteNote(int noteId)
        {
            await _userService.DeleteNote(noteId);
            return Ok();
        }
    }
}

