using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyGoAPI.Models;
using MoneyGoAPI.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MoneyGoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransaccionesController : ControllerBase
    {
        RepositoryTransacciones repo;
        public TransaccionesController(RepositoryTransacciones repo)
        {
            this.repo = repo;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<List<Transacciones>> GetTransaccionesUsuario()
        {
            List<Claim> claims = HttpContext.User.Claims.ToList();
            String jsonusuario = claims.SingleOrDefault(x => x.Type == "UserData").Value;

            Usuarios usuario = JsonConvert.DeserializeObject<Usuarios>(jsonusuario);
            return this.repo.GetTransacciones(usuario.IdUsuario);
        }
        [Authorize]
        [HttpGet]
        [Route("[action]/{idtransaccion}")]
        public ActionResult<Transacciones> GetTransaccion(int idtransaccion)
        {
            Transacciones trnsc = this.repo.BuscarTransacciones(idtransaccion);
            return trnsc;
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<Transacciones> NuevaTransaccion(Transacciones transaccion)
        {


            this.repo.NuevaTransaccion(transaccion);
            return RedirectToAction("GetTransaccionesUsuario");
        }

        [HttpPut]
        [Route("[action]/{idtransaccion}")]
        public ActionResult<Transacciones> Modificar(Transacciones transaccion)
        {
          
            this.repo.ModificarTransaccion(transaccion);
            return RedirectToAction("GetTransaccionesUsuario");
        }

        [HttpDelete]
        [Route("[action]/{idtransaccion}")]
        [Authorize]
        public ActionResult<Transacciones> Eliminar(int idtransaccion)
        {
            this.repo.EliminarTransaccion(idtransaccion);
            return RedirectToAction("GetTransaccionesUsuario");
        }
    }
}
