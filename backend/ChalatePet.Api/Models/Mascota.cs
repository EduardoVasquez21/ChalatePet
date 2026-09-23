namespace ChalatePet.Api.Models;

public class Mascota
{
    public long IdMascota { get; set; }

    public long IdCliente { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Especie { get; set; } = string.Empty;

    public string? Raza { get; set; }

    public char Sexo { get; set; }

    public DateTime? FechaNacimiento { get; set; }

    public string? Color { get; set; }

    public string? Observaciones { get; set; }

    public bool Activo { get; set; } = true;

    // Relación con el propietario
    public Cliente Cliente { get; set; } = null!;
}