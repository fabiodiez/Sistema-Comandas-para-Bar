using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TesteBarClearSale.Models
{
    
    public class PromocaoRequisito
    {
        
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? PromocaoId { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public int QtdMinima { get; set; }
    }
}
