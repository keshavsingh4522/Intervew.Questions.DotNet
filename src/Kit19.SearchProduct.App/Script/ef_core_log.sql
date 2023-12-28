create database Kit19;
use kit19;

DELIMITER $$

CREATE PROCEDURE sp_SearchProducts(
    IN p_ProductName VARCHAR(255),
    IN p_Size VARCHAR(50),
    IN p_Price DECIMAL(18, 2),
    IN p_MfgDate DATETIME,
    IN p_Category VARCHAR(50),
    IN p_Conjunction VARCHAR(3)
)
BEGIN
    SET @Query = 'SELECT * FROM tbl_Product WHERE 1=1';
    SET @WhereClause = '';

    IF p_ProductName IS NOT NULL AND TRIM(p_ProductName) <> '' THEN
        SET @WhereClause = CONCAT(@WhereClause, ' (ProductName LIKE "%', p_ProductName, '%")');
    END IF;

    IF p_Size IS NOT NULL AND TRIM(p_Size) <> '' THEN
        IF LENGTH(@WhereClause) > 0 THEN
            SET @WhereClause = CONCAT(@WhereClause, ' ', p_Conjunction);
        END IF;
        SET @WhereClause = CONCAT(@WhereClause, ' (Size = "', p_Size, '")');
    END IF;

    IF p_Price IS NOT NULL THEN
        IF LENGTH(@WhereClause) > 0 THEN
            SET @WhereClause = CONCAT(@WhereClause, ' ', p_Conjunction);
        END IF;
        SET @WhereClause = CONCAT(@WhereClause, ' (Price = ', p_Price, ')');
    END IF;

    IF p_MfgDate IS NOT NULL THEN
        IF LENGTH(@WhereClause) > 0 THEN
            SET @WhereClause = CONCAT(@WhereClause, ' ', p_Conjunction);
        END IF;
        SET @WhereClause = CONCAT(@WhereClause, ' (MfgDate = "', p_MfgDate, '")');
    END IF;

    IF p_Category IS NOT NULL AND TRIM(p_Category) <> '' THEN
        IF LENGTH(@WhereClause) > 0 THEN
            SET @WhereClause = CONCAT(@WhereClause, ' ', p_Conjunction);
        END IF;
        SET @WhereClause = CONCAT(@WhereClause, ' (Category = "', p_Category, '")');
    END IF;

    IF LENGTH(@WhereClause) > 0 THEN
        SET @Query = CONCAT(@Query, ' AND (', @WhereClause, ')');
    END IF;

    PREPARE stmt FROM @Query;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
END$$

DELIMITER ;

CREATE TABLE `tbl_Product` (
    `ProductId` INT AUTO_INCREMENT PRIMARY KEY,
    `ProductName` VARCHAR(255) NOT NULL,
    `Size` VARCHAR(50) NULL,
    `Price` DECIMAL(18, 2) NULL,
    `MfgDate` DATE NULL,
    `Category` VARCHAR(50) NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

INSERT INTO `tbl_Product` (`ProductName`, `Size`, `Price`, `MfgDate`, `Category`) VALUES
('Laptop Pro', 'L', 1200.00, '2021-03-15', 'Electronics'),
('Smartphone X', 'M', 800.00, '2022-06-10', 'Electronics'),
('Water Bottle', 'S', 10.00, '2020-01-05', 'Accessories'),
('Backpack Explorer', 'M', 70.00, '2021-08-20', 'Outdoor'),
('Running Shoes', 'L', 100.00, '2022-05-30', 'Sportswear'),
('Desk Lamp', 'S', 35.00, '2021-12-10', 'Furniture'),
('Gaming Monitor', 'L', 300.00, '2023-01-01', 'Electronics'),
('Bluetooth Headphones', 'M', 150.00, '2022-07-22', 'Electronics'),
('Leather Wallet', 'S', 45.00, '2020-11-15', 'Accessories'),
('Coffee Maker', 'M', 120.00, '2022-03-05', 'Appliances');
