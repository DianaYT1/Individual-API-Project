-- CREATE DATABASE myDataBase;
USE myDataBase;

CREATE TABLE [Company] (
    OrganizationId varchar(36) NOT NULL PRIMARY KEY,
    Name varchar(50) NOT NULL,
    Website varchar(255) NOT NULL,
    Country varchar(255) NOT NULL,
    Description TEXT NOT NULL,
    Founded varchar(4) NOT NULL,
    Industry varchar(255) NOT NULL,
    NumberOfEmployees INT NOT NULL,
);

CREATE TABLE [Country] (
    OrganizationId VARCHAR(36) NOT NULL PRIMARY KEY,
    Country varchar(255) NOT NULL,
    Name varchar(50) NOT NULL,
    FOREIGN KEY (OrganizationId) REFERENCES Company(OrganizationId)
);


