using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using LibApi;

namespace API.Data
{
    public class Reserva
    {
        [Key]
        public int ReservaId { get; set; }

        [Required]
        public int LivroId { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public DateTime DataReserva { get; set; }

        // Chave estrangeira para Livros
        [ForeignKey("LivroId")]
        public Livro? Livro { get; set; }

        // Chave estrangeira para Usuarios
        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }
    }
}
