-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema contactos
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema contactos
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `contactos` DEFAULT CHARACTER SET utf8 ;
USE `contactos` ;

-- -----------------------------------------------------
-- Table `contactos`.`Contacta`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `contactos`.`Contacta` (
  `Username` VARCHAR(30) NOT NULL,
  `Contacto` VARCHAR(30) NOT NULL,
  `Bloqueado` TINYINT NULL,
  PRIMARY KEY (`Username`, `Contacto`))
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
