use master


IF EXISTS(SELECT 1 FROM sys.databases WHERE name = 'RouletteAppTest')
BEGIN
    ALTER DATABASE RouletteAppTest SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE RouletteAppTest;
END

-- Create a new database for the roulette app
CREATE DATABASE RouletteAppTest;
GO

-- Use the newly created database
USE RouletteAppTest;
GO

-- Create the users table
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    Balance MONEY NOT NULL DEFAULT 0
);
GO

-- Create the bets table
CREATE TABLE Bets (
    BetID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
	SpinIdNumber INT,
    BetOnColorRed MONEY,
    BetOnColorBlack MONEY,
    BetOnEven MONEY,
    BetOnOdd MONEY,
    BetOnLow MONEY,
    BetOnHigh MONEY,
    BetOnFirstDozen MONEY,
    BetOnSecondDozen MONEY,
    BetOnThirdDozen MONEY,
    BetOnFirstColumn MONEY,
    BetOnSecondColumn MONEY,
    BetOnThirdColumn MONEY,
    BetOnNumber0 MONEY,
    BetOnNumber1 MONEY,
    BetOnNumber2 MONEY,
    BetOnNumber3 MONEY,
    BetOnNumber4 MONEY,
    BetOnNumber5 MONEY,
    BetOnNumber6 MONEY,
    BetOnNumber7 MONEY,
    BetOnNumber8 MONEY,
    BetOnNumber9 MONEY,
    BetOnNumber10 MONEY,
    BetOnNumber11 MONEY,
    BetOnNumber12 MONEY,
    BetOnNumber13 MONEY,
    BetOnNumber14 MONEY,
    BetOnNumber15 MONEY,
    BetOnNumber16 MONEY,
    BetOnNumber17 MONEY,
    BetOnNumber18 MONEY,
    BetOnNumber19 MONEY,
    BetOnNumber20 MONEY,
    BetOnNumber21 MONEY,
    BetOnNumber22 MONEY,
    BetOnNumber23 MONEY,
    BetOnNumber24 MONEY,
    BetOnNumber25 MONEY,
    BetOnNumber26 MONEY,
    BetOnNumber27 MONEY,
    BetOnNumber28 MONEY,
    BetOnNumber29 MONEY,
    BetOnNumber30 MONEY,
    BetOnNumber31 MONEY,
    BetOnNumber32 MONEY,
    BetOnNumber33 MONEY,
    BetOnNumber34 MONEY,
    BetOnNumber35 MONEY,
    BetOnNumber36 MONEY
);
GO

-- Create the spin results table
CREATE TABLE SpinResults (
    SpinIdNumber INT PRIMARY KEY IDENTITY(1,1),
    Result INT NOT NULL
);
GO

-- Create the payouts table
CREATE TABLE Payouts (
    PayoutID INT PRIMARY KEY IDENTITY(1,1),
    BetID INT FOREIGN KEY REFERENCES Bets(BetID),
    SpinIdNumber INT FOREIGN KEY REFERENCES SpinResults(SpinIdNumber),
    PayoutAmount MONEY
);
GO


CREATE PROCEDURE pr_AddSpinResult
    @Result INT
AS
BEGIN
    INSERT INTO SpinResults (Result)
    VALUES (@Result);

	SELECT SCOPE_IDENTITY()
END
GO

CREATE PROCEDURE pr_GetSpinResult
    @SpinIdNumber INT
AS
BEGIN
    SELECT *
    FROM SpinResults
    WHERE SpinIdNumber = @SpinIdNumber;
END
GO

CREATE PROCEDURE pr_GetAllSpinResults
AS
BEGIN
    SELECT *
    FROM SpinResults
END
GO

CREATE PROCEDURE pr_AddPayout
    @BetID INT,
    @SpinIdNumber  INT,
    @PayoutAmount MONEY
AS
BEGIN
    INSERT INTO Payouts (BetID, SpinIdNumber, PayoutAmount)
    VALUES (@BetID, @SpinIdNumber, @PayoutAmount);
END
GO

