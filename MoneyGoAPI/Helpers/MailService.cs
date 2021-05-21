using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MoneyGo.Helpers
{
    public class MailService { 
    
        PathProvider pathProvider;
        IWebHostEnvironment env;
        IConfiguration configuration;

        public MailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void SendEmail(String receptor, String asunto, String mensaje)
        {
            MailMessage mail = new MailMessage();
            //UploadService archivo = new UploadService(this.pathProvider, this.configuration);

            String usermail = this.configuration["usuariomail"];
            String passwordmail = this.configuration["passwordmail"];

            mail.From = new MailAddress(usermail);
            mail.To.Add(new MailAddress(receptor));
            mail.Subject = asunto;
            mail.Body = mensaje;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.Normal;

            String smtpserver = this.configuration["host"];
            int port = int.Parse(this.configuration["port"]);
            bool ssl = bool.Parse(this.configuration["ssl"]);
            bool defaultcreadentials = bool.Parse(this.configuration["defaultcredentials"]);

            SmtpClient smtpClient = new SmtpClient();

            smtpClient.Host = smtpserver;
            smtpClient.Port = port;
            smtpClient.EnableSsl = ssl;
            smtpClient.UseDefaultCredentials = defaultcreadentials;

            //Necesario para verificar la cuenta con credenciales
            NetworkCredential usercredential = new NetworkCredential(usermail, passwordmail);

            smtpClient.Credentials = usercredential;
            smtpClient.Send(mail);
        }

        public void SendEmailRegistro(String receptor, String nombre)
        {
            MailMessage mail = new MailMessage();
            //UploadService archivo = new UploadService(this.pathProvider, this.configuration);

            String usermail = this.configuration["usuariomail"];
            String passwordmail = this.configuration["passwordmail"];

            String mensaje = "Hola, " + nombre + ". Gracias por registrarse en MoneyGo. Ya puede empezar a uilizar la aplicacion.";

            mail.From = new MailAddress(usermail);
            mail.To.Add(new MailAddress(receptor));
            mail.Subject = "Gracias por registrarse";
            mail.Body = mensaje;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.Normal;

            String smtpserver = this.configuration["host"];
            int port = int.Parse(this.configuration["port"]);
            bool ssl = bool.Parse(this.configuration["ssl"]);
            bool defaultcreadentials = bool.Parse(this.configuration["defaultcredentials"]);

            SmtpClient smtpClient = new SmtpClient();

            smtpClient.Host = smtpserver;
            smtpClient.Port = port;
            smtpClient.EnableSsl = ssl;
            smtpClient.UseDefaultCredentials = defaultcreadentials;

            //Necesario para verificar la cuenta con credenciales
            NetworkCredential usercredential = new NetworkCredential(usermail, passwordmail);

            smtpClient.Credentials = usercredential;
            smtpClient.Send(mail);
        }

        public void SendEmailRecuperacion(string email, string link)
        {
            MailMessage mail = new MailMessage();
            String usermail = this.configuration["usuariomail"];
            String passwordmail = this.configuration["passwordmail"];

            String mensaje = "Ha recibido este mensaje porque ha solicitado el reseteo de su contraseña. Si no lo ha solicitado, ignore este mensaje.\nSu enlace es: <a href=" + link + ">" + link + "</a>";

            mail.From = new MailAddress(usermail);
            mail.To.Add(new MailAddress(email));
            mail.Subject = "Reseteo de contraseña";
            mail.Body = mensaje;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.Normal;

            String smtpserver = this.configuration["host"];
            int port = int.Parse(this.configuration["port"]);
            bool ssl = bool.Parse(this.configuration["ssl"]);
            bool defaultcreadentials = bool.Parse(this.configuration["defaultcredentials"]);

            SmtpClient smtpClient = new SmtpClient();

            smtpClient.Host = smtpserver;
            smtpClient.Port = port;
            smtpClient.EnableSsl = ssl;
            smtpClient.UseDefaultCredentials = defaultcreadentials;

            NetworkCredential usercredential = new NetworkCredential(usermail, passwordmail);

            smtpClient.Credentials = usercredential;
            smtpClient.Send(mail);
        }
    }
}
