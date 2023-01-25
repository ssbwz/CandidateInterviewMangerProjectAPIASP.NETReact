-- phpMyAdmin SQL Dump
-- version 4.9.3
-- https://www.phpmyadmin.net/
--
-- Host: studmysql01.fhict.local
-- Generation Time: Jan 24, 2023 at 02:19 PM
-- Server version: 5.7.26-log
-- PHP Version: 7.4.23

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `dbi459574`
--

-- --------------------------------------------------------

--
-- Table structure for table `applications`
--

CREATE TABLE `applications` (
  `id` int(11) NOT NULL,
  `vacancy_id` int(11) NOT NULL,
  `candidate_id` int(11) NOT NULL,
  `recruiter_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `applications`
--

INSERT INTO `applications` (`id`, `vacancy_id`, `candidate_id`, `recruiter_id`) VALUES
(48, 1, 59, 11),
(49, 1, 50, 11),
(50, 1, 62, 11),
(51, 2, 63, 64),
(53, 5, 65, 64),
(54, 8, 65, 11);

-- --------------------------------------------------------

--
-- Table structure for table `appointment`
--

CREATE TABLE `appointment` (
  `id` int(11) NOT NULL,
  `MSGraph_id` text NOT NULL,
  `application_id` int(11) NOT NULL,
  `start_date` datetime NOT NULL,
  `end_date` datetime NOT NULL,
  `location` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `appointment`
--

INSERT INTO `appointment` (`id`, `MSGraph_id`, `application_id`, `start_date`, `end_date`, `location`) VALUES
(201, 'AAMkADVmZDQxMTMzLWI5NzUtNDlkOC04YzM1LTMxOGZjZDU4MDZmYQBGAAAAAACmAyCJz1bESIuA6T7vCICMBwAfa2UJ8gnpRYkMJ4AfSqOGAAAAAAENAAAfa2UJ8gnpRYkMJ4AfSqOGAAApkfkvAAA=', 48, '2023-01-23 09:00:00', '2023-01-23 09:30:00', 'HTC 37, 5656 AA Eindhoven'),
(202, 'AAMkADVmZDQxMTMzLWI5NzUtNDlkOC04YzM1LTMxOGZjZDU4MDZmYQBGAAAAAACmAyCJz1bESIuA6T7vCICMBwAfa2UJ8gnpRYkMJ4AfSqOGAAAAAAENAAAfa2UJ8gnpRYkMJ4AfSqOGAAApkfkwAAA=', 49, '2023-01-23 10:00:00', '2023-01-23 10:30:00', 'HTC 37, 5656 AA Eindhoven'),
(203, 'AAMkADVmZDQxMTMzLWI5NzUtNDlkOC04YzM1LTMxOGZjZDU4MDZmYQBGAAAAAACmAyCJz1bESIuA6T7vCICMBwAfa2UJ8gnpRYkMJ4AfSqOGAAAAAAENAAAfa2UJ8gnpRYkMJ4AfSqOGAAApkfkxAAA=', 50, '2023-01-23 07:30:00', '2023-01-23 08:00:00', 'HTC 37, 5656 AA Eindhoven'),
(207, 'AAMkADVmZDQxMTMzLWI5NzUtNDlkOC04YzM1LTMxOGZjZDU4MDZmYQBGAAAAAACmAyCJz1bESIuA6T7vCICMBwAfa2UJ8gnpRYkMJ4AfSqOGAAAAAAENAAAfa2UJ8gnpRYkMJ4AfSqOGAAApkfk4AAA=', 54, '2023-01-19 13:30:00', '2023-01-19 14:00:00', 'Rachelsmolen 1, R10, 5612 MA Eindhoven');

-- --------------------------------------------------------

--
-- Table structure for table `appointments_links`
--

CREATE TABLE `appointments_links` (
  `id` int(11) NOT NULL,
  `usernameHashed` text NOT NULL,
  `identifierHashed` text NOT NULL,
  `status` enum('Created','Used') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `appointments_links`
--

INSERT INTO `appointments_links` (`id`, `usernameHashed`, `identifierHashed`, `status`) VALUES
(39, '3435383532364073747564656E742E666F6E7479732E6E6C', '31', 'Used'),
(40, '3438383336344073747564656E742E666F6E7479732E6E6C', '31', 'Used'),
(41, '6A65737369652E76616E6E75656E656E4073747564656E742E666F6E7479732E6E6C', '31', 'Used'),
(42, '732E626177617A6965724073747564656E742E666F6E7479732E6E6C', '32', 'Used'),
(44, '3437383934384073747564656E742E666F6E7479732E6E6C', '35', 'Used'),
(45, '3437383934384073747564656E742E666F6E7479732E6E6C', '38', 'Used');

-- --------------------------------------------------------

--
-- Table structure for table `role`
--

CREATE TABLE `role` (
  `id` int(11) NOT NULL,
  `title` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `role`
--

INSERT INTO `role` (`id`, `title`) VALUES
(1, 'admin'),
(2, 'recruiter'),
(3, 'candidate');

-- --------------------------------------------------------

--
-- Table structure for table `user`
--

CREATE TABLE `user` (
  `id` int(11) NOT NULL,
  `first_name` varchar(255) NOT NULL,
  `last_name` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `password` varchar(255) DEFAULT NULL,
  `role` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `user`
--

INSERT INTO `user` (`id`, `first_name`, `last_name`, `email`, `password`, `role`) VALUES
(11, 'Jessie', 'van Nuenen', 'jessie@1bkt6z.onmicrosoft.com', '123Jessie', 2),
(50, 'Saeed', 'Ba Wazir', '488364@student.fontys.nl', NULL, 3),
(59, 'Niels', 'Roefs', '458526@student.fontys.nl', NULL, 3),
(62, 'Amber', 'van Neuenen', 'jessie.vannuenen@student.fontys.nl', NULL, 3),
(63, 'Amber', 'van Neuenen', 's.bawazier@student.fontys.nl', NULL, 3),
(64, 'Lucas', 'van Eindhoven', 'lucas@1bkt6z.onmicrosoft.com', '123Lucas', 2),
(65, 'Bojidar', 'van Geldrop', '478948@student.fontys.nl', NULL, 3);

-- --------------------------------------------------------

--
-- Table structure for table `vacancy`
--

CREATE TABLE `vacancy` (
  `id` int(11) NOT NULL,
  `title` varchar(255) NOT NULL,
  `location` varchar(255) NOT NULL,
  `meeting_location` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `vacancy`
--

INSERT INTO `vacancy` (`id`, `title`, `location`, `meeting_location`) VALUES
(1, 'Web developer', 'Eindhoven', 'HTC 37, 5656 AA Eindhoven'),
(2, 'Fullstack developer', 'Eindhoven', 'Online'),
(5, 'Developer', 'Amsterdam', 'Amsterdam'),
(8, 'DevOps Fontys', 'Eindhoven', 'Rachelsmolen 1, R10, 5612 MA Eindhoven');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `applications`
--
ALTER TABLE `applications`
  ADD PRIMARY KEY (`id`),
  ADD KEY `Applications_fk0` (`vacancy_id`),
  ADD KEY `Applications_fk1` (`candidate_id`),
  ADD KEY `Applications_fk2` (`recruiter_id`);

--
-- Indexes for table `appointment`
--
ALTER TABLE `appointment`
  ADD PRIMARY KEY (`id`),
  ADD KEY `Appointment_fk1` (`application_id`);

--
-- Indexes for table `appointments_links`
--
ALTER TABLE `appointments_links`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `UX_usernameHashed_identifierHashed` (`usernameHashed`(300),`identifierHashed`(300));

--
-- Indexes for table `role`
--
ALTER TABLE `role`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email` (`email`),
  ADD KEY `User_fk0` (`role`);

--
-- Indexes for table `vacancy`
--
ALTER TABLE `vacancy`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `applications`
--
ALTER TABLE `applications`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=55;

--
-- AUTO_INCREMENT for table `appointment`
--
ALTER TABLE `appointment`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=208;

--
-- AUTO_INCREMENT for table `appointments_links`
--
ALTER TABLE `appointments_links`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=46;

--
-- AUTO_INCREMENT for table `role`
--
ALTER TABLE `role`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `user`
--
ALTER TABLE `user`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=66;

--
-- AUTO_INCREMENT for table `vacancy`
--
ALTER TABLE `vacancy`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1001;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `applications`
--
ALTER TABLE `applications`
  ADD CONSTRAINT `Applications_fk0` FOREIGN KEY (`vacancy_id`) REFERENCES `vacancy` (`id`),
  ADD CONSTRAINT `Applications_fk1` FOREIGN KEY (`candidate_id`) REFERENCES `user` (`id`),
  ADD CONSTRAINT `Applications_fk2` FOREIGN KEY (`recruiter_id`) REFERENCES `user` (`id`);

--
-- Constraints for table `appointment`
--
ALTER TABLE `appointment`
  ADD CONSTRAINT `Appointment_fk1` FOREIGN KEY (`application_id`) REFERENCES `applications` (`id`);

--
-- Constraints for table `user`
--
ALTER TABLE `user`
  ADD CONSTRAINT `User_fk0` FOREIGN KEY (`role`) REFERENCES `role` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
