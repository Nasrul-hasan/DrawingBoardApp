using System.Text.Json;
using DrawingBoardApp.Data;
using DrawingBoardApp.Models;
using Microsoft.AspNetCore.SignalR;
namespace DrawingBoardApp.Hubs
{
    public class BoardHub : Hub
    {
        private readonly AppDbContext _db;

        public BoardHub(AppDbContext db)
        {
            _db = db;
        }
        public async Task JoinBoard(string boardId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, boardId);

            var elements = _db.BoardElements
                .Where(x => x.BoardId == int.Parse(boardId))
                .ToList();

            foreach (var el in elements)
            {
                if (string.IsNullOrWhiteSpace(el.Data))
                {
                    continue;
                }

                var stroke = System.Text.Json.JsonSerializer.Deserialize<object>(el.Data);
                await Clients.Caller.SendAsync("ReceiveStroke", stroke);
            }
        }

        public async Task SendStroke(string boardId, object stroke)
        {
            var element = new BoardElement
            {
                BoardId = int.Parse(boardId),
                Data = System.Text.Json.JsonSerializer.Serialize(stroke)
            };

            _db.BoardElements.Add(element);
            await _db.SaveChangesAsync();

            await Clients.Group(boardId).SendAsync("ReceiveStroke", stroke);
        }
        public async Task ClearBoard(string boardId)
        {
            int id = int.Parse(boardId);

            var elements = _db.BoardElements.Where(x => x.BoardId == id);
            _db.BoardElements.RemoveRange(elements);
            await _db.SaveChangesAsync();

            await Clients.Group(boardId).SendAsync("BoardCleared");
        }
    }
}
