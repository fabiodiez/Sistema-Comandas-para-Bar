using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using TesteBarClearSale.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesteBarClearSale.Models
{
    public class Promocao
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]        
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(30, ErrorMessage = "Este Campo deve conter entre 3 e 30 caracteres")]
        [MinLength(3, ErrorMessage = "Este Campo deve conter entre 3 e 30 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(100, ErrorMessage = "Este Campo deve conter entre 3 e 100 caracteres")]
        [MinLength(3, ErrorMessage = "Este Campo deve conter entre 3 e 100 caracteres")]
        public string Descricao { get; set; }

        public virtual List<PromocaoRequisito> Requisitos { get; set; }
        
        [Required(ErrorMessage = "Este campo é obrigatório")]        
        public eTipoDesconto TipoDesconto { get; set; }
        public int ProdutoPromocionalId { get; set; }
        public Produto ProdutoPromocional { get; set; }
            
        public decimal ValorDesconto { get; set; }     

    }
}
