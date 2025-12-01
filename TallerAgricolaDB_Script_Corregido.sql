-- =============================================
-- Script de Base de Datos: Sistema de Gestión de Taller Agrícola
-- Version Corregida - Compatible con Backend ASP.NET Core
-- =============================================

USE master;
GO

-- Crear la base de datos si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'TallerAgricolaDB')
BEGIN
    CREATE DATABASE TallerAgricolaDB;
END
GO

USE TallerAgricolaDB;
GO

-- =============================================
-- Tabla: Empresas
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Empresas')
BEGIN
    CREATE TABLE Empresas (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(200) NOT NULL,
        RUC NVARCHAR(20) NULL,
        Direccion NVARCHAR(500) NULL,
        Telefono NVARCHAR(20) NULL,
        Email NVARCHAR(100) NULL,
        Activo BIT NOT NULL DEFAULT 1,
        FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE()
    );

    -- Insertar datos iniciales
    INSERT INTO Empresas (Nombre, RUC, Direccion, Telefono, Email, Activo, FechaCreacion)
    VALUES
        ('Top Green S.A', '20123456789', 'Av. Principal 123', '999-8888', 'contacto@topgreen.com', 1, GETDATE()),
        ('Tecnología Agrícola S.A', '20987654321', 'Calle Secundaria 456', '999-7777', 'info@tecagricola.com', 1, GETDATE()),
        ('Servicios Agrícolas del Pacífico', '20456789123', 'Jr. Los Olivos 789', '999-6666', 'contacto@sapsa.com', 1, GETDATE());
END
GO

-- =============================================
-- Tabla: Usuarios
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Usuarios')
BEGIN
    CREATE TABLE Usuarios (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        NombreCompleto NVARCHAR(200) NOT NULL,
        Email NVARCHAR(100) NOT NULL UNIQUE,
        Username NVARCHAR(50) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(255) NOT NULL,
        Rol TINYINT NOT NULL CHECK (Rol IN (1, 2, 3, 4, 5)), -- 1=Admin, 2=Coordinador, 3=Mecanico, 4=Bodeguero, 5=Visualizador
        EmpresaId INT NOT NULL,
        Telefono NVARCHAR(20) NULL,
        Activo BIT NOT NULL DEFAULT 1,
        FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Usuarios_Empresas FOREIGN KEY (EmpresaId) REFERENCES Empresas(Id) ON DELETE NO ACTION
    );

    -- Insertar usuario administrador (Password: Admin123! - Hash BCrypt)
    INSERT INTO Usuarios (NombreCompleto, Email, Username, PasswordHash, Rol, EmpresaId, Activo, FechaCreacion)
    VALUES ('Administrador del Sistema', 'admin@taller.com', 'admin', '$2a$11$uD4ZHvm19cWNCFSocthxEuadtAnBF2bf044eDxNTisadfzRFb..xK', 1, 1, 1, GETDATE());
END
GO

-- =============================================
-- Tabla: Equipos
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Equipos')
BEGIN
    CREATE TABLE Equipos (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Codigo NVARCHAR(50) NOT NULL UNIQUE,
        Nombre NVARCHAR(200) NOT NULL,
        Marca NVARCHAR(100) NULL,
        Modelo NVARCHAR(100) NULL,
        NumeroSerie NVARCHAR(100) NULL,
        TipoEquipo TINYINT NOT NULL CHECK (TipoEquipo IN (1, 2, 3, 4, 5)), -- 1=Vehiculo, 2=Tractor, 3=Cosechadora, 4=Fumigadora, 5=Implemento
        EmpresaId INT NOT NULL,
        Anio INT NULL,
        KilometrajeActual DECIMAL(18, 2) NULL,
        HorasUsoActuales DECIMAL(18, 2) NULL,
        TipoControl TINYINT NULL, -- 1=Kilometros, 2=Horas, 3=TiempoCalendario (NULLABLE)
        ConsumoPromedioCombustible DECIMAL(18, 2) NULL,
        Observaciones NVARCHAR(MAX) NULL,
        FechaUltimoMantenimiento DATETIME2 NULL,
        Activo BIT NOT NULL DEFAULT 1,
        FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Equipos_Empresas FOREIGN KEY (EmpresaId) REFERENCES Empresas(Id) ON DELETE NO ACTION
    );
END
GO

-- =============================================
-- Tabla: Bodegas
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Bodegas')
BEGIN
    CREATE TABLE Bodegas (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(200) NOT NULL,
        Ubicacion NVARCHAR(500) NULL,
        Responsable NVARCHAR(200) NULL,
        Telefono NVARCHAR(20) NULL,
        Activo BIT NOT NULL DEFAULT 1,
        FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE()
    );

    -- Insertar datos iniciales
    INSERT INTO Bodegas (Nombre, Ubicacion, Responsable, Activo, FechaCreacion)
    VALUES
        ('Bodega San Ignacio', 'San Ignacio - Tumbes', 'Juan Pérez', 1, GETDATE()),
        ('Bodega Mirador', 'Mirador - Tumbes', 'María García', 1, GETDATE()),
        ('Bodega Bonanza', 'Bonanza - Tumbes', 'Carlos López', 1, GETDATE());
