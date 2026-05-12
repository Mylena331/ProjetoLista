using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoLista.Data;
using ProjetoLista.Models;


namespace ProjetoLista.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly Contexto _context;
        public UsuarioController(Contexto context)
        {
            _context = context;
        }
        [HttpGet] 
        public IActionResult BuscarUsuarios()
        {
            return Ok(_context.Usuarios.ToList());
        }
        [HttpGet("{id}")]
        public IActionResult BuscarUsuario(int id)
        {
            var usuarioBanco = _context.Usuarios.Find(id);
            if (usuarioBanco == null)
            {
                return NotFound("Usuário não encontrado");
            }
            return Ok(usuarioBanco);
        }
        [HttpPost("login")]
        public IActionResult Login(Usuario dadosLogin)
        {
            var loginU = _context.Usuarios.Where(u => u.Email.Equals(dadosLogin.Email) && u.Senha.Equals(dadosLogin.Senha)).ToList();


            if (loginU.Count == 0)

                return Unauthorized("Email ou Senha Incorretas");
            HttpContext.Session.SetString("email", dadosLogin.Email);
            Response.Cookies.Append("Idusado", loginU[0].Id.ToString(),

            new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(30),
                Secure = true,
                HttpOnly = true
            });


            return Ok("Login realizado com sucesso!");
        }


        [HttpPost]
        public IActionResult CadastrarUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return Ok("Usuário cadastrado");
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarUsuario(int id, Usuario usuario)
        {
            var usuarioBanco = _context.Usuarios.Find(id);
            if (usuarioBanco == null)
            {
                return NotFound("Usuário não encontrado");
            }
            usuarioBanco.Nome = usuario.Nome;
            usuarioBanco.Email = usuario.Email;
            usuarioBanco.Senha = usuario.Senha;
            _context.SaveChanges();
            return Ok("Usuário atualizado");
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarUsuario(int id)
        {
            var usuarioBanco = _context.Usuarios.Find(id);
            if (usuarioBanco == null)
            {
                return NotFound("Usuário não encontrado");
            }
            _context.Usuarios.Remove(usuarioBanco);
            _context.SaveChanges();
            return Ok("Usuário deletado");
        }
    }
}
