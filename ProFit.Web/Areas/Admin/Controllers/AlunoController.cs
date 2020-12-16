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
using ProFit.Web.Services;
using X.PagedList;

namespace ProFit.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminUser)]
    public class AlunoController : Controller
    {
        private readonly AppDbContext _db;

        public AlunoController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(string busca, int? pagina)
        {
            int itensPorPagina = 4;
            int numeroPagina = (pagina ?? 1);

            if (!String.IsNullOrEmpty(busca))
            {
                itensPorPagina = 1000;
                var alunos = _db.Alunos.Where(m => m.Email.Contains(busca)).OrderBy(m => m.Nome);
                return View(nameof(Index), await alunos.ToPagedListAsync(numeroPagina, itensPorPagina));
            }
            return View(await _db.Alunos.OrderBy(m => m.Nome).ToPagedListAsync(numeroPagina, itensPorPagina));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProFit.Web.Models.Aluno aluno)
        {
            if(!ModelState.IsValid)
            {
                return View(aluno);
            }
            if (ValidaCpf.IsValid(aluno.Cpf))
            {
                await _db.AddAsync(aluno);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            TempData["Msg"] = "Cpf inválido";
            return View(aluno);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var aluno = await _db.Alunos.FindAsync(id);
            if (aluno == null)
            {
                return NotFound();
            }
            return View(aluno);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var aluno = await _db.Alunos.FindAsync(id);
            if (aluno == null)
            {
                return NotFound();
            }
            return View(aluno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProFit.Web.Models.Aluno aluno)
        {
            if (!ModelState.IsValid)
            {
                return View(aluno);
            }
            if (ValidaCpf.IsValid(aluno.Cpf))
            {
                _db.Update(aluno);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            TempData["Msg"] = "Cpf inválido";
            return View(aluno);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var aluno = await _db.Alunos.FindAsync(id);
            if (aluno == null)
            {
                return NotFound();
            }
            return View(aluno);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var aluno = await _db.Alunos.FindAsync(id);
            if (aluno == null)
            {
                return NotFound();
            }
            _db.Alunos.Remove(aluno);
            await _db.SaveChangesAsync();
            TempData["Msg"] = "Aluno " + aluno.Nome.ToUpper() + " removido com sucesso.";
            return RedirectToAction(nameof(Index));
        }
    }
}
