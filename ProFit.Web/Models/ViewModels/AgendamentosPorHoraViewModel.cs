using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProFit.Web.Models.ViewModels
{
    public class AgendamentosPorHoraViewModel
    {
        public int horaId { get; set; }
        public DateTime FiltraHora { get; set; }
        public IEnumerable<Agendamento> Agendamentos { get; set; }
        public IEnumerable<Horario> Horarios { get; set; }
    }
}
