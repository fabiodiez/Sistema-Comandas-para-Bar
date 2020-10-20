using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesteBarClearSale.Models
{
    public class ItemComanda
    {
        public ItemComanda()
        {

        }
        public ItemComanda(int comandaid, int produtoid, decimal valorunit, int qtd)
        {
            ComandaId = comandaid;
            ProdutoId = produtoid;
            ValorUnit = valorunit;
            CalcularTotal();
        }
       
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ComandaId { get; set; }
        public int ProdutoId { get; set; }  
        public Produto Produto { get; set; }
        public decimal ValorUnit { get; set; }
        [Range(1, 999, ErrorMessage = "A Qtd. deve ser maior ou igual a 1")]
        public int Qtd  { get; set; }

        public decimal ValorTotal { get; set; }
        public bool Status { get; set; }      
        
        public decimal CalcularTotal()
        {
            ValorUnit = Produto.Valor;
            ValorTotal = ValorUnit * Qtd;
            return ValorTotal;
        }
    }


}
