USE master;
GO


IF DB_ID(N'FifaStats') IS NULL
	CREATE DATABASE FifaStats;

	GO


USE FifaStats;

GO


IF OBJECT_ID('Spielergebnisse') IS NOT NULL
DROP TABLE Spielergebnisse;
GO


IF OBJECT_ID('Mannschaften') IS NOT NULL
DROP TABLE Mannschaften;
GO


CREATE TABLE Mannschaften (
MannschaftsID INT IDENTITY PRIMARY KEY,
Torwart varchar(100) NOT NULL,
Defensive varchar(100) NOT NULL,
Mittelfeld varchar(100) NOT NULL,
Sturm varchar(100) NOT NULL);


IF OBJECT_ID('Formationen') IS NOT NULL
DROP TABLE Formationen;
GO


CREATE TABLE Formationen(
FormationsID INT IDENTITY PRIMARY KEY,
Aufstellung nvarchar(100) NOT NULL);


CREATE TABLE Spielergebnisse(
SpielID INT IDENTITY PRIMARY KEY,
Tore INT,
Gegentore INT,
MannschaftsID INT,
CONSTRAINT FK_MannschatfsID FOREIGN KEY (MannschaftsID)
REFERENCES Mannschaften(MannschaftsID),
FormationsID INT,
CONSTRAINT FK_FormationsID FOREIGN KEY(FormationsID)
REFERENCES Formationen(FormationsID)
);
