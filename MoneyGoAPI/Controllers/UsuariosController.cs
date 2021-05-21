using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyGo.Helpers;
using MoneyGoAPI.Models;
using MoneyGoAPI.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MoneyGoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        RepositoryTransacciones repo;
        PathProvider PathProvider;
        public UsuariosController(RepositoryTransacciones repo, PathProvider path)
        {
            this.repo = repo;
            this.PathProvider = path;
        }

        private Usuarios GetUsuario() {
            List<Claim> claims = HttpContext.User.Claims.ToList();
            String jsonusuario = claims.SingleOrDefault(x => x.Type == "UserData").Value;
            Usuarios usuario = JsonConvert.DeserializeObject<Usuarios>(jsonusuario);
            return usuario;
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize]
        public ActionResult<Usuarios> GetDataUsuario()
        {
            List<Claim> claims = HttpContext.User.Claims.ToList();
            String jsonusuario = claims.SingleOrDefault(x => x.Type == "UserData").Value;

            Usuarios usuario = JsonConvert.DeserializeObject<Usuarios>(jsonusuario);
            return this.repo.getDataUsuario(usuario.IdUsuario);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [Authorize]
        public ActionResult<Usuarios> GetDataUsuario(int id)
        {

            Usuarios usuario = this.repo.getDataUsuario(id);
            return this.repo.getDataUsuario(usuario.IdUsuario);
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult<Usuarios> ValidarUsuario(String email, String password)
        {


            Usuarios usuario = this.repo.ValidarUsuario(email, password);
            return usuario;
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<Usuarios> NuevoUsuario(String nombreUsuario, String password, String Nombre, String email)
        {
            this.repo.InsertarUsuario(Nombre, nombreUsuario, password, email);
            return RedirectToAction("GetDataUsuario");
        }

        [HttpPut]
        [Route("[action]")]
        [Authorize]
        public ActionResult<Usuarios> ModificarPassword(Usuarios usr)
        {
            Usuarios usuario = GetUsuario();

            string password = Encoding.ASCII.GetString(usr.Password);

            this.repo.CambiarPassword(usuario, password);
            return RedirectToAction("GetTransaccionesUsuario");
        }

        [HttpGet]
        [Route("[action]/{email}")]
        public ActionResult<bool> BuscarEmail(String email)
        { 
            return this.repo.BuscarEmail(email);
        }

        [HttpPut]
        [Route("[action]")]
        [Authorize]
        public async Task<ActionResult<Usuarios>> ModificarImagenAsync(IFormFile imagen)
        {
            List<Claim> claims = HttpContext.User.Claims.ToList();
            String jsonusuario = claims.SingleOrDefault(x => x.Type == "UserData").Value;
            Usuarios usuario = JsonConvert.DeserializeObject<Usuarios>(jsonusuario);

            String filename = imagen.FileName;
            String path = this.PathProvider.MapPath(filename, Folders.Images);
            if (filename != null)
            {
                using (var Stream = new FileStream(path, FileMode.Create))
                {
                    await imagen.CopyToAsync(Stream);
                }
                this.repo.UpdateImagen(usuario.IdUsuario, filename);
            }
         
            return RedirectToAction("GetTransaccionesUsuario");
        }

        [HttpDelete]
        [Route("[action]")]
        [Authorize]
        public ActionResult<Usuarios> EliminarCuenta()
        {
            List<Claim> claims = HttpContext.User.Claims.ToList();
            String jsonusuario = claims.SingleOrDefault(x => x.Type == "UserData").Value;

            Usuarios usuario = JsonConvert.DeserializeObject<Usuarios>(jsonusuario);

            this.repo.EliminarCuenta(usuario.IdUsuario);
            return RedirectToAction("Auth", "Index");
        }

        [HttpGet]
        [Route("[action]/{email}")]       
        public ActionResult<Usuarios> GetUsuarioEmail(String email) 
        {

                this.repo.GetUsuarioEmail(email);
            return RedirectToAction("Auth", "Index");
        }
    }
}
