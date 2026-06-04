USE TechHelpDB;
GO

INSERT INTO Usuarios (RolId, NombreCompleto, NombreUsuario, Correo, ClaveHash, Estado, FechaRegistro) 
VALUES 
(1, 'Administrador del Sistema', 'admin', 'admin@institucion.cl', 'admin123', 1, GETDATE()),
(2, 'Operador de Soporte', 'soporte', 'soporte@institucion.cl', 'soporte123', 1, GETDATE()),
(3, 'Usuario de Prueba', 'usuario', 'usuario@institucion.cl', 'usuario123', 1, GETDATE());
GO
