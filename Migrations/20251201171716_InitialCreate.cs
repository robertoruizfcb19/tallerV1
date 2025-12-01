using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace tallerV1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bodegas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Responsable = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bodegas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RUC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Repuestos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CodigoBarras = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Marca = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UnidadMedida = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BodegaId = table.Column<int>(type: "int", nullable: false),
                    StockActual = table.Column<int>(type: "int", nullable: false),
                    StockMinimo = table.Column<int>(type: "int", nullable: false),
                    StockMaximo = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MesesGarantia = table.Column<int>(type: "int", nullable: true),
                    Proveedor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repuestos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Repuestos_Bodegas_BodegaId",
                        column: x => x.BodegaId,
                        principalTable: "Bodegas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Equipos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TipoEquipo = table.Column<int>(type: "int", nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Modelo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NumeroSerie = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Placa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AnioFabricacion = table.Column<int>(type: "int", nullable: true),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    TipoControl = table.Column<int>(type: "int", nullable: true),
                    KilometrajeActual = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HorasActuales = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GalonesPorKm = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    GalonesPorHora = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimoMantenimiento = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipos_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompleto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<int>(type: "int", nullable: false),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UltimoAcceso = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MantenimientosPreventivos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EquipoId = table.Column<int>(type: "int", nullable: false),
                    TipoControl = table.Column<int>(type: "int", nullable: false),
                    IntervaloKilometros = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IntervaloHoras = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IntervaloDias = table.Column<int>(type: "int", nullable: true),
                    UltimoKilometraje = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UltimasHoras = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UltimaFecha = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProximoKilometraje = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProximasHoras = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProximaFecha = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NotificacionEnviada = table.Column<bool>(type: "bit", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MantenimientosPreventivos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MantenimientosPreventivos_Equipos_EquipoId",
                        column: x => x.EquipoId,
                        principalTable: "Equipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MovimientosInventario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepuestoId = table.Column<int>(type: "int", nullable: false),
                    TipoMovimiento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    StockAnterior = table.Column<int>(type: "int", nullable: false),
                    StockNuevo = table.Column<int>(type: "int", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    FechaMovimiento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientosInventario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimientosInventario_Repuestos_RepuestoId",
                        column: x => x.RepuestoId,
                        principalTable: "Repuestos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovimientosInventario_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrdenesTrabajo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroOrden = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EquipoId = table.Column<int>(type: "int", nullable: false),
                    UsuarioCreadorId = table.Column<int>(type: "int", nullable: false),
                    UsuarioAsignadoId = table.Column<int>(type: "int", nullable: true),
                    UsuarioAprobadorId = table.Column<int>(type: "int", nullable: true),
                    TipoMantenimiento = table.Column<int>(type: "int", nullable: false),
                    Prioridad = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    DescripcionProblema = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DiagnosticoTecnico = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    KilometrajeEquipo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HorometroEquipo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CostoRepuestos = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CostoManoObra = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CostoServiciosExternos = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CostoTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HorasHombre = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ObservacionesFinales = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaAprobacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaFinalizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaEntrega = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenesTrabajo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdenesTrabajo_Equipos_EquipoId",
                        column: x => x.EquipoId,
                        principalTable: "Equipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdenesTrabajo_Usuarios_UsuarioAprobadorId",
                        column: x => x.UsuarioAprobadorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdenesTrabajo_Usuarios_UsuarioAsignadoId",
                        column: x => x.UsuarioAsignadoId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdenesTrabajo_Usuarios_UsuarioCreadorId",
                        column: x => x.UsuarioCreadorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MantenimientosPreventivosTareas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MantenimientoPreventivoId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MantenimientosPreventivosTareas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MantenimientosPreventivosTareas_MantenimientosPreventivos_MantenimientoPreventivoId",
                        column: x => x.MantenimientoPreventivoId,
                        principalTable: "MantenimientosPreventivos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistorialEquipos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipoId = table.Column<int>(type: "int", nullable: false),
                    TipoEvento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    OrdenTrabajoId = table.Column<int>(type: "int", nullable: true),
                    FechaEvento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialEquipos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialEquipos_Equipos_EquipoId",
                        column: x => x.EquipoId,
                        principalTable: "Equipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistorialEquipos_OrdenesTrabajo_OrdenTrabajoId",
                        column: x => x.OrdenTrabajoId,
                        principalTable: "OrdenesTrabajo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HistorialEquipos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notificaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    TipoNotificacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Leida = table.Column<bool>(type: "bit", nullable: false),
                    EnviadaPorEmail = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaLeida = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrdenTrabajoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notificaciones_OrdenesTrabajo_OrdenTrabajoId",
                        column: x => x.OrdenTrabajoId,
                        principalTable: "OrdenesTrabajo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notificaciones_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrdenesTrabajoImagenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrdenTrabajoId = table.Column<int>(type: "int", nullable: false),
                    RutaArchivo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TipoImagen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenesTrabajoImagenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdenesTrabajoImagenes_OrdenesTrabajo_OrdenTrabajoId",
                        column: x => x.OrdenTrabajoId,
                        principalTable: "OrdenesTrabajo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrdenesTrabajoRepuestos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrdenTrabajoId = table.Column<int>(type: "int", nullable: false),
                    RepuestoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NumeroSerie = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FechaInstalacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenesTrabajoRepuestos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdenesTrabajoRepuestos_OrdenesTrabajo_OrdenTrabajoId",
                        column: x => x.OrdenTrabajoId,
                        principalTable: "OrdenesTrabajo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdenesTrabajoRepuestos_Repuestos_RepuestoId",
                        column: x => x.RepuestoId,
                        principalTable: "Repuestos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Bodegas",
                columns: new[] { "Id", "Activo", "FechaCreacion", "Nombre", "Responsable", "Ubicacion" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 12, 1, 11, 17, 16, 352, DateTimeKind.Local).AddTicks(6368), "San Ignacio", null, null },
                    { 2, true, new DateTime(2025, 12, 1, 11, 17, 16, 352, DateTimeKind.Local).AddTicks(6369), "Mirador", null, null },
                    { 3, true, new DateTime(2025, 12, 1, 11, 17, 16, 352, DateTimeKind.Local).AddTicks(6371), "Bonanza", null, null }
                });

            migrationBuilder.InsertData(
                table: "Empresas",
                columns: new[] { "Id", "Activo", "Direccion", "Email", "FechaCreacion", "Nombre", "RUC", "Telefono" },
                values: new object[,]
                {
                    { 1, true, null, null, new DateTime(2025, 12, 1, 11, 17, 16, 352, DateTimeKind.Local).AddTicks(6253), "Top Green S.A", null, null },
                    { 2, true, null, null, new DateTime(2025, 12, 1, 11, 17, 16, 352, DateTimeKind.Local).AddTicks(6254), "Tecnología Agrícola S.A", null, null },
                    { 3, true, null, null, new DateTime(2025, 12, 1, 11, 17, 16, 352, DateTimeKind.Local).AddTicks(6256), "Servicios Agrícolas del Pacífico", null, null }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Activo", "Email", "EmpresaId", "FechaCreacion", "NombreCompleto", "PasswordHash", "Rol", "Telefono", "UltimoAcceso", "Username" },
                values: new object[] { 1, true, "admin@taller.com", 1, new DateTime(2025, 12, 1, 11, 17, 16, 456, DateTimeKind.Local).AddTicks(3046), "Administrador del Sistema", "$2a$11$uD4ZHvm19cWNCFSocthxEuadtAnBF2bf044eDxNTisadfzRFb..xK", 1, null, null, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Equipos_Codigo",
                table: "Equipos",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Equipos_EmpresaId",
                table: "Equipos",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialEquipos_EquipoId",
                table: "HistorialEquipos",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialEquipos_OrdenTrabajoId",
                table: "HistorialEquipos",
                column: "OrdenTrabajoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialEquipos_UsuarioId",
                table: "HistorialEquipos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_MantenimientosPreventivos_EquipoId",
                table: "MantenimientosPreventivos",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_MantenimientosPreventivosTareas_MantenimientoPreventivoId",
                table: "MantenimientosPreventivosTareas",
                column: "MantenimientoPreventivoId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosInventario_RepuestoId",
                table: "MovimientosInventario",
                column: "RepuestoId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosInventario_UsuarioId",
                table: "MovimientosInventario",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_OrdenTrabajoId",
                table: "Notificaciones",
                column: "OrdenTrabajoId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_UsuarioId",
                table: "Notificaciones",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesTrabajo_EquipoId",
                table: "OrdenesTrabajo",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesTrabajo_NumeroOrden",
                table: "OrdenesTrabajo",
                column: "NumeroOrden",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesTrabajo_UsuarioAprobadorId",
                table: "OrdenesTrabajo",
                column: "UsuarioAprobadorId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesTrabajo_UsuarioAsignadoId",
                table: "OrdenesTrabajo",
                column: "UsuarioAsignadoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesTrabajo_UsuarioCreadorId",
                table: "OrdenesTrabajo",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesTrabajoImagenes_OrdenTrabajoId",
                table: "OrdenesTrabajoImagenes",
                column: "OrdenTrabajoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesTrabajoRepuestos_OrdenTrabajoId",
                table: "OrdenesTrabajoRepuestos",
                column: "OrdenTrabajoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesTrabajoRepuestos_RepuestoId",
                table: "OrdenesTrabajoRepuestos",
                column: "RepuestoId");

            migrationBuilder.CreateIndex(
                name: "IX_Repuestos_BodegaId",
                table: "Repuestos",
                column: "BodegaId");

            migrationBuilder.CreateIndex(
                name: "IX_Repuestos_Codigo",
                table: "Repuestos",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_EmpresaId",
                table: "Usuarios",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Username",
                table: "Usuarios",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistorialEquipos");

            migrationBuilder.DropTable(
                name: "MantenimientosPreventivosTareas");

            migrationBuilder.DropTable(
                name: "MovimientosInventario");

            migrationBuilder.DropTable(
                name: "Notificaciones");

            migrationBuilder.DropTable(
                name: "OrdenesTrabajoImagenes");

            migrationBuilder.DropTable(
                name: "OrdenesTrabajoRepuestos");

            migrationBuilder.DropTable(
                name: "MantenimientosPreventivos");

            migrationBuilder.DropTable(
                name: "OrdenesTrabajo");

            migrationBuilder.DropTable(
                name: "Repuestos");

            migrationBuilder.DropTable(
                name: "Equipos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Bodegas");

            migrationBuilder.DropTable(
                name: "Empresas");
        }
    }
}
