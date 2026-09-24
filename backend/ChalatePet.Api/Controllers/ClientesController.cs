using ChalatePet.Api.Data;
using ChalatePet.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChalatePet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly ChalatePetDbContext _context;

    public ClientesController(ChalatePetDbContext context)
    {
        _context = context;
    }

    // GET: api/clientes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cliente>>> ObtenerClientes()
    {
        var clientes = await _context.Clientes
            .Where(c => c.Activo)
            .ToListAsync();

        return Ok(clientes);
    }

    // GET: api/clientes/5
    [HttpGet("{id:long}")]
    public async Task<ActionResult<Cliente>> ObtenerCliente(long id)
    {
        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(c => c.IdCliente == id && c.Activo);

        if (cliente == null)
        {
            return NotFound();
        }

        return Ok(cliente);
    }

    // POST: api/clientes
    [HttpPost]
    public async Task<ActionResult<Cliente>> CrearCliente(Cliente cliente)
    {
        cliente.FechaRegistro = DateTime.Now;
        cliente.Activo = true;

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(ObtenerCliente),
            new { id = cliente.IdCliente },
            cliente
        );
    }

    // PUT: api/clientes/5
    [HttpPut("{id:long}")]
    public async Task<IActionResult> ActualizarCliente(long id, Cliente cliente)
    {
        if (id != cliente.IdCliente)
        {
            return BadRequest();
        }

        var clienteExistente = await _context.Clientes
            .FirstOrDefaultAsync(c => c.IdCliente == id && c.Activo);

        if (clienteExistente == null)
        {
            return NotFound();
        }

        clienteExistente.Nombre = cliente.Nombre;
        clienteExistente.Apellido = cliente.Apellido;
        clienteExistente.TelefonoPrincipal = cliente.TelefonoPrincipal;
        clienteExistente.TelefonoSecundario = cliente.TelefonoSecundario;
        clienteExistente.Email = cliente.Email;
        clienteExistente.Direccion = cliente.Direccion;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/clientes/5
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DesactivarCliente(long id)
    {
        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(c => c.IdCliente == id && c.Activo);

        if (cliente == null)
        {
            return NotFound();
        }

        cliente.Activo = false;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}