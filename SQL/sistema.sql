create database sistema

create table empresa(
id_empresa int primary key identity(1,1),
nome varchar (200) not null,
senha varchar (50) not null,
rua varchar (60) not null,
bairro varchar (60) not null,
complemento varchar (30),
cep varchar (9) not null,
numero varchar (5) not null,
telefone numeric (13) not null,
) 

create table funcionario(
id_funcionario int primary key identity(1,1),
nome varchar (200) not null,
cargo varchar (13) not null,
salario numeric (15) not null,
id_empresa int,
CONSTRAINT fk_EmpFun FOREIGN KEY (id_empresa) REFERENCES empresa (id_empresa)
)

insert into empresa values('Teste3','1234567','Rua José','Tal','',14403830,'4100',16988016082)

insert into funcionario values('Fu','Desenvolvedor',3300,1)


select id_empresa from empresa where nome = 'Teste1' and senha = '1234567'
select * from funcionario

drop table empresa

select f.nome, f.cargo, f.salario from empresa e inner join funcionario f on e.id_empresa = f.id_empresa where e.id_empresa = 1

