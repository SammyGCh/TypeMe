-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema Multimedias
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema Multimedias
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `Multimedias` DEFAULT CHARACTER SET utf8 ;
USE `Multimedias` ;

-- -----------------------------------------------------
-- Table `Multimedias`.`TipoMultimedia`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Multimedias`.`TipoMultimedia` (
  `IdTipoMultimedia` INT NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(20) NOT NULL,
  PRIMARY KEY (`IdTipoMultimedia`))
ENGINE = InnoDB;

INSERT INTO TipoMultimedia (Nombre) VALUES ("Foto");
INSERT INTO TipoMultimedia (Nombre) VALUES ("Video");
INSERT INTO TipoMultimedia (Nombre) VALUES ("Archivo");
-- -----------------------------------------------------
-- Table `Multimedias`.`Multimedias`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Multimedias`.`Multimedias` (
  `IdMultimedia` VARCHAR(40) NOT NULL,
  `Ruta` VARCHAR(100) NOT NULL,
  `IdTipoMultimedia` INT NOT NULL,
  `IdMensaje` INT NOT NULL,
  PRIMARY KEY (`IdMultimedia`),
  INDEX `fk_Multimedia_TipoMultimedia_idx` (`IdTipoMultimedia` ASC) VISIBLE,
  CONSTRAINT `fk_Multimedia_TipoMultimedia`
    FOREIGN KEY (`IdTipoMultimedia`)
    REFERENCES `Multimedias`.`TipoMultimedia` (`IdTipoMultimedia`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SET SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';
CREATE USER 'adminMultimedia' IDENTIFIED BY 'proyecto2021';
GRANT ALL PRIVILEGES ON Multimedias.* TO 'adminMultimedia'@'%' WITH GRANT OPTION;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
