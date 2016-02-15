CREATE DATABASE  IF NOT EXISTS `intosport1` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `intosport1`;
-- MySQL dump 10.13  Distrib 5.6.17, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: intosport1
-- ------------------------------------------------------
-- Server version	5.5.41

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
-- Table structure for table `aanbieding`
--

DROP TABLE IF EXISTS `aanbieding`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `aanbieding` (
  `aanbiedingscode` int(5) NOT NULL AUTO_INCREMENT,
  `kortingspercentage` int(11) DEFAULT NULL,
  `naam` varchar(45) DEFAULT 'aanbieding',
  `geldig_tot` date DEFAULT NULL,
  PRIMARY KEY (`aanbiedingscode`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aanbieding`
--

LOCK TABLES `aanbieding` WRITE;
/*!40000 ALTER TABLE `aanbieding` DISABLE KEYS */;
INSERT INTO `aanbieding` VALUES (1,50,'aanbieding','2016-03-01'),(2,30,'aanbieding','2016-03-01'),(3,15,'aanbieding','2017-03-01'),(4,2,'aanbieding','2016-03-01'),(5,5,'aanbieding','2015-04-16'),(6,1,'aanbieding','2015-10-10'),(9,5,'test','2015-10-10'),(10,5,'test','0001-01-01');
/*!40000 ALTER TABLE `aanbieding` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `bestelling_detail`
--

DROP TABLE IF EXISTS `bestelling_detail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bestelling_detail` (
  `bestelling_detail_id` int(11) NOT NULL AUTO_INCREMENT,
  `bestelling_id` varchar(60) NOT NULL,
  `bestelling_keuze` varchar(45) DEFAULT NULL COMMENT 'Twee waardes, "Ophalen" of "Bestellen"',
  `betaald` char(3) NOT NULL DEFAULT 'nee' COMMENT '2 Waardes, "Nee" of "Ja"',
  PRIMARY KEY (`bestelling_detail_id`),
  KEY `bestelling_id_guidFk_idx` (`bestelling_id`)
) ENGINE=InnoDB AUTO_INCREMENT=66 DEFAULT CHARSET=utf8 COMMENT='Extra informatie over de recente bestelling, zoals Verzenden, Ophalen, Kosten.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bestelling_detail`
--

LOCK TABLES `bestelling_detail` WRITE;
/*!40000 ALTER TABLE `bestelling_detail` DISABLE KEYS */;
INSERT INTO `bestelling_detail` VALUES (1,'=fb27aee1-ee47-4904-adc9-95ac1e3f0f35','ophalen','nee'),(2,'=b6b4db9d-757c-49bc-b756-bab5081e0728','ophalen','nee'),(3,'=b6b4db9d-757c-49bc-b756-bab5081e0728','ophalen','nee'),(4,'=8c929d30-b327-47e6-994d-5a0ebb5d14f4','ophalen','nee'),(57,'=5e154bcd-653b-43b2-8eef-4c7b7b48103d','verzenden','nee'),(58,'=5e154bcd-653b-43b2-8eef-4c7b7b48103d','verzenden','nee'),(59,'=5e154bcd-653b-43b2-8eef-4c7b7b48103d','verzenden','nee'),(60,'=7af52724-cc91-4a97-b512-049fd26f6645','ophalen','nee'),(61,'=52625c39-f0c6-4ab9-85d5-cc301e797252','ophalen','nee'),(62,'=e9dc3875-8368-4d3b-89cd-d6aad18b953f','ophalen','nee'),(63,'=e9dc3875-8368-4d3b-89cd-d6aad18b953f','ophalen','nee'),(64,'=e9dc3875-8368-4d3b-89cd-d6aad18b953f','ophalen','nee'),(65,'=eed67e4d-7be1-428e-933b-a588ecccc4d4','ophalen','nee');
/*!40000 ALTER TABLE `bestelling_detail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `categorie`
--

DROP TABLE IF EXISTS `categorie`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `categorie` (
  `sport` int(3) NOT NULL,
  `product` int(3) NOT NULL DEFAULT '0',
  `producttype` varchar(50) NOT NULL,
  `categorie_Id` int(11) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`categorie_Id`),
  KEY `ProductFK_idx` (`product`),
  KEY `sportFK_idx` (`sport`),
  CONSTRAINT `productFK` FOREIGN KEY (`product`) REFERENCES `product` (`productcode`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `sportFK` FOREIGN KEY (`sport`) REFERENCES `sport` (`sportcode`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categorie`
--

LOCK TABLES `categorie` WRITE;
/*!40000 ALTER TABLE `categorie` DISABLE KEYS */;
INSERT INTO `categorie` VALUES (1,2,'handschoen',1),(1,2,'Binnen',2),(8,4,'Buiten',3);
/*!40000 ALTER TABLE `categorie` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `factuur`
--

DROP TABLE IF EXISTS `factuur`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `factuur` (
  `factuur_code` int(10) NOT NULL AUTO_INCREMENT,
  `datum` date NOT NULL,
  `klant_code` int(10) NOT NULL,
  `product_code` int(10) NOT NULL,
  `totaalbedrag` double DEFAULT NULL,
  `status` varchar(45) DEFAULT 'in behandeling',
  `kwantiteit` int(10) DEFAULT '1',
  `maat` varchar(45) DEFAULT 'M',
  `kleur` varchar(45) DEFAULT 'zwart',
  `guid` varchar(60) NOT NULL,
  PRIMARY KEY (`factuur_code`,`datum`,`klant_code`,`product_code`,`guid`),
  KEY `productsleutelFK_idx` (`product_code`),
  KEY `klantsleutelFK_idx` (`klant_code`),
  CONSTRAINT `klantsleutelFK` FOREIGN KEY (`klant_code`) REFERENCES `klant` (`klantcode`) ON DELETE NO ACTION ON UPDATE CASCADE,
  CONSTRAINT `productsleutel` FOREIGN KEY (`product_code`) REFERENCES `product` (`productcode`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=91 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `factuur`
--

LOCK TABLES `factuur` WRITE;
/*!40000 ALTER TABLE `factuur` DISABLE KEYS */;
INSERT INTO `factuur` VALUES (1,'2015-04-01',4,2,19,'gereed',1,'M','zwart','1'),(2,'2013-04-01',2,4,504,'klaar',1,'M','zwart','2'),(3,'2015-04-01',5,5,499,'in behandeling',1,'M','zwart','3'),(4,'2015-04-01',6,6,499,'in behandeling',1,'M','zwart','4'),(5,'2015-04-01',4,8,4,'Geannulleerd',1,'M','zwart','1'),(6,'2015-04-01',2,12,567,'in behandeling',1,'M','zwart','2'),(7,'2015-04-01',5,18,543,'in behandeling',1,'M','zwart','3'),(8,'2009-04-01',6,4,214,'in behandeling',1,'M','zwart','4'),(90,'2015-05-03',4,4,50,'in behandeling',1,'4XL','Zwart','=eed67e4d-7be1-428e-933b-a588ecccc4d4');
/*!40000 ALTER TABLE `factuur` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `goldmember`
--

DROP TABLE IF EXISTS `goldmember`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `goldmember` (
  `goldmember_id` int(5) NOT NULL AUTO_INCREMENT,
  `kortingspercentage` decimal(10,0) DEFAULT NULL,
  `kalenderjaar` date DEFAULT NULL,
  `goldmembership` int(5) NOT NULL DEFAULT '0',
  PRIMARY KEY (`goldmember_id`),
  KEY `gold_klant_idx` (`goldmembership`),
  CONSTRAINT `gold_klant` FOREIGN KEY (`goldmembership`) REFERENCES `klant` (`klantcode`) ON DELETE NO ACTION ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `goldmember`
--

LOCK TABLES `goldmember` WRITE;
/*!40000 ALTER TABLE `goldmember` DISABLE KEYS */;
INSERT INTO `goldmember` VALUES (1,5,'2015-03-01',2),(2,5,'2014-04-16',2),(3,5,'2015-05-03',4);
/*!40000 ALTER TABLE `goldmember` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `klant`
--

DROP TABLE IF EXISTS `klant`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `klant` (
  `klantcode` int(10) NOT NULL AUTO_INCREMENT,
  `gebruikersnaam` varchar(45) DEFAULT NULL,
  `wachtwoord` varchar(45) DEFAULT NULL,
  `naam` varchar(30) NOT NULL,
  `adres` varchar(45) NOT NULL,
  `woonplaats` varchar(45) NOT NULL,
  `telefoonnummer` varchar(13) NOT NULL,
  `email` varchar(50) NOT NULL,
  `rechten` enum('klant','beheerder','manager') DEFAULT 'klant',
  `datum_inschrijving` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`klantcode`),
  UNIQUE KEY `gebruikersnaam_UNIQUE` (`gebruikersnaam`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `klant`
--

LOCK TABLES `klant` WRITE;
/*!40000 ALTER TABLE `klant` DISABLE KEYS */;
INSERT INTO `klant` VALUES (1,'kees','kees','Kees','Keeslaan 12','Zoetermeer','066548948','kees@gmail.com','klant','2015-03-30 13:41:37'),(2,'karin','karin','Karin','Karinlaan 12','Delft','061565498','Karin@outlook.com','klant','2015-03-30 13:41:37'),(3,'peti','peti','Peti','Petilaan 12','Delft','0646544498','Peti@outlook.com','klant','2015-03-30 13:41:37'),(4,'klant','klant','Fritz','sPITSLAAN','Gouda','0612579632','FRITZ@acces4All.com','klant','2015-03-30 13:41:37'),(5,'manager','manager','Simon','Pietersma','Rotterdam','0624958372','VVD@Wannadoo.com','manager','2015-03-30 13:41:37'),(6,'beheerder','beheerder','Willem','Jansen','Willemstad','0639293474','WJansen@gmail.com','beheerder','2015-03-30 13:41:37'),(7,'test','test','test','test','test','1234567890','test@test.nl','klant','2015-04-13 18:49:01'),(8,'test2','test','test','test','test','0640535291','voorbeeld@email','klant','2015-04-13 18:51:58'),(9,'test4','test','testje','test','test','640535291','test@test.nl','klant','2015-04-14 14:43:26'),(10,'Tester','test','test','test','test','6','test@test.nl','klant','2015-04-14 14:57:17');
/*!40000 ALTER TABLE `klant` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `product`
--

DROP TABLE IF EXISTS `product`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `product` (
  `productcode` int(3) NOT NULL AUTO_INCREMENT,
  `naam` varchar(45) NOT NULL,
  `merk` varchar(45) NOT NULL,
  `inkoopprijs` double NOT NULL,
  `verkoopprijs` double NOT NULL,
  `voorraad` int(10) DEFAULT '0',
  `aanbiedingscode` int(3) DEFAULT '0',
  `producttype` varchar(20) DEFAULT '---',
  `omschrijving` varchar(50) DEFAULT '---',
  PRIMARY KEY (`productcode`,`naam`),
  KEY `aanbiedingFK_idx` (`aanbiedingscode`),
  CONSTRAINT `aanbiedingFK` FOREIGN KEY (`aanbiedingscode`) REFERENCES `aanbieding` (`aanbiedingscode`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=42 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product`
--

LOCK TABLES `product` WRITE;
/*!40000 ALTER TABLE `product` DISABLE KEYS */;
INSERT INTO `product` VALUES (2,'VoetbalSchoen','Montford',30,35,20,0,'WK','---'),(4,'HockeyStick','Perrier',30,50,100,3,'---','De hockeystick voor u'),(5,'Hockey Bal','Streetwise',7,12.5,2,0,'---','---'),(6,'Hockey Doel','Cornerstone',70,99.9,5,0,'---','---'),(8,'Voetbal Doel','Promise',20.5,40.8,12,0,'Kinderen','---'),(12,'Voetbal','We trust',20,40,30,0,'Voetbal','---'),(18,'Sportscoenen','Addidas',20,40,12,0,'Promotie','---'),(19,'Skeelers','Spexx',20,40,60,0,'XL','---'),(21,'Sport Jas met Kapuchon','C&A',7,9,1,0,'Jas','---'),(22,'Racket','PLT',30,50,20,0,'--','---'),(25,'HockeyStickNimble','HSN2',5,10,15,3,'---','---');
/*!40000 ALTER TABLE `product` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `product_inkoop`
--

DROP TABLE IF EXISTS `product_inkoop`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `product_inkoop` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `voorraad` int(10) NOT NULL,
  `maat` varchar(10) DEFAULT NULL,
  `productcode` int(10) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `productcode_idx` (`productcode`),
  KEY `voorraad_idx` (`maat`),
  CONSTRAINT `productcode` FOREIGN KEY (`productcode`) REFERENCES `product` (`productcode`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=59 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product_inkoop`
--

LOCK TABLES `product_inkoop` WRITE;
/*!40000 ALTER TABLE `product_inkoop` DISABLE KEYS */;
INSERT INTO `product_inkoop` VALUES (15,25,'-',25),(16,25,'3XL',25),(17,25,'4XL',25),(18,25,'L',25),(19,25,'M',25),(20,25,'M/L',25),(21,25,'S',25),(22,25,'XL',25),(23,25,'XS',25),(24,25,'XXL',25),(25,25,'XXXL',25),(26,25,'-',25),(27,25,'3XL',25),(28,25,'4XL',25),(29,25,'L',25),(30,25,'M',25),(31,25,'M/L',25),(32,25,'S',25),(33,25,'XL',25),(34,25,'XS',25),(35,25,'XXL',25),(36,25,'XXXL',25);
/*!40000 ALTER TABLE `product_inkoop` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `product_maat`
--

DROP TABLE IF EXISTS `product_maat`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `product_maat` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `maat` varchar(45) DEFAULT NULL,
  `grootte` varchar(45) DEFAULT '-',
  `kledingstuk` enum('schoenen','kleding dames','kleding heer','kleding overhemd') DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=63 DEFAULT CHARSET=utf8 COMMENT='Systeem tabel, alleen select, geen Update, Delete.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product_maat`
--

LOCK TABLES `product_maat` WRITE;
/*!40000 ALTER TABLE `product_maat` DISABLE KEYS */;
INSERT INTO `product_maat` VALUES (11,'32','XS','kleding dames'),(12,'34','XS','kleding dames'),(13,'36','S','kleding dames'),(14,'38','S','kleding dames'),(15,'40','M','kleding dames'),(16,'42','M','kleding dames'),(17,'46','XL','kleding dames'),(18,'48','XXL','kleding dames'),(19,'50','XXL','kleding dames'),(20,'52','XXXL','kleding dames'),(21,'44','L','kleding dames'),(22,'54','XL','kleding heer'),(23,'56','XXL','kleding heer'),(24,'58','3XL','kleding heer'),(25,'60','4XL','kleding heer'),(26,'37','S','kleding overhemd'),(27,'39','M','kleding overhemd'),(28,'41','M','kleding overhemd'),(29,'42','L','kleding overhemd'),(30,'43','XL','kleding overhemd'),(31,'44','XL','kleding overhemd'),(32,'45','XXL','kleding overhemd'),(33,'46','XXL','kleding overhemd'),(34,'47','3XL','kleding overhemd'),(35,'48','3XL','kleding overhemd'),(36,'44','XS','kleding heer'),(37,'46','S','kleding heer'),(38,'48','M','kleding heer'),(39,'50','M/L','kleding heer'),(40,'52','L','kleding heer'),(41,'36','-','schoenen'),(42,'36.5','-','schoenen'),(43,'37','-','schoenen'),(44,'37.5','-','schoenen'),(45,'38','-','schoenen'),(46,'38.5','-','schoenen'),(47,'39','-','schoenen'),(48,'39.5','-','schoenen'),(49,'40','-','schoenen'),(50,'40.5','-','schoenen'),(51,'41','-','schoenen'),(52,'42','-','schoenen'),(53,'42.5','-','schoenen'),(54,'43','-','schoenen'),(55,'43.5','-','schoenen'),(56,'44','-','schoenen'),(57,'44.5','-','schoenen'),(58,'45','-','schoenen'),(59,'46','-','schoenen'),(60,'46.5','-','schoenen'),(61,'47','-','schoenen'),(62,'48','-','schoenen');
/*!40000 ALTER TABLE `product_maat` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sport`
--

DROP TABLE IF EXISTS `sport`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sport` (
  `naam` varchar(30) DEFAULT NULL,
  `type` varchar(45) DEFAULT NULL,
  `sportcode` int(3) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`sportcode`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sport`
--

LOCK TABLES `sport` WRITE;
/*!40000 ALTER TABLE `sport` DISABLE KEYS */;
INSERT INTO `sport` VALUES ('Boogschieten','Schietsport',1),('Airsoft','Schietsport',2),('Schaatsen','Wintersport',3),('Curling','Wintersport',4),('Fitness','Binnensport',5),('Bowlen','Binnensport',6),('Voetbal','Teamsport',7),('Hockey','Teamsport',8),('Athletiek','Buitensport',9),('Joggen','Buitensport',10),('Volleybal','BuitenSport',11);
/*!40000 ALTER TABLE `sport` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tempwinkelwagen`
--

DROP TABLE IF EXISTS `tempwinkelwagen`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tempwinkelwagen` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `guid` varchar(60) NOT NULL,
  `productcode` int(11) DEFAULT NULL,
  `kwantiteit` int(11) DEFAULT '1',
  `maat` varchar(45) DEFAULT 'M',
  `kleur` varchar(45) DEFAULT 'zwart',
  `eind_datum` datetime NOT NULL,
  PRIMARY KEY (`id`),
  KEY `tempProductcode_idx` (`productcode`),
  CONSTRAINT `tempProductcode` FOREIGN KEY (`productcode`) REFERENCES `product` (`productcode`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=89 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tempwinkelwagen`
--

LOCK TABLES `tempwinkelwagen` WRITE;
/*!40000 ALTER TABLE `tempwinkelwagen` DISABLE KEYS */;
INSERT INTO `tempwinkelwagen` VALUES (72,'=c452de75-3fd6-4a5c-bc04-cdc2b45af1e3',25,1,'M','Zwart','2015-06-03 17:46:46'),(73,'=115b1101-83da-4067-8e7f-4505fcf193d6',25,1,'M','Zwart','2015-06-03 17:51:55'),(74,'=8a27abbe-602e-4daf-8ced-dc7b6e206a80',25,1,'M','Zwart','2015-06-03 17:51:55'),(75,'=adcfe607-ab40-49f3-a85f-6bf851ee31f0',25,1,'M','Zwart','2015-06-03 17:51:55'),(88,'=ebf99bf2-89b4-4770-ad87-3555b1d3b7d4',4,2,'S','Zwart','2015-06-03 20:31:38');
/*!40000 ALTER TABLE `tempwinkelwagen` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2015-05-03 20:32:29
