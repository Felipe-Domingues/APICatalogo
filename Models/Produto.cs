using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models;

[Table("Produtos")]
public class Produto : IValidatableObject
{
    [Key]
    public int ProdutoId { get; set; }
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(30, ErrorMessage = "Nome deve ter entre 5 e 30 caracteres", MinimumLength = 5)]
    public string? Nome { get; set; }
    [Required(ErrorMessage = "Descrição é obrigatório")]
    [StringLength(300, ErrorMessage = "Descrição deve conter no máximo {1} caracteres")]
    public string? Descricao { get; set; }
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    [Range(1, 10000, ErrorMessage = "Preço deve estar entre {1} e {2}")]
    public decimal Preco { get; set; }
    [Required]
    [StringLength(300, MinimumLength = 10)]
    public string? ImagemUrl { get; set; }
    public float Estoque { get; set; }
    public DateTime DataCadastro { get; set; }

    public int CategoriaId { get; set; }
    [JsonIgnore]
    public Categoria? Categoria { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrEmpty(this.Nome))
        {
            var primeiraLetra = this.Nome[0].ToString();
            if (primeiraLetra != primeiraLetra.ToUpper())
            {
                yield return new ValidationResult("A primeira letra do nome deve ser maiúscula",
                    new[] { nameof(this.Nome) }
                    );
            }
        }

        if (!string.IsNullOrEmpty(this.Descricao))
        {
            var primeiraLetra = this.Descricao[0].ToString();
            if (primeiraLetra != primeiraLetra.ToUpper())
            {
                yield return new ValidationResult("A primeira letra da descrição deve ser maiúscula",
                    new[] { nameof(this.Descricao) }
                    );
            }
        }

        if (this.Estoque <= 0)
        {
            yield return new ValidationResult("O estoque deve ser maior que zero (0)",
             new[] { nameof(this.Estoque) }
             );
        }
    }
}