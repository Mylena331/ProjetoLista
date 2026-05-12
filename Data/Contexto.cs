using Microsoft.EntityFrameworkCore;
using ProjetoLista.Models;

namespace ProjetoLista.Data
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options)
             : base(options){}
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }

    }
}
