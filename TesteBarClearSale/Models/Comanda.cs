using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TesteBarClearSale.Enums;

namespace TesteBarClearSale.Models
{
    public class Comanda
    {
        public Comanda(int codigoComanda, eStatusComanda statuscomanda, ItemComanda itemcomanda) {
            CodigoComanda = codigoComanda;
            StatusComanda = statuscomanda;
            ItensComanda.Add(itemcomanda);
            CalcularSubTotal();
            CalcularValorFinal();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Range(1, 99999, ErrorMessage = "O Código da Comanda deve ser maior ou igual a 1")]
        public int CodigoComanda { get; set; }        
        public virtual List<ComandaPromocao> PromocoesAplicadas { get; set; }        
        public decimal SubTotal { get; set; }        
        public decimal ValorFinal { get; set; }        
        public eStatusComanda StatusComanda { get; set; }

        public virtual List<ItemComanda> ItensComanda { get; set; }
         

        public void CalcularSubTotal()
        {
            decimal total = 0;
            foreach (var item in ItensComanda)
            {
                total += item.CalcularTotal();
            }
            SubTotal = total;
        }
        public void CalcularValorFinal()
        {
            CalcularSubTotal();
            decimal total = SubTotal;
            if (PromocoesAplicadas != null && PromocoesAplicadas.Any())
            {
                foreach (var promocao in PromocoesAplicadas)
                    total -= promocao.Promocao.ValorDesconto;
            }
             
           
            //total -= Descontos != null ? Descontos.Value() : 0;
            ValorFinal = total;
        }


    }
}
