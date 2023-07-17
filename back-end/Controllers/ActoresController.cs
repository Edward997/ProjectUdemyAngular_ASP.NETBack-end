using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers
{
    [Route("api/actores")]
    [ApiController]
    public class ActoresController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ActoresController(
            ApplicationDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            //var actor = mapper.Map<Actor>(actorCreacionDTO);
            //context.Add(actor);
            //await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
