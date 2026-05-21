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
        [HttpGet("atividade")]
        public IActionResult TarefasUsuario()
        {
            var sessaoUsuario = HttpContext.Session.GetString("Idusado");
            if (sessaoUsuario == null)
            {
                return Unauthorized("Faça login Antes");
            }
            var resultado = from c in _context.Usuarios
                            join r in _context.Tarefas
                            on c.Id equals r.UsuarioId
                            where c.Id == int.Parse(sessaoUsuario)
                            select new
                            {
                                Usuario = c.Nome,
                                c.Email,
                                Tarefa = r.Id,
                                r.Descricao, r.Status
                                    
                                };
                                return Ok(resultado.ToList());
                     
        }

        [HttpPost("Cadastrar")]
        public IActionResult CriarTarefas(Tarefa tarefa)
        {

            var idPessoa = HttpContext.Session.GetString("Idusado");
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

        [HttpPut("atualizar/{id}")]
        public IActionResult AtualizarTarefa(int id, Tarefa tarefa)
        {
            var idPessoa = HttpContext.Session.GetString("Idusado");
            if (idPessoa == null) return Unauthorized("não autorizado");

            var tarefaBanco = _context.Tarefas.Find(id);
            if (tarefaBanco == null)
            {
                return NotFound("Tarefa não encontrada");
            }
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Status = tarefa.Status;
           
            _context.SaveChanges();
            return Ok("Tarefa atualizada");
        }
        [HttpDelete("Deletar/{id}")]
        public IActionResult DeletarTarefa(int id)
        {
            var idPessoa = HttpContext.Session.GetString("Idusado");
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
