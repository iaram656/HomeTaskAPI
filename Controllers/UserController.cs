using appAPI.Data;
using appAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<TareaController> _logger;
    private readonly MyDbContext _dbContext;

    public UserController(ILogger<TareaController> logger, MyDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDTO>>> GetUsers()
    {
        try
        {
            var users = await _dbContext.USER
                .AsNoTracking()
                .Select(c => new UserDTO
                {
                    Id = c.ID,
                    Name = c.NAME,
                    Puntos = c.PUNTOS,
                })
                .ToListAsync();

            return Ok(users);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost("update")]
    public async Task<ActionResult<bool>> UpdateUser(UserDTO us)
    {
        if (us == null)
        {
            return BadRequest("El usuario no puede ser nulo.");
        }

        try
        {
            var user = await _dbContext.USER
                .Where(c => c.ID == us.Id)
                .Select(c => new UserDTO
                {
                    Id = c.ID,
                    Name = c.NAME,
                    Puntos = c.PUNTOS,
                })
                .FirstOrDefaultAsync(); // Cambiado a FirstOrDefaultAsync para manejar casos donde el usuario no existe

            if (user == null)
            {
                return NotFound("Usuario no encontrado."); // Retornar un mensaje adecuado si no se encuentra el usuario
            }

            user.Puntos = us.Puntos;

            // Actualizar la entidad en el contexto de la base de datos
            var userToUpdate = await _dbContext.USER.FindAsync(us.Id); // Buscar la entidad original para actualizarla
            if (userToUpdate == null)
            {
                return NotFound("Usuario no encontrado."); // Seguridad adicional para verificar que el usuario existe en la base de datos
            }

            userToUpdate.PUNTOS = user.Puntos; // Actualizar los puntos

            await _dbContext.SaveChangesAsync(); // Usar SaveChangesAsync para operaciones asincrónicas

            return Ok(true);
        }
        catch (Exception e)
        {
            // Registrar el error si es necesario
            return StatusCode(500, "Error interno del servidor");
        }
    }


}

