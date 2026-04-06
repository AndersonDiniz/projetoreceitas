namespace ReceitasApp.Models;

public class IngredienteDaReceita
{
    public int Id { get; set; }

    public int ReceitaId { get; set; }
    public Receita? Receita { get; set; }

    public int IngredienteId { get; set; }
    public Ingrediente? Ingrediente { get; set; }

    public decimal Quantidade { get; set; }
    public string Medida { get; set; } = string.Empty;
}