using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProFit.Web.Data;
using ProFit.Web.Models;
using ProFit.Web.Utility;

namespace ProFit.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminUser)]
    public class FuncionamentoController : Controller
    {
        private readonly AppDbContext _db;

        public FuncionamentoController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var funcionamento = await _db.Funcionamento.FirstOrDefaultAsync();
            if (funcionamento == null)
            {
                return NotFound();
            }
            return View(funcionamento);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var funcionamento = await _db.Funcionamento.FindAsync(id);
            if (funcionamento == null)
            {
                return NotFound();
            }
            return View(funcionamento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Funcionamento funcionamento)
        {
            if (ModelState.IsValid)
            {
                _db.Funcionamento.Update(funcionamento);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(funcionamento);
        }
    }
}
