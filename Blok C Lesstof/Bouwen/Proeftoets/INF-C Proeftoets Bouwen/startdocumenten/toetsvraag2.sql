CREATE DATABASE  IF NOT EXISTS `toetsvraag2` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `toetsvraag2`;
-- MySQL dump 10.13  Distrib 5.5.16, for Win32 (x86)
--
-- Host: localhost    Database: terminaltoetsblokc
-- ------------------------------------------------------
-- Server version	5.5.28

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `operatingsystem`
--

DROP TABLE IF EXISTS `operatingsystem`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `operatingsystem` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `naam` varchar(255) DEFAULT NULL,
  `versie` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `operatingsystem`
--
-- ORDER BY:  `id`

LOCK TABLES `operatingsystem` WRITE;
/*!40000 ALTER TABLE `operatingsystem` DISABLE KEYS */;
/*!40000 ALTER TABLE `operatingsystem` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2013-03-26 13:40:17


--
-- Table structure for table `mobieletelefoon`
--

DROP TABLE IF EXISTS `mobieletelefoon`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mobieletelefoon` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `fabrikant` varchar(255) DEFAULT NULL,
  `type` varchar(255) DEFAULT NULL,
  `prijs` double DEFAULT NULL,
  `operatingsystemid` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_operatingsystem_idx` (`operatingsystemid`),
  CONSTRAINT `fk_operatingsystem` FOREIGN KEY (`operatingsystemid`) REFERENCES `operatingsystem` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mobieletelefoon`
--
-- ORDER BY:  `id`

LOCK TABLES `mobieletelefoon` WRITE;
/*!40000 ALTER TABLE `mobieletelefoon` DISABLE KEYS */;
/*!40000 ALTER TABLE `mobieletelefoon` ENABLE KEYS */;
UNLOCK TABLES;


