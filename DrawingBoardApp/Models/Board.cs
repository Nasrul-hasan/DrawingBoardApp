namespace DrawingBoardApp.Models
{
    public class Board
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string CreatedBy { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<BoardElement> Elements { get; set; } = new();
    }
}
