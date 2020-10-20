using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TesteBarClearSale.Models;
using TesteBarClearSale.Enums;

namespace TesteBarClearSale.Tests.Domain
{
    [TestClass]
    public class ComandaTest
    {
        private readonly Produto _produto = new Produto("Água",70,null);
        private readonly ItemComanda _itemComanda = new ItemComanda(1,1,70,2);
        

        [TestMethod]
        [TestCategory("Domain")]

        public void Dado_uma_novo_item_na_comanda_o_valor_total_do_item_deve_ser_210()
        {
            var itemcomanda = new ItemComanda(1, 1, 70, 3);            
            Assert.AreEqual(210, itemcomanda.ValorTotal);
        }
        

        [TestMethod]
        [TestCategory("Domain")]
        public void Dado_uma_nova_comanda_o_subtotal_deve_ser_140()
        {
            var comanda = new Comanda(972,eStatusComanda.Aberta,_itemComanda);            
            Assert.AreEqual(140, comanda.ValorFinal);
        }

    }
}