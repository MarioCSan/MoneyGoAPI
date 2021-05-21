using Microsoft.EntityFrameworkCore;
using MoneyGoAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyGoAPI.Data
{
    public class TransaccionesContext: DbContext
    {
        public TransaccionesContext(DbContextOptions<TransaccionesContext> options): base(options) { }

        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Transacciones> Transacciones { get;  set; }
    }
}