END
GO

-- =============================================
-- Tabla: Repuestos
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Repuestos')
BEGIN
    CREATE TABLE Repuestos (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Codigo NVARCHAR(50) NOT NULL UNIQUE,
        Nombre NVARCHAR(200) NOT NULL,
        Descripcion NVARCHAR(MAX) NULL,
        Marca NVARCHAR(100) NULL,
        StockActual DECIMAL(18, 2) NOT NULL DEFAULT 0,
        StockMinimo DECIMAL(18, 2) NOT NULL DEFAULT 0,
        UnidadMedida NVARCHAR(50) NULL,
        PrecioUnitario DECIMAL(18, 2) NULL,
        BodegaId INT NOT NULL,
        Activo BIT NOT NULL DEFAULT 1,
        FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Repuestos_Bodegas FOREIGN KEY (BodegaId) REFERENCES Bodegas(Id) ON DELETE NO ACTION
    );
END
GO

-- =============================================
-- Tabla: OrdenesTrabajo
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OrdenesTrabajo')
BEGIN
    CREATE TABLE OrdenesTrabajo (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        NumeroOrden NVARCHAR(50) NOT NULL UNIQUE,
        EquipoId INT NOT NULL,
        TipoMantenimiento TINYINT NOT NULL CHECK (TipoMantenimiento IN (1, 2, 3)), -- 1=Preventivo, 2=Correctivo, 3=Predictivo
        Prioridad TINYINT NOT NULL CHECK (Prioridad IN (1, 2, 3, 4)) DEFAULT 2, -- 1=Baja, 2=Media, 3=Alta, 4=Urgente
        Estado TINYINT NOT NULL CHECK (Estado IN (1, 2, 3, 4, 5, 6, 7, 8, 9, 10)) DEFAULT 1, -- 1=Registrada ... 10=Cancelada
        DescripcionProblema NVARCHAR(MAX) NULL,
        DiagnosticoInicial NVARCHAR(MAX) NULL,
        TrabajoRealizado NVARCHAR(MAX) NULL,
        Observaciones NVARCHAR(MAX) NULL,
        KilometrajeRegistrado DECIMAL(18, 2) NULL,
        HorasUsoRegistradas DECIMAL(18, 2) NULL,
        UsuarioCreadorId INT NOT NULL,
        UsuarioAsignadoId INT NULL,
        FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
        FechaInicio DATETIME2 NULL,
        FechaFinalizacion DATETIME2 NULL,
        FechaEntrega DATETIME2 NULL,
        CostoRepuestos DECIMAL(18, 2) NULL DEFAULT 0,
        CostoManoObra DECIMAL(18, 2) NULL DEFAULT 0,
        CostoTotal DECIMAL(18, 2) NULL DEFAULT 0,
        CONSTRAINT FK_OrdenesTrabajo_Equipos FOREIGN KEY (EquipoId) REFERENCES Equipos(Id) ON DELETE NO ACTION,
        CONSTRAINT FK_OrdenesTrabajo_UsuarioCreador FOREIGN KEY (UsuarioCreadorId) REFERENCES Usuarios(Id) ON DELETE NO ACTION,
        CONSTRAINT FK_OrdenesTrabajo_UsuarioAsignado FOREIGN KEY (UsuarioAsignadoId) REFERENCES Usuarios(Id) ON DELETE NO ACTION
    );
END
GO

-- =============================================
-- Tabla: OrdenesTrabajoRepuestos
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OrdenesTrabajoRepuestos')
BEGIN
    CREATE TABLE OrdenesTrabajoRepuestos (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        OrdenTrabajoId INT NOT NULL,
        RepuestoId INT NOT NULL,
        Cantidad DECIMAL(18, 2) NOT NULL,
        PrecioUnitario DECIMAL(18, 2) NOT NULL,
        Subtotal DECIMAL(18, 2) NOT NULL,
        NumerosSerieUsados NVARCHAR(MAX) NULL,
        CONSTRAINT FK_OrdenesTrabajoRepuestos_OrdenesTrabajo FOREIGN KEY (OrdenTrabajoId) REFERENCES OrdenesTrabajo(Id) ON DELETE CASCADE,
        CONSTRAINT FK_OrdenesTrabajoRepuestos_Repuestos FOREIGN KEY (RepuestoId) REFERENCES Repuestos(Id) ON DELETE NO ACTION
    );
