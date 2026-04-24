using DrawingBoardApp.Models;

namespace DrawingBoardApp.ViewModels
{
    public class HomeVm
    {
        public string Nickname { get; set; } = "";
        public string NewBoardName { get; set; } = "";
        public List<Board> Boards { get; set; } = new();
    }
}
