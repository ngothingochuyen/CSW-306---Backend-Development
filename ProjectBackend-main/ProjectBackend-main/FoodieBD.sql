CREATE DATABASE  IF NOT EXISTS `fastfooddb` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `fastfooddb`;
-- MySQL dump 10.13  Distrib 8.0.44, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: fastfooddb
-- ------------------------------------------------------
-- Server version	8.0.44

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `carts`
--

DROP TABLE IF EXISTS `carts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `carts` (
  `CartId` int NOT NULL AUTO_INCREMENT,
  `ProductId` int DEFAULT NULL,
  `Quantity` int DEFAULT NULL,
  `UserId` int DEFAULT NULL,
  PRIMARY KEY (`CartId`),
  KEY `ProductId` (`ProductId`),
  KEY `UserId` (`UserId`),
  CONSTRAINT `carts_ibfk_1` FOREIGN KEY (`ProductId`) REFERENCES `products` (`ProductId`) ON DELETE CASCADE,
  CONSTRAINT `carts_ibfk_2` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `carts`
--

LOCK TABLES `carts` WRITE;
/*!40000 ALTER TABLE `carts` DISABLE KEYS */;
/*!40000 ALTER TABLE `carts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `categories`
--

DROP TABLE IF EXISTS `categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categories` (
  `CategoryId` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) DEFAULT NULL,
  `ImageUrl` text,
  `IsActive` tinyint(1) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`CategoryId`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categories`
--

LOCK TABLES `categories` WRITE;
/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
INSERT INTO `categories` VALUES (1,'Burger','Images/Category/burger.jpg',1,'2026-05-28 23:03:27'),(2,'Pizza','Images/Category/pizza.jpg',1,'2026-05-28 23:03:27');
/*!40000 ALTER TABLE `categories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contact`
--

DROP TABLE IF EXISTS `contact`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `contact` (
  `ContactId` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) DEFAULT NULL,
  `Email` varchar(50) DEFAULT NULL,
  `Subject` varchar(200) DEFAULT NULL,
  `Message` text,
  `CreateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`ContactId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contact`
--

