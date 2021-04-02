-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema Typers
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema Typers
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `Typers` DEFAULT CHARACTER SET utf8 ;
USE `Typers` ;

-- -----------------------------------------------------
-- Table `Typers`.`Typer`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Typers`.`Typers` (
  `IdTyper` VARCHAR(40) NOT NULL,
  `Username` VARCHAR(30) NOT NULL,
  `Estado` VARCHAR(100) NULL,
  `FotoDePerfil` VARCHAR(100) NULL,
  `Estatus` TINYINT NOT NULL,
  PRIMARY KEY (`IdTyper`))
ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table `Typers`.`Correos`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Typers`.`Correos` ;

CREATE TABLE IF NOT EXISTS `Typers`.`Correos` (
  `IdCorreo` INT NOT NULL AUTO_INCREMENT,
  `Direccion` VARCHAR(80) NOT NULL,
  `EsPrincipal` TINYINT NOT NULL,
  `IdTyper` VARCHAR(40) NOT NULL,
  PRIMARY KEY (`IdCorreo`),
  INDEX `fk_Correos_Typers_idx` (`IdTyper` ASC) VISIBLE,
  CONSTRAINT `fk_Correos_Typers`
    FOREIGN KEY (`IdTyper`)
    REFERENCES `Typers`.`Typers` (`IdTyper`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Typers`.`Contrasenias`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Typers`.`Contrasenias` ;

CREATE TABLE IF NOT EXISTS `Typers`.`Contrasenias` (
  `IdContrasenia` INT NOT NULL AUTO_INCREMENT,
  `Contrasenia` VARCHAR(120) NOT NULL,
  `IdTyper` VARCHAR(40) NOT NULL,
  PRIMARY KEY (`IdContrasenia`),
  INDEX `fk_Contrasenias_Typers1_idx` (`IdTyper` ASC) VISIBLE,
  CONSTRAINT `fk_Contrasenias_Typers1`
    FOREIGN KEY (`IdTyper`)
    REFERENCES `Typers`.`Typers` (`IdTyper`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SET SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';
CREATE USER 'adminTypers' IDENTIFIED BY 'proyecto2021';
GRANT ALL PRIVILEGES ON Typers.* TO 'adminTypers'@'%' WITH GRANT OPTION;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
