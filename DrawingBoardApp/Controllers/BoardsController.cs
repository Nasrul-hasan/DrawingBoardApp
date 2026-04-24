using DrawingBoardApp.Data;
using DrawingBoardApp.Models;
using DrawingBoardApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DrawingBoardApp.Controllers
{
    public class BoardsController : Controller
    {
        private readonly AppDbContext _db;

        public BoardsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string nickname, string boardName)
        {
            if (string.IsNullOrWhiteSpace(nickname) || string.IsNullOrWhiteSpace(boardName))
            {
                return RedirectToAction("Index", "Home");
            }

            var board = new Board
            {
                Name = boardName.Trim(),
                CreatedBy = nickname.Trim()
            };

            _db.Boards.Add(board);
            await _db.SaveChangesAsync();

            return RedirectToAction("Room", new { id = board.Id, nickname = nickname.Trim() });
        }

        [HttpGet]
        public async Task<IActionResult> Room(int id, string nickname)
        {
            var board = await _db.Boards
                .Include(b => b.Elements)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (board == null)
                return NotFound();

            var vm = new BoardVm
            {
                BoardId = board.Id,
                BoardName = board.Name,
                Nickname = nickname ?? "Guest",
                Elements = board.Elements.OrderBy(e => e.CreatedAt).ToList()
            };

            return View(vm);
        }
    }
}
