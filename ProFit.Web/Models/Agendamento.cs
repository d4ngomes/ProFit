using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProFit.Web.Models
{
    public class Agendamento
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A data é obrigatória.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Data { get; set; }
        public Aluno Aluno { get; set; }
        public int HoraId { get; set; }
        [ForeignKey("HoraId")]
        public virtual Horario Horario { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
