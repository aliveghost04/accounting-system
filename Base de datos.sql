use master;
drop database Contabilidad;
create database Contabilidad;
use Contabilidad;

create table parameters (
	id int primary key not null identity(1,1),
	year int not null,
	month int not null,
	processed char(1) not null,
	rnc varchar(15) not null,
	month_close int not null
);

create table account_types (
	id int primary key not null identity(1,1),
	description varchar(200) not null,
	type char(2) not null,
	state bit not null
);

CREATE TABLE currencies_types (
	id int primary key not null identity(1,1),
	description varchar(200) not null,
	exchange_rate decimal(10,2) not null,
	state bit not null
);

CREATE TABLE countables_accounts (
	id int primary key not null identity(1,1),
	description varchar(200) not null, 
	account_type int not null FOREIGN KEY REFERENCES account_types(id),
	allow_transaction bit not null,
	[level] int not null,
	account_major int null,
	balance decimal(10,2) not null,
	[state] bit not null
);

ALTER TABLE countables_accounts ADD FOREIGN KEY (account_major) REFERENCES countables_accounts(id);

CREATE TABLE placements (
	id int primary key not null identity(1,1),
	description varchar(200) not null, 
	auxiliary_id int not null,
	date datetime not null,
	currency_type int not null FOREIGN KEY REFERENCES currencies_types(id),
	exchange_rate decimal(10,2) not null,
	state char(1) not null
);

CREATE TABLE placements_movements (
	id int primary key not null identity(1,1),
	account int not null FOREIGN KEY REFERENCES countables_accounts(id),
	movement_type char(2) not null,
	amount decimal(10,2) not null,
	placement_id int not null FOREIGN KEY REFERENCES placements(id)
);

CREATE TABLE wholesale (
	id int primary key not null identity(1,1),
	major_account int not null FOREIGN KEY REFERENCES countables_accounts(id),
	type_movement char(2) not null,
	process_date datetime not null,
	balance decimal(10,2) not null,
	state char(1) not null
);

CREATE TABLE users (
	id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	name VARCHAR(100) NOT NULL,
	username VARCHAR(25) UNIQUE NOT NULL,
	[password] VARCHAR(200) NOT NULL,
	created_at DATETIME NOT NULL,
	updated_at DATETIME NULL,
	permission INT NOT NULL
);

INSERT INTO users VALUES('Erick', 'ejimenez', '81dc9bdb52d04dc20036dbd8313ed055', GETDATE(), NULL, 1)

select * from users
select * from account_types
select * from currencies_types
select * from countables_accounts
select * from placements
select * from placements_movements where id = 20
select * from parameters

delete from parameters
delete from countables_accounts where id = 6