CREATE PROCEDURE pr_GetAllPayouts
AS
BEGIN
    SELECT *
    FROM Payouts
END
GO

CREATE PROCEDURE pr_GetSinglePayout
    @PayoutID INT
AS
BEGIN
    SELECT *
    FROM Payouts
    WHERE PayoutID = @PayoutID;
END
GO

CREATE PROCEDURE pr_GetAllBets
AS
BEGIN
    SELECT *
    FROM Bets
END
GO

CREATE PROCEDURE pr_GetSingleBet
    @BetID INT
AS
BEGIN
    SELECT *
    FROM Bets
    WHERE BetID = @BetID;
END
GO

CREATE PROCEDURE pr_GetBetsForSpin
    @SpinIdNumber INT
AS
BEGIN
    SELECT *
    FROM Bets
    WHERE SpinIdNumber = @SpinIdNumber;
END
GO

CREATE PROCEDURE pr_GetBetsForUser
    @UserId INT
AS
BEGIN
    SELECT *
    FROM Bets
    WHERE UserID = @UserId;
END
GO


CREATE PROCEDURE pr_AddBet
    @UserID INT,
    @BetOnNumber0 MONEY,
    @BetOnNumber1 MONEY,
    @BetOnNumber2 MONEY,
    @BetOnNumber3 MONEY,
    @BetOnNumber4 MONEY,
    @BetOnNumber5 MONEY,
    @BetOnNumber6 MONEY,
    @BetOnNumber7 MONEY,
    @BetOnNumber8 MONEY,
    @BetOnNumber9 MONEY,
    @BetOnNumber10 MONEY,
    @BetOnNumber11 MONEY,
    @BetOnNumber12 MONEY,
    @BetOnNumber13 MONEY,
    @BetOnNumber14 MONEY,
    @BetOnNumber15 MONEY,
    @BetOnNumber16 MONEY,
    @BetOnNumber17 MONEY,
    @BetOnNumber18 MONEY,
    @BetOnNumber19 MONEY,
    @BetOnNumber20 MONEY,
    @BetOnNumber21 MONEY,
    @BetOnNumber22 MONEY,
    @BetOnNumber23 MONEY,
    @BetOnNumber24 MONEY,
    @BetOnNumber25 MONEY,
    @BetOnNumber26 MONEY,
    @BetOnNumber27 MONEY,
    @BetOnNumber28 MONEY,
    @BetOnNumber29 MONEY,
    @BetOnNumber30 MONEY,
    @BetOnNumber31 MONEY,
    @BetOnNumber32 MONEY,
    @BetOnNumber33 MONEY,
    @BetOnNumber34 MONEY,
    @BetOnNumber35 MONEY,
    @BetOnNumber36 MONEY,
    @BetOnColorRed MONEY,
    @BetOnColorBlack MONEY,
    @BetOnEven MONEY,
    @BetOnOdd MONEY,
    @BetOnLow MONEY,
    @BetOnHigh MONEY,
    @BetOnFirstDozen MONEY,
    @BetOnSecondDozen MONEY,
    @BetOnThirdDozen MONEY,
    @BetOnFirstColumn MONEY,
    @BetOnSecondColumn MONEY,
    @BetOnThirdColumn MONEY
