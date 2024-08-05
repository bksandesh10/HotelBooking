using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using UserData.Model;

namespace UserData.Controllers
{
    [Route("userdata/[controller]")]
    [ApiController]
    public class UserDataController : ControllerBase
    {
        private static List<UserItems> user = new List<UserItems>();
        private readonly string _imageStoragePath;

        public UserDataController()
        {
            _imageStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedImages");
            if (!Directory.Exists(_imageStoragePath))
            {
                Directory.CreateDirectory(_imageStoragePath);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostItem([FromBody] UserItems item)
        {
            if (item.Id != 0 && user.Any(u => u.Id == item.Id))
            {
                return BadRequest("Item with this Id already exists.");
            }

            if (item.Id == 0)
            {
                item.Id = user.Any() ? user.Max(i => i.Id) + 1 : 1;
            }

            if (!string.IsNullOrEmpty(item.ImageUrl) && item.ImageUrl.StartsWith("data:image/jpg;base64,"))
            {
                try
                {
                    var base64Data = item.ImageUrl.Substring(item.ImageUrl.IndexOf(",") + 1);
                    if (IsValidBase64(base64Data))
                    {
                        var imageBytes = Convert.FromBase64String(base64Data);
                        var imageFileName = $"{item.Id}.jpg";
                        var imageFilePath = Path.Combine(_imageStoragePath, imageFileName);

                        await System.IO.File.WriteAllBytesAsync(imageFilePath, imageBytes);

                        item.ImageUrl = $"UploadedImages/{imageFileName}";
                    }
                    else
                    {
                        return BadRequest("Invalid base64 image format.");
                    }
                }
                catch (FormatException)
                {
                    return BadRequest("Invalid base64 image format.");
                }
            }

            user.Add(item);
            return CreatedAtAction(nameof(PostItem), new { id = item.Id }, item);
        }

        [HttpGet]
        public IActionResult GetItems()
        {
            var result = user.Select(u => new
            {
                u.Id,
                u.RoomName,
                u.Description,
                u.Date,
                u.Price,
                u.email,
                u.name, 
                u.check,
                ImageUrl = GetBase64ImageUrl(u.ImageUrl)
            });

            return Ok(result);
        }

        private string? GetBase64ImageUrl(string imageUrl)
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), imageUrl);
            if (System.IO.File.Exists(imagePath))
            {
                var imageBytes = System.IO.File.ReadAllBytes(imagePath);
                var base64Image = Convert.ToBase64String(imageBytes);
                return $"data:image/jpg;base64,{base64Image}";
            }
            return null;
        }

        private bool IsValidBase64(string base64)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out _);
        }
    }
}
