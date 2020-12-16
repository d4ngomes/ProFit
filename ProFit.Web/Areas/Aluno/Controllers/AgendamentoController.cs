using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProFit.Web.Data;
using ProFit.Web.Models;
using ProFit.Web.Models.ViewModels;
using ProFit.Web.Utility;

namespace ProFit.Web.Areas.Aluno.Controllers
{
    [Area("Aluno")]
    [Authorize(Roles = SD.AlunoUser)]
    public class AgendamentoController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        [BindProperty]
        public AgendamentoViewModel AgendamentoViewModel { get; set; }

        public AgendamentoController(AppDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userEmail = user.Email;
            var agendamento = await _db.Agendamentos
                .Include(m => m.Horario)
                .Where(m => m.Aluno.Email == userEmail && m.Data >= DateTime.Now.AddDays(-30))
                .OrderByDescending(m => m.Id)
                .ToListAsync();
            return View(agendamento);
        }

        public async Task<IActionResult> Create()
        {
            return View(await CarregaAgendamento(DateTime.Now.Date));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerificaData(DateTime data)
        {
            if (data == null)
            {
                return RedirectToAction(nameof(Create));
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userEmail = user.Email;
            var aluno = await _db.Alunos.FirstOrDefaultAsync(a => a.Email == userEmail);
            var agendado = _db.Agendamentos
                .Include(m => m.Horario)
                .Where(m => m.Data.Date == data.Date && m.Aluno.Id == aluno.Id && m.Ativo == true)
                .FirstOrDefault();
            
            if (agendado != null)
            {
                AgendamentoViewModel = new AgendamentoViewModel
                {
                    Agendado = true,
                    DataEscolhida = data
                };
                return View(AgendamentoViewModel);
            }
            else
            {
                return View(await CarregaAgendamento(data));
            }
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(DateTime data, int horaId)
        {
            if (!ModelState.IsValid)
            {
                return View(AgendamentoViewModel);
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userEmail = user.Email;
            var aluno = await _db.Alunos.FirstOrDefaultAsync(a => a.Email == userEmail && a.Ativo == true);
            if (aluno == null)
            {
                TempData["ErroCpf"] = "Não autorizado";
                return RedirectToAction("ErroCpf", "Home", new { area = "Aluno" });
            }
            var horario = await _db.Horarios.FirstOrDefaultAsync(h => h.Id == horaId);
            var agendamento = new Agendamento {
                Aluno = aluno,
                Data = data,
                Horario = horario
            };
            _db.Agendamentos.Add(agendamento);
            await _db.SaveChangesAsync();
            TempData["Msg"] = "Agendamento para o dia " + agendamento.Data.ToString("dd/MM/yyyy") + " às " + agendamento.Horario.Hora.ToString("HH:mm") + " horas realizado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Cancelar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var agendamento = await _db.Agendamentos.Include(m => m.Horario).FirstOrDefaultAsync(m => m.Id == id);
            if (agendamento == null)
            {
                return NotFound();
            }
            return View(agendamento);
        }

        [HttpPost, ActionName("Cancelar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelarConfirma(int? id)
        {
            if (ModelState.IsValid)
            {
                var agendamento = await _db.Agendamentos.FindAsync(id);
                agendamento.Ativo = false;
                _db.Agendamentos.Update(agendamento);
                await _db.SaveChangesAsync();
                TempData["Msg"] = "Agendamento cancelado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<AgendamentoViewModel> CarregaAgendamento(DateTime data)
        {
            var feriado = await _db.Feriados.Where(m => m.Data.Date == data).FirstOrDefaultAsync();
            var isFeriado = feriado != null ? true : false;
            var funcionamento = await _db.Funcionamento.FirstOrDefaultAsync();
            var agendamentoVagas = await _db.Agendamentos.Where(m => m.Data == data).ToListAsync();
            AgendamentoViewModel = new AgendamentoViewModel
            {
                Agendamento = new Agendamento(),
                Horario = _db.Horarios,
                Agendado = false,
                DataEscolhida = data,
                AgendamentoVagas = agendamentoVagas,
                Vagas = funcionamento.Vagas,
                AbreDomingo = funcionamento.AbreDomingo,
                isFeriado = isFeriado
            };
            if ((int)data.DayOfWeek == 0)
            {
                if (funcionamento.AbreDomingo)
                {
                    AgendamentoViewModel.HoraInicio = funcionamento.HoraInicioDomingo;
                    AgendamentoViewModel.HoraFim = funcionamento.HoraFimDomingo;
                }
            }
            else
            {
                if ((int)data.DayOfWeek == 6)
                {
                    AgendamentoViewModel.HoraInicio = funcionamento.HoraInicioSabado;
                    AgendamentoViewModel.HoraFim = funcionamento.HoraFimSabado;
                }
                else
                {
                    AgendamentoViewModel.HoraInicio = funcionamento.HoraInicioSemana;
                    AgendamentoViewModel.HoraFim = funcionamento.HoraFimSemana;
                }
            }
            return AgendamentoViewModel;
        }
    }
}
