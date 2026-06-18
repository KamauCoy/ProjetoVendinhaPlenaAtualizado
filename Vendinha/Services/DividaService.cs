using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Vendinha.Data;
using Vendinha.Models;

namespace Vendinha.Services
{
    public class DividaService
    {
        private readonly VendinhaDbContext context;

    public DividaService(VendinhaDbContext context)
        {
            this.context = context;
        }

        public bool Validar(
            Divida divida,
            out List<ValidationResult> erros)
        {
            var validation = new ValidationContext(divida);

            erros = new List<ValidationResult>();

            return Validator.TryValidateObject(
                divida,
                validation,
                erros,
                true);
        }

        public bool Criar(
            Divida divida,
            out List<ValidationResult> erros)
        {
            if (!Validar(divida, out erros))
                return false;

            var clienteExiste = context.Clientes
                .Any(c => c.Id == divida.ClienteId);

            if (!clienteExiste)
            {
                erros.Add(
                    new ValidationResult(
                        "Cliente não encontrado."
                    ));

                return false;
            }

            var possuiDividaAberta = context.Dividas.Any(d =>
                d.ClienteId == divida.ClienteId &&
                !d.Paga);

            if (possuiDividaAberta)
            {
                erros.Add(
                    new ValidationResult(
                        "O cliente já possui uma dívida em aberto."
                    ));

                return false;
            }

            divida.data_criacao = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            divida.Paga = false;

            context.Dividas.Add(divida);
            context.SaveChanges();

            return true;
        }

        public List<Divida> Listar()
        {
            return context.Dividas
                .Include(d => d.Cliente)
                .ToList();
        }

        public Divida? BuscaPorId(int id)
        {
            return context.Dividas
                .Include(d => d.Cliente)
                .FirstOrDefault(d => d.Id == id);
        }

        public bool Atualizar(int id, Divida dividaAtualizada)
        {
            var divida = context.Dividas
                .FirstOrDefault(d => d.Id == id);

            if (divida == null)
                return false;

            divida.Valor = dividaAtualizada.Valor;

            context.SaveChanges();

            return true;
        }

        public bool Remover(int id)
        {
            var divida = context.Dividas
                .FirstOrDefault(d => d.Id == id);

            if (divida == null)
                return false;

            context.Dividas.Remove(divida);

            context.SaveChanges();

            return true;
        }

        public bool MarcarPagamento(int id)
        {
            var divida = context.Dividas
                .FirstOrDefault(d => d.Id == id);

            if (divida == null)
                return false;

            if (divida.Paga)
                return false;

            divida.Paga = true;
            divida.data_criacao = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

            context.SaveChanges();

            return true;
        }
    }


}
