-- Create UsersTable
CREATE TABLE Users
(
    id       INT PRIMARY KEY,
    name     TEXT NOT NULL,
    email    TEXT NOT NULL,
    password TEXT NOT NULL
);
CREATE UNIQUE INDEX users_email_key ON Users("email");

-- Create GameState Table
CREATE TABLE GameState
(
    id  INT PRIMARY KEY,
    name TEXT NOT NULL,
    saveState TEXT
);