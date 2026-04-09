-- --------------------------------------------------------
-- Host:                         113.161.129.118
-- Server version:               10.11.11-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win64
-- HeidiSQL Version:             12.10.0.7000
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

-- Dumping structure for table erp.CategoryDepartment
CREATE TABLE IF NOT EXISTS `CategoryDepartment` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `ParentID` bigint(20) DEFAULT NULL,
  `ParentName` varchar(1000) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `CreateUserID` int(11) DEFAULT NULL,
  `CreateUserCode` varchar(1000) DEFAULT NULL,
  `CreateUserName` varchar(1000) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  `UpdateUserID` int(11) DEFAULT NULL,
  `UpdateUserCode` varchar(1000) DEFAULT NULL,
  `UpdateUserName` varchar(1000) DEFAULT NULL,
  `RowVersion` int(11) DEFAULT NULL,
  `SortOrder` int(11) DEFAULT NULL,
  `Active` bit(1) DEFAULT NULL,
  `Code` varchar(1000) DEFAULT NULL,
  `Name` varchar(1000) DEFAULT NULL,
  `Display` varchar(1000) DEFAULT NULL,
  `Description` varchar(1000) DEFAULT NULL,
  `Note` varchar(1000) DEFAULT NULL,
  `FileName` varchar(1000) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Data exporting was unselected.

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
