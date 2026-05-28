using MySqlConnector;
using SistemaSupermercado_AV1.Data;
using SistemaSupermercado_AV1.Models;
using System.Data;
using System.Drawing;

namespace SistemaSupermercado_AV1.Forms;

public class FrmProdutos : Form
{
    TextBox txtCodigo = new();
    TextBox txtNome = new();
    ComboBox cbCategoria = new();
    TextBox txtEstoque = new();
    TextBox txtPreco = new();
    TextBox txtBusca = new();
    DataGridView dgvProdutos = new();
    Panel pnlForm = new();
    Panel pnlGrid = new();
    DataTable tabelaCompleta = new();

    public FrmProdutos()
    {
        Text = "Cadastro de Produtos";
        Size = new Size(1020, 700);
        MinimumSize = new Size(800, 580);
        StartPosition = FormStartPosition.CenterScreen;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Tema.FundoApp;
        FormBorderStyle = FormBorderStyle.Sizable;

        CriarTela();
        ListarProdutos();
        Resize += (s, e) => AjustarLayout();
    }

    private void CriarTela()
    {
        Controls.Add(Tema.CriarHeader("Cadastro de Produtos"));

        pnlForm = Tema.CriarCard(16, 90, 0, 210);
        pnlForm.Name = "pnlForm";

        Label lblSec = new Label { Text = "DADOS DO PRODUTO", Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Tema.CinzaSecundario, Location = new Point(16, 14), AutoSize = true };
        Panel sepForm = new Panel { BackColor = Tema.CinzaBorda, Location = new Point(16, 32), Height = 1, Name = "sepForm" };

        Label lCod = new Label { Text = "Código *", Font = Tema.FontePequena, ForeColor = Tema.CinzaSecundario, Location = new Point(16, 46), AutoSize = true };
        txtCodigo.Location = new Point(16, 62); txtCodigo.Size = new Size(150, 28);
        txtCodigo.Font = Tema.FontePadrao; txtCodigo.BorderStyle = BorderStyle.FixedSingle; txtCodigo.BackColor = Tema.AzulMenta;

        Label lNome = new Label { Text = "Nome *", Font = Tema.FontePequena, ForeColor = Tema.CinzaSecundario, Location = new Point(180, 46), AutoSize = true };
        txtNome.Location = new Point(180, 62); txtNome.Size = new Size(300, 28);
        txtNome.Font = Tema.FontePadrao; txtNome.BorderStyle = BorderStyle.FixedSingle; txtNome.BackColor = Tema.AzulMenta;

        Label lCat = new Label { Text = "Categoria *", Font = Tema.FontePequena, ForeColor = Tema.CinzaSecundario, Location = new Point(496, 46), AutoSize = true };
        cbCategoria.Location = new Point(496, 62); cbCategoria.Size = new Size(190, 28);
        cbCategoria.Items.AddRange(new string[] { "Alimentos", "Bebidas", "Limpeza", "Higiene", "Hortifruti", "Frios", "Outros" });
        cbCategoria.FlatStyle = FlatStyle.Flat; cbCategoria.Font = Tema.FontePadrao; cbCategoria.BackColor = Tema.AzulMenta;

        Label lEst = new Label { Text = "Estoque *", Font = Tema.FontePequena, ForeColor = Tema.CinzaSecundario, Location = new Point(16, 106), AutoSize = true };
        txtEstoque.Location = new Point(16, 122); txtEstoque.Size = new Size(130, 28);
        txtEstoque.Font = Tema.FontePadrao; txtEstoque.BorderStyle = BorderStyle.FixedSingle; txtEstoque.BackColor = Tema.AzulMenta;

        Label lPreco = new Label { Text = "Preço * (ex: 12,99)", Font = Tema.FontePequena, ForeColor = Tema.CinzaSecundario, Location = new Point(162, 106), AutoSize = true };
        txtPreco.Location = new Point(162, 122); txtPreco.Size = new Size(150, 28);
        txtPreco.Font = Tema.FontePadrao; txtPreco.BorderStyle = BorderStyle.FixedSingle; txtPreco.BackColor = Tema.AzulMenta;

        Button btnCadastrar = new Button { Text = "✔  Cadastrar", Size = new Size(130, 34), Location = new Point(16, 164) };
        Tema.EstilizarBotao(btnCadastrar); btnCadastrar.Click += BtnCadastrar_Click;
        btnCadastrar.BackColor = ColorTranslator.FromHtml("#2E7D32");

        Button btnAtualizar = new Button { Text = "✎  Atualizar", Size = new Size(130, 34), Location = new Point(158, 164) };
        Tema.EstilizarBotao(btnAtualizar); btnAtualizar.Click += BtnAtualizar_Click;

        Button btnExcluir = new Button { Text = "✖  Excluir", Size = new Size(120, 34), Location = new Point(300, 164) };
        Tema.EstilizarBotao(btnExcluir, "vermelho"); btnExcluir.Click += BtnExcluir_Click;

        Button btnLimpar = new Button { Text = "↺  Limpar", Size = new Size(110, 34), Location = new Point(432, 164) };
        Tema.EstilizarBotao(btnLimpar, "cinza"); btnLimpar.Click += (s, e) => LimparCampos();

        pnlForm.Controls.AddRange(new Control[] { lblSec, sepForm, lCod, txtCodigo, lNome, txtNome, lCat, cbCategoria, lEst, txtEstoque, lPreco, txtPreco, btnCadastrar, btnAtualizar, btnExcluir, btnLimpar });

        pnlGrid = Tema.CriarCard(16, 318, 0, 0);
        pnlGrid.Name = "pnlGrid";

        Label lblLista = new Label { Text = "PRODUTOS CADASTRADOS", Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Tema.CinzaSecundario, Location = new Point(16, 14), AutoSize = true };
        Panel sepGrid = new Panel { BackColor = Tema.CinzaBorda, Location = new Point(16, 32), Height = 1, Name = "sepGrid" };

        Label lblBuscaTitulo = new Label
        {
            Text = "Buscar:",
            Font = Tema.FontePequena,
            ForeColor = Tema.CinzaSecundario,
            Location = new Point(16, 48),
            AutoSize = true
        };
        txtBusca.Location = new Point(62, 44);
        txtBusca.Size = new Size(300, 26);
        txtBusca.Font = Tema.FontePadrao;
        txtBusca.BorderStyle = BorderStyle.FixedSingle;
        txtBusca.BackColor = Tema.AzulMenta;
        txtBusca.PlaceholderText = "Nome ou código...";
        txtBusca.TextChanged += (s, e) => FiltrarProdutos();

        dgvProdutos.Location = new Point(10, 84);
        dgvProdutos.ReadOnly = true;
        dgvProdutos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvProdutos.CellClick += DgvProdutos_CellClick;
        Tema.EstilizarGrid(dgvProdutos);

        pnlGrid.Controls.AddRange(new Control[] { lblLista, sepGrid, lblBuscaTitulo, txtBusca, dgvProdutos });

        Controls.Add(pnlForm);
        Controls.Add(pnlGrid);

        AjustarLayout();
    }

