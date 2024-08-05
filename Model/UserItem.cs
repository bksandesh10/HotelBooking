namespace UserData.Model
{
    public class UserItems
    {
        public int Id { get; set; }
        public required string RoomName { get; set; }
        public required string Description { get; set; }
        public required string Date { get; set; }
        public required decimal Price { get; set; }
        public required string ImageUrl { get; set; }
        public required string email {get; set;}
        public required string name {get; set;}
        public bool check {get; set;}
    }
}
