using back_end.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace back_end.Controllers
{
    [Route("api/cuentas")]
    [ApiController]
    public class CuentasController: ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;

        public CuentasController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
        }

        [HttpPost("crear")]
        public async Task<ActionResult<RespuestaAutenticacion>> Crear([FromBody] CredencialesUsuario credenciales)
        {
            var usuario = new IdentityUser { UserName = credenciales.Email, Email = credenciales.Email };
            var resultado = await userManager.CreateAsync(usuario, credenciales.Password);

            if (resultado.Succeeded)
            {

            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }

        private async Task<RespuestaAutenticacion> ContruirToken(CredencialesUsuario credenciales)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", credenciales.Email)
            };

            var usuario = await userManager.FindByEmailAsync(credenciales.Email);
            var claimsDB = await userManager.GetClaimsAsync(usuario);

            claims.AddRange(claimsDB);

            var llave = new SymmetricSecurityKey(HeaderEncodingSelector.UTF8.GetBytes(ConfigurationBinder[""]))
        }
    }
}
