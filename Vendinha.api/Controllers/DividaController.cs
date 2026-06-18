using Microsoft.AspNetCore.Mvc;
using Vendinha.Models;
using Vendinha.Services;

namespace Vendinha.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DividaController : ControllerBase
    {
        private readonly DividaService service;

    public DividaController(DividaService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(service.Listar());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var divida = service.BuscaPorId(id);

            if (divida == null)
                return NotFound("Dívida não encontrada.");

            return Ok(divida);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Divida divida)
        {
            var sucesso = service.Criar(divida, out var erros);

            return sucesso
                ? CreatedAtAction(
                    nameof(GetById),
                    new { id = divida.Id },
                    divida)
                : UnprocessableEntity(erros);
        }

        [HttpPut("{id}")]
        public IActionResult Update(
            int id,
            [FromBody] Divida divida)
        {
            var sucesso =
                service.Atualizar(id, divida);

            if (!sucesso)
                return NotFound(
                    "Dívida não encontrada."
                );

            return Ok(divida);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var sucesso =
                service.Remover(id);

            if (!sucesso)
                return NotFound(
                    "Dívida não encontrada."
                );

            return Ok(
                "Dívida removida com sucesso."
            );
        }

        [HttpPut("{id}/pagar")]
        public IActionResult Pagar(int id)
        {
            var sucesso =
                service.MarcarPagamento(id);

            if (!sucesso)
                return BadRequest(
                    "Dívida não encontrada ou já está paga."
                );

            return Ok(
                "Pagamento registrado com sucesso."
            );
        }
    }


}