LOCK TABLES `contact` WRITE;
/*!40000 ALTER TABLE `contact` DISABLE KEYS */;
/*!40000 ALTER TABLE `contact` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orders`
--

DROP TABLE IF EXISTS `orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `orders` (
  `OrderDetailsId` int NOT NULL AUTO_INCREMENT,
  `OrderNo` varchar(100) DEFAULT NULL,
  `ProductId` int DEFAULT NULL,
  `Quantity` int DEFAULT NULL,
  `UserId` int DEFAULT NULL,
  `Status` varchar(50) DEFAULT NULL,
  `PaymentId` int DEFAULT NULL,
  `OrderDate` datetime DEFAULT NULL,
  PRIMARY KEY (`OrderDetailsId`),
  UNIQUE KEY `OrderNo` (`OrderNo`),
  KEY `ProductId` (`ProductId`),
  KEY `UserId` (`UserId`),
  CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`ProductId`) REFERENCES `products` (`ProductId`),
  CONSTRAINT `orders_ibfk_2` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orders`
--

LOCK TABLES `orders` WRITE;
/*!40000 ALTER TABLE `orders` DISABLE KEYS */;
/*!40000 ALTER TABLE `orders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `payment`
--

DROP TABLE IF EXISTS `payment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `payment` (
  `PaymentId` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) DEFAULT NULL,
  `CardNo` varchar(50) DEFAULT NULL,
  `ExpiryDate` varchar(50) DEFAULT NULL,
  `CvvNo` int DEFAULT NULL,
  `Address` text,
  `PaymentMode` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`PaymentId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `payment`
--

LOCK TABLES `payment` WRITE;
/*!40000 ALTER TABLE `payment` DISABLE KEYS */;
/*!40000 ALTER TABLE `payment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `products`
--

DROP TABLE IF EXISTS `products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `products` (
  `ProductId` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) DEFAULT NULL,
  `Description` text,
  `Price` decimal(10,2) DEFAULT NULL,
  `Quantity` int DEFAULT NULL,
  `ImageUrl` text,
  `CategoryId` int DEFAULT NULL,
  `IsActive` tinyint(1) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`ProductId`),
  KEY `CategoryId` (`CategoryId`),
  CONSTRAINT `products_ibfk_1` FOREIGN KEY (`CategoryId`) REFERENCES `categories` (`CategoryId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `products`
--

LOCK TABLES `products` WRITE;
/*!40000 ALTER TABLE `products` DISABLE KEYS */;
INSERT INTO `products` VALUES (1,'Cheese Burger','Cheese Burger with fries',120.00,10,'Images/Product/burger.jpg',1,1,'2026-05-28 23:03:27'),(2,'Cheese Pizza','Cheese Pizza Large Size',200.00,20,'Images/Product/pizza.jpg',2,1,'2026-05-28 23:03:27');
/*!40000 ALTER TABLE `products` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `UserId` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) DEFAULT NULL,
  `Username` varchar(50) DEFAULT NULL,
  `Mobile` varchar(50) DEFAULT NULL,
  `Email` varchar(50) DEFAULT NULL,
  `Address` varchar(100) DEFAULT NULL,
  `PostCode` varchar(20) DEFAULT NULL,
  `Password` varchar(100) DEFAULT NULL,
  `ImageUrl` text,
  `CreatedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`UserId`),
  UNIQUE KEY `Username` (`Username`),
  UNIQUE KEY `Mobile` (`Mobile`),
  UNIQUE KEY `Email` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'Admin User','admin','0123456789','admin@gmail.com','Ho Chi Minh City','700000','123456','','2026-05-28 23:03:27');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'fastfooddb'
--
/*!50003 DROP PROCEDURE IF EXISTS `Cart_Crud` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `Cart_Crud`(
    IN Action VARCHAR(20),
    IN p_ProductId INT,
    IN p_Quantity INT,
    IN p_UserId INT
)
BEGIN
    IF Action = 'SELECT' THEN
        SELECT * FROM Carts WHERE UserId = p_UserId;
    ELSEIF Action = 'INSERT' THEN
        IF EXISTS (SELECT 1 FROM Carts WHERE ProductId = p_ProductId AND UserId = p_UserId) THEN
            UPDATE Carts SET Quantity = Quantity + p_Quantity WHERE ProductId = p_ProductId AND UserId = p_UserId;
        ELSE
            INSERT INTO Carts (ProductId, Quantity, UserId) VALUES (p_ProductId, p_Quantity, p_UserId);
        END IF;
    ELSEIF Action = 'UPDATE' THEN
        UPDATE Carts SET Quantity = p_Quantity WHERE ProductId = p_ProductId AND UserId = p_UserId;
    ELSEIF Action = 'DELETE' THEN
        DELETE FROM Carts WHERE ProductId = p_ProductId AND UserId = p_UserId;
    END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Category_Crud` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `Category_Crud`(
    IN Action VARCHAR(20),
    IN p_CategoryId INT,
    IN p_Name VARCHAR(50),
    IN p_IsActive BOOLEAN,
    IN p_ImageUrl TEXT
)
BEGIN
    IF Action = 'SELECT' THEN
        SELECT * FROM Categories ORDER BY CreatedDate DESC;
    ELSEIF Action = 'GETBYID' THEN
        SELECT * FROM Categories WHERE CategoryId = p_CategoryId;
    ELSEIF Action = 'INSERT' THEN
        INSERT INTO Categories (Name, ImageUrl, IsActive, CreatedDate)
        VALUES (p_Name, p_ImageUrl, p_IsActive, NOW());
    ELSEIF Action = 'UPDATE' THEN
        IF p_ImageUrl IS NULL OR p_ImageUrl = '' THEN
            UPDATE Categories SET Name = p_Name, IsActive = p_IsActive WHERE CategoryId = p_CategoryId;
        ELSE
            UPDATE Categories SET Name = p_Name, ImageUrl = p_ImageUrl, IsActive = p_IsActive WHERE CategoryId = p_CategoryId;
        END IF;
    ELSEIF Action = 'DELETE' THEN
        DELETE FROM Categories WHERE CategoryId = p_CategoryId;
    END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Product_Crud` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `Product_Crud`(
    IN Action VARCHAR(20),
    IN p_ProductId INT,
    IN p_Name VARCHAR(50),
    IN p_Description TEXT,
    IN p_Price DECIMAL(10,2),
    IN p_Quantity INT,
    IN p_CategoryId INT,
    IN p_IsActive BOOLEAN,
    IN p_ImageUrl TEXT
)
BEGIN
    IF Action = 'SELECT' THEN
        SELECT p.ProductId, p.Name, p.Description, p.Price, p.Quantity, p.ImageUrl, p.CategoryId, p.IsActive, p.CreatedDate, c.Name AS CategoryName
        FROM Products p INNER JOIN Categories c ON p.CategoryId = c.CategoryId ORDER BY p.CreatedDate DESC;
    ELSEIF Action = 'GETBYID' THEN
        SELECT * FROM Products WHERE ProductId = p_ProductId;
    ELSEIF Action = 'INSERT' THEN
        INSERT INTO Products (Name, Description, Price, Quantity, ImageUrl, CategoryId, IsActive, CreatedDate)
        VALUES (p_Name, p_Description, p_Price, p_Quantity, p_ImageUrl, p_CategoryId, p_IsActive, NOW());
    ELSEIF Action = 'UPDATE' THEN
        IF p_ImageUrl IS NULL OR p_ImageUrl = '' THEN
            UPDATE Products SET Name = p_Name, Description = p_Description, Price = p_Price, Quantity = p_Quantity, CategoryId = p_CategoryId, IsActive = p_IsActive WHERE ProductId = p_ProductId;
        ELSE
UPDATE Products SET Name = p_Name, Description = p_Description, Price = p_Price, Quantity = p_Quantity, ImageUrl = p_ImageUrl, CategoryId = p_CategoryId, IsActive = p_IsActive WHERE ProductId = p_ProductId;
        END IF;
ELSEIF Action = 'DELETE' THEN
        DELETE FROM Products WHERE ProductId = p_ProductId;
    END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `savePayment` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `savePayment`(
    IN p_Name VARCHAR(100),
    IN p_CardNo VARCHAR(50),
    IN p_ExpiryDate VARCHAR(50),
    IN p_Cvv INT,
    IN p_Address TEXT,
    IN p_PaymentMode VARCHAR(50),
    OUT p_InsertedId INT
)
BEGIN
    INSERT INTO Payment (Name, CardNo, ExpiryDate, Cvv, Address, PaymentMode)
    VALUES (p_Name, p_CardNo, p_ExpiryDate, p_Cvv, p_Address, p_PaymentMode);

    -- Lấy ID tự động tăng của bản ghi vừa chèn gán vào tham số OUT
    SET p_InsertedId = LAST_INSERT_ID();
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `save_order_item` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `save_order_item`(
    IN p_OrderNo VARCHAR(100),
    IN p_ProductId INT,
    IN p_Quantity INT,
    IN p_UserId INT,
    IN p_Status VARCHAR(50),
    IN p_PaymentId INT
)
BEGIN
    INSERT INTO Orders (OrderNo, ProductId, Quantity, UserId, Status, PaymentId, OrderDate)
    VALUES (p_OrderNo, p_ProductId, p_Quantity, p_UserId, p_Status, p_PaymentId, NOW());
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `save_payment` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `save_payment`(
    IN p_Name VARCHAR(50),
    IN p_CardNo VARCHAR(50),
    IN p_ExpiryDate VARCHAR(50),
    IN p_CvvNo INT,
    IN p_Address TEXT,
    IN p_PaymentMode VARCHAR(50),
    OUT p_InsertedId INT
)
BEGIN
    INSERT INTO Payment (Name, CardNo, ExpiryDate, CvvNo, Address, PaymentMode)
    VALUES (p_Name, p_CardNo, p_ExpiryDate, p_CvvNo, p_Address, p_PaymentMode);
    
    SET p_InsertedId = LAST_INSERT_ID();
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_GetInvoiceById` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_GetInvoiceById`(
    IN p_PaymentId INT,
    IN p_UserId INT
)
BEGIN
    SET @row_num := 0;
    
    SELECT 
        (@row_num := @row_num + 1) AS SrNo,
        o.OrderNo,
        p.Name AS ProductName,
        p.Price,
        o.Quantity,
        (o.Quantity * p.Price) AS TotalPrice,
        o.OrderDate,
        o.Status
    FROM Orders o
    INNER JOIN Products p ON p.ProductId = o.ProductId
    WHERE o.PaymentId = p_PaymentId AND o.UserId = p_UserId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_GetOrderHistory` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_GetOrderHistory`(
    IN p_UserId INT
)
BEGIN
    SELECT 
        o.OrderDetailId,
        o.OrderNo,
        (o.Quantity * p.Price) AS TotalPrice,
        o.Status,
        o.OrderDate,
        pm.PaymentMode,
        p.Name AS ProductName
    FROM Orders o
    INNER JOIN Payment pm ON pm.PaymentId = o.PaymentId
    INNER JOIN Products p ON p.ProductId = o.ProductId
    WHERE o.UserId = p_UserId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_GetOrderStatus` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_GetOrderStatus`(
    IN p_OrderDetailId INT
)
BEGIN
    SELECT 
        o.OrderDetailId,
        o.OrderNo,
        (o.Quantity * p.Price) AS TotalPrice,
        o.Status,
        o.OrderDate,
        pm.PaymentMode,
        p.Name AS ProductName
    FROM Orders o
    INNER JOIN Payment pm ON pm.PaymentId = o.PaymentId
    INNER JOIN Products p ON p.ProductId = o.ProductId
    WHERE o.OrderDetailId = p_OrderDetailId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sp_UpdateOrderStatus` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_UpdateOrderStatus`(
    IN p_OrderDetailId INT,
    IN p_Status VARCHAR(50)
)
BEGIN
    UPDATE Orders 
    SET Status = p_Status 
    WHERE OrderDetailId = p_OrderDetailId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `User_Crud` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `User_Crud`(
    IN p_Action VARCHAR(20),
    IN p_UserId INT,
    IN p_Name VARCHAR(50),
    IN p_Username VARCHAR(50),
    IN p_Mobile VARCHAR(50),
    IN p_Email VARCHAR(50),
    IN p_Address TEXT,
    IN p_PostCode VARCHAR(20),
    IN p_Password VARCHAR(50),
    IN p_ImageUrl TEXT
)
BEGIN
    IF p_Action = 'SELECT4LOGIN' THEN
        SELECT * FROM Users WHERE Username = p_Username AND Password = p_Password;
    ELSEIF p_Action = 'SELECT4PROFILE' THEN
        SELECT * FROM Users WHERE UserId = p_UserId;
    ELSEIF p_Action = 'INSERT' THEN
        INSERT INTO Users (Name, Username, Mobile, Email, Address, PostCode, Password, ImageUrl, CreatedDate)
        VALUES (p_Name, p_Username, p_Mobile, p_Email, p_Address, p_PostCode, p_Password, p_ImageUrl, NOW());
    ELSEIF p_Action = 'UPDATE' THEN
        IF p_ImageUrl IS NULL OR p_ImageUrl = '' THEN
            UPDATE Users SET Name = p_Name, Username = p_Username, Mobile = p_Mobile, Email = p_Email, Address = p_Address, PostCode = p_PostCode WHERE UserId = p_UserId;
        ELSE
            UPDATE Users SET Name = p_Name, Username = p_Username, Mobile = p_Mobile, Email = p_Email, Address = p_Address, PostCode = p_PostCode, ImageUrl = p_ImageUrl WHERE UserId = p_UserId;
        END IF;
    ELSEIF p_Action = 'SELECT4ADMIN' THEN
        SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SrNo, UserId, Name, Username, Email, CreatedDate FROM Users;
    ELSEIF p_Action = 'DELETE' THEN
        DELETE FROM Users WHERE UserId = p_UserId;
    END IF;products
END ;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-05-usersUserIdUserIdNameUsernameMobile
use  fastfooddb;
DROP PROCEDURE IF EXISTS `Cart_Crud`;

DELIMITER ;;
CREATE PROCEDURE `Cart_Crud`(
    IN Action VARCHAR(20),
    IN p_ProductId INT,
    IN p_Quantity INT,
    IN p_UserId INT
)
BEGIN
    IF Action = 'SELECT' THEN
        -- SỬA Ở ĐÂY: Thực hiện JOIN để lấy Name, Price, ImageUrl và số lượng tồn kho (Quantity AS PrdQty)
        SELECT 
            c.CartId,
            c.ProductId, 
            c.Quantity, 
            c.UserId, 
            p.Name AS ProductName, 
            p.Price, 
            p.ImageUrl, 
            p.Quantity AS PrdQty
        FROM carts c 
        INNER JOIN products p ON c.ProductId = p.ProductId 
        WHERE c.UserId = p_UserId;
        
    ELSEIF Action = 'INSERT' THEN
        IF EXISTS (SELECT 1 FROM carts WHERE ProductId = p_ProductId AND UserId = p_UserId) THEN
            UPDATE carts SET Quantity = Quantity + p_Quantity WHERE ProductId = p_ProductId AND UserId = p_UserId;
        ELSE
            INSERT INTO carts (ProductId, Quantity, UserId) VALUES (p_ProductId, p_Quantity, p_UserId);
        END IF;
    ELSEIF Action = 'UPDATE' THEN
        UPDATE carts SET Quantity = p_Quantity WHERE ProductId = p_ProductId AND UserId = p_UserId;
    ELSEIF Action = 'DELETE' THEN
        DELETE FROM carts WHERE ProductId = p_ProductId AND UserId = p_UserId;
    END IF;
END ;;
DELIMITER ;

USE `fastfooddb`;

-- 1. Xóa bỏ các Procedure cũ bị lệch tên cột
DROP PROCEDURE IF EXISTS `save_order_item`;
DROP PROCEDURE IF EXISTS `sp_GetOrderHistory`;
DROP PROCEDURE IF EXISTS `sp_GetOrderStatus`;
DROP PROCEDURE IF EXISTS `sp_UpdateOrderStatus`;

-- 2. Tạo lại save_order_item chuẩn theo bảng orders (OrderDetailsId)
DELIMITER ;;
CREATE PROCEDURE `save_order_item`(
    IN p_OrderNo VARCHAR(100),
    IN p_ProductId INT,
    IN p_Quantity INT,
    IN p_UserId INT,
    IN p_Status VARCHAR(50),
    IN p_PaymentId INT
)
BEGIN
    -- Chỉ định rõ tên cột để tránh lỗi ép kiểu cấu trúc dữ liệu
    INSERT INTO `orders` (OrderNo, ProductId, Quantity, UserId, Status, PaymentId, OrderDate)
    VALUES (p_OrderNo, p_ProductId, p_Quantity, p_UserId, p_Status, p_PaymentId, NOW());
END ;;
DELIMITER ;

-- 3. Sửa lại sp_GetOrderHistory (OrderDetailId -> OrderDetailsId)
DELIMITER ;;
CREATE PROCEDURE `sp_GetOrderHistory`(
    IN p_UserId INT
)
BEGIN
    SELECT 
        o.OrderDetailsId, -- Đã thêm chữ s cho khớp với bảng chính
        o.OrderNo,
        (o.Quantity * p.Price) AS TotalPrice,
        o.Status,
        o.OrderDate,
        pm.PaymentMode,
        p.Name AS ProductName
    FROM orders o
    INNER JOIN payment pm ON pm.PaymentId = o.PaymentId
    INNER JOIN products p ON p.ProductId = o.ProductId
    WHERE o.UserId = p_UserId;
END ;;
DELIMITER ;

-- 4. Sửa lại sp_GetOrderStatus (OrderDetailId -> OrderDetailsId)
DELIMITER ;;
CREATE PROCEDURE `sp_GetOrderStatus`(
    IN p_OrderDetailsId INT
)
BEGIN
    SELECT 
        o.OrderDetailsId,
        o.OrderNo,
        (o.Quantity * p.Price) AS TotalPrice,
        o.Status,
        o.OrderDate,
        pm.PaymentMode,
        p.Name AS ProductName
    FROM orders o
    INNER JOIN payment pm ON pm.PaymentId = o.PaymentId
    INNER JOIN products p ON p.ProductId = o.ProductId
    WHERE o.OrderDetailsId = p_OrderDetailsId;
END ;;
DELIMITER ;

-- 5. Sửa lại sp_UpdateOrderStatus (OrderDetailId -> OrderDetailsId)
DELIMITER ;;
CREATE PROCEDURE `sp_UpdateOrderStatus`(
    IN p_OrderDetailsId INT,
    IN p_Status VARCHAR(50)
)
BEGIN
    UPDATE orders 
    SET Status = p_Status 
    WHERE OrderDetailsId = p_OrderDetailsId;
END ;;
DELIMITER ;