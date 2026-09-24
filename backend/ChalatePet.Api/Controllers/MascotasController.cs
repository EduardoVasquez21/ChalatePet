using ChalatePet.Api.Data;
using ChalatePet.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChalatePet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MascotasController : ControllerBase
{
    private readonly ChalatePetDbContext _context;

    public MascotasController(ChalatePetDbContext context)
    {
        _context = context;
    }

    // GET: api/mascotas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Mascota>>> ObtenerMascotas()
    {
        var mascotas = await _context.Mascotas
            .Where(m => m.Activo)
            .ToListAsync();

        return Ok(mascotas);
    }

    // GET: api/mascotas/5
    [HttpGet("{id:long}")]
    public async Task<ActionResult<Mascota>> ObtenerMascota(long id)
    {
        var mascota = await _context.Mascotas
            .FirstOrDefaultAsync(m => m.IdMascota == id && m.Activo);

        if (mascota == null)
        {
            return NotFound();
        }

        return Ok(mascota);
    }



    // POST: api/mascotas
    [HttpPost]
    public async Task<ActionResult<Mascota>> CrearMascota(Mascota mascota)
    {
        var clienteExiste = await _context.Clientes
            .AnyAsync(c => c.IdCliente == mascota.IdCliente && c.Activo);

        if (!clienteExiste)
        {
            return BadRequest("El cliente indicado no existe o está inactivo.");
        }

        mascota.Activo = true;

        _context.Mascotas.Add(mascota);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(ObtenerMascota),
            new { id = mascota.IdMascota },
            mascota
        );
    }


    // PUT: api/mascotas/1
[HttpPut("{id:long}")]
public async Task<IActionResult> ActualizarMascota(long id, Mascota mascota)
{
    if (id != mascota.IdMascota)
    {
        return BadRequest();
    }

    var mascotaExistente = await _context.Mascotas
        .FirstOrDefaultAsync(m => m.IdMascota == id && m.Activo);

    if (mascotaExistente == null)
    {
        return NotFound();
    }

    var clienteExiste = await _context.Clientes
        .AnyAsync(c => c.IdCliente == mascota.IdCliente && c.Activo);

    if (!clienteExiste)
    {
        return BadRequest("El cliente indicado no existe o está inactivo.");
    }

    mascotaExistente.IdCliente = mascota.IdCliente;
    mascotaExistente.Nombre = mascota.Nombre;
    mascotaExistente.Especie = mascota.Especie;
    mascotaExistente.Raza = mascota.Raza;
    mascotaExistente.Sexo = mascota.Sexo;
    mascotaExistente.FechaNacimiento = mascota.FechaNacimiento;
    mascotaExistente.Color = mascota.Color;
    mascotaExistente.Observaciones = mascota.Observaciones;

    await _context.SaveChangesAsync();

    return NoContent();
}

// DELETE: api/mascotas/1
[HttpDelete("{id:long}")]
public async Task<IActionResult> DesactivarMascota(long id)
{
    var mascota = await _context.Mascotas
        .FirstOrDefaultAsync(m => m.IdMascota == id && m.Activo);

    if (mascota == null)
    {
        return NotFound();
    }

    mascota.Activo = false;

    await _context.SaveChangesAsync();

    return NoContent();
}
}