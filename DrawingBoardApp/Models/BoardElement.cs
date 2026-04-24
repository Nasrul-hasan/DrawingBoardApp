namespace DrawingBoardApp.Models
{
    public class BoardElement
    {
        public int Id { get; set; }

        public int BoardId { get; set; }
        public string? Data { get; set; } // JSON
        public Board? Board { get; set; }

        public string Type { get; set; } = "pen";
        public string DataJson { get; set; } = "";
        public string Color { get; set; } = "#000000";
        public float StrokeWidth { get; set; } = 2;
        public string CreatedBy { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
