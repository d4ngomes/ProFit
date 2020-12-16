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
    public class FeriadoController : Controller
    {
        private readonly AppDbContext _db;

        public FeriadoController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _db.Feriados.Where(m => m.Data.Year == DateTime.Now.Year).OrderByDescending(m => m.Data).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Feriado feriado)
        {
            if (!ModelState.IsValid)
            {
                return View(feriado);
            }
            var feriadoCadastrado = await _db.Feriados.FirstOrDefaultAsync(m => m.Data == feriado.Data);
            if (feriadoCadastrado != null)
            {
                TempData["Msg"] = "Esta data já está cadastrada.";
                return View(feriado);
            }
            _db.Feriados.Add(feriado);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var feriado = await _db.Feriados.FindAsync(id);
            if (feriado == null)
            {
                return NotFound();
            }
            return View(feriado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Feriado feriado)
        {
            if (!ModelState.IsValid)
            {
                return View(feriado);
            }
            _db.Update(feriado);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var feriado = await _db.Feriados.FindAsync(id);
            if (feriado == null)
            {
                return NotFound();
            }
            return View(feriado);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var feriado = await _db.Feriados.FindAsync(id);
            if (feriado == null)
            {
                return NotFound();
            }
            _db.Remove(feriado);
            await _db.SaveChangesAsync();
            TempData["Msg"] = "Feriado de " + feriado.Data.ToString("dd/MM/yyyy") + " foi removido.";
            return RedirectToAction(nameof(Index));
        }
    }
}
