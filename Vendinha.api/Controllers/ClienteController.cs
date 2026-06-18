using Microsoft.AspNetCore.Mvc;
using Vendinha.Models;
using Vendinha.Services;

namespace Vendinha.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService service;


    public ClienteController(ClienteService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult Get(
            int pagina = 1,
            string? nome = null)
        {
            return Ok(
                service.Listar(
                    pagina,
                    nome
                )
            );
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var cliente = service.Listar(id);

            if (cliente == null)
                return NotFound("Cliente não encontrado.");

            return Ok(cliente);
        }


        [HttpPost]
        public IActionResult Create([FromBody] Cliente cliente)
        {
            var sucesso = service.Criar(cliente, out var erros);

            return sucesso
                ? CreatedAtAction(
                    nameof(GetById),
                    new { id = cliente.Id },
                    cliente)
                : UnprocessableEntity(erros);
        }

        [HttpPut("{id}")]
        public IActionResult Update(
            int id,
            [FromBody] Cliente cliente)
        {
            var sucesso =
                service.Atualizar(id, cliente);

            if (!sucesso)
                return NotFound(
                    "Cliente não encontrado."
                );

            return Ok(cliente);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var sucesso =
                service.Remover(id);

            if (!sucesso)
                return NotFound(
                    "Cliente não encontrado."
                );

            return Ok(
                "Cliente removido com sucesso."
            );
        }
    }


}
