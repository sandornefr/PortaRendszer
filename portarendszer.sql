-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2025. Már 25. 13:45
-- Kiszolgáló verziója: 10.4.32-MariaDB
-- PHP verzió: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `portarendszer`
--
CREATE DATABASE IF NOT EXISTS `portarendszer` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_hungarian_ci;
USE `portarendszer`;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `belepes`
--

CREATE TABLE `belepes` (
  `id` int(11) NOT NULL,
  `felhasznalo_id` int(11) NOT NULL,
  `belepesi_ido` timestamp NOT NULL DEFAULT current_timestamp(),
  `kilepesi_ido` timestamp NULL DEFAULT NULL,
  `utolso_aktivitas` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `felhasznalo`
--

CREATE TABLE `felhasznalo` (
  `id` int(11) NOT NULL,
  `nev` varchar(100) NOT NULL,
  `beosztas` enum('igazgato','igazgatohelyettes','osztalyfonok','tanar','napkozis_nevelo','pedagogiai_asszisztens','portas') NOT NULL,
  `felhasznalonev` varchar(20) NOT NULL,
  `email` varchar(100) NOT NULL,
  `jelszo` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `osztaly`
--

CREATE TABLE `osztaly` (
  `id` int(11) NOT NULL,
  `nev` varchar(10) NOT NULL,
  `egyedi_azonosito` varchar(20) DEFAULT NULL,
  `osztalyfonok_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `porta_uzenet`
--

CREATE TABLE `porta_uzenet` (
  `id` int(11) NOT NULL,
  `tanulo_id` int(11) NOT NULL,
  `uzenet` text NOT NULL,
  `statusz` enum('Jelen_van','Hianyzo','Kulon_foglalkozas','Hazament') DEFAULT 'Jelen_van',
  `idopont` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `spec_nap`
--

CREATE TABLE `spec_nap` (
  `id` int(11) NOT NULL,
  `datum` date NOT NULL,
  `megnevezes` varchar(100) NOT NULL,
  `felugyelet_opcio` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `tanterem`
--

CREATE TABLE `tanterem` (
  `id` int(11) NOT NULL,
  `nev` varchar(50) NOT NULL,
  `aktiv` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `tanterem_hasznalat`
--

CREATE TABLE `tanterem_hasznalat` (
  `id` int(11) NOT NULL,
  `felhasznalo_id` int(11) NOT NULL,
  `tanterem_id` int(11) NOT NULL,
  `osztaly_id` int(11) DEFAULT NULL,
  `idopont` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `tanulo`
--

CREATE TABLE `tanulo` (
  `id` int(11) NOT NULL,
  `okt_azonosito` varchar(11) NOT NULL,
  `nev` varchar(100) NOT NULL,
  `osztaly_id` int(11) DEFAULT NULL,
  `tanszobas` tinyint(1) DEFAULT 1,
  `spec_hazavitel` tinyint(1) DEFAULT 0,
  `gondviselo_nev` varchar(100) DEFAULT NULL,
  `gondviselo_statusz` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `tanulo_archiv`
--

CREATE TABLE `tanulo_archiv` (
  `id` int(11) NOT NULL,
  `okt_azonosito` varchar(11) NOT NULL,
  `nev` varchar(100) NOT NULL,
  `osztaly_nev` varchar(10) DEFAULT NULL,
  `torles_idopont` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `belepes`
--
ALTER TABLE `belepes`
  ADD PRIMARY KEY (`id`),
  ADD KEY `felhasznalo_id` (`felhasznalo_id`);

--
-- A tábla indexei `felhasznalo`
--
ALTER TABLE `felhasznalo`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `felhasznalonev` (`felhasznalonev`),
  ADD UNIQUE KEY `email` (`email`),
  ADD KEY `idx_felhasznalonev` (`felhasznalonev`),
  ADD KEY `idx_email` (`email`);

--
-- A tábla indexei `osztaly`
--
ALTER TABLE `osztaly`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `egyedi_azonosito` (`egyedi_azonosito`),
  ADD KEY `osztalyfonok_id` (`osztalyfonok_id`);

--
-- A tábla indexei `porta_uzenet`
--
ALTER TABLE `porta_uzenet`
  ADD PRIMARY KEY (`id`),
  ADD KEY `tanulo_id` (`tanulo_id`);

--
-- A tábla indexei `spec_nap`
--
ALTER TABLE `spec_nap`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `tanterem`
--
ALTER TABLE `tanterem`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `tanterem_hasznalat`
--
ALTER TABLE `tanterem_hasznalat`
  ADD PRIMARY KEY (`id`),
  ADD KEY `felhasznalo_id` (`felhasznalo_id`),
  ADD KEY `tanterem_id` (`tanterem_id`),
  ADD KEY `osztaly_id` (`osztaly_id`);

--
-- A tábla indexei `tanulo`
--
ALTER TABLE `tanulo`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `okt_azonosito` (`okt_azonosito`),
  ADD KEY `osztaly_id` (`osztaly_id`),
  ADD KEY `idx_okt_azonosito` (`okt_azonosito`);

--
-- A tábla indexei `tanulo_archiv`
--
ALTER TABLE `tanulo_archiv`
  ADD PRIMARY KEY (`id`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `belepes`
--
ALTER TABLE `belepes`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `felhasznalo`
--
ALTER TABLE `felhasznalo`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `osztaly`
--
ALTER TABLE `osztaly`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `porta_uzenet`
--
ALTER TABLE `porta_uzenet`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `spec_nap`
--
ALTER TABLE `spec_nap`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `tanterem`
--
ALTER TABLE `tanterem`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `tanterem_hasznalat`
--
ALTER TABLE `tanterem_hasznalat`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `tanulo`
--
ALTER TABLE `tanulo`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `tanulo_archiv`
--
ALTER TABLE `tanulo_archiv`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `belepes`
--
ALTER TABLE `belepes`
  ADD CONSTRAINT `belepes_ibfk_1` FOREIGN KEY (`felhasznalo_id`) REFERENCES `felhasznalo` (`id`) ON DELETE CASCADE;

--
-- Megkötések a táblához `osztaly`
--
ALTER TABLE `osztaly`
  ADD CONSTRAINT `osztaly_ibfk_1` FOREIGN KEY (`osztalyfonok_id`) REFERENCES `felhasznalo` (`id`) ON DELETE SET NULL;

--
-- Megkötések a táblához `porta_uzenet`
--
ALTER TABLE `porta_uzenet`
  ADD CONSTRAINT `porta_uzenet_ibfk_1` FOREIGN KEY (`tanulo_id`) REFERENCES `tanulo` (`id`) ON DELETE CASCADE;

--
-- Megkötések a táblához `tanterem_hasznalat`
--
ALTER TABLE `tanterem_hasznalat`
  ADD CONSTRAINT `tanterem_hasznalat_ibfk_1` FOREIGN KEY (`felhasznalo_id`) REFERENCES `felhasznalo` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `tanterem_hasznalat_ibfk_2` FOREIGN KEY (`tanterem_id`) REFERENCES `tanterem` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `tanterem_hasznalat_ibfk_3` FOREIGN KEY (`osztaly_id`) REFERENCES `osztaly` (`id`) ON DELETE SET NULL;

--
-- Megkötések a táblához `tanulo`
--
ALTER TABLE `tanulo`
  ADD CONSTRAINT `tanulo_ibfk_1` FOREIGN KEY (`osztaly_id`) REFERENCES `osztaly` (`id`) ON DELETE SET NULL;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
