using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesteBarClearSale.Models
{
    public class Usuario
    {
        
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(20, ErrorMessage = "Este Campo deve conter entre 3 e 20 caracteres")]
        [MinLength(3, ErrorMessage = "Este Campo deve conter entre 3 e 20 caracteres")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(20, ErrorMessage = "Este Campo deve conter entre 3 e 20 caracteres")]
        [MinLength(3, ErrorMessage = "Este Campo deve conter entre 3 e 20 caracteres")]
        public string Password { get; set; }
        
        public string Perfil { get; set; } 
    }
}
