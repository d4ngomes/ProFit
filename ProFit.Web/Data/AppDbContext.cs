using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProFit.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProFit.Web.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base (options)
        {

        }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Feriado> Feriados { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Funcionamento> Funcionamento { get; set; }
    }
}
