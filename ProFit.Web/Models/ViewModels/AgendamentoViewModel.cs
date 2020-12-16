using Microsoft.EntityFrameworkCore;
using ProFit.Web.Data;
using ProFit.Web.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProFit.Web.Models.ViewModels
{
    public class AgendamentoViewModel
    {
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public DateTime HoraInicio { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public DateTime HoraFim { get; set; }
        public bool Agendado { get; set; }
        public bool AbreDomingo { get; set; }
        public bool isFeriado { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataEscolhida { get; set; }

        public Agendamento Agendamento { get; set; }
        public IEnumerable<Horario> Horario { get; set; }

        public ICollection<Agendamento> AgendamentoVagas { get; set; }
        public int Vagas { get; set; }

        public bool VerificaHorario(int horaId)
        {
            var count = AgendamentoVagas.Where(m => m.HoraId == horaId && m.Ativo == true).Count();
            if (count >= 1)
            {
                return true;
            }
            return false;
        }
    }
}
