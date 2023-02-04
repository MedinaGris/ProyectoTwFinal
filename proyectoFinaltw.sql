create database prueba;

use prueba;

create table productos(
id integer identity(1,1) primary key,
nombre varchar(50) not null ,
codigo varchar(50) not null ,
descripcion varchar(100)  not null ,
cantidad integer not null  
)

create table proveedores(
id integer identity(1,1) primary key,
nombre varchar(50) not null ,
direccion varchar(50) not null ,
correo varchar(100)  not null ,
edad integer not null  
)

create table clientes(
id integer identity(1,1) primary key,
nombre varchar(50) not null ,
direccion varchar(50) not null ,
correo varchar(100)  not null ,
edad integer not null  
)

select *from productos;
select *from clientes;
select *from proveedores;