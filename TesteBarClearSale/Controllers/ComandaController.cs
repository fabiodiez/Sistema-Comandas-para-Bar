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

    [Route("v1/comandas")]
    public class ComandaController : ControllerBase
    {

        #region Métodos Públicos  

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<List<Comanda>>> Get(
            [FromServices] DataContext context
        )
        {
            var comandas = await context.Comandas
                                .Include("ItensComanda.Produto")
                                .Include("PromocoesAplicadas.Promocao.Requisitos")
                                .Include("PromocoesAplicadas.Promocao.Requisitos.Produto")
                                .AsNoTracking().ToListAsync();
            try {
                foreach (var comanda in comandas)
                {
                    if (comanda.ItensComanda != null && comanda.ItensComanda.Any()) { 
                        comanda.ItensComanda.FirstOrDefault().CalcularTotal();
                        comanda.CalcularSubTotal();
                    }
                }                
            } catch (Exception)
            {
                return BadRequest(new { message = "Nao Foi Possivel Consultar as Comandas;" });
            }
            return Ok(comandas);
        }
        [HttpGet]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult<Comanda>> GetById(
            int id,
            [FromServices] DataContext context
        )
        {
            var comanda = await context.Comandas.Include("ItensComanda.Produto")
                                .Include("PromocoesAplicadas.Promocao.Requisitos")
                                .Include("PromocoesAplicadas.Promocao.Requisitos.Produto")
                                .Where(x => x.Id == id)
                                .FirstOrDefaultAsync();
            if (comanda.ItensComanda != null && comanda.ItensComanda.Any()) { 
                comanda.ItensComanda.FirstOrDefault().CalcularTotal();
                comanda.CalcularSubTotal();
            }
            return Ok(comanda);
        }

        [HttpPost]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<Comanda>> Post(
            [FromBody]Comanda model,
            [FromServices] DataContext context
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.ItensComanda == null)
            {
                return BadRequest(new { message = "É Necessário Adicionar ao menos 1 Item" });
            }

            //Verifica se possui alguma comanda aberta com o mesmo codigo.
            var comanda = await context.Comandas.Include("ItensComanda.Produto")
                                .AsNoTracking()
                                .Where(p => p.CodigoComanda == model.CodigoComanda && p.StatusComanda == eStatusComanda.Aberta)
                                .FirstOrDefaultAsync();

            if (comanda != null)
                return BadRequest(new { message = "Já existe uma comanda aberta com este código." });

            try
            {

                context.Comandas.Add(model);               
                await context.SaveChangesAsync();
                comanda = await context.Comandas.Where(p => p.Id == model.Id)
                                .Include("ItensComanda.Produto")
                                .AsNoTracking()
                                .FirstOrDefaultAsync();

                comanda.ItensComanda.FirstOrDefault().CalcularTotal();
                comanda.CalcularSubTotal();



                return Ok(comanda);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível adicionar o Registro." });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult<Comanda>> Put(
            int id,
            [FromBody] Comanda model,
            [FromServices] DataContext context
        )
        {

            if (model.Id != id)
                return NotFound(new { message = "Comanda não encontrada" });

            if (model.ItensComanda == null)
            {
                return BadRequest(new { message = "É Necessário Adicionar ao menos 1 Item" });
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                               
                var itensComanda = model.ItensComanda.FirstOrDefault();
                
                itensComanda.ComandaId = model.Id;
             
                var comandaPersistida = await context.Comandas.Include("ItensComanda.Produto")
                                        .AsNoTracking()
                                        .Where(p => p.Id == model.Id)
                                        .FirstOrDefaultAsync();
                if (comandaPersistida.StatusComanda == eStatusComanda.Fechada)
                    return BadRequest(new { message = "A Comanda está Fechada." });

                //Verificar o limite permitido de produtos
                var produto = await context.Produtos.AsNoTracking()
                             .Where(p => p.Id == itensComanda.ProdutoId).FirstOrDefaultAsync();

                var QtdProdutoAdicionados = comandaPersistida.ItensComanda.Where(p => p.ProdutoId == produto.Id).Count();                                


                if (produto.LimitePorComanda != null 
                        && (produto.LimitePorComanda < QtdProdutoAdicionados + itensComanda.Qtd))
                {
                    return BadRequest(new { message = "Quantidade Máxima Permitida do Produto " 
                                                        + produto.Descricao 
                                                        + " é de " + produto.LimitePorComanda.ToString() 
                                                        + " unidades" });
                }

                itensComanda.Produto = await context.Produtos.AsNoTracking().Where(p => p.Id == itensComanda.ProdutoId).FirstOrDefaultAsync();
                comandaPersistida.ItensComanda.Add(itensComanda);
                itensComanda.CalcularTotal();
                comandaPersistida.CalcularSubTotal();


                context.Entry<Comanda>(comandaPersistida).State = EntityState.Modified;
                await context.SaveChangesAsync();

                context.Entry<ItemComanda>(itensComanda).State = EntityState.Added;
                await context.SaveChangesAsync();


                return Ok(comandaPersistida);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível atualizar o Registro." });
            }
        }
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult<Comanda>> Delete(
            int id,
            [FromServices]DataContext context
        )
        {
            var comanda = await context.Comandas.FirstOrDefaultAsync(p => p.Id == id);
             
            if (comanda == null)
                return NotFound(new { message = "Registro não encontrado" });

            try
            {
                context.Comandas.Remove(comanda);
                await context.SaveChangesAsync();
                return Ok(new { message = "Registro removido com sucesso" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível Remover o Registro." });
            }

        }


        [HttpPost]
        [Route("resetarcomanda/{id:int}")]
        [Authorize]
        public async Task<ActionResult<Comanda>> PostResetarComanda(
             int Id,
            [FromServices] DataContext context
        )
        {
            if (Id <= 0)
                return BadRequest(new { message = "Não foi possível Resetar a Comanda." });

            var comanda = await context.Comandas.Include("ItensComanda.Produto")
                                .AsNoTracking()
                                .Where(c => c.Id == Id)
                                .FirstOrDefaultAsync();

            if (comanda.StatusComanda == eStatusComanda.Fechada)
                return BadRequest(new { message = "A Comanda está Fechada." });
            try
            {
                if (comanda.ItensComanda.Any())

                {                           
                    foreach (var item in comanda.ItensComanda) {
                        context.ItensComanda.Remove(item);                        
                    }
                }
                comanda.SubTotal = 0;
                context.Entry<Comanda>(comanda).State = EntityState.Modified;
                await context.SaveChangesAsync();
                comanda = await context.Comandas.Include("ItensComanda.Produto")
                                .AsNoTracking()
                                .Where(c => c.Id == Id)
                                .FirstOrDefaultAsync();
                return Ok(comanda);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível resetar a comanda." });
            }
        }



        [HttpPost]
        [Route("fecharcomanda/{id:int}")]
        [Authorize]
        public async Task<ActionResult<Comanda>> PostFecharComanda(
            int Id,
            [FromServices] DataContext context
        )
        {
            if (Id <= 0)
                return BadRequest(new { message = "Não foi possível Fechar a Comanda." });
            try
            {              

                return Ok(ComandaService.FecharComanda(Id, context));
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível Fechar a Comanda." });
            }
        }



        #endregion


        #region Métodos Privados
        
        #endregion
    }
}