AS
BEGIN
    INSERT INTO Bets (UserID, BetOnNumber0, BetOnNumber1, BetOnNumber2, BetOnNumber3, BetOnNumber4, BetOnNumber5, BetOnNumber6, BetOnNumber7, BetOnNumber8,
                      BetOnNumber9, BetOnNumber10, BetOnNumber11, BetOnNumber12, BetOnNumber13, BetOnNumber14, BetOnNumber15, BetOnNumber16, BetOnNumber17,
                      BetOnNumber18, BetOnNumber19, BetOnNumber20, BetOnNumber21, BetOnNumber22, BetOnNumber23, BetOnNumber24, BetOnNumber25, BetOnNumber26,
                      BetOnNumber27, BetOnNumber28, BetOnNumber29, BetOnNumber30, BetOnNumber31, BetOnNumber32, BetOnNumber33, BetOnNumber34, BetOnNumber35,
                      BetOnNumber36, BetOnColorRed, BetOnColorBlack, BetOnEven, BetOnOdd, BetOnLow, BetOnHigh,
                      BetOnFirstDozen, BetOnSecondDozen, BetOnThirdDozen, BetOnFirstColumn, BetOnSecondColumn,
                      BetOnThirdColumn)
    VALUES (@UserID, @BetOnNumber0, @BetOnNumber1, @BetOnNumber2, @BetOnNumber3, @BetOnNumber4, @BetOnNumber5, @BetOnNumber6, @BetOnNumber7, @BetOnNumber8,
            @BetOnNumber9, @BetOnNumber10, @BetOnNumber11, @BetOnNumber12, @BetOnNumber13, @BetOnNumber14, @BetOnNumber15, @BetOnNumber16, @BetOnNumber17,
            @BetOnNumber18, @BetOnNumber19, @BetOnNumber20, @BetOnNumber21, @BetOnNumber22, @BetOnNumber23, @BetOnNumber24, @BetOnNumber25, @BetOnNumber26,
            @BetOnNumber27, @BetOnNumber28, @BetOnNumber29, @BetOnNumber30, @BetOnNumber31, @BetOnNumber32, @BetOnNumber33, @BetOnNumber34, @BetOnNumber35,
            @BetOnNumber36, @BetOnColorRed, @BetOnColorBlack, @BetOnEven, @BetOnOdd, @BetOnLow, @BetOnHigh,
            @BetOnFirstDozen, @BetOnSecondDozen, @BetOnThirdDozen, @BetOnFirstColumn, @BetOnSecondColumn,
            @BetOnThirdColumn);
END
GO






INSERT INTO Users (Username, Email, Password, Balance) VALUES
('user1', 'user1@example.com', 'password1', 100.00),
('user2', 'user2@example.com', 'password2', 200.00),
('user3', 'user3@example.com', 'password3', 300.00),
('user4', 'user4@example.com', 'password4', 400.00)


