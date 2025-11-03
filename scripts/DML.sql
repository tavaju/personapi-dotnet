USE persona_db;
GO

-- ============================================
-- LIMPIEZA PREVIA (para evitar duplicados)
-- Orden importante: eliminar primero las tablas con foreign keys
-- ============================================
IF OBJECT_ID('telefono', 'U') IS NOT NULL
BEGIN
    DELETE FROM telefono;
END
GO

IF OBJECT_ID('estudios', 'U') IS NOT NULL
BEGIN
    DELETE FROM estudios;
END
GO

IF OBJECT_ID('profesion', 'U') IS NOT NULL
BEGIN
    DELETE FROM profesion;
END
GO

IF OBJECT_ID('persona', 'U') IS NOT NULL
BEGIN
    DELETE FROM persona;
END
GO

-- ============================================
-- TABLA: persona
-- ============================================
IF OBJECT_ID('persona', 'U') IS NOT NULL
BEGIN
    INSERT INTO persona (cc, nombre, apellido, genero, edad)
    VALUES 
    (1001, 'Tatiana', 'Vivas', 'F', 22),
    (1002, 'Juan', 'Canon', 'M', 23),
    (1003, 'Valeria', 'Arenas', 'F', 21);
END
GO

-- ============================================
-- TABLA: profesion
-- ============================================
IF OBJECT_ID('profesion', 'U') IS NOT NULL
BEGIN
    INSERT INTO profesion (id, nom, des)
    VALUES
    (1, 'Ingeniería de Sistemas', 'Diseño, desarrollo y gestión de software'),
    (2, 'Arquitectura', 'Diseño y planificación de edificaciones y estructuras'),
    (3, 'Administración de Empresas', 'Gestión de recursos organizacionales y liderazgo');
END
GO

-- ============================================
-- TABLA: estudios
-- ============================================
IF OBJECT_ID('estudios', 'U') IS NOT NULL
BEGIN
    INSERT INTO estudios (id_prof, cc_per, fecha, univer)
    VALUES
    (1, 1001, '2024-06-01', 'Pontificia Universidad Javeriana'),
    (2, 1002, '2023-11-15', 'Universidad Nacional de Colombia'),
    (3, 1003, '2022-12-10', 'Universidad de los Andes');
END
GO

-- ============================================
-- TABLA: telefono
-- ============================================
IF OBJECT_ID('telefono', 'U') IS NOT NULL
BEGIN
    INSERT INTO telefono (num, oper, duenio)
    VALUES
    ('3001112233', 'Claro', 1001),
    ('3129876543', 'Tigo', 1002),
    ('3205559999', 'Movistar', 1003);
END
GO

-- ============================================
-- CONSULTAS DE VALIDACIÓN
-- ============================================

-- 1️⃣ Personas con su profesión y universidad
SELECT 
    p.cc,
    p.nombre + ' ' + p.apellido AS NombreCompleto,
    pr.nom AS Profesion,
    e.univer AS Universidad,
    e.fecha AS Fecha_Estudio
FROM persona p
JOIN estudios e ON p.cc = e.cc_per
JOIN profesion pr ON e.id_prof = pr.id;

-- 2️⃣ Teléfonos por persona
SELECT 
    p.nombre + ' ' + p.apellido AS NombreCompleto,
    t.num AS Telefono,
    t.oper AS Operador
FROM persona p
JOIN telefono t ON p.cc = t.duenio;

-- 3️⃣ Conteo de personas por género
SELECT genero, COUNT(*) AS Cantidad
FROM persona
GROUP BY genero;
GO
