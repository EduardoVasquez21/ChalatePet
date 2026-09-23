namespace ChalatePet.Api.Models;

public class Cliente
{
    public long IdCliente { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;

    public string TelefonoPrincipal { get; set; } = string.Empty;

    public string? TelefonoSecundario { get; set; }

    public string? Email { get; set; }

    public string? Direccion { get; set; }

    public DateTime FechaRegistro { get; set; }

    public bool Activo { get; set; } = true;
}