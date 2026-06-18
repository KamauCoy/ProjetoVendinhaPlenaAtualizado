using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Vendinha.Data;
using Vendinha.Models;

namespace Vendinha.Services
{
    public class ClienteService
    {
        private readonly VendinhaDbContext context;

    public ClienteService(VendinhaDbContext context)
        {
            this.context = context;
        }

        public bool Validar(
            Cliente cliente,
            out List<ValidationResult> erros)
        {
            var validation = new ValidationContext(cliente);

            erros = new List<ValidationResult>();

            return Validator.TryValidateObject(
                cliente,
                validation,
                erros,
                true);
        }

        public bool Criar(
            Cliente cliente,
            out List<ValidationResult> erros)
        {
            if (!Validar(cliente, out erros))
                return false;

            if (context.Clientes.Any(c => c.cpf == cliente.cpf))
            {
                erros.Add(
                    new ValidationResult(
                        "CPF já cadastrado."
                    ));

                return false;
            }

            if (cliente.cpf.Length != 11)
            {
                erros.Add(
                    new ValidationResult(
                        "CPF deve possuir 11 dígitos."
                    ));

                return false;
            }

            cliente.data_nascimento = DateTime.SpecifyKind(cliente.data_nascimento, DateTimeKind.Utc);

            context.Clientes.Add(cliente);
            context.SaveChanges();

            return true;
        }

        public List<Cliente> Listar(
            int pagina = 1,
            string? nome = null)

        {
            var query = context.Clientes
                .Include(c => c.Dividas)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(c =>
                    c.nome_completo.Contains(nome));
            }

            
            query = query.OrderByDescending(c =>
                c.Dividas
                    .Where(d => !d.Paga)
                    .Sum(d => d.Valor));

            query = query
                .Skip((pagina - 1) * 10)
                .Take(10);

            return query.ToList();
        }

        public bool Atualizar(int id, Cliente clienteAtualizado)
        {
            var cliente = context.Clientes
                .FirstOrDefault(c => c.Id == id);

            if (cliente == null)
                return false;

            cliente.nome_completo = clienteAtualizado.nome_completo;
            cliente.cpf = clienteAtualizado.cpf;
            cliente.data_nascimento = clienteAtualizado.data_nascimento;
            cliente.Email = clienteAtualizado.Email;

            cliente.data_nascimento = DateTime.SpecifyKind(clienteAtualizado.data_nascimento, DateTimeKind.Utc);

            context.SaveChanges();

            return true;
        }

        public bool Remover(int id)
        {
            var cliente = context.Clientes
                .FirstOrDefault(c => c.Id == id);

            if (cliente == null)
                return false;

            context.Clientes.Remove(cliente);

            context.SaveChanges();

            return true;
        }
    }


}
