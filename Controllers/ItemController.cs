using HotelApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace HotelApi.Controllers
{
    [Route("hotelApi/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private static List<HotelRoom> rooms = new List<HotelRoom>
        {
            new HotelRoom{Id= 1, RoomName="Standard" , Description="it is a standard room and provides good experience" ,Date="27 july", Price= 5000},
            new HotelRoom{Id= 2, RoomName="Standard" , Description="it is a standard room and provides good experience",Date="27 july",  Price= 4500},
            new HotelRoom{Id= 3, RoomName="Standard" , Description="it is a standard room and provides good experience",Date="28 july" , Price= 4000},
            new HotelRoom{Id= 4, RoomName="High" , Description="it is a High category room and provides good experience" ,Date="28 july", Price= 3000},
            new HotelRoom{Id= 5, RoomName="High" , Description="it is a High category room and provides good experience" ,Date="29 july", Price= 3200},
            new HotelRoom{Id= 6, RoomName="High" , Description="it is a High category room and provides good experience" , Date="29 july",Price= 3000},
            new HotelRoom{Id= 7, RoomName="Medium" , Description="it is a Medium range room and provides good experience" ,Date="30 july", Price= 2000},
            new HotelRoom{Id= 8, RoomName="Medium" , Description="it is a Medium range room and provides good experience" ,Date="30 july", Price= 1500},
        };

        [HttpGet]
        public IActionResult Get()
        {
            var imageBase64Strings = GetImageBase64Strings();
            var response = rooms.Select((r, index) => new {
                r.Id,
                r.RoomName,
                r.Description,
                r.Date,
                r.Price,
                ImageUrl = imageBase64Strings.Count > index ? imageBase64Strings[index] : null
            }).ToList();

            return Ok(response);
        }

      private static List<string> GetImageBase64Strings()
{
    var baseUrl = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "images");
    var base64Strings = new List<string>();
    for (int i = 1; i <= 9; i++)
    {
        var imageName = $"hotel{i}.jpg";
        var imagePath = Path.Combine(baseUrl, imageName);
        if (System.IO.File.Exists(imagePath))
        {
            var imageBytes = System.IO.File.ReadAllBytes(imagePath);
            var base64String = Convert.ToBase64String(imageBytes);
            base64Strings.Add($"data:image/jpg;base64,{base64String}");
        }
        else
        {
            // Log the image path for debugging purposes
            Console.WriteLine($"Image not found: {imagePath}");
        }
    }
    return base64Strings;
}

    }
}
