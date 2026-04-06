using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ReceitasApp.ViewModels;

public class ReceitaCreateViewModel
{
    [Required]
    public string Nome { get; set; } = string.Empty;

    [Display(Name = "Tempo de Preparação")]
    public int TempoPreparacao { get; set; }

    [Display(Name = "Número de Pessoas")]
    public int NumeroPessoas { get; set; }

    public string Dificuldade { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string Preparacao { get; set; } = string.Empty;

    public List<IngredienteItemViewModel> Ingredientes { get; set; } = new();

    public List<SelectListItem> Dificuldades { get; set; } = new();
    public List<SelectListItem> Categorias { get; set; } = new();
    public List<SelectListItem> Medidas { get; set; } = new();
}

public class IngredienteItemViewModel
{
    public string Nome { get; set; } = string.Empty;
    public decimal Quantidade { get; set; }
    public string Medida { get; set; } = string.Empty;
}