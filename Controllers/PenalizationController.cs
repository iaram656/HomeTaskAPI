using appAPI.Data;
using appAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
[ApiController]
[Route("[controller]")]
public class PenalizationController : ControllerBase
{
    private readonly ILogger<TareaController> _logger;
    private readonly MyDbContext _dbContext;

    public PenalizationController(ILogger<TareaController> logger, MyDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }


    [HttpPost]
    public async Task<ActionResult<bool>> CreatePenalization(PenalizationDTO pen)
    {
        try
        {
            var t = new PENALIZATION
            {
                USERID = pen.UserId,
                REASON = pen.Reason,
                DATE = DateTime.UtcNow,
                POINTS = (long)pen.Points,
                PENID = pen.PenId
            };
            PENALIZATIONS pp = await _dbContext.PENALIZATIONS.Where(c => c.ID == pen.PenId).FirstOrDefaultAsync();
            USER u = await _dbContext.USER.Where(c => c.ID == t.USERID).FirstOrDefaultAsync();
            if(u != null)
            {
                u.PUNTOS = u.PUNTOS - (int)pp.PUNTUEK;
            }
            t.POINTS = pp.PUNTUEK;
            await _dbContext.PENALIZATION.AddAsync(t);

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
    public async Task<ActionResult<List<PenalizationDTO>>> GetPenalizations()
    {
        try
        {
            var pen = await _dbContext.PENALIZATION
                .AsNoTracking()
                .Select(c => new PenalizationDTO
                {
                    Id = c.ID,
                    UserId = c.USERID,
                    Reason = c.REASON,
                    Date = c.DATE,
                    Points = c.POINTS,
                })
                .ToListAsync();

            return Ok(pen);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Error interno del servidor");
        }
    }

}
