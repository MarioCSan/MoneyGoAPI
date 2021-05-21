using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyGo.Helpers
{
    public enum Folders
    {
        Images = 0, Documents = 1, tmp = 2
    }

    public class PathProvider
    {
        IWebHostEnvironment env;

        public PathProvider(IWebHostEnvironment env)
        {
            this.env = env;
        }

        //Metodos para devolver la ruta a ficheros
        public String MapPath(String filename, Folders folder)
        {
            String carpeta = ""; //Documents o images

            if (folder == Folders.Documents)
            {
                carpeta = "documents";
            }
            else if (folder == Folders.Images)
            {
                carpeta = "Images";
            }
            else if (folder == Folders.tmp)
            {
                carpeta = "tmp";
            }
            String path = Path.Combine(this.env.WebRootPath, carpeta, filename);
            return path;
        }
    }
}