using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Net.Http.Headers;

namespace EJ.SistemaAgendamentos.Models
{
    public class Agendamento
    {
        [Key]
        public int Id { get; set;}
        [MaxLength(255)]
        [Required()]
        public string? Title { get; set;}
        public DateTime Start {get; set;}
        public DateTime End {get; set;}

        [Column(TypeName = "text")]
        public string? Description{get;set;}
    }
}