END
GO

-- =============================================
-- Tabla: OrdenesTrabajoImagenes
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OrdenesTrabajoImagenes')
BEGIN
    CREATE TABLE OrdenesTrabajoImagenes (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        OrdenTrabajoId INT NOT NULL,
        RutaImagen NVARCHAR(500) NOT NULL,
        TipoImagen NVARCHAR(50) NULL,
        Descripcion NVARCHAR(500) NULL,
        FechaSubida DATETIME2 NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_OrdenesTrabajoImagenes_OrdenesTrabajo FOREIGN KEY (OrdenTrabajoId) REFERENCES OrdenesTrabajo(Id) ON DELETE CASCADE
    );
END
GO

-- =============================================
-- Tabla: MantenimientosPreventivos
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MantenimientosPreventivos')
BEGIN
    CREATE TABLE MantenimientosPreventivos (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        EquipoId INT NOT NULL,
        Nombre NVARCHAR(200) NOT NULL,
        Descripcion NVARCHAR(MAX) NULL,
        TipoControl TINYINT NOT NULL CHECK (TipoControl IN (1, 2, 3)), -- 1=Kilometros, 2=Horas, 3=TiempoCalendario
        IntervaloDias INT NULL,
        IntervaloKilometros DECIMAL(18, 2) NULL,
        IntervaloHoras DECIMAL(18, 2) NULL,
        UltimaFechaEjecucion DATETIME2 NULL,
        UltimoKilometrajeEjecucion DECIMAL(18, 2) NULL,
        UltimasHorasEjecucion DECIMAL(18, 2) NULL,
        ProximaFecha DATETIME2 NULL,
        ProximoKilometraje DECIMAL(18, 2) NULL,
        ProximasHoras DECIMAL(18, 2) NULL,
        Activo BIT NOT NULL DEFAULT 1,
        FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_MantenimientosPreventivos_Equipos FOREIGN KEY (EquipoId) REFERENCES Equipos(Id) ON DELETE NO ACTION
    );
END
GO

-- =============================================
-- Tabla: MantenimientosPreventivosTareas
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MantenimientosPreventivosTareas')
BEGIN
    CREATE TABLE MantenimientosPreventivosTareas (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        MantenimientoPreventivoId INT NOT NULL,
        Descripcion NVARCHAR(500) NOT NULL,
        TiempoEstimadoMinutos INT NULL,
        Orden INT NOT NULL DEFAULT 0,
        CONSTRAINT FK_MantenimientosPreventivosTareas_MantenimientosPreventivos FOREIGN KEY (MantenimientoPreventivoId) REFERENCES MantenimientosPreventivos(Id) ON DELETE CASCADE
    );
END
GO

-- =============================================
-- Tabla: MovimientosInventario
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MovimientosInventario')
BEGIN
    CREATE TABLE MovimientosInventario (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        RepuestoId INT NOT NULL,
        TipoMovimiento NVARCHAR(50) NOT NULL,
        Cantidad DECIMAL(18, 2) NOT NULL,
        StockAnterior DECIMAL(18, 2) NOT NULL,
        StockNuevo DECIMAL(18, 2) NOT NULL,
        OrdenTrabajoId INT NULL,
        UsuarioId INT NOT NULL,
        Observaciones NVARCHAR(MAX) NULL,
        FechaMovimiento DATETIME2 NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_MovimientosInventario_Repuestos FOREIGN KEY (RepuestoId) REFERENCES Repuestos(Id) ON DELETE NO ACTION,
        CONSTRAINT FK_MovimientosInventario_OrdenesTrabajo FOREIGN KEY (OrdenTrabajoId) REFERENCES OrdenesTrabajo(Id) ON DELETE NO ACTION,
        CONSTRAINT FK_MovimientosInventario_Usuarios FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id) ON DELETE NO ACTION
    );
END
GO

-- =============================================
-- Tabla: HistorialEquipos
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'HistorialEquipos')
BEGIN
    CREATE TABLE HistorialEquipos (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        EquipoId INT NOT NULL,
        TipoEvento NVARCHAR(100) NOT NULL,
        Descripcion NVARCHAR(MAX) NULL,
        OrdenTrabajoId INT NULL,
        UsuarioId INT NOT NULL,
        FechaEvento DATETIME2 NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_HistorialEquipos_Equipos FOREIGN KEY (EquipoId) REFERENCES Equipos(Id) ON DELETE NO ACTION,
        CONSTRAINT FK_HistorialEquipos_OrdenesTrabajo FOREIGN KEY (OrdenTrabajoId) REFERENCES OrdenesTrabajo(Id) ON DELETE NO ACTION,
        CONSTRAINT FK_HistorialEquipos_Usuarios FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id) ON DELETE NO ACTION
    );
