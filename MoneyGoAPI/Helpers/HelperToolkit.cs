using Newtonsoft.Json;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyGo.Helpers
{
    public class HelperToolkit
    {
    

        public static bool CompararArrayBytes(byte[] a, byte[] b)
        {
            bool iguales = true;
            if (a.Length != b.Length)
            {
                iguales = false;
            }
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i].Equals(b[i]) == false)
                {
                    iguales = false;
                    break;
                }
            }
            return iguales;
        }

        public static String Normalize(String filename)
        {
            String extension = System.IO.Path.GetExtension(filename).Trim('.');
            bool valido = ValidarFormatoImagen(extension);
            if (valido)
            {
                string name = System.IO.Path.GetFileNameWithoutExtension(filename);

                HashSet<char> removeChars = new HashSet<char>(" ?&^$#@!()+´`^`·¨-,:;<>’\'-_*=");
                StringBuilder result = new StringBuilder(name.Length);
                foreach (char c in name)
                {
                    if (!removeChars.Contains(c))
                    {
                        result.Append(c);
                    }
                }

                return result.ToString() + '.' + extension;
            }
            else 
            {
                //TempData["name"] = "La extensión de la imagen no es válida. Los formatos válidos son: .jpg, .png y .gif";
                return "error";
            }
        }

        public static bool ValidarFormatoImagen(String extension)
        {
            extension = extension.ToUpper();
            List<String> extensionesValidas = new List<string>{ "JPG", "PNG", "JPEG", "GIF" };

            foreach(String ext in extensionesValidas)
            {
                if (extension == ext)
                {
                    return true;
                }
            }
            return false;
        }

        public static String SerializeJsonObject(Object objeto)
        {
            String respuesta = JsonConvert.SerializeObject(objeto);
            return respuesta;
        }

        //MEtodo que recibira un String json y devolvera un json
        public static Object DeserializeJsonObject(String json, Type type)
        {
            Object respuesta = JsonConvert.DeserializeObject(json, type);
            return respuesta;
        }


        internal static T DeserializeJsonObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
