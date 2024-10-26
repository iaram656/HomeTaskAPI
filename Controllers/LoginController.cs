using appAPI.Data;
using appAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;
    private readonly MyDbContext _dbContext;

    public LoginController(ILogger<LoginController> logger, MyDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<ActionResult<bool>> Login([FromBody] LoginDTO loginDto)
    {
        string password = "ogarm74";

        // Crear un hash de la contraseña
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);


        if (loginDto == null || string.IsNullOrEmpty(loginDto.Username) || string.IsNullOrEmpty(loginDto.Password))
        {
            return BadRequest("El nombre de usuario y la contraseña son requeridos.");
        }

        // Buscar el usuario en la base de datos
        var user = await _dbContext.USER.FirstOrDefaultAsync(u => u.NAME == loginDto.Username);

        if (user == null)
        {
            // El usuario no fue encontrado
            return Ok(false);
        }

        // Comparar la contraseña hasheada
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PASSWORD);

        if (!isPasswordValid)
        {
            return Ok(false);
        }

        // Si la autenticación es exitosa
        return Ok(true);
    }
}

