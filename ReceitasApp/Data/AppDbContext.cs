using Microsoft.EntityFrameworkCore;
using ReceitasApp.Models;

namespace ReceitasApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Receita> Receitas { get; set; }
    public DbSet<Ingrediente> Ingredientes { get; set; }
    public DbSet<IngredienteDaReceita> IngredientesDaReceita { get; set; }
}