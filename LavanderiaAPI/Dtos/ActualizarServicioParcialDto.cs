namespace LavanderiaAPI.Dtos;

public class ActualizarServicioParcialDto
{
    public string? NombreCliente { get; set; }
    public string? Telefono { get; set; }
    public string? TipoServicio { get; set; }
    public string? Color { get; set; }
    public string? TipoRopa { get; set; }
    public string? TipoTela { get; set; }
    public int? Cantidad { get; set; }
    public decimal? Precio { get; set; }
    public string? EstadoPago { get; set; }
    public string? EstadoEntrega { get; set; }
}