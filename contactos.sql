-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema Contactos
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema Contactos
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `Contactos` DEFAULT CHARACTER SET utf8 ;
USE `Contactos` ;

-- -----------------------------------------------------
-- Table `Contactos`.`Contacta`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Contactos`.`Contacta` (
  `IdContacta` VARCHAR(40) NOT NULL,
  `IdTyper` VARCHAR(40) NOT NULL,
  `IdContacto` VARCHAR(40) NOT NULL,
  `Bloqueado` TINYINT NOT NULL,
  `EsFavorito` TINYINT NOT NULL,
  PRIMARY KEY (`IdContacta`))
ENGINE = InnoDB;

SET SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';
CREATE USER 'adminContactos' IDENTIFIED BY 'proyecto2021';
GRANT ALL PRIVILEGES ON Contactos.* TO 'adminContactos'@'%' WITH GRANT OPTION;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
