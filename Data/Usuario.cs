using System.ComponentModel.DataAnnotations;

namespace API.Data
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }

        public bool Adm { get; set; }

        // Relacionamento com Emprestimos
        public ICollection<Emprestimo>? Emprestimos { get; set; }

        // Relacionamento com Reservas
        public ICollection<Reserva>? Reservas { get; set; }
    }
}
