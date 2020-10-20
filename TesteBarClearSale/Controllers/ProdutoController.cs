using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TesteBarClearSale.Data;
using TesteBarClearSale.Models;
using Microsoft.AspNetCore.Authorization;
using TesteBarClearSale.Services;

namespace TesteBarClearSale.Controllers
{
    
    [Route("v1/produtos")]
    public class ProdutoController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<List<Produto>>> Get(
            [FromServices] DataContext context
        )
        {
            var produtos = await context.Produtos.AsNoTracking().ToListAsync();
            return Ok(produtos);
        }
        [HttpGet]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult<Produto>> GetById(
            int id,
            [FromServices] DataContext context
        )
        {
            var produto = await context.Produtos.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
            return Ok(produto);
        }

        [HttpPost]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<Produto>> Post(
            [FromBody]Produto model,
            [FromServices] DataContext context
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try{                 
                context.Produtos.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }catch(Exception)
            {
                return BadRequest(new { message = "Não foi possível adicionar o Registro." });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult<Produto>> Put(
            int id,
            [FromBody] Produto model,
            [FromServices] DataContext context
        )
        {
            if (model.Id != id)
                return NotFound(new { message = "Produto não encontrado" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                context.Entry<Produto>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }             
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível atualizar o Registro." });
            }
        }
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult<Produto>> Delete(
            int id,
            [FromServices]DataContext context
        )
        {
            var produto = await context.Produtos.FirstOrDefaultAsync(p => p.Id == id);
            if (produto == null)
                return NotFound(new { message = "Registro não encontrado" });

            try
            {
                context.Produtos.Remove(produto);
                await context.SaveChangesAsync();
                return Ok(new { messa = "Registro removido com sucesso"});
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível Remover o Registro." });
            }

        }

    }
}
