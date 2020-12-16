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
    public class HorarioController : Controller
    {
        private readonly AppDbContext _db;

        public HorarioController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _db.Horarios.OrderBy(m => m.Hora).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Horario horario)
        {
            if(!ModelState.IsValid)
            {
                return View(horario);
            }
            var horarioCadastrado = await _db.Horarios.FirstOrDefaultAsync(m => m.Hora.TimeOfDay == horario.Hora.TimeOfDay);
            if (horarioCadastrado != null)
            {
                TempData["Msg"] = "Este horário já está cadastrado.";
                return View(horario);
            }
            _db.Horarios.Add(horario);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var horario = await _db.Horarios.FindAsync(id);
            if (horario == null)
            {
                return NotFound();
            }
            return View(horario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Horario horario)
        {
            if (!ModelState.IsValid)
            {
                return View(horario);
            }
            _db.Update(horario);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var horario = await _db.Horarios.FindAsync(id);
            if (horario == null)
            {
                return NotFound();
            }
            return View(horario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var horario = await _db.Horarios.FindAsync(id);
            if (horario == null)
            {
                return NotFound();
            }
            _db.Remove(horario);
            await _db.SaveChangesAsync();
            TempData["Msg"] = "Horário de " + horario.Hora.ToString("HH:mm") + " foi removido.";
            return RedirectToAction(nameof(Index));
        }
    }
}
