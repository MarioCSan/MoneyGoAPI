using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace MoneyGo.Helpers
{
    internal class UploadService
    {
        PathProvider pathProvider;

        public UploadService(PathProvider pathProvider, IConfiguration configuration)
        {
            this.pathProvider = pathProvider;
        }

        public async Task<String> UploadFileAsync(IFormFile fichero, Folders folder)
        {
            
            String filename = HelperToolkit.Normalize(fichero.FileName);
            String path = this.pathProvider.MapPath(filename, Folders.Images);

            if (filename.ToLower() != "error")
            {
                using (var Stream = new FileStream(path, FileMode.Create))
                {
                    if (HelperToolkit.ValidarFormatoImagen(filename.ToLower()))
                    {
                        await fichero.CopyToAsync(Stream);
                    }
                }
            }


            return path;
        }
    }
}