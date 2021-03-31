-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema typers
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema typers
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `typers` DEFAULT CHARACTER SET utf8 ;
USE `typers` ;

-- -----------------------------------------------------
-- Table `typers`.`Typer`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `typers`.`Typer` (
  `Username` VARCHAR(30) NOT NULL,
  `Contraseña` VARCHAR(72) NOT NULL,
  `Email` VARCHAR(80) NOT NULL,
  `Estado` VARCHAR(100) NULL,
  `FotoDePerfil` VARCHAR(100) NULL,
  PRIMARY KEY (`Username`))
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
