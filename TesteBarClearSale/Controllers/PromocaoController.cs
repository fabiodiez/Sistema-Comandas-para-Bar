using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TesteBarClearSale.Data;
using TesteBarClearSale.Models;
using TesteBarClearSale.Services;
using TesteBarClearSale.Enums;

namespace TesteBarClearSale.Controllers
{
    [Route("v1/promocoes")]
    public class PromocaoController : ControllerBase
    {

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Promocao>>> Get(
            [FromServices] DataContext context
        )
        {
            var promocoes = await context.Promocoes.Include(x => x.Requisitos).AsNoTracking().ToListAsync();
            return Ok(promocoes);
        }
        [HttpGet]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult<Promocao>> GetById(
            int id,
            [FromServices] DataContext context
        )
        {
            var promocao = await context.Promocoes.Include(i => i.Requisitos).AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
            foreach (var item in promocao.Requisitos)
            {
                item.Produto = await context.Produtos.AsNoTracking().Where(p => p.Id == item.ProdutoId).FirstOrDefaultAsync();
            }
            return Ok(promocao);
        }

        [HttpPost]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<Promocao>> Post(
            [FromBody]Promocao model,
            [FromServices] DataContext context
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.Requisitos == null)
            {
                return BadRequest(new { message = "É Necessário Adicionar ao menos 1 Item" });
            }            

            try
            {
                context.Promocoes.Add(model);
                await context.SaveChangesAsync();
                var promocao = await context.Promocoes.Include("Requisitos.Produto").AsNoTracking().Where(p => p.Id == model.Id).FirstOrDefaultAsync();

                //Preenche o Elemento  produtos dentro da Lista de ItensComanda
                //foreach (var item in promocao.Requisitos)
                //{
                //    item.Produto = await context.Produtos.AsNoTracking().Where(p => p.Id == item.ProdutoId).FirstOrDefaultAsync();
                //}
                return Ok(promocao);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível adicionar o Registro." });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult<Promocao>> Put(
            int id,
            [FromBody] Promocao model,
            [FromServices] DataContext context
        )
        {

            if (model.Id != id)
                return NotFound(new { message = "Comanda não encontrada" });

            if (model.Requisitos == null)
            {
                return BadRequest(new { message = "É Necessário Adicionar ao menos 1 Item" });
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var requisitos = model.Requisitos.FirstOrDefault();
                requisitos.PromocaoId = model.Id;

                var promocaoPersistida = await context.Promocoes.Include(i => i.Requisitos).AsNoTracking().Where(p => p.Id == model.Id).FirstOrDefaultAsync();
                promocaoPersistida.Requisitos.Add(requisitos);

                context.Entry<PromocaoRequisito>(requisitos).State = EntityState.Added;
                await context.SaveChangesAsync();

                foreach (var item in promocaoPersistida.Requisitos)
                {
                    item.Produto = await context.Produtos.AsNoTracking().Where(p => p.Id == item.ProdutoId).FirstOrDefaultAsync();
                }
                return Ok(promocaoPersistida);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível atualizar o Registro." });
            }
        }
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult<Promocao>> Delete(
            int id,
            [FromServices]DataContext context
        )
        {
            var promocao = await context.Promocoes.FirstOrDefaultAsync(p => p.Id == id);
            if (promocao == null)
                return NotFound(new { message = "Registro não encontrado" });

            try
            {
                context.Promocoes.Remove(promocao);
                await context.SaveChangesAsync();
                return Ok(new { message = "Registro removido com sucesso" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível Remover o Registro." });
            }

        }
    }
}
