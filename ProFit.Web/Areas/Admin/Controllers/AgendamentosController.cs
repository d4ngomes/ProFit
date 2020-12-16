using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProFit.Web.Data;
using ProFit.Web.Utility;
using ProFit.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProFit.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminUser)]
    public class AgendamentosController : Controller
    {
        private readonly AppDbContext _db;

        public AgendamentosController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var agendamentos = await _db.Agendamentos
                .Include(m => m.Horario)
                .Include(m => m.Aluno)
                .Where(m => m.Data == DateTime.Now.Date)
                .OrderBy(m => m.Horario.Hora)
                .ToListAsync();
            AgendamentosPorHoraViewModel viewModel = new AgendamentosPorHoraViewModel
            {
                Agendamentos = agendamentos,
                Horarios = _db.Horarios
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> FiltraPorHorario(int? horaId)
        {
            if (horaId == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var agendamentos = await _db.Agendamentos
                .Include(m => m.Horario)
                .Include(m => m.Aluno)
                .Where(m => m.Data == DateTime.Now.Date && m.HoraId == horaId)
                .OrderBy(m => m.Horario.Hora)
                .ToListAsync();
            var hora = await _db.Horarios.Where(m => m.Id == horaId).FirstOrDefaultAsync();
            AgendamentosPorHoraViewModel viewModel = new AgendamentosPorHoraViewModel
            {
                Agendamentos = agendamentos,
                Horarios = _db.Horarios,
                FiltraHora = hora.Hora
            };
            return View(viewModel);
        }
    }
}
