using appAPI.Data;
using appAPI.DTO;
using Microsoft.AspNetCore.Mvc;

namespace appAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TareaController: ControllerBase
    {
        private readonly MyDbContext _dbContext;

        private readonly ILogger<TareaController> _logger;

        public TareaController(ILogger<TareaController> logger, MyDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpPost]
        public ActionResult<bool> CreateTarea( TareaDTO tarea)
        {
            try
            {
                TAREA t = new TAREA();
                t.LIMITDATE = tarea.LimitDate.ToUniversalTime(); ;
                t.USERID = tarea.UserId;
                t.DESCRIPTION = tarea.Description;
                t.STATUS = false;

                _dbContext.TAREA.Add(t);
                _dbContext.SaveChanges();

                return Ok(true);
            }catch(Exception e)
            {
                return Ok(false);
            }
        }
    }
}
