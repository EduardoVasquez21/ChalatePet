namespace ChalatePet.Api.Models;

public class Servicio
{
    public long IdServicio { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int? DuracionMinutos { get; set; }

    public bool Activo { get; set; } = true;
}