-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema Mensajes
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema Mensajes
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `Mensajes` DEFAULT CHARACTER SET utf8 ;
USE `Mensajes` ;

-- -----------------------------------------------------
-- Table `Mensajes`.`Grupo`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Mensajes`.`Grupos` (
  `IdGrupo` INT NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(45) NULL,
  `Descripcion` VARCHAR(200) NULL,
  `FechaCreacion` DATE NOT NULL,
  PRIMARY KEY (`IdGrupo`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Mensajes`.`Mensaje`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Mensajes`.`Mensajes` (
  `IdMensaje` INT NOT NULL AUTO_INCREMENT,
  `Contenido` MEDIUMTEXT NOT NULL,
  `Fecha` DATE NOT NULL,
  `Hora` TIME NOT NULL,
  `IdGrupo` INT NOT NULL,
  `IdTyper` VARCHAR(40) NOT NULL,
  `IdMultimedia` VARCHAR(40) NOT NULL,
  PRIMARY KEY (`IdMensaje`),
  CONSTRAINT `fk_Mensaje_Grupo`
    FOREIGN KEY (`IdGrupo`)
    REFERENCES `Mensajes`.`Grupos` (`IdGrupo`)
    ON DELETE CASCADE
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Mensajes`.`Pertenece`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Mensajes`.`Pertenece` (
  `IdGrupo` INT NOT NULL,
  `IdTyper` VARCHAR(40) NOT NULL,
  PRIMARY KEY (`IdGrupo`, `IdTyper`),
  CONSTRAINT `fk_Pertenece_Grupo1`
    FOREIGN KEY (`IdGrupo`)
    REFERENCES `Mensajes`.`Grupos` (`IdGrupo`)
    ON DELETE CASCADE
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SET SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';
CREATE USER 'adminMensajes' IDENTIFIED BY 'proyecto2021';
GRANT ALL PRIVILEGES ON Mensajes.* TO 'adminMensajes'@'%' WITH GRANT OPTION;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
