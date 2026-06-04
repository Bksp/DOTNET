CREATE DATABASE TechHelpDB;
GO
 
USE TechHelpDB;
GO
 
CREATE TABLE Roles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreRol NVARCHAR(50) NOT NULL UNIQUE,
    Descripcion NVARCHAR(200) NULL
);
GO
 
CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RolId INT NOT NULL,
    NombreCompleto NVARCHAR(100) NOT NULL,
    NombreUsuario NVARCHAR(50) NOT NULL UNIQUE,
    Correo NVARCHAR(100) NOT NULL UNIQUE,
    ClaveHash NVARCHAR(255) NOT NULL,
    Estado BIT NOT NULL DEFAULT 1,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Usuarios_Roles
        FOREIGN KEY (RolId) REFERENCES Roles(Id)
);
GO
 
CREATE TABLE AreasSolicitantes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreArea NVARCHAR(100) NOT NULL UNIQUE
);
GO
 
CREATE TABLE TiposProblema (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreTipoProblema NVARCHAR(100) NOT NULL UNIQUE
);
GO
 
CREATE TABLE Prioridades (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombrePrioridad NVARCHAR(20) NOT NULL UNIQUE
);
GO
 
CREATE TABLE EstadosSolicitud (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreEstado NVARCHAR(50) NOT NULL UNIQUE
);
GO
 
CREATE TABLE SolicitudesSoporte (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId INT NOT NULL,
    AreaSolicitanteId INT NOT NULL,
    TipoProblemaId INT NOT NULL,
    PrioridadId INT NOT NULL,
    EstadoSolicitudId INT NOT NULL DEFAULT 1,
    Descripcion NVARCHAR(500) NOT NULL,
    FechaSolicitud DATE NOT NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Solicitudes_Usuarios
        FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id),
    CONSTRAINT FK_Solicitudes_Areas
        FOREIGN KEY (AreaSolicitanteId) REFERENCES AreasSolicitantes(Id),
    CONSTRAINT FK_Solicitudes_TiposProblema
        FOREIGN KEY (TipoProblemaId) REFERENCES TiposProblema(Id),
    CONSTRAINT FK_Solicitudes_Prioridades
        FOREIGN KEY (PrioridadId) REFERENCES Prioridades(Id),
    CONSTRAINT FK_Solicitudes_Estados
        FOREIGN KEY (EstadoSolicitudId) REFERENCES EstadosSolicitud(Id)
);
GO
-- Datos iniciales obligatorios
INSERT INTO Roles (NombreRol, Descripcion) VALUES
('Administrador', 'Usuario con acceso a la administración del sistema'),
('Soporte', 'Usuario encargado de revisar solicitudes de soporte'),
('Usuario', 'Usuario que registra solicitudes de soporte');
GO
 
INSERT INTO AreasSolicitantes (NombreArea) VALUES
('Administración'), ('Docencia'), ('Laboratorio'),
('Biblioteca'), ('Dirección'), ('Otro');
GO
 
INSERT INTO TiposProblema (NombreTipoProblema) VALUES
('Problema con computador'), ('Problema con internet'),
('Problema con software'), ('Problema con impresora'),
('Problema con cuenta institucional'), ('Otro');
GO
 
INSERT INTO Prioridades (NombrePrioridad) VALUES
('Baja'), ('Media'), ('Alta');
GO
 
INSERT INTO EstadosSolicitud (NombreEstado) VALUES
('Pendiente'), ('En proceso'), ('Resuelto');
GO
