using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.Helpers
{
    public class ImageHelper
    {
        public readonly string wwwRootPath;
        private readonly string subDirectory;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wwwRootPath"></param>
        /// <param name="subDirectory">Промежуточнавя папка (без /)</param>
        public ImageHelper(string wwwRootPath, string subDirectory)
        {
            this.wwwRootPath = wwwRootPath;
            this.subDirectory = subDirectory;
        }
        public async Task<string> SaveImage(IFormFile image)
        {
            DirectoryInfo di = new DirectoryInfo(wwwRootPath + "/images/" + subDirectory);
            if (!di.Exists)
                di.Create();

            string fileName = Path.GetRandomFileName();
            string extension = Path.GetExtension(image.FileName);

            fileName = fileName + extension;
            string path = Path.Combine(wwwRootPath + "/images" + $"/{subDirectory}/" + fileName);

            using (var fs = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(fs);
            }
            return "/images" + $"/{subDirectory}/" + fileName;
        }
        public async Task<bool> DeletePreviousImage(string path)
        {
            FileInfo fi = new FileInfo(wwwRootPath + '/' + path);
            if (fi.Exists)
            {
                fi.Delete();
                return true;
            }
            return false;

        }
    }
}
