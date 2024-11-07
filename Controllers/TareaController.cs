using appAPI.Data;
using appAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading;
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
                TAREAID = tarea.TareaId,
                STATUS = false
            };
            _dbContext.TAREA.Add(t);
            USER u = await _dbContext.USER.Where(c => c.ID == tarea.UserId).FirstOrDefaultAsync();
            TRABAJOS tg = await _dbContext.TRABAJOS.Where(z => z.ID == tarea.TareaId).FirstOrDefaultAsync();
            if(u != null && tg != null)
            {
                u.PUNTOS = u.PUNTOS + (int)tg.PUNTUEK;
            }
            await _dbContext.SaveChangesAsync();
            return Ok(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error al crear tarea");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("penalizations")]
    public async Task<ActionResult<List<PenalizationOrokorraDTO>>> GetPenalizations()
    {
        try
        {
            List<PenalizationOrokorraDTO> p = await _dbContext.PENALIZATIONS.Select(c => new PenalizationOrokorraDTO
            {
                Id = c.ID,
                Desc = c.DESC,
                Puntuek = c.PUNTUEK
            }).ToListAsync();
            
            return Ok(p);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error al crear tarea");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("general")]
    public async Task<ActionResult<List<TrabajoDTO>>> GetTareasGenerales()
    {
        try
        {
            List<TrabajoDTO> p = await _dbContext.TRABAJOS.Select(c => new TrabajoDTO
            {
                Id = c.ID,
                Desc = c.DESC,
                Puntuek = c.PUNTUEK
            }).ToListAsync();

            return Ok(p);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error al crear tarea");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost("{id}")]
    public async Task<ActionResult<bool>> UpdateTarea(string id, TareaDTO tarea)
    {
        try
        {
            TAREA t = await _dbContext.TAREA.Where(c => c.ID.ToString() == id).FirstOrDefaultAsync();
            if (t != null)
            {
                t.LIMITDATE = tarea.LimitDate.ToUniversalTime();
                t.DESCRIPTION = tarea.Description;
                t.USERID = tarea.UserId;
                t.STATUS = tarea.Status;
            }
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

    [HttpGet("{id}")]
    public async Task<ActionResult<TareaDTO>> GetTareas(string id)
    {
        try
        {
            var tareas = await _dbContext.TAREA.Where(c => c.ID.ToString() == id)
                .AsNoTracking()
                .Select(c => new TareaDTO
                {
                    Id = c.ID,
                    UserId = c.USERID,
                    Description = c.DESCRIPTION,
                    LimitDate = c.LIMITDATE,
                    Status = c.STATUS
                }).ToListAsync();

            return Ok(tareas.First());
        }
        catch (Exception e)
        {
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteTarea(string id)
    {
        try
        {
            // Buscar la tarea por ID de forma asíncrona
            TAREA t = await _dbContext.TAREA
                                      .Where(c => c.ID.ToString() == id)
                                      .FirstOrDefaultAsync();

            // Si la tarea no existe, devolver NotFound
            if (t == null)
            {
                return NotFound("Tarea no encontrada.");
            }

            // Buscar el trabajo asociado a la tarea
            TRABAJOS t2 = await _dbContext.TRABAJOS
                                          .Where(c => c.ID == t.TAREAID)
                                          .FirstOrDefaultAsync();

            // Si el trabajo existe, actualizar los puntos del usuario
            if (t2 != null)
            {
                USER u = await _dbContext.USER
                                         .Where(c => c.ID == t.USERID)
                                         .FirstOrDefaultAsync();

                if (u != null)
                {
                    u.PUNTOS -= (int)t2.PUNTUEK;
                    _dbContext.Update(u); // Asegúrate de actualizar el usuario
                }
            }

            // Eliminar la tarea
            _dbContext.Remove(t);

            // Guardar los cambios
            await _dbContext.SaveChangesAsync();

            return Ok(true); // Indica que la tarea se eliminó correctamente
        }
        catch (Exception e)
        {
            // Registrar el error (si tienes un logger configurado)
            Console.WriteLine(e.Message);  // O usa un logger en producción

            // Devolver un error genérico, pero puedes incluir el mensaje de error si es necesario
            return StatusCode(500, "Error interno del servidor: " + e.Message);
        }
    }


}