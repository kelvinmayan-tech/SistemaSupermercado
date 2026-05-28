using MySqlConnector;
using SistemaSupermercado_AV1.Data;
using SistemaSupermercado_AV1.Models;
using System.Data;
using System.Drawing;

namespace SistemaSupermercado_AV1.Forms;

public class FrmCompras : Form
{
    ComboBox cbProdutos = new();
    NumericUpDown numQtd = new();
    DataGridView dgvCarrinho = new();
    Label lblTotal = new();
    List<ItemCarrinho> carrinho = new();
    Panel pnlSelecao = new();
    Panel pnlCarrinho = new();

    public FrmCompras()
    {
        Text = "Nova Compra";
        Size = new Size(980, 680);
        MinimumSize = new Size(760, 540);
        StartPosition = FormStartPosition.CenterScreen;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Tema.FundoApp;
        FormBorderStyle = FormBorderStyle.Sizable;

        CriarTela();
        CarregarProdutos();
        AtualizarCarrinho();
        Resize += (s, e) => AjustarLayout();
    }

    private void CriarTela()
    {
        Controls.Add(Tema.CriarHeader("Nova Compra"));

        pnlSelecao = Tema.CriarCard(16, 90, 0, 104);
        pnlSelecao.Name = "pnlSelecao";

        Label lblSec = new Label { Text = "ADICIONAR PRODUTO", Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Tema.CinzaSecundario, Location = new Point(16, 14), AutoSize = true };
        Panel sepSel = new Panel { BackColor = Tema.CinzaBorda, Location = new Point(16, 32), Height = 1, Name = "sep" };

        Label lProd = new Label { Text = "Produto", Font = Tema.FontePequena, ForeColor = Tema.CinzaSecundario, Location = new Point(16, 44), AutoSize = true };
        cbProdutos.Location = new Point(16, 60); cbProdutos.Size = new Size(380, 28);
        cbProdutos.DropDownStyle = ComboBoxStyle.DropDownList;
        cbProdutos.FlatStyle = FlatStyle.Flat; cbProdutos.Font = Tema.FontePadrao; cbProdutos.BackColor = Tema.AzulMenta;

        Label lQtd = new Label { Text = "Quantidade", Font = Tema.FontePequena, ForeColor = Tema.CinzaSecundario, Location = new Point(414, 44), AutoSize = true };
        numQtd.Location = new Point(414, 60); numQtd.Size = new Size(90, 28);
        numQtd.Minimum = 1; numQtd.Maximum = 9999; numQtd.Value = 1; numQtd.Font = Tema.FontePadrao;

        Button btnAdd = new Button { Text = "✔  Adicionar", Size = new Size(130, 34), Location = new Point(522, 58) };
        Tema.EstilizarBotao(btnAdd); btnAdd.Click += BtnAdicionar_Click;

        Button btnRem = new Button { Text = "✖  Remover", Size = new Size(130, 34), Location = new Point(664, 58) };
        Tema.EstilizarBotao(btnRem, "vermelho"); btnRem.Click += BtnRemover_Click;

        pnlSelecao.Controls.AddRange(new Control[] { lblSec, sepSel, lProd, cbProdutos, lQtd, numQtd, btnAdd, btnRem });

        pnlCarrinho = Tema.CriarCard(16, 210, 0, 0);
        pnlCarrinho.Name = "pnlCarrinho";

        Label lblCart = new Label { Text = "ITENS DO CARRINHO", Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Tema.CinzaSecundario, Location = new Point(16, 14), AutoSize = true };
        Panel sepCart = new Panel { BackColor = Tema.CinzaBorda, Location = new Point(16, 32), Height = 1, Name = "sep" };

        dgvCarrinho.Location = new Point(10, 46);
        dgvCarrinho.ReadOnly = true;
        dgvCarrinho.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        Tema.EstilizarGrid(dgvCarrinho);

        pnlCarrinho.Controls.AddRange(new Control[] { lblCart, sepCart, dgvCarrinho });

        Panel pnlRodape = new Panel
        {
            BackColor = Tema.AzulHeader,
            Dock = DockStyle.Bottom,
            Height = 62,
            Name = "pnlRodape"
        };

        lblTotal.Text = "Total: R$ 0,00";
        lblTotal.Font = new Font("Segoe UI", 14, FontStyle.Bold);
        lblTotal.ForeColor = Tema.Branco;
        lblTotal.Location = new Point(20, 18);
        lblTotal.AutoSize = true;

        Button btnFinalizar = new Button { Text = "✔   Finalizar Compra", Size = new Size(200, 40), Name = "btnFinalizar" };
        Tema.EstilizarBotao(btnFinalizar);
        btnFinalizar.BackColor = Tema.AzulSecundario;
        btnFinalizar.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        btnFinalizar.Click += BtnFinalizar_Click;

        pnlRodape.Controls.AddRange(new Control[] { lblTotal, btnFinalizar });
        pnlRodape.Resize += (s, e) =>
        {
            if (pnlRodape.Controls["btnFinalizar"] is Button b)
                b.Location = new Point(pnlRodape.Width - 220, 11);
        };

        Controls.Add(pnlSelecao);
        Controls.Add(pnlCarrinho);
        Controls.Add(pnlRodape);

        AjustarLayout();
    }

