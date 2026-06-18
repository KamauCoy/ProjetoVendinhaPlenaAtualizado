using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vendinha.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O Nome é obrigatório")]
        public string nome_completo { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório")]
        public string cpf { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        public DateTime data_nascimento { get; set; }

        [EmailAddress(ErrorMessage = "Email inválido")]
        public string? Email { get; set; }

        public List<Divida> Dividas { get; set; } = new();

        [NotMapped]
        public int Idade
        {
            get
            {
                var hoje = DateTime.Today;
                var idade = hoje.Year - data_nascimento.Year;

                if (data_nascimento.Date > hoje.AddYears(-idade))
                    idade--;

                return idade;
            }
        }

        [NotMapped]
        public decimal TotalDividas
        {
            get
            {
                return Dividas
                    .Where(d => !d.Paga)
                    .Sum(d => d.Valor);
            }
        }
    }

   
}
