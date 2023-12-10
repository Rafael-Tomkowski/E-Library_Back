using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibApi;

namespace API.Data
{
    public class Emprestimo
    {
        [Key]
        public int EmprestimoId { get; set; }

        [Required]
        public int LivroId { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public DateTime DataRetirada { get; set; }

        [Required]
        public DateTime DataDevolucaoEsperada { get; set; }

        public DateTime? DataDevolucao { get; set; }

        // Chave estrangeira para Livros
        [ForeignKey("LivroId")]
        public Livro? Livro { get; set; }

        // Chave estrangeira para Usuarios
        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }
    }
}
