using ChalatePet.Api.Data;
using ChalatePet.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChalatePet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiciosController : ControllerBase
{
    private readonly ChalatePetDbContext _context;

    public ServiciosController(ChalatePetDbContext context)
    {
        _context = context;
    }

    // GET: api/servicios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Servicio>>> ObtenerServicios()
    {
        var servicios = await _context.Servicios
            .Where(s => s.Activo)
            .ToListAsync();

        return Ok(servicios);
    }

    // GET: api/servicios/1
    [HttpGet("{id:long}")]
    public async Task<ActionResult<Servicio>> ObtenerServicio(long id)
    {
        var servicio = await _context.Servicios
            .FirstOrDefaultAsync(s => s.IdServicio == id && s.Activo);

        if (servicio == null)
        {
            return NotFound();
        }

        return Ok(servicio);
    }

    // POST: api/servicios
[HttpPost]
public async Task<ActionResult<Servicio>> CrearServicio(Servicio servicio)
{
    servicio.Activo = true;

    _context.Servicios.Add(servicio);
    await _context.SaveChangesAsync();

    return CreatedAtAction(
        nameof(ObtenerServicio),
        new { id = servicio.IdServicio },
        servicio
    );
}
// PUT: api/servicios/1
[HttpPut("{id:long}")]
public async Task<IActionResult> ActualizarServicio(long id, Servicio servicio)
{
    if (id != servicio.IdServicio)
    {
        return BadRequest();
    }

    var servicioExistente = await _context.Servicios
        .FirstOrDefaultAsync(s => s.IdServicio == id && s.Activo);

    if (servicioExistente == null)
    {
        return NotFound();
    }

    servicioExistente.Nombre = servicio.Nombre;
    servicioExistente.Descripcion = servicio.Descripcion;
    servicioExistente.Precio = servicio.Precio;
    servicioExistente.DuracionMinutos = servicio.DuracionMinutos;

    await _context.SaveChangesAsync();

    return NoContent();
}

// DELETE: api/servicios/1
[HttpDelete("{id:long}")]
public async Task<IActionResult> DesactivarServicio(long id)
{
    var servicio = await _context.Servicios
        .FirstOrDefaultAsync(s => s.IdServicio == id && s.Activo);

    if (servicio == null)
    {
        return NotFound();
    }

    servicio.Activo = false;

    await _context.SaveChangesAsync();

    return NoContent();
}
}