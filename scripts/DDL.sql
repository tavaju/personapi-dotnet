-- ============================================
-- Script convertido desde MySQL a SQL Server
-- Base de datos: lab1Arqui
-- ============================================

-- Asegúrate de estar en la base de datos correcta
USE persona_db;
GO

-- ============================================
-- Tabla: persona
-- ============================================
IF OBJECT_ID('persona', 'U') IS NOT NULL DROP TABLE persona;
GO
CREATE TABLE persona (
    cc INT NOT NULL PRIMARY KEY,
    nombre VARCHAR(45) NOT NULL,
    apellido VARCHAR(45) NOT NULL,
    genero CHAR(1) CHECK (genero IN ('M', 'F')) NOT NULL,
    edad INT NULL
);
GO

-- ============================================
-- Tabla: profesion
-- ============================================
IF OBJECT_ID('profesion', 'U') IS NOT NULL DROP TABLE profesion;
GO
CREATE TABLE profesion (
    id INT NOT NULL PRIMARY KEY,
    nom VARCHAR(90) NOT NULL,
    des NVARCHAR(MAX) NULL
);
GO

-- ============================================
-- Tabla: estudios
-- ============================================
IF OBJECT_ID('estudios', 'U') IS NOT NULL DROP TABLE estudios;
GO
CREATE TABLE estudios (
    id_prof INT NOT NULL,
    cc_per INT NOT NULL,
    fecha DATE NULL,
    univer VARCHAR(50) NULL,
    CONSTRAINT PK_estudios PRIMARY KEY (id_prof, cc_per),
    CONSTRAINT FK_estudio_persona FOREIGN KEY (cc_per) REFERENCES persona(cc),
    CONSTRAINT FK_estudio_profesion FOREIGN KEY (id_prof) REFERENCES profesion(id)
);
GO

-- ============================================
-- Tabla: telefono
-- ============================================
IF OBJECT_ID('telefono', 'U') IS NOT NULL DROP TABLE telefono;
GO
CREATE TABLE telefono (
    num VARCHAR(15) NOT NULL PRIMARY KEY,
    oper VARCHAR(45) NOT NULL,
    duenio INT NOT NULL,
    CONSTRAINT FK_telefono_persona FOREIGN KEY (duenio) REFERENCES persona(cc)
);
GO
