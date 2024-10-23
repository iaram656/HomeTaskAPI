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
                    Name = c.NAME
                })
                .ToListAsync();

            return Ok(users);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Error interno del servidor");
        }
    }

}

