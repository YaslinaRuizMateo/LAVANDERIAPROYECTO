using LavanderiaAPI.Models;
using LavanderiaAPI.Dtos;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Lista en memoria
List<ServicioLavanderia> servicios = [
    new ServicioLavanderia
    {
        Id = 1,
        NombreCliente = "María Sánchez",
        Telefono = "809-555-1234",
        TipoServicio = "Lavado y secado",
        Color = "Blanco",
        TipoRopa = "Camisa",
        TipoTela = "Algodón",
        Cantidad = 5,
        Precio = 750,
        EstadoPago = "Pagado",
        EstadoEntrega = "Pendiente"
    },
    new ServicioLavanderia
    {
        Id = 2,
        NombreCliente = "Juan Rodríguez",
        Telefono = "829-555-5678",
        TipoServicio = "Planchado",
        Color = "Negro",
        TipoRopa = "Pantalón",
        TipoTela = "Jean",
        Cantidad = 3,
        Precio = 450,
        EstadoPago = "Debe",
        EstadoEntrega = "Pendiente"
    }
];

// GET /
app.MapGet("/", () =>
{
    return Results.Ok("API de Lavandería funcionando correctamente");
});

// POST /servicios
app.MapPost("/servicios", (CrearServicioDto dto) =>
{
    if (string.IsNullOrWhiteSpace(dto.NombreCliente))
        return Results.BadRequest("El nombre del cliente es obligatorio.");

    if (string.IsNullOrWhiteSpace(dto.Telefono))
        return Results.BadRequest("El teléfono es obligatorio.");

    if (string.IsNullOrWhiteSpace(dto.TipoServicio))
        return Results.BadRequest("El tipo de servicio es obligatorio.");

    if (string.IsNullOrWhiteSpace(dto.Color))
        return Results.BadRequest("El color es obligatorio.");

    if (string.IsNullOrWhiteSpace(dto.TipoRopa))
        return Results.BadRequest("El tipo de ropa es obligatorio.");

    if (string.IsNullOrWhiteSpace(dto.TipoTela))
        return Results.BadRequest("El tipo de tela es obligatorio.");

    if (dto.Cantidad <= 0)
        return Results.BadRequest("La cantidad debe ser mayor que cero.");

    if (dto.Precio <= 0)
        return Results.BadRequest("El precio debe ser mayor que cero.");

    if (string.IsNullOrWhiteSpace(dto.EstadoPago))
        return Results.BadRequest("El estado de pago es obligatorio.");

    if (string.IsNullOrWhiteSpace(dto.EstadoEntrega))
        return Results.BadRequest("El estado de entrega es obligatorio.");

    var nuevoId = servicios.Count == 0
        ? 1
        : servicios.Max(s => s.Id) + 1;

    var nuevoServicio = new ServicioLavanderia
    {
        Id = nuevoId,
        NombreCliente = dto.NombreCliente,
        Telefono = dto.Telefono,
        TipoServicio = dto.TipoServicio,
        Color = dto.Color,
        TipoRopa = dto.TipoRopa,
        TipoTela = dto.TipoTela,
        Cantidad = dto.Cantidad,
        Precio = dto.Precio,
        EstadoPago = dto.EstadoPago,
        EstadoEntrega = dto.EstadoEntrega
    };

    servicios.Add(nuevoServicio);

    return Results.Created($"/servicios/{nuevoServicio.Id}", nuevoServicio);
});

// GET /servicios
// También permite filtrar con ?nombre=valor
app.MapGet("/servicios", (string? nombre) =>
{
    if (string.IsNullOrWhiteSpace(nombre))
        return Results.Ok(servicios);

    var resultados = servicios
        .Where(s => s.NombreCliente.Contains(
            nombre,
            StringComparison.OrdinalIgnoreCase))
        .ToList();

    if (resultados.Count == 0)
        return Results.NotFound("No se encontraron servicios para ese cliente.");

    return Results.Ok(resultados);
});

// GET /servicios/{id}
app.MapGet("/servicios/{id}", (int id) =>
{
    var servicio = servicios.FirstOrDefault(s => s.Id == id);

    if (servicio is null)
        return Results.NotFound("Servicio no encontrado.");

    return Results.Ok(servicio);
});

