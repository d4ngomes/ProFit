using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProFit.Web.Models
{
    public class Funcionamento
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A quantidade de vagas é obrigatório.")]
        [Range(1, 200, ErrorMessage = "O valor deve ser de {1} a {2}.")]
        public int Vagas { get; set; }

        [Required(ErrorMessage = "Este horário é obrigatório.")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        [Display(Name = "Primeiro horário de Sábado")]
        public DateTime HoraInicioSabado { get; set; }

        [Required(ErrorMessage = "Este horário é obrigatório.")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        [Display(Name = "Último horário de Sábado")]
        public DateTime HoraFimSabado { get; set; }

        [Required(ErrorMessage = "Este horário é obrigatório.")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        [Display(Name = "Primeiro horário de Domingo")]
        public DateTime HoraInicioDomingo { get; set; }

        [Required(ErrorMessage = "Este horário é obrigatório.")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        [Display(Name = "Último horário de Domingo")]
        public DateTime HoraFimDomingo { get; set; }

        [Required(ErrorMessage = "Este horário é obrigatório.")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        [Display(Name = "Primeiro horário durante a semana")]
        public DateTime HoraInicioSemana { get; set; }

        [Required(ErrorMessage = "Este horário é obrigatório.")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        [Display(Name = "Último horário durante a semana")]
        public DateTime HoraFimSemana { get; set; }

        [Display(Name = "Abre aos domingos?")]
        public bool AbreDomingo { get; set; } = false;
    }
}
