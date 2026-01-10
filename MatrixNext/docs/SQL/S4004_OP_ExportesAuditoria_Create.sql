-- Script de creación: OP_ExportesAuditoria
-- Propósito: Auditoría de exportaciones Excel (IPS, Planillas, etc.)
-- Fecha: 2026-01-09
-- Sprint: S4-004

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OP_ExportesAuditoria')
BEGIN
    CREATE TABLE [dbo].[OP_ExportesAuditoria]
    (
        [IdExporte] BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
        [TrabajoId] BIGINT NOT NULL,
        [Tipo] NVARCHAR(50) NOT NULL,  -- 'IPS', 'Planillas', 'Presupuestos', etc.
        [Usuario] BIGINT,               -- UserId who initiated export
        [FechaExportacion] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        [RutaArchivo] NVARCHAR(500) NOT NULL,
        [NombreArchivo] NVARCHAR(255) NOT NULL,
        [TamanoBytes] BIGINT,
        [Exitoso] BIT NOT NULL DEFAULT 1,
        [MensajeError] NVARCHAR(1000),
        [FechaProgramadaLimpieza] DATETIME2(7),  -- Fecha en que se ejecutará limpieza
        [Limpiado] BIT NOT NULL DEFAULT 0,
        [FechaLimpieza] DATETIME2(7),
        
        -- Índices para búsqueda eficiente
        CONSTRAINT FK_OP_ExportesAuditoria_Trabajo FOREIGN KEY ([TrabajoId]) 
            REFERENCES [dbo].[PYTrabajos]([IdTrabajoOP])
    )

    CREATE NONCLUSTERED INDEX [IX_OP_ExportesAuditoria_TrabajoId] 
        ON [dbo].[OP_ExportesAuditoria]([TrabajoId])

    CREATE NONCLUSTERED INDEX [IX_OP_ExportesAuditoria_FechaExportacion] 
        ON [dbo].[OP_ExportesAuditoria]([FechaExportacion])

    CREATE NONCLUSTERED INDEX [IX_OP_ExportesAuditoria_Tipo] 
        ON [dbo].[OP_ExportesAuditoria]([Tipo], [FechaExportacion])

    CREATE NONCLUSTERED INDEX [IX_OP_ExportesAuditoria_Limpieza] 
        ON [dbo].[OP_ExportesAuditoria]([Limpiado], [FechaProgramadaLimpieza])
        WHERE [Limpiado] = 0

    PRINT 'Tabla OP_ExportesAuditoria creada exitosamente'
END
ELSE
BEGIN
    PRINT 'Tabla OP_ExportesAuditoria ya existe'
END
GO
