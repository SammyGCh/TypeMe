-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mensajes
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema mensajes
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `mensajes` DEFAULT CHARACTER SET utf8 ;
USE `mensajes` ;

-- -----------------------------------------------------
-- Table `mensajes`.`Grupo`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mensajes`.`Grupo` (
  `IdGrupo` INT NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(45) NULL,
  `Descripcion` VARCHAR(200) NULL,
  `FechaCreacion` DATE NOT NULL,
  PRIMARY KEY (`IdGrupo`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mensajes`.`Mensaje`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mensajes`.`Mensaje` (
  `IdMensaje` INT NOT NULL AUTO_INCREMENT,
  `Contenido` MEDIUMTEXT NOT NULL,
  `Fecha` DATE NOT NULL,
  `Hora` TIME NOT NULL,
  `IdGrupo` INT NOT NULL,
  `Username` VARCHAR(30) NOT NULL,
  PRIMARY KEY (`IdMensaje`),
  INDEX `fk_Mensaje_Grupo_idx` (`IdGrupo` ASC) VISIBLE,
  CONSTRAINT `fk_Mensaje_Grupo`
    FOREIGN KEY (`IdGrupo`)
    REFERENCES `mensajes`.`Grupo` (`IdGrupo`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mensajes`.`Pertenece`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mensajes`.`Pertenece` (
  `IdGrupo` INT NOT NULL,
  `Username` VARCHAR(30) NOT NULL,
  PRIMARY KEY (`IdGrupo`),
  CONSTRAINT `fk_Pertenece_Grupo1`
    FOREIGN KEY (`IdGrupo`)
    REFERENCES `mensajes`.`Grupo` (`IdGrupo`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