    private void AjustarLayout()
    {
        int w = ClientSize.Width;
        int h = ClientSize.Height;
        int m = 16;
        int pw = w - m * 2;

        pnlForm.Size = new Size(pw, 210);
        pnlForm.Location = new Point(m, 90);
        foreach (Control c in pnlForm.Controls)
            if (c is Panel sep && sep.Height == 1) sep.Width = pw - 32;

        pnlGrid.Size = new Size(pw, h - 330);
        pnlGrid.Location = new Point(m, 318);
        foreach (Control c in pnlGrid.Controls)
        {
            if (c is Panel sep && sep.Height == 1) sep.Width = pw - 32;
            if (c == dgvProdutos) dgvProdutos.Size = new Size(pw - 20, pnlGrid.Height - 98);
        }
    }

    private void ListarProdutos()
    {
        using var conn = Conexao.Conectar(); conn.Open();
        string sql = "SELECT id AS ID, codigo AS Código, nome AS Nome, categoria AS Categoria, estoque AS Estoque, preco AS Preço, data_cadastro AS Cadastro FROM produtos ORDER BY id DESC";
        var da = new MySqlDataAdapter(sql, conn);
        tabelaCompleta = new DataTable();
        da.Fill(tabelaCompleta);
        dgvProdutos.DataSource = tabelaCompleta;
    }

