using ChalatePet.Api.Data;
using ChalatePet.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChalatePet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitasController : ControllerBase
{
    private readonly ChalatePetDbContext _context;

    public CitasController(ChalatePetDbContext context)
    {
        _context = context;
    }

//Guardar estados para la cita para manejarla por etapas
//posterriormente debe trabajarse como una tabla extra
    private static readonly string[] EstadosValidos =
    {
        "Pendiente",
        "Confirmada",
        "EnAtencion",
        "Completada",
        "Cancelada",
        "NoAsistio"
    };

        private static bool EsEstadoValido(string estado)
    {
        return EstadosValidos.Contains(estado);
    }

    private static bool EsTransicionValida(string estadoActual, string nuevoEstado)
    {
        return estadoActual switch
        {
            "Pendiente" =>
                nuevoEstado == "Pendiente" ||
                nuevoEstado == "Confirmada" ||
                nuevoEstado == "Cancelada",

            "Confirmada" =>
                nuevoEstado == "Confirmada" ||
                nuevoEstado == "EnAtencion" ||
                nuevoEstado == "Cancelada" ||
                nuevoEstado == "NoAsistio",

            "EnAtencion" =>
                nuevoEstado == "EnAtencion" ||
                nuevoEstado == "Completada",

            "Completada" =>
                nuevoEstado == "Completada",

            "Cancelada" =>
                nuevoEstado == "Cancelada",

            "NoAsistio" =>
                nuevoEstado == "NoAsistio",

            _ => false
        };
    }
    // GET: api/citas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cita>>> ObtenerCitas()
    {
        var citas = await _context.Citas
            .Include(c => c.Mascota)
            .Include(c => c.Servicio)
            .ToListAsync();

        return Ok(citas);

    }




    // GET: api/citas/1
    [HttpGet("{id:long}")]
    public async Task<ActionResult<Cita>> ObtenerCita(long id)
    {
        var cita = await _context.Citas
            .Include(c => c.Mascota)
            .Include(c => c.Servicio)
            .FirstOrDefaultAsync(c => c.IdCita == id);

        if (cita == null)
        {
            return NotFound();
        }

        return Ok(cita);
    }

    // POST: api/citas
    [HttpPost]
    public async Task<ActionResult<Cita>> CrearCita(Cita cita)
    {
        var mascotaExiste = await _context.Mascotas
            .AnyAsync(m => m.IdMascota == cita.IdMascota && m.Activo);

        if (!mascotaExiste)
        {
            return BadRequest("La mascota indicada no existe o está inactiva.");
        }

        var servicioExiste = await _context.Servicios
            .AnyAsync(s => s.IdServicio == cita.IdServicio && s.Activo);

        if (!servicioExiste)
        {
            return BadRequest("El servicio indicado no existe o está inactivo.");
        }

        cita.Estado = "Pendiente";
        cita.FechaCreacion = DateTime.Now;

        _context.Citas.Add(cita);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(ObtenerCita),
            new { id = cita.IdCita },
            cita
        );
    }

    // PUT: api/citas/1
    [HttpPut("{id:long}")]
    public async Task<IActionResult> ActualizarCita(long id, Cita cita)
    {
        if (id != cita.IdCita)
        {
            return BadRequest();
        }

        var citaExistente = await _context.Citas
            .FirstOrDefaultAsync(c => c.IdCita == id);

        if (citaExistente == null)
        {
            return NotFound();
        }

        if (!EsEstadoValido(cita.Estado))
{
    return BadRequest("El estado indicado no es válido.");
}
if (!EsTransicionValida(citaExistente.Estado, cita.Estado))
{
    return BadRequest(
        $"No se permite cambiar el estado de '{citaExistente.Estado}' a '{cita.Estado}'."
    );
}

        var mascotaExiste = await _context.Mascotas
            .AnyAsync(m => m.IdMascota == cita.IdMascota && m.Activo);

        if (!mascotaExiste)
        {
            return BadRequest("La mascota indicada no existe o está inactiva.");
        }

        var servicioExiste = await _context.Servicios
            .AnyAsync(s => s.IdServicio == cita.IdServicio && s.Activo);

        if (!servicioExiste)
        {
            return BadRequest("El servicio indicado no existe o está inactivo.");
        }

        citaExistente.IdMascota = cita.IdMascota;
        citaExistente.IdServicio = cita.IdServicio;
        citaExistente.FechaHora = cita.FechaHora;
        citaExistente.Motivo = cita.Motivo;
        citaExistente.Estado = cita.Estado;
        citaExistente.Observaciones = cita.Observaciones;

        await _context.SaveChangesAsync();

        return NoContent();
    }


    // PUT: api/citas/1/cancelar
    [HttpPut("{id:long}/cancelar")]
    public async Task<IActionResult> CancelarCita(long id)
    {
        var cita = await _context.Citas
            .FirstOrDefaultAsync(c => c.IdCita == id);

        if (cita == null)
        {
            return NotFound();
        }
        if (!EsTransicionValida(cita.Estado, "Cancelada"))
    {
        return BadRequest(
            $"No se permite cancelar una cita que está en estado '{cita.Estado}'."
        );
    }

        cita.Estado = "Cancelada";

        await _context.SaveChangesAsync();

        return NoContent();
    }


}