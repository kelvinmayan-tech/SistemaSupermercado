# Sistema de Supermercado AV1

## Integrantes

- Kelvin Mayan Ramos Neri RA: 923206227
  
## Descrição do Projeto

Sistema desktop desenvolvido em C# com Windows Forms para gerenciamento de supermercado.

O sistema permite realizar o cadastro de produtos, atualização, exclusão e listagem dos produtos cadastrados. Também possui uma tela de compras/vendas, onde é possível selecionar produtos, informar a quantidade, adicionar itens ao carrinho, calcular o total da compra e emitir uma nota simples da venda.

## Funcionalidades

- Cadastro de produtos
- Atualização de produtos
- Exclusão de produtos
- Listagem de produtos
- Registro de compras/vendas
- Inserção de múltiplos produtos no carrinho
- Cálculo automático do subtotal
- Cálculo automático do valor total da compra
- Emissão de nota simples
- Salvamento da nota em arquivo `.txt`

## Tecnologias Utilizadas

- C#
- Windows Forms
- .NET
- MySQL
- MySQL Workbench
- MySqlConnector
- Visual Studio Code

## Banco de Dados

O banco de dados utilizado é o MySQL.

O script de criação do banco está na pasta:

Database/banco_supermercado.sql

Para configurar o banco:

1. Abra o MySQL Workbench.
2. Abra o arquivo `banco_supermercado.sql`.
3. Execute o script.
4. Verifique se o banco `supermercado` foi criado.

Comando para testar:

```sql
USE supermercado;

SELECT * FROM produtos;

## Script SQL das Tabelas

```sql
CREATE DATABASE supermercado;

USE supermercado;

CREATE TABLE produtos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    codigo VARCHAR(20) NOT NULL,
    nome VARCHAR(100) NOT NULL,
    categoria VARCHAR(50) NOT NULL,
    estoque INT NOT NULL,
    preco DECIMAL(10,2) NOT NULL,
    data_cadastro DATE NOT NULL
);

CREATE TABLE vendas (
    id INT AUTO_INCREMENT PRIMARY KEY,
    data_venda DATETIME NOT NULL,
    total DECIMAL(10,2) NOT NULL
);

CREATE TABLE itens_venda (
    id INT AUTO_INCREMENT PRIMARY KEY,
    venda_id INT NOT NULL,
    produto_id INT NOT NULL,
    quantidade INT NOT NULL,
    preco_unitario DECIMAL(10,2) NOT NULL,
    subtotal DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (venda_id) REFERENCES vendas(id),
    FOREIGN KEY (produto_id) REFERENCES produtos(id)
);
```

## Dados de Teste

```sql
INSERT INTO produtos
(codigo, nome, categoria, estoque, preco, data_cadastro)
VALUES
('001', 'Arroz 5kg', 'Alimentos', 50, 25.90, CURDATE()),
('002', 'Feijão 1kg', 'Alimentos', 80, 8.50, CURDATE()),
('003', 'Refrigerante 2L', 'Bebidas', 40, 9.99, CURDATE()),
('004', 'Sabão em Pó', 'Limpeza', 30, 14.50, CURDATE()),
('005', 'Shampoo', 'Higiene', 25, 18.90, CURDATE());
```
