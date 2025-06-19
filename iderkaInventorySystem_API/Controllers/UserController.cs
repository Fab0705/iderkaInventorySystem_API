using iderkaInventorySystem_API.Models;
using iderkaInventorySystem_API.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace iderkaInventorySystem_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly iUser _usr;
        public UserController(iUser usr)
        {
            _usr = usr;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _usr.GetAllUsers());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _usr.GetUserById(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] JsonElement json)
        {
            var roleIds = json.GetProperty("roles")
                      .EnumerateArray()
                      .Select(r => r.GetString()!)
                      .ToList();

            var user = new User
            {
                Usr = json.GetProperty("usr").GetString()!,
                Pwd = json.GetProperty("pwd").GetString()!,
                Email = json.GetProperty("email").GetString()!,
                IdLoc = json.GetProperty("idLoc").GetString()!,
            };

            await _usr.Add(user, roleIds);
            return Ok(new { message = "Usuario creado exitosamente" });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            if (user == null)
                return BadRequest(new { message = "Usuario no puede ser nulo" });
            await _usr.Update(user);
            return Ok(new { message = "Usuario actualizado exitosamente" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _usr.GetUserById(id);
            if (user == null)
                return NotFound(new { message = "Usuario no encontrado" });
            await _usr.Delete(id);
            return Ok(new { message = "Usuario eliminado exitosamente" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var result = await _usr.Login(login.Username, login.Password);
            if (result == null)
                return Unauthorized(new { message = "Credenciales incorrectas" });

            return Ok(result);
        }
    }
}
