using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.API.Data;
using SmartSchool.API.Services;
using SmartSchool.API.V2.Dtos;

namespace SmartSchool.API.V1.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IRepository _repo;
        private readonly IMapper _mapper;
        /// <summary>
        /// Controller de account
        /// </summary>
        public AccountController(IRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Método responsável por receber o POST com usuário e senha para autenticação.
        /// Será retornada a chave JWT gerada
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate(UserDto model)
        {
            var user = await _repo.GetUser(model.Username, model.Password);

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            var token = TokenService.GenerateToken(new Models.User { Username = model.Username, Password = model.Password, Role = user.Role });
            user.Password = "";
            return new
            {
                user = user,
                token = token
            };
        }
    }
}
