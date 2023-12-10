using System.ComponentModel.DataAnnotations;

namespace LibApi;

public class Livro
{
    [Key]
    public int LivroId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Titulo { get; set; }

    [Required]
    [MaxLength(100)]
    public string Autor { get; set; }

    [MaxLength(500)]
    public string? Descricao { get; set; }

    public bool Retirado {get; set;}
}
