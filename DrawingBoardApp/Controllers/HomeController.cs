using DrawingBoardApp.Data;
using DrawingBoardApp.Models;
using DrawingBoardApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DrawingBoardApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new HomeVm
            {
                Boards = await _db.Boards
                    .OrderByDescending(b => b.CreatedAt)
                    .ToListAsync()
            };

            return View(vm);
        }
    }
}
