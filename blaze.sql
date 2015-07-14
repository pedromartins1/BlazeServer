-- --------------------------------------------------------
-- Poslužitelj:                  127.0.0.1
-- Server version:               5.6.25 - MySQL Community Server (GPL)
-- Server OS:                    Win32
-- HeidiSQL Verzija:             9.2.0.4947
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping database structure for blaze
CREATE DATABASE IF NOT EXISTS `blaze` /*!40100 DEFAULT CHARACTER SET utf8 COLLATE utf8_unicode_ci */;
USE `blaze`;


-- Dumping structure for table blaze.games
CREATE TABLE IF NOT EXISTS `games` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `clientid` int(11) DEFAULT NULL,
  `name` varchar(50) COLLATE utf8_unicode_ci NOT NULL,
  `attributes` mediumtext COLLATE utf8_unicode_ci NOT NULL,
  `capacity` varchar(50) COLLATE utf8_unicode_ci NOT NULL,
  `level` varchar(50) COLLATE utf8_unicode_ci NOT NULL,
  `gametype` varchar(50) COLLATE utf8_unicode_ci NOT NULL,
  `max_players` smallint(3) unsigned NOT NULL,
  `not_resetable` tinyint(1) unsigned NOT NULL,
  `queue_capacity` smallint(6) unsigned NOT NULL,
  `presence_mode` smallint(6) unsigned NOT NULL,
  `state` smallint(6) unsigned NOT NULL,
  `network_topology` smallint(6) unsigned NOT NULL,
  `voip_topology` smallint(6) unsigned NOT NULL DEFAULT '0',
  `internal_ip` int(11) unsigned DEFAULT NULL,
  `internal_port` smallint(6) unsigned DEFAULT NULL,
  `external_ip` int(11) unsigned DEFAULT NULL,
  `external_port` smallint(6) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- Dumping data for table blaze.games: ~0 rows (approximately)
DELETE FROM `games`;
/*!40000 ALTER TABLE `games` DISABLE KEYS */;
/*!40000 ALTER TABLE `games` ENABLE KEYS */;


-- Dumping structure for table blaze.personas
CREATE TABLE IF NOT EXISTS `personas` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(50) COLLATE utf8_unicode_ci NOT NULL,
  `last_login` int(11) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- Dumping data for table blaze.personas: ~2 rows (approximately)
DELETE FROM `personas`;
/*!40000 ALTER TABLE `personas` DISABLE KEYS */;
INSERT INTO `personas` (`id`, `name`, `last_login`) VALUES
	(1, 'bf3-server-pc', 0),
	(2, 'bf3player1', 0);
/*!40000 ALTER TABLE `personas` ENABLE KEYS */;


-- Dumping structure for table blaze.sessions
CREATE TABLE IF NOT EXISTS `sessions` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `persona` int(10) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_sessions_personas` (`persona`),
  CONSTRAINT `FK_sessions_personas` FOREIGN KEY (`persona`) REFERENCES `personas` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- Dumping data for table blaze.sessions: ~0 rows (approximately)
DELETE FROM `sessions`;
/*!40000 ALTER TABLE `sessions` DISABLE KEYS */;
/*!40000 ALTER TABLE `sessions` ENABLE KEYS */;


-- Dumping structure for table blaze.users
CREATE TABLE IF NOT EXISTS `users` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `mail` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `password` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `personaid` int(10) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_users_personas` (`personaid`),
  CONSTRAINT `FK_users_personas` FOREIGN KEY (`personaid`) REFERENCES `personas` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- Dumping data for table blaze.users: ~2 rows (approximately)
DELETE FROM `users`;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` (`id`, `mail`, `password`, `personaid`) VALUES
	(1, 'bf3.server.pc@ea.com', '9SWsHCq5YVYvKY5S', 1),
	(2, 'bf3player1@localhost', '1234', 2);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