INSERT INTO Bets(UserID, SpinIdNumber, BetOnColorRed, BetOnColorBlack, BetOnEven, BetOnOdd, BetOnLow, BetOnHigh, BetOnFirstDozen, BetOnSecondDozen, BetOnThirdDozen, BetOnFirstColumn, BetOnSecondColumn, BetOnThirdColumn, BetOnNumber0, BetOnNumber1, BetOnNumber2, BetOnNumber3, BetOnNumber4, BetOnNumber5, BetOnNumber6, BetOnNumber7, BetOnNumber8, BetOnNumber9, BetOnNumber10, BetOnNumber11, BetOnNumber12, BetOnNumber13, BetOnNumber14, BetOnNumber15, BetOnNumber16, BetOnNumber17, BetOnNumber18, BetOnNumber19, BetOnNumber20, BetOnNumber21, BetOnNumber22, BetOnNumber23, BetOnNumber24, BetOnNumber25, BetOnNumber26, BetOnNumber27, BetOnNumber28, BetOnNumber29, BetOnNumber30, BetOnNumber31, BetOnNumber32, BetOnNumber33, BetOnNumber34, BetOnNumber35, BetOnNumber36)
SELECT
    1,
	2,
    RAND() * 100 AS BetOnColorRed,
    RAND() * 100 AS BetOnColorBlack,
    RAND() * 100 AS BetOnEven,
    RAND() * 100 AS BetOnOdd,
    RAND() * 100 AS BetOnLow,
    RAND() * 100 AS BetOnHigh,
    RAND() * 100 AS BetOnFirstDozen,
    RAND() * 100 AS BetOnSecondDozen,
    RAND() * 100 AS BetOnThirdDozen,
    RAND() * 100 AS BetOnFirstColumn,
    RAND() * 100 AS BetOnSecondColumn,
    RAND() * 100 AS BetOnThirdColumn,
    RAND() * 100 AS BetOnNumber0,
    RAND() * 100 AS BetOnNumber1,
    RAND() * 100 AS BetOnNumber2,
    RAND() * 100 AS BetOnNumber3,
    RAND() * 100 AS BetOnNumber4,
    RAND() * 100 AS BetOnNumber5,
    RAND() * 100 AS BetOnNumber6,
    RAND() * 100 AS BetOnNumber7,
    RAND() * 100 AS BetOnNumber8,
    RAND() * 100 AS BetOnNumber9,
    RAND() * 100 AS BetOnNumber10,
    RAND() * 100 AS BetOnNumber11,
    RAND() * 100 AS BetOnNumber12,
    RAND() * 100 AS BetOnNumber13,
    RAND() * 100 AS BetOnNumber14,
    RAND() * 100 AS BetOnNumber15,
    RAND() * 100 AS BetOnNumber16,
    RAND() * 100 AS BetOnNumber17,
    RAND() * 100 AS BetOnNumber18,
    RAND() * 100 AS BetOnNumber19,
    RAND() * 100 AS BetOnNumber20,
    RAND() * 100 AS BetOnNumber21,
    RAND() * 100 AS BetOnNumber22,
    RAND() * 100 AS BetOnNumber23,
    RAND() * 100 AS BetOnNumber24,
    RAND() * 100 AS BetOnNumber25,
    RAND() * 100 AS BetOnNumber26,
    RAND() * 100 AS BetOnNumber27,
    RAND() * 100 AS BetOnNumber28,
    RAND() * 100 AS BetOnNumber29,
    RAND() * 100 AS BetOnNumber30,
    RAND() * 100 AS BetOnNumber31,
    RAND() * 100 AS BetOnNumber32,
    RAND() * 100 AS BetOnNumber33,
    RAND() * 100 AS BetOnNumber34,
    RAND() * 100 AS BetOnNumber35,
    RAND() * 100 AS BetOnNumber36
	UNION 
	SELECT
    2,
	8,
    RAND() * 100 AS BetOnColorRed,
    0 AS BetOnColorBlack,
    0 AS BetOnEven,
    0 AS BetOnOdd,
    0 AS BetOnLow,
    0 AS BetOnHigh,
    RAND() * 100 AS BetOnFirstDozen,
    0 AS BetOnSecondDozen,
    0 AS BetOnThirdDozen,
    0 AS BetOnFirstColumn,
    0 AS BetOnSecondColumn,
    0 AS BetOnThirdColumn,
    0 AS BetOnNumber0,
    0 AS BetOnNumber1,
    0 AS BetOnNumber2,
    0 AS BetOnNumber3,
    0 AS BetOnNumber4,
    0 AS BetOnNumber5,
    0 AS BetOnNumber6,
    0 AS BetOnNumber7,
    0 AS BetOnNumber8,
    0 AS BetOnNumber9,
    0 AS BetOnNumber10,
    0 AS BetOnNumber11,
    0 AS BetOnNumber12,
    0 AS BetOnNumber13,
    0 AS BetOnNumber14,
    0 AS BetOnNumber15,
    0 AS BetOnNumber16,
    0 AS BetOnNumber17,
    0 AS BetOnNumber18,
    0 AS BetOnNumber19,
    0 AS BetOnNumber20,
    0 AS BetOnNumber21,
    0 AS BetOnNumber22,
    0 AS BetOnNumber23,
    0 AS BetOnNumber24,
    0 AS BetOnNumber25,
    0 AS BetOnNumber26,
    0 AS BetOnNumber27,
    0 AS BetOnNumber28,
    0 AS BetOnNumber29,
    0 AS BetOnNumber30,
    0 AS BetOnNumber31,
    0 AS BetOnNumber32,
    0 AS BetOnNumber33,
    0 AS BetOnNumber34,
    0 AS BetOnNumber35,
    RAND() * 100 AS BetOnNumber36;


INSERT INTO SpinResults (Result)
VALUES (Cast(RAND() * 36 as int)),(Cast(RAND() * 36 as int)),(Cast(RAND() * 36 as int)),(Cast(RAND() * 36 as int)),(Cast(RAND() * 36 as int)),(Cast(RAND() * 36 as int)),(Cast(RAND() * 36 as int)),(Cast(RAND() * 36 as int))
