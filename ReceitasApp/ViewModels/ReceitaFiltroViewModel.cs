using Microsoft.AspNetCore.Mvc.Rendering;
using ReceitasApp.Models;

namespace ReceitasApp.ViewModels;

public class ReceitaFiltroViewModel
{
    public string? Nome { get; set; }
    public int? TempoPreparacao { get; set; }
    public int? NumeroPessoas { get; set; }
    public string? Dificuldade { get; set; }
    public string? Categoria { get; set; }
    public string? Preparacao { get; set; }

    public List<SelectListItem> Dificuldades { get; set; } = new();
    public List<SelectListItem> Categorias { get; set; } = new();

    public List<Receita> Receitas { get; set; } = new();
}