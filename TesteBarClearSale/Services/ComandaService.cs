using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TesteBarClearSale.Data;
using TesteBarClearSale.Models;
using TesteBarClearSale.Enums;
using TesteBarClearSale.Services;

namespace TesteBarClearSale.Services
{
    public static class ComandaService
    {

        public static Comanda FecharComanda(int Id,
                 DataContext context)
        {

            var comanda =  context.Comandas.Include("ItensComanda.Produto")
                                   .AsNoTracking()
                                   .Where(c => c.Id == Id)
                                   .FirstOrDefault();

            if (comanda.StatusComanda == eStatusComanda.Fechada)
                throw new Exception("A Comanda está Fechada.");


            foreach (var item in comanda.ItensComanda)
            {
                item.Produto =  context.Produtos.AsNoTracking()
                                .Where(p => p.Id == item.ProdutoId)
                                .FirstOrDefault();
            }
            var promocoes =  context.Promocoes.Include(i => i.Requisitos).AsNoTracking().ToList();

            //Verificando e Aplicando as Promoções.
            foreach (var promocao in promocoes)
            {
                bool aplicarPromocao = true;

                foreach (var requisito in promocao.Requisitos)
                {
                    if (!comanda.ItensComanda.Where(i => i.ProdutoId == requisito.ProdutoId).Any())
                    {
                        aplicarPromocao = false;
                        break;
                    }

                    var listaItens = comanda.ItensComanda.Where(p => p.ProdutoId == requisito.ProdutoId).ToList();
                    var totalItem = 0;
                    foreach (var item in listaItens)
                    {
                        totalItem += item.Qtd;
                    }
                    if (totalItem < requisito.QtdMinima)
                    {
                        aplicarPromocao = false;
                        break;
                    }
                }
                if (aplicarPromocao)
                {
                    var promocaoAplicada = new ComandaPromocao();
                    if (comanda.PromocoesAplicadas == null || !comanda.PromocoesAplicadas.Any())
                        comanda.PromocoesAplicadas = new List<ComandaPromocao>();
                    //Incluido as Promoções aplicadas na comanda 
                    promocaoAplicada.ComandaId = Id;
                    promocaoAplicada.Promocao = promocao;
                    comanda.PromocoesAplicadas.Add(promocaoAplicada);
                    context.Entry<ComandaPromocao>(promocaoAplicada).State = EntityState.Added;
                    context.SaveChanges();
                }
            }


            comanda.CalcularValorFinal();
            comanda.StatusComanda = eStatusComanda.Fechada;
            context.Entry<Comanda>(comanda).State = EntityState.Modified;
            context.SaveChanges();



            comanda = context.Comandas.Include("ItensComanda.Produto")
                        .Include("PromocoesAplicadas.Promocao.ProdutoPromocional")
                        .Include("PromocoesAplicadas.Promocao.Requisitos")
                        .AsNoTracking()
                        .Where(c => c.Id == Id).FirstOrDefault();
            return (comanda);       
        }
         

    }
}
