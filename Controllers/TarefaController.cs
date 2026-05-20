using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoLista.Data;
using ProjetoLista.Models;

namespace ProjetoLista.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly Contexto _context;
        public TarefaController(Contexto context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult BuscarTarefas()
        {
            var idPessoa = HttpContext.Session.GetString("email");
            if (idPessoa == null) return Unauthorized("não autorizado");

            return Ok(_context.Tarefas.ToList());
        }
        [HttpGet("{id}")]
        public IActionResult BuscarTarefa(int id)
        {
            var idPessoa = HttpContext.Session.GetString("email");
            if (idPessoa == null) return Unauthorized("não autorizado"); 

            var tarefaBanco = _context.Tarefas.Find(id);
            if (tarefaBanco == null)
            {
                return NotFound("Tarefa não encontrada");
            }
            return Ok(tarefaBanco);
        }
        [HttpPost("Cadastrar")]
        public IActionResult CriarTarefas(Tarefa tarefa)
        {

            var idPessoa = HttpContext.Session.GetString("email");
            if (idPessoa == null) return Unauthorized("não autorizado");

            var sessao = Request.Cookies["Idusado"];

            if (sessao != null)
            {
                tarefa.UsuarioId = int.Parse(sessao);
            }
            _context.Add(tarefa);
            _context.SaveChanges();
            return Created("Teste", tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarTarefa(int id, Tarefa tarefa)
        {
            var idPessoa = HttpContext.Session.GetString("email");
            if (idPessoa == null) return Unauthorized("não autorizado");

            var tarefaBanco = _context.Tarefas.Find(id);
            if (tarefaBanco == null)
            {
                return NotFound("Tarefa não encontrada");
            }
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Status = tarefa.Status;
            tarefaBanco.UsuarioId = tarefa.UsuarioId;
            _context.SaveChanges();
            return Ok("Tarefa atualizada");
        }
        [HttpDelete("{id}")]
        public IActionResult DeletarTarefa(int id)
        {
            var idPessoa = HttpContext.Session.GetString("email");
            if (idPessoa == null) return Unauthorized("não autorizado");

            var tarefaBanco = _context.Tarefas.Find(id);
            if (tarefaBanco == null)
            {
                return NotFound("Tarefa não encontrada");
            }
            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();
            return Ok("Tarefa deletada");
        }
    
    }
}
