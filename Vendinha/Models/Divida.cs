using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vendinha.Models
{
    public class Divida
    {
        public int Id { get; set; }

    [Required(ErrorMessage = "O valor da dívida é obrigatório")]
        public decimal Valor { get; set; }

        public bool Paga { get; set; } = false;

        public DateTime data_criacao { get; set; }

        public DateTime? data_pagamento { get; set; }

        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }

        public Cliente? Cliente { get; set; }
    }


}
