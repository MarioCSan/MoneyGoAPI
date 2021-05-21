using MoneyGo.Helpers;
using MoneyGoAPI.Data;
using MoneyGoAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyGoAPI.Repositories
{
    public class RepositoryTransacciones
    {
        TransaccionesContext context;
        MailService MailService;

        public RepositoryTransacciones(TransaccionesContext context, MailService MailService)
        {
            this.context = context;
            this.MailService = MailService;
        }

        #region management

        public Usuarios getDataUsuario(int id)
        {
            return this.context.Usuarios.Where(z => z.IdUsuario == id).FirstOrDefault();
        }

        public void UpdateImagen(int idusuario, String imagen)
        {
            Usuarios user = this.getDataUsuario(idusuario);
            user.ImagenUsuario = imagen;
            this.context.SaveChanges();
        }

        #endregion

        #region transacciones
        public List<Transacciones> GetTransacciones(int idusuario)
        {
            var consulta = from datos in this.context.Transacciones
                           where datos.IdUsuario == idusuario
                           select datos;

            if (consulta.Count() == 0)
            {
                return null;
            }
            return consulta.ToList();
        }



        public Transacciones BuscarTransacciones(int IdTransaccion)
        {
            return this.context.Transacciones.Where(z => z.IdTransaccion == IdTransaccion).FirstOrDefault();
        }

        public void NuevaTransaccion(Transacciones transaccion)
        {
            var consulta = from datos in this.context.Transacciones
                           select datos.IdTransaccion;


            int maxId = consulta.Max();

            Transacciones trnsc = new Transacciones();
            trnsc.IdTransaccion = maxId + 1;
            trnsc.IdUsuario = transaccion.IdUsuario;
            trnsc.Cantidad = transaccion.Cantidad;
            trnsc.TipoTransaccion = transaccion.TipoTransaccion;
            trnsc.Concepto = transaccion.Concepto;
            trnsc.FechaTransaccion = transaccion.FechaTransaccion;
            this.context.Add(trnsc);
            this.context.SaveChanges();

        }

        public void ModificarTransaccion(Transacciones trnsc)
        {
            Transacciones transaccion = this.BuscarTransacciones(trnsc.IdTransaccion);
            transaccion.Cantidad = trnsc.Cantidad;
            transaccion.TipoTransaccion = trnsc.TipoTransaccion;
            transaccion.Concepto = trnsc.Concepto;

            this.context.SaveChanges();
        }

        public void EliminarTransaccion(int idtransaccion)
        {
            //RGPD.¿Se que almacenar los datos X tiempo?¿Necesario campo extra a nulo o booleano para que no se muestre?
            Transacciones trnsc = this.BuscarTransacciones(idtransaccion);
            this.context.Transacciones.Remove(trnsc);
            this.context.SaveChanges();
        }



        public List<Transacciones> GetTransaccionesAsc(int idusuario, string tipoTransaccion)
        {
            var consulta = (from datos in this.context.Transacciones
                            where datos.IdUsuario == idusuario && datos.TipoTransaccion == "Ingreso"
                            select datos).OrderBy(x => x.Cantidad);

            if (consulta.Count() == 0)
            {
                return null;
            }
            return consulta.ToList();
        }

        public List<Transacciones> GetTransaccionesDesc(int idusuario, string tipoTransaccion)
        {
            var consulta = (from datos in this.context.Transacciones
                            where datos.IdUsuario == idusuario && datos.TipoTransaccion == "Ingreso"
                            select datos).OrderByDescending(x => x.Cantidad);

            if (consulta.Count() == 0)
            {
                return null;
            }
            return consulta.ToList();
        }


        #endregion

        #region usuariosLogin

        public Usuarios ExisteUsuario(String Email, Byte[] password)
        {
            return this.context.Usuarios.SingleOrDefault(x => x.Email == Email && x.Password == password);
        }

        public bool BuscarEmail(String email)
        {
            bool emailValido = false;
            var consulta = from datos in this.context.Usuarios
                           where datos.Email == email
                           select datos;

            if (consulta != null)
            {
                emailValido = true;
            }
            return emailValido;
        }

        public Usuarios GetUsuarioEmail(String email)
        {

            return this.context.Usuarios.SingleOrDefault(x => x.Email == email);

        }

        public bool BuscarEmailRecuperacion(String email)
        {
            bool emailValido = false;
            var consulta = from datos in this.context.Usuarios
                           where datos.Email == email
                           select datos;

            if (consulta != null)
            {
                emailValido = true;
            }
            return emailValido;
        }
        //Storedprocedure para el alta de usuario??
        public void InsertarUsuario(String nombreUsuario, String password, String Nombre, String email)
        {

            var consulta = from datos in this.context.Usuarios
                           select datos.IdUsuario;

            int maxId = consulta.Max() + 1;

            Usuarios user = new Usuarios();
            user.IdUsuario = maxId;
            user.Nombre = Nombre;
            user.NombreUsuario = nombreUsuario;
            user.Email = email;
            String salt = CypherService.GetSalt();
            user.Salt = salt;
            user.Password = CypherService.CifrarContenido(password, salt);

            this.context.Usuarios.Add(user);
            this.context.SaveChanges();

            this.MailService.SendEmailRegistro(email, Nombre);

        }

        public void CambiarPassword(Usuarios usuario, string password)
        {
            Usuarios user = usuario;

            String salt = CypherService.GetSalt();
            user.Salt = salt;
            user.Password = CypherService.CifrarContenido(password, salt);

            this.context.SaveChanges();

        }

        public Usuarios ValidarUsuario(String email, String password)
        {
            Usuarios user = this.context.Usuarios.Where(z => z.Email == email).FirstOrDefault();
            if (user == null)
            {
                return null;
            }
            else
            {
                String salt = user.Salt;
                byte[] passbbdd = user.Password;
                byte[] passtmp = CypherService.CifrarContenido(password, salt);
                // comparar array bytes[]
                bool respuesta =
                HelperToolkit.CompararArrayBytes(passbbdd, passtmp);
                if (respuesta)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        public String GetEmail(int idusuario)
        {
            Usuarios user = this.context.Usuarios.Where(z => z.IdUsuario == idusuario).FirstOrDefault();

            string email = user.Email;
            return email;
        }

        public void EliminarCuenta(int idusuario)
        {
            Usuarios user = this.context.Usuarios.Where(z => z.IdUsuario == idusuario).FirstOrDefault();
            this.context.Usuarios.Remove(user);
            this.context.SaveChanges();
        }
        #endregion

        public string GenerarToken()
        {
            Random rnd = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, 16).Select(s => s[rnd.Next(s.Length)]).ToArray());
        }
    }
}