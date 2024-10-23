using appAPI.Data;
using appAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
[ApiController]
[Route("[controller]")]
public class TareaController : ControllerBase
{
    private readonly ILogger<TareaController> _logger;
    private readonly MyDbContext _dbContext;

    public TareaController(ILogger<TareaController> logger, MyDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<ActionResult<bool>> CreateTarea(TareaDTO tarea)
    {
        try
        {
            var t = new TAREA
            {
                LIMITDATE = tarea.LimitDate.ToUniversalTime(),
                USERID = tarea.UserId,
                DESCRIPTION = tarea.Description,
                STATUS = false
            };
            _dbContext.TAREA.Add(t);
            await _dbContext.SaveChangesAsync();
            return Ok(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error al crear tarea");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<TareaDTO>>> GetTareas()
    {
        try
        {
            var tareas = await _dbContext.TAREA
                .AsNoTracking() 
                .Select(c => new TareaDTO
                {
                    Id = c.ID,
                    UserId = c.USERID,
                    Description = c.DESCRIPTION,
                    LimitDate = c.LIMITDATE,
                    Status = c.STATUS
                })
                .ToListAsync();

            return Ok(tareas);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Error interno del servidor");
        }
    }
}