END
GO

-- =============================================
-- Tabla: Notificaciones
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Notificaciones')
BEGIN
    CREATE TABLE Notificaciones (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UsuarioId INT NOT NULL,
        Titulo NVARCHAR(200) NOT NULL,
        Mensaje NVARCHAR(MAX) NOT NULL,
        TipoNotificacion NVARCHAR(50) NULL,
        Leida BIT NOT NULL DEFAULT 0,
        FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
        FechaLeida DATETIME2 NULL,
        OrdenTrabajoId INT NULL,
        CONSTRAINT FK_Notificaciones_Usuarios FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id) ON DELETE NO ACTION,
        CONSTRAINT FK_Notificaciones_OrdenesTrabajo FOREIGN KEY (OrdenTrabajoId) REFERENCES OrdenesTrabajo(Id) ON DELETE NO ACTION
    );
END
GO

-- =============================================
-- Índices para mejorar rendimiento
-- =============================================

-- Índices en Equipos
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Equipos_EmpresaId' AND object_id = OBJECT_ID('Equipos'))
    CREATE NONCLUSTERED INDEX IX_Equipos_EmpresaId ON Equipos(EmpresaId);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Equipos_TipoEquipo' AND object_id = OBJECT_ID('Equipos'))
    CREATE NONCLUSTERED INDEX IX_Equipos_TipoEquipo ON Equipos(TipoEquipo);

-- Índices en OrdenesTrabajo
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_OrdenesTrabajo_EquipoId' AND object_id = OBJECT_ID('OrdenesTrabajo'))
    CREATE NONCLUSTERED INDEX IX_OrdenesTrabajo_EquipoId ON OrdenesTrabajo(EquipoId);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_OrdenesTrabajo_Estado' AND object_id = OBJECT_ID('OrdenesTrabajo'))
    CREATE NONCLUSTERED INDEX IX_OrdenesTrabajo_Estado ON OrdenesTrabajo(Estado);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_OrdenesTrabajo_UsuarioAsignadoId' AND object_id = OBJECT_ID('OrdenesTrabajo'))
    CREATE NONCLUSTERED INDEX IX_OrdenesTrabajo_UsuarioAsignadoId ON OrdenesTrabajo(UsuarioAsignadoId);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_OrdenesTrabajo_FechaCreacion' AND object_id = OBJECT_ID('OrdenesTrabajo'))
    CREATE NONCLUSTERED INDEX IX_OrdenesTrabajo_FechaCreacion ON OrdenesTrabajo(FechaCreacion);

-- Índices en Repuestos
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Repuestos_BodegaId' AND object_id = OBJECT_ID('Repuestos'))
    CREATE NONCLUSTERED INDEX IX_Repuestos_BodegaId ON Repuestos(BodegaId);

-- Índices en MovimientosInventario
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_MovimientosInventario_RepuestoId' AND object_id = OBJECT_ID('MovimientosInventario'))
    CREATE NONCLUSTERED INDEX IX_MovimientosInventario_RepuestoId ON MovimientosInventario(RepuestoId);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_MovimientosInventario_FechaMovimiento' AND object_id = OBJECT_ID('MovimientosInventario'))
    CREATE NONCLUSTERED INDEX IX_MovimientosInventario_FechaMovimiento ON MovimientosInventario(FechaMovimiento);

-- Índices en HistorialEquipos
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_HistorialEquipos_EquipoId' AND object_id = OBJECT_ID('HistorialEquipos'))
    CREATE NONCLUSTERED INDEX IX_HistorialEquipos_EquipoId ON HistorialEquipos(EquipoId);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_HistorialEquipos_FechaEvento' AND object_id = OBJECT_ID('HistorialEquipos'))
    CREATE NONCLUSTERED INDEX IX_HistorialEquipos_FechaEvento ON HistorialEquipos(FechaEvento);

-- Índices en Notificaciones
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Notificaciones_UsuarioId' AND object_id = OBJECT_ID('Notificaciones'))
    CREATE NONCLUSTERED INDEX IX_Notificaciones_UsuarioId ON Notificaciones(UsuarioId);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Notificaciones_Leida' AND object_id = OBJECT_ID('Notificaciones'))
    CREATE NONCLUSTERED INDEX IX_Notificaciones_Leida ON Notificaciones(Leida);

GO

PRINT '============================================='
PRINT 'Script completado exitosamente'
PRINT 'Base de datos: TallerAgricolaDB'
PRINT '============================================='
PRINT 'Datos iniciales creados:'
PRINT '  - 3 Empresas'
PRINT '  - 3 Bodegas'
PRINT '  - 1 Usuario Administrador'
PRINT '    Username: admin'
PRINT '    Password: Admin123!'
PRINT '============================================='
GO