    private void FiltrarProdutos()
    {
        string f = txtBusca.Text.Trim();
        if (f == "") { dgvProdutos.DataSource = tabelaCompleta; return; }
        var rows = tabelaCompleta.AsEnumerable()
            .Where(r => r["Nome"].ToString()!.Contains(f, StringComparison.OrdinalIgnoreCase)
                     || r["Código"].ToString()!.Contains(f, StringComparison.OrdinalIgnoreCase))
            .ToList();
        dgvProdutos.DataSource = rows.Count > 0 ? rows.CopyToDataTable() : tabelaCompleta.Clone();
    }

    private bool CamposValidos()
    {
        if (txtCodigo.Text == "" || txtNome.Text == "" || cbCategoria.Text == "" || txtEstoque.Text == "" || txtPreco.Text == "")
        { MessageBox.Show("Preencha todos os campos obrigatórios.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
        if (!decimal.TryParse(txtPreco.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out _))
        { MessageBox.Show("Preço inválido. Use vírgula. Ex: 12,99", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
        return true;
    }

    private void BtnCadastrar_Click(object? sender, EventArgs e)
    {
        if (!CamposValidos()) return;
        using var conn = Conexao.Conectar(); conn.Open();
        var cmd = new MySqlCommand("INSERT INTO produtos (codigo,nome,categoria,estoque,preco,data_cadastro) VALUES (@c,@n,@cat,@e,@p,CURDATE())", conn);
        cmd.Parameters.AddWithValue("@c", txtCodigo.Text); cmd.Parameters.AddWithValue("@n", txtNome.Text);
        cmd.Parameters.AddWithValue("@cat", cbCategoria.Text); cmd.Parameters.AddWithValue("@e", Convert.ToInt32(txtEstoque.Text));
        cmd.Parameters.AddWithValue("@p", decimal.Parse(txtPreco.Text, System.Globalization.CultureInfo.CurrentCulture));
        cmd.ExecuteNonQuery();
        MessageBox.Show("Produto cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        LimparCampos(); ListarProdutos();
    }

    private void BtnAtualizar_Click(object? sender, EventArgs e)
    {
        if (!CamposValidos()) return;
        using var conn = Conexao.Conectar(); conn.Open();
        var cmd = new MySqlCommand("UPDATE produtos SET nome=@n,categoria=@cat,estoque=@e,preco=@p WHERE codigo=@c", conn);
        cmd.Parameters.AddWithValue("@c", txtCodigo.Text); cmd.Parameters.AddWithValue("@n", txtNome.Text);
        cmd.Parameters.AddWithValue("@cat", cbCategoria.Text); cmd.Parameters.AddWithValue("@e", Convert.ToInt32(txtEstoque.Text));
        cmd.Parameters.AddWithValue("@p", decimal.Parse(txtPreco.Text, System.Globalization.CultureInfo.CurrentCulture));
        cmd.ExecuteNonQuery();
        MessageBox.Show("Produto atualizado!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        LimparCampos(); ListarProdutos();
    }

    private void BtnExcluir_Click(object? sender, EventArgs e)
    {
        if (txtCodigo.Text == "") { MessageBox.Show("Selecione um produto.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        if (MessageBox.Show("Confirma a exclusão?", "Excluir", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        using var conn = Conexao.Conectar(); conn.Open();
        var cmd = new MySqlCommand("DELETE FROM produtos WHERE codigo=@c", conn);
        cmd.Parameters.AddWithValue("@c", txtCodigo.Text);
        cmd.ExecuteNonQuery();
        MessageBox.Show("Produto excluído!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        LimparCampos(); ListarProdutos();
    }

    private void DgvProdutos_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var linha = dgvProdutos.Rows[e.RowIndex];
        txtCodigo.Text = linha.Cells["Código"].Value.ToString();
        txtNome.Text = linha.Cells["Nome"].Value.ToString();
        cbCategoria.Text = linha.Cells["Categoria"].Value.ToString();
        txtEstoque.Text = linha.Cells["Estoque"].Value.ToString();
        txtPreco.Text = linha.Cells["Preço"].Value.ToString();
    }

    private void LimparCampos()
    {
        txtCodigo.Clear(); txtNome.Clear(); cbCategoria.Text = ""; txtEstoque.Clear(); txtPreco.Clear();
    }
}