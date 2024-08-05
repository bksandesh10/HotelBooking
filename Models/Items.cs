namespace HotelApi.Models {

    public class HotelRoom {
        public int Id {get; set;}
        public required string RoomName {get; set;}
        public required string Description {get; set;}
        public int Price {get; set;}
        public required string Date {get; set;}
        

    }
}