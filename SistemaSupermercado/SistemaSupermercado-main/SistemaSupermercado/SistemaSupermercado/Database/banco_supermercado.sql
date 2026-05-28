CREATE DATABASE IF NOT EXISTS supermercado;
USE supermercado;

CREATE TABLE IF NOT EXISTS produtos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    codigo VARCHAR(20) NOT NULL,
    nome VARCHAR(100) NOT NULL,
    categoria VARCHAR(50) NOT NULL,
    estoque INT NOT NULL,
    preco DECIMAL(10,2) NOT NULL,
    data_cadastro DATE NOT NULL
);

CREATE TABLE IF NOT EXISTS vendas (
    id INT AUTO_INCREMENT PRIMARY KEY,
    data_venda DATETIME NOT NULL,
    cliente VARCHAR(100) DEFAULT 'Cliente Padrão',
    forma_pagamento VARCHAR(30) DEFAULT 'Dinheiro',
    total DECIMAL(10,2) NOT NULL
);

CREATE TABLE IF NOT EXISTS itens_venda (
    id INT AUTO_INCREMENT PRIMARY KEY,
    venda_id INT NOT NULL,
    produto_id INT NOT NULL,
    quantidade INT NOT NULL,
    preco_unitario DECIMAL(10,2) NOT NULL,
    subtotal DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (venda_id) REFERENCES vendas(id),
    FOREIGN KEY (produto_id) REFERENCES produtos(id)
);

INSERT INTO produtos (codigo, nome, categoria, estoque, preco, data_cadastro)
SELECT '000123', 'Arroz Branco 5kg', 'Alimentos', 50, 23.90, CURDATE()
WHERE NOT EXISTS (SELECT 1 FROM produtos WHERE codigo = '000123');

INSERT INTO produtos (codigo, nome, categoria, estoque, preco, data_cadastro)
SELECT '000124', 'Feijão Carioca 1kg', 'Alimentos', 80, 7.50, CURDATE()
WHERE NOT EXISTS (SELECT 1 FROM produtos WHERE codigo = '000124');

INSERT INTO produtos (codigo, nome, categoria, estoque, preco, data_cadastro)
SELECT '000126', 'Óleo de Soja 900ml', 'Alimentos', 60, 6.89, CURDATE()
WHERE NOT EXISTS (SELECT 1 FROM produtos WHERE codigo = '000126');

INSERT INTO produtos (codigo, nome, categoria, estoque, preco, data_cadastro)
SELECT '000127', 'Café Torrado 500g', 'Bebidas', 40, 15.90, CURDATE()
WHERE NOT EXISTS (SELECT 1 FROM produtos WHERE codigo = '000127');
