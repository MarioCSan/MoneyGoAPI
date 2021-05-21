using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MoneyGo.Helpers
{
    public class CypherService
    {

        public static String GetSalt()
        {
            Random random = new Random();
            String salt = "";

            for (int i = 1; i <= 50; i++)
            {
                int rnd = random.Next(0, 255);
                char letra = Convert.ToChar(rnd);
                salt += letra;
            }
            return salt;
        }

        public static byte[] CifrarContenido(string password, string salt)
        {
            //PARA EL SALT, PUES SE ALMACENA ENTRE MEDIAS
            //DEL CONTENIDO, EN POSICIONES QUE YO QUIERO...
            String contenidosalt = password + salt;
            SHA256Managed sha = new SHA256Managed();
            byte[] salida;
            salida = Encoding.UTF8.GetBytes(contenidosalt);
            //CIFRAMOS EL NUMERO DE ITERACIONES QUE NOS INDICAN
            for (int i = 1; i <= 100; i++)
            {
                //REALIZAMOS EL CIFRADO n VECES
                salida = sha.ComputeHash(salida);
            }
            sha.Clear();
            return salida;
        }
    }
}
