using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProFit.Web.Models
{
    public class Horario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O horário é obrigatório.")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public DateTime Hora { get; set; }
    }
}
