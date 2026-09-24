namespace ChalatePet.Api.Models;

public class Cita
{
    public long IdCita { get; set; }

    public long IdMascota { get; set; }

    public long IdServicio { get; set; }

    public DateTime FechaHora { get; set; }

    public string? Motivo { get; set; }

    public string Estado { get; set; } = "Pendiente";

    public string? Observaciones { get; set; }

    public DateTime FechaCreacion { get; set; }

    // Relaciones
    public Mascota? Mascota { get; set; } 

    public Servicio? Servicio { get; set; } 
}