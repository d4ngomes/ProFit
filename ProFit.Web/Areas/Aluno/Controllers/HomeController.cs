using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProFit.Web.Data;
using ProFit.Web.Models;
using ProFit.Web.Services;

namespace ProFit.Web.Controllers
{
    [Area("Aluno")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;

        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckCadastro(string Cpf)
        {
            if (ModelState.IsValid)
            {
                if (!ValidaCpf.IsValid(Cpf))
                {
                    TempData["Msg"] = "Cpf inválido.";
                    return RedirectToAction(nameof(Index));
                }
                var aluno = await _db.Alunos.Where(aluno => aluno.Cpf == Cpf).FirstOrDefaultAsync();
                if (aluno != null && aluno.Ativo == true)
                {
                    var alunoTemLogin = await _db.ApplicationUser.FirstOrDefaultAsync(a => a.Cpf == aluno.Cpf);
                    if (alunoTemLogin != null)
                    {
                        return RedirectToPage("/Account/Login", new { area = "Identity" });
                    }
                    else
                    {
                        return RedirectToPage("/Account/Register", new { area = "Identity", Cpf, aluno.Nome, aluno.Email });
                    }
                }
                if (aluno != null && aluno.Ativo == false) TempData["ErroCpf"] = "Aluno inativo";
                if (aluno == null) TempData["ErroCpf"] = "Cpf " + ValidaCpf.FormataCpf(Cpf) + " não encontrado";
                return RedirectToAction(nameof(ErroCpf));
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ErroCpf()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
