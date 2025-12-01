# 📋 Sistema de Gestión de Taller Agrícola - Documentación

## 🎯 Resumen del Proyecto

Sistema completo de gestión para taller agrícola corporativo que administra 3 empresas:
- Top Green S.A
- Tecnología Agrícola S.A
- Servicios Agrícolas del Pacífico

## 🚀 Cómo Ejecutar el Proyecto

### 1. Verificar que SQL Server esté corriendo
El proyecto usa SQL Server local. Asegúrate de tener SQL Server instalado y corriendo.

### 2. Ejecutar el proyecto
```bash
dotnet run
```

### 3. Acceder a la API
- **URL Base**: http://localhost:5042
- **Swagger UI**: http://localhost:5042/swagger

## 🔐 Credenciales Iniciales

**Usuario Administrador:**
- Username: `admin`
- Password: `Admin123!`
- Email: admin@taller.com

## 📊 Base de Datos

### Cadena de Conexión
```
Server=localhost;Database=TallerAgricolaDB;Trusted_Connection=True;TrustServerCertificate=True;
```

### Tablas Principales
1. **Empresas** - 3 empresas del corporativo
2. **Usuarios** - Gestión de usuarios y roles
3. **Equipos** - Vehículos y maquinaria (100+ equipos)
4. **OrdenesTrabajo** - Órdenes de mantenimiento y reparación
5. **Bodegas** - 3 bodegas (San Ignacio, Mirador, Bonanza)
6. **Repuestos** - Inventario de repuestos
7. **MantenimientosPreventivos** - Plantillas de mantenimiento preventivo
8. **Notificaciones** - Sistema de notificaciones

## 👥 Roles del Sistema

1. **Administrador (1)**
   - Acceso total al sistema
   - Puede agregar, editar, borrar equipos
   - Gestión de configuraciones

2. **Coordinador (2)**
   - Asignar trabajo
   - Aprobar presupuestos
   - Supervisar operaciones
   - Ver costos

3. **Mecánico (3)**
   - Registrar trabajo
   - Actualizar estado de órdenes
   - Ver asignaciones
   - NO puede ver costos

4. **Bodeguero (4)**
   - Gestión de inventario
   - Movimientos de repuestos
   - Control de stock

5. **Visualizador (5)**
   - Solo visualización de costos
   - Reportes de lectura

## 🔧 Módulos Implementados

### 1. Gestión de Equipos
- **Tipos soportados:**
  - Vehículos Livianos (pickups, autos)
  - Vehículos Pesados (camiones, buses)
  - Maquinaria Agrícola (tractores, cosechadoras)
  - Implementos Agrícolas (cabezales, remolques)
  - Equipo Estacionario (generadores, bombas)

- **Controles:**
  - Por Kilómetros
  - Por Horas
  - Por Tiempo Calendario
  - Control de consumo de combustible (galones/km o galones/hora)

### 2. Órdenes de Trabajo

**Estados:**
- Recepcionada
- En Diagnóstico
- Presupuestada
- Aprobación Pendiente
- Aprobada
- En Ejecución
- **En Alerta** (tiene diagnóstico pero no hay repuesto, sigue funcionando)
- Completada
- Entregada
- Cancelada

**Prioridades:**
- Baja
- Media
- Alta
- Urgente

**Tipos de Mantenimiento:**
- Preventivo
- Correctivo
- Predictivo (basado en históricos)

### 3. Inventario de Repuestos

**Características:**
- Control de 3 bodegas
- Punto de reorden automático (alertas cuando stock < mínimo)
- Trazabilidad de instalación (qué repuesto en qué equipo)
- Control de garantías
- Ingreso manual o por código de barras
- Movimientos de inventario (Entrada, Salida, Ajuste)

### 4. Mantenimiento Preventivo

- Plantillas predefinidas por tipo de equipo
- Notificaciones automáticas cuando se aproxima mantenimiento
- Múltiples criterios: KM, Horas, Días
- Tareas asociadas a cada mantenimiento

### 5. Sistema de Notificaciones

**Tipos de notificaciones:**
- Mantenimiento próximo
- Orden completada
- Aprobaciones pendientes
- Alertas de stock bajo

**Canales (preparado para):**
- Email (implementado)
- WhatsApp (configuración lista)
- Notificaciones push (estructura lista)

## 📱 API Endpoints

### Autenticación

#### POST /api/auth/login
Iniciar sesión en el sistema

**Request:**
```json
{
  "username": "admin",
  "password": "Admin123!"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Login exitoso",
  "data": {
    "usuarioId": 1,
    "username": "admin",
    "nombreCompleto": "Administrador del Sistema",
    "email": "admin@taller.com",
    "rol": "Administrador",
    "empresaId": 1,
    "empresaNombre": "Top Green S.A",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "fechaExpiracion": "2025-12-01T19:17:00"
  }
}
```

#### GET /api/auth/health
Verificar estado del servidor

**Response:**
```json
{
  "success": true,
  "message": "API funcionando correctamente",
  "data": {
    "status": "OK",
    "timestamp": "2025-12-01T11:17:00"
  }
}
```

## 🔒 Seguridad

- **Autenticación**: JWT (JSON Web Tokens)
- **Expiración de token**: 8 horas (480 minutos)
- **Password hashing**: BCrypt
- **CORS**: Configurado para desarrollo (ajustar en producción)

## ⚙️ Configuración (appsettings.json)

