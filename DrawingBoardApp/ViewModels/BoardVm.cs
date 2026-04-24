using DrawingBoardApp.Models;

namespace DrawingBoardApp.ViewModels
{
    public class BoardVm
    {
        public int BoardId { get; set; }
        public string BoardName { get; set; } = "";
        public string Nickname { get; set; } = "";
        public List<BoardElement> Elements { get; set; } = new();

    }
}