    private void AjustarLayout()
    {
        int w = ClientSize.Width;
        int h = ClientSize.Height;
        int m = 16;
        int pw = w - m * 2;

        pnlSelecao.Size = new Size(pw, 104);
        pnlSelecao.Location = new Point(m, 90);
        foreach (Control c in pnlSelecao.Controls)
            if (c is Panel sep && sep.Height == 1) sep.Width = pw - 32;

        pnlCarrinho.Size = new Size(pw, h - 286);
        pnlCarrinho.Location = new Point(m, 210);
        foreach (Control c in pnlCarrinho.Controls)
        {
            if (c is Panel sep && sep.Height == 1) sep.Width = pw - 32;
            if (c == dgvCarrinho) dgvCarrinho.Size = new Size(pw - 20, pnlCarrinho.Height - 58);
        }
    }

    private void CarregarProdutos()
    {
        using var conn = Conexao.Conectar(); conn.Open();
        var da = new MySqlDataAdapter("SELECT id, nome, preco, estoque FROM produtos WHERE estoque > 0 ORDER BY nome", conn);
        DataTable t = new(); da.Fill(t);
        cbProdutos.DataSource = t;
        cbProdutos.DisplayMember = "nome";
        cbProdutos.ValueMember = "id";
    }

    private void BtnAdicionar_Click(object? sender, EventArgs e)
    {
        if (cbProdutos.SelectedValue == null) return;
        var prod = (DataRowView)cbProdutos.SelectedItem!;
        int id = Convert.ToInt32(prod["id"]);
        string nome = prod["nome"].ToString()!;
        decimal preco = Convert.ToDecimal(prod["preco"]);
        int estoque = Convert.ToInt32(prod["estoque"]);
        int qtd = Convert.ToInt32(numQtd.Value);

        if (qtd > estoque) { MessageBox.Show("Quantidade maior que o estoque.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

        var existente = carrinho.FirstOrDefault(x => x.ProdutoId == id);
        if (existente != null) existente.Quantidade += qtd;
        else carrinho.Add(new ItemCarrinho { ProdutoId = id, Produto = nome, Quantidade = qtd, PrecoUnitario = preco });

        AtualizarCarrinho();
    }

    private void BtnRemover_Click(object? sender, EventArgs e)
    {
        if (dgvCarrinho.CurrentRow == null || dgvCarrinho.CurrentRow.Index < 0) return;
        carrinho.RemoveAt(dgvCarrinho.CurrentRow.Index);
        AtualizarCarrinho();
    }

    private void AtualizarCarrinho()
    {
        dgvCarrinho.DataSource = null;
        dgvCarrinho.DataSource = carrinho.Select(x => new
        {
            Produto = x.Produto,
            Quantidade = x.Quantidade,
            Unitário = "R$ " + x.PrecoUnitario.ToString("N2"),
            Total = "R$ " + x.Subtotal.ToString("N2")
        }).ToList();
        lblTotal.Text = "Total: R$ " + carrinho.Sum(x => x.Subtotal).ToString("N2");
    }

    private void BtnFinalizar_Click(object? sender, EventArgs e)
    {
        if (carrinho.Count == 0) { MessageBox.Show("Adicione produtos ao carrinho.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

        using var conn = Conexao.Conectar(); conn.Open();
        using var tr = conn.BeginTransaction();
        try
        {
            decimal total = carrinho.Sum(x => x.Subtotal);
            var cmdV = new MySqlCommand("INSERT INTO vendas (data_venda,cliente,forma_pagamento,total) VALUES (NOW(),'Cliente Padrão','Dinheiro',@t); SELECT LAST_INSERT_ID();", conn, tr);
            cmdV.Parameters.AddWithValue("@t", total);
            int vendaId = Convert.ToInt32(cmdV.ExecuteScalar());

            foreach (var item in carrinho)
            {
                var cmdI = new MySqlCommand("INSERT INTO itens_venda (venda_id,produto_id,quantidade,preco_unitario,subtotal) VALUES (@v,@p,@q,@u,@s)", conn, tr);
                cmdI.Parameters.AddWithValue("@v", vendaId); cmdI.Parameters.AddWithValue("@p", item.ProdutoId);
                cmdI.Parameters.AddWithValue("@q", item.Quantidade); cmdI.Parameters.AddWithValue("@u", item.PrecoUnitario);
                cmdI.Parameters.AddWithValue("@s", item.Subtotal); cmdI.ExecuteNonQuery();

                var cmdE = new MySqlCommand("UPDATE produtos SET estoque=estoque-@q WHERE id=@p", conn, tr);
                cmdE.Parameters.AddWithValue("@q", item.Quantidade); cmdE.Parameters.AddWithValue("@p", item.ProdutoId);
                cmdE.ExecuteNonQuery();
            }

            tr.Commit();
            MessageBox.Show("Compra finalizada com sucesso! 🎉", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            new FrmNota(vendaId).ShowDialog();
            carrinho.Clear(); AtualizarCarrinho(); CarregarProdutos();
        }
        catch (Exception ex) { tr.Rollback(); MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }
}