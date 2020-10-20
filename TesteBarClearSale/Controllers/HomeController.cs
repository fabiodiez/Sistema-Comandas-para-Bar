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

namespace TesteBarClearSale.Controllers
{
    [Route("v1")]
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<dynamic>> Get([FromServices] DataContext context)
        {

            try
            {
                var usuarios = await context.Usuarios.AsNoTracking().ToListAsync();
                if (usuarios.Count != 0 )
                {
                    return BadRequest(new
                    {
                        message = "Não Foi possível processar sua requisição"
                    });
                }
                var usuarioPadrao   = new Usuario { Username = "fabio", Password = "fabio", Perfil = "atendente" };
                var produtoAgua     = new Produto("Água",70,null);
                var produtoCerveja  = new Produto ("Cerveja",5,null);
                var produtoSuco     = new Produto ("Suco",50,3);
                var produtoConhaque    = new Produto("Conhaque",20,null);

                var RequisitoPromo1 = new List<PromocaoRequisito>();
                RequisitoPromo1.Add(new PromocaoRequisito { ProdutoId = 2, QtdMinima = 1 });
                RequisitoPromo1.Add(new PromocaoRequisito { ProdutoId = 3, QtdMinima = 1 });

                var RequisitoPromo2 = new List<PromocaoRequisito>();
                RequisitoPromo2.Add(new PromocaoRequisito { ProdutoId = 2, QtdMinima = 2 });
                RequisitoPromo2.Add(new PromocaoRequisito { ProdutoId = 4, QtdMinima = 3 });


                var promocao1 = new Promocao { Titulo = "Promo 1", Descricao = "Na compra de 1 cerveja e 1 suco, Cerveja sai por R$ 3,0", TipoDesconto = eTipoDesconto.Valor, ProdutoPromocionalId = 2, ValorDesconto  = 2, Requisitos = RequisitoPromo1 };
                var promocao2 = new Promocao { Titulo = "Promo 2", Descricao = "Comprando 3 Conhaques mais 2 Cervejas, Peça uma Água de Graça", TipoDesconto = eTipoDesconto.ProdutoBrinde, ProdutoPromocionalId = 1, ValorDesconto  = 0, Requisitos = RequisitoPromo2 };
            
                context.Usuarios.Add(usuarioPadrao);            
                context.Produtos.Add(produtoAgua);            
                context.Produtos.Add(produtoCerveja);            
                context.Produtos.Add(produtoSuco);            
                context.Produtos.Add(produtoConhaque);        
            
                context.Promocoes.Add(promocao1);        
                context.Promocoes.Add(promocao2);

            
            
                await context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Dados configurados"
                });
            }
            catch (Exception e) {
                return BadRequest(new
                {
                    message = "Não Foi possível processar sua requisição"
                });
            }
        }
    }
}