// PUT /servicios/{id}
// Actualización completa
app.MapPut("/servicios/{id}", (int id, ActualizarServicioDto dto) =>
{
    var servicio = servicios.FirstOrDefault(s => s.Id == id);

    if (servicio is null)
        return Results.NotFound("Servicio no encontrado.");

    if (string.IsNullOrWhiteSpace(dto.NombreCliente))
        return Results.BadRequest("El nombre del cliente es obligatorio.");

    if (string.IsNullOrWhiteSpace(dto.Telefono))
        return Results.BadRequest("El teléfono es obligatorio.");

    if (string.IsNullOrWhiteSpace(dto.TipoServicio))
        return Results.BadRequest("El tipo de servicio es obligatorio.");

    if (string.IsNullOrWhiteSpace(dto.Color))
        return Results.BadRequest("El color es obligatorio.");

    if (string.IsNullOrWhiteSpace(dto.TipoRopa))
        return Results.BadRequest("El tipo de ropa es obligatorio.");

    if (string.IsNullOrWhiteSpace(dto.TipoTela))
        return Results.BadRequest("El tipo de tela es obligatorio.");

    if (dto.Cantidad <= 0)
        return Results.BadRequest("La cantidad debe ser mayor que cero.");

    if (dto.Precio <= 0)
        return Results.BadRequest("El precio debe ser mayor que cero.");

    if (string.IsNullOrWhiteSpace(dto.EstadoPago))
        return Results.BadRequest("El estado de pago es obligatorio.");

    if (string.IsNullOrWhiteSpace(dto.EstadoEntrega))
        return Results.BadRequest("El estado de entrega es obligatorio.");

    servicio.NombreCliente = dto.NombreCliente;
    servicio.Telefono = dto.Telefono;
    servicio.TipoServicio = dto.TipoServicio;
    servicio.Color = dto.Color;
    servicio.TipoRopa = dto.TipoRopa;
    servicio.TipoTela = dto.TipoTela;
    servicio.Cantidad = dto.Cantidad;
    servicio.Precio = dto.Precio;
    servicio.EstadoPago = dto.EstadoPago;
    servicio.EstadoEntrega = dto.EstadoEntrega;

    return Results.Ok(servicio);
});

// PATCH /servicios/{id}
// Actualización parcial
app.MapPatch("/servicios/{id}", (int id, ActualizarServicioParcialDto dto) =>
{
    var servicio = servicios.FirstOrDefault(s => s.Id == id);

    if (servicio is null)
        return Results.NotFound("Servicio no encontrado.");

    if (dto.NombreCliente is not null)
    {
        if (string.IsNullOrWhiteSpace(dto.NombreCliente))
            return Results.BadRequest(
                "El nombre del cliente no puede estar vacío.");

        servicio.NombreCliente = dto.NombreCliente;
    }

    if (dto.Telefono is not null)
    {
        if (string.IsNullOrWhiteSpace(dto.Telefono))
            return Results.BadRequest(
                "El teléfono no puede estar vacío.");

        servicio.Telefono = dto.Telefono;
    }

    if (dto.TipoServicio is not null)
    {
        if (string.IsNullOrWhiteSpace(dto.TipoServicio))
            return Results.BadRequest(
                "El tipo de servicio no puede estar vacío.");

        servicio.TipoServicio = dto.TipoServicio;
    }

    if (dto.Color is not null)
    {
        if (string.IsNullOrWhiteSpace(dto.Color))
            return Results.BadRequest(
                "El color no puede estar vacío.");

        servicio.Color = dto.Color;
    }

    if (dto.TipoRopa is not null)
    {
        if (string.IsNullOrWhiteSpace(dto.TipoRopa))
            return Results.BadRequest(
                "El tipo de ropa no puede estar vacío.");

        servicio.TipoRopa = dto.TipoRopa;
    }

    if (dto.TipoTela is not null)
    {
        if (string.IsNullOrWhiteSpace(dto.TipoTela))
            return Results.BadRequest(
                "El tipo de tela no puede estar vacío.");

        servicio.TipoTela = dto.TipoTela;
    }

    if (dto.Cantidad.HasValue)
    {
        if (dto.Cantidad <= 0)
            return Results.BadRequest(
                "La cantidad debe ser mayor que cero.");

        servicio.Cantidad = dto.Cantidad.Value;
    }

    if (dto.Precio.HasValue)
    {
        if (dto.Precio <= 0)
            return Results.BadRequest(
                "El precio debe ser mayor que cero.");

        servicio.Precio = dto.Precio.Value;
    }

    if (dto.EstadoPago is not null)
    {
        if (string.IsNullOrWhiteSpace(dto.EstadoPago))
            return Results.BadRequest(
                "El estado de pago no puede estar vacío.");

        servicio.EstadoPago = dto.EstadoPago;
    }

    if (dto.EstadoEntrega is not null)
    {
        if (string.IsNullOrWhiteSpace(dto.EstadoEntrega))
            return Results.BadRequest(
                "El estado de entrega no puede estar vacío.");

        servicio.EstadoEntrega = dto.EstadoEntrega;
    }

    return Results.Ok(servicio);
});

// DELETE /servicios/{id}
app.MapDelete("/servicios/{id}", (int id) =>
{
    var servicio = servicios.FirstOrDefault(s => s.Id == id);

    if (servicio is null)
        return Results.NotFound("Servicio no encontrado.");

    servicios.Remove(servicio);

    return Results.Ok(new
    {
        mensaje = "Servicio eliminado correctamente.",
        servicio
    });
});

app.Run();