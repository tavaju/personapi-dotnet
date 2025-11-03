-- ============================================
-- Script de inicialización de base de datos
-- ============================================

-- Crear la base de datos si no existe
IF DB_ID('persona_db') IS NULL
BEGIN
    CREATE DATABASE persona_db;
    PRINT 'Base de datos persona_db creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La base de datos persona_db ya existe.';
END
GO

-- Establecer el contexto de la base de datos
USE persona_db;
GO

