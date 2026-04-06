namespace ReceitasApp.Models;

public class Receita
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int TempoPreparacao { get; set; }
    public int NumeroPessoas { get; set; }
    public string Dificuldade { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string Preparacao { get; set; } = string.Empty;

    public List<IngredienteDaReceita> Ingredientes { get; set; } = new List<IngredienteDaReceita>();
}