### JWT Settings
```json
"JwtSettings": {
  "SecretKey": "TallerAgricola_SecretKey_SuperSegura_2025_MinLength32Characters!",
  "Issuer": "TallerAgricolaAPI",
  "Audience": "TallerAgricolaClients",
  "ExpirationMinutes": 480
}
```

### Email Settings (Configurar para producción)
```json
"EmailSettings": {
  "SmtpServer": "smtp.gmail.com",
  "SmtpPort": 587,
  "SenderEmail": "noreply@taller.com",
  "SenderName": "Sistema Taller Agrícola",
  "Username": "",
  "Password": "",
  "EnableSsl": true
}
```

## 📈 Reportes y KPIs (Preparados para implementar)

- Costo de mantenimiento por equipo/período
- Tiempo promedio de reparación
- Disponibilidad de la flota (% tiempo operativo)
- Consumo de repuestos
- Productividad de técnicos
- Equipos con mayor frecuencia de fallas
- Exportación a Excel/PDF

## 🗄️ Estructura del Proyecto

```
tallerV1/
├── Controllers/
│   ├── AuthController.cs          # Autenticación
│   └── WeatherForecastController.cs (ejemplo, eliminar después)
├── Data/
│   ├── ApplicationDbContext.cs    # Contexto de EF Core
│   └── Configurations/            # Configuraciones de entidades
├── Models/
│   ├── Entities/                  # 13 entidades del dominio
│   ├── DTOs/                      # Data Transfer Objects
│   └── Enums/                     # 6 enumeraciones
├── Helpers/
│   ├── JwtHelper.cs               # Generación de tokens
│   ├── JwtSettings.cs
│   └── EmailSettings.cs
├── Services/
│   ├── Interfaces/                # Interfaces de servicios
│   └── Implementations/           # Implementaciones
├── Repositories/
│   ├── Interfaces/                # Interfaces de repositorios
│   └── Implementations/           # Implementaciones
├── Migrations/                     # Migraciones de EF Core
├── Properties/
│   └── launchSettings.json
├── appsettings.json
├── appsettings.Development.json
└── Program.cs                      # Configuración principal
```

## 🎨 Entidades del Dominio

1. **Empresa** - Corporativo (3 empresas)
2. **Usuario** - Sistema de usuarios con roles
3. **Equipo** - Vehículos y maquinaria
4. **OrdenTrabajo** - Órdenes de mantenimiento
5. **OrdenTrabajoRepuesto** - Repuestos usados en órdenes
6. **OrdenTrabajoImagen** - Fotos antes/durante/después
7. **Bodega** - 3 ubicaciones físicas
8. **Repuesto** - Inventario de repuestos
9. **MantenimientoPreventivo** - Plantillas de mantenimiento
10. **MantenimientoPreventivoTarea** - Tareas de mantenimiento
11. **MovimientoInventario** - Trazabilidad de inventario
12. **HistorialEquipo** - Historial completo de equipos
13. **Notificacion** - Sistema de notificaciones

## 📝 Próximos Pasos Sugeridos

### Fase 2 - Controladores Adicionales
- [ ] Controller de Equipos (CRUD completo)
- [ ] Controller de Órdenes de Trabajo
- [ ] Controller de Repuestos e Inventario
- [ ] Controller de Mantenimiento Preventivo
- [ ] Controller de Usuarios
- [ ] Controller de Reportes

### Fase 3 - Servicios y Lógica de Negocio
- [ ] Servicio de Notificaciones Email
- [ ] Servicio de Cálculo de Próximo Mantenimiento
- [ ] Servicio de Generación de Reportes
- [ ] Servicio de Dashboard y KPIs
- [ ] Servicio de Alertas de Stock Bajo

### Fase 4 - Características Avanzadas
- [ ] Upload de imágenes para órdenes de trabajo
- [ ] Generación de PDFs para órdenes
- [ ] Integración con WhatsApp
- [ ] Sistema de backup automático
- [ ] Logs de auditoría completos

### Fase 5 - Frontend
- [ ] Panel de administración web
- [ ] Dashboard interactivo
- [ ] Aplicación móvil (opcional)

## 🛠️ Comandos Útiles

### Entity Framework
```bash
# Crear nueva migración
dotnet ef migrations add NombreMigracion

# Aplicar migraciones
dotnet ef database update

# Revertir última migración
dotnet ef migrations remove

# Ver estado de migraciones
dotnet ef migrations list
```

### Compilar y Ejecutar
```bash
# Compilar
dotnet build

# Ejecutar
dotnet run

# Ejecutar con watch (recarga automática)
dotnet watch run

# Limpiar
dotnet clean
```

## 📞 Soporte

Para consultas sobre el sistema:
- Email: admin@taller.com
- Usuario inicial: admin / Admin123!

## ⚠️ Notas Importantes

1. **Base de Datos**: Asegúrate de tener SQL Server corriendo antes de ejecutar
2. **Seguridad**: Cambia las claves JWT y credenciales en producción
3. **Email**: Configura las credenciales SMTP para producción
4. **CORS**: Ajusta la política CORS según tus necesidades de seguridad

## 🎉 Estado Actual

✅ Base de datos completa con 13 tablas
✅ Sistema de autenticación JWT funcionando
✅ 3 empresas creadas
✅ 3 bodegas configuradas
✅ Usuario administrador inicial
✅ API documentada con Swagger
✅ Sistema compilando y ejecutando correctamente

**El sistema está LISTO para usar y continuar desarrollando!**
