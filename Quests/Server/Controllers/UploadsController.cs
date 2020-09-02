using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;

namespace Quests.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadsController : ControllerBase
    {
        private readonly IHostEnvironment _env;

        public UploadsController(IHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string fileName)
        {
            var path = Path.Combine(_env.ContentRootPath, "wwwroot");
            var name = ResizeImage(path, fileName);
            if (name != "")
                return Ok(name);

            return BadRequest();
        }

        private static string ResizeImage(string path, string fileName)
        {
            try
            {
                string imagePrefix = fileName.Remove(0,fileName.LastIndexOf(",", StringComparison.Ordinal));
                byte[] data = Convert.FromBase64String(imagePrefix.TrimStart(','));
                string namePath = "";
                using (Image image = Image.Load(data, out IImageFormat format))
                {
                    string newName = Guid.NewGuid().ToString("N").Substring(0, 12);
                    string formats = format.Name == "JPEG" ? ".jpg" : "." + format.Name.ToLower();
                    string name = newName + formats;

                    
                    image.Save(path + "/images/quests/" + name);
                    namePath = "/images/quests/"+name;

                }

                return namePath;
            }
            catch 
            {
                return "";
            }
        }
    }
}