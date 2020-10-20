using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesteBarClearSale.Models
{
    public class Produto
    {   
        public Produto()
        {

        }
        public Produto(string descricao, decimal valor, int? limiteporcomanda)
        {
            Descricao = descricao;
            Valor = valor;
            LimitePorComanda = limiteporcomanda;
        }
       

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(30, ErrorMessage = "Este Campo deve conter entre 3 e 30 caracteres")]
        [MinLength(3, ErrorMessage = "Este Campo deve conter entre 3 e 30 caracteres")]
        public string Descricao { get; set; }
        
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Range(0,99999, ErrorMessage = "O Preço deve ser maior ou igual a zero")]
        public decimal Valor { get; set; }
        
        public int? LimitePorComanda { get; set; }


    }
}
