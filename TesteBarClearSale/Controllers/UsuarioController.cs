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



namespace TesteBarClearSale.Controllers
{
    [Route("v1/usuarios")]
    public class UsuarioController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "gerente")]
        public async Task<ActionResult<List<Usuario>>> Get([FromServices] DataContext context)
        {
            var users = await context
                .Usuarios
                .AsNoTracking()
                .ToListAsync();
            return users;
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<Usuario>> Post(
            [FromServices] DataContext context,
            [FromBody]Usuario model)
        {
            // Verifica se os dados são válidos
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Força o usuário a ser sempre "funcionário"
                model.Perfil = "atendente";

                context.Usuarios.Add(model);
                await context.SaveChangesAsync();

                // Esconde a senha
                model.Password = "";
                return model;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível criar o usuário" });

            }
        }



        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "gerente")]
        public async Task<ActionResult<Usuario>> Put(
         [FromServices] DataContext context,
         int id,
         [FromBody]Usuario model)
        {
            // Verifica se os dados são válidos
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verifica se o ID informado é o mesmo do modelo
            if (id != model.Id)
                return NotFound(new { message = "Usuário não encontrada" });

            try
            {
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível criar o usuário" });

            }
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate(
                    [FromServices] DataContext context,
                    [FromBody]Usuario model)
        {
            var user = await context.Usuarios
                .AsNoTracking()
                .Where(x => x.Username == model.Username && x.Password == model.Password)
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            var token = TokenService.GenerateToken(user);
            // Esconde a senha
            user.Password = "";
            return new
            {
                user = user,
                token = token
            };
        }

        [HttpPost]
        [Route("validatoken")]
        [Authorize]
        public ActionResult<dynamic> ValidaToken()
        {            
            return Ok(new
            {
                logado = true
            });
        }

    }

}
