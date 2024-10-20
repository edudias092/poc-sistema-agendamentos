using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EJ.SistemaAgendamentos.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EJ.SistemaAgendamentos.Data
{
    public class MyDbContext : IdentityDbContext<IdentityUser>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Agendamento>(t => t.ToTable("tb_agendamentos"));
        }

        public DbSet<Agendamento> Agendamentos {get; set;}
    }
}