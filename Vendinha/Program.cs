using Vendinha.Data;
using Vendinha.Models;

Environment.SetEnvironmentVariable(
"ConnectionStrings__Default",
"Server=localhost;Port=5432;Database=Vendinha;User Id=postgres;Password=1234"
);

var context = new VendinhaDbContext();

Console.WriteLine("Digite o Nome Completo:");
var nome = Console.ReadLine();

Console.WriteLine("Digite o seu CPF:");
var cpf = Console.ReadLine();

Console.WriteLine("Digite a Data de Nascimento (dd/MM/yyyy):");
var dataNascimento = DateTime.Parse(Console.ReadLine());

Console.WriteLine("Digite o seu Email:");
var email = Console.ReadLine();

var cliente = new Cliente
{
    nome_completo = nome,
    cpf = cpf,
    data_nascimento = dataNascimento,
    Email = email
};

context.Clientes.Add(cliente);

context.SaveChanges();

Console.WriteLine("Cliente cadastrado com sucesso!");
