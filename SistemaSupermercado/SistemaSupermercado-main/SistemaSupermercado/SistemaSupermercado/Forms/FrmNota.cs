using MySqlConnector;
using SistemaSupermercado_AV1.Data;
using SistemaSupermercado_AV1.Models;
using System.Drawing;
using System.Text;

namespace SistemaSupermercado_AV1.Forms;

public class FrmNota : Form
{
    TextBox txtNota = new();
    int vendaId;

    public FrmNota(int idVenda)
    {
        vendaId = idVenda;
        Text = "Nota de Compra";
        Size = new Size(580, 680);
        MinimumSize = new Size(480, 520);
        StartPosition = FormStartPosition.CenterScreen;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Tema.FundoApp;
        FormBorderStyle = FormBorderStyle.Sizable;

        Controls.Add(Tema.CriarHeader("Nota de Compra"));

        Panel pnlNota = new Panel
        {
            BackColor = Tema.Branco,
            Location = new Point(16, 90),
            Name = "pnlNota"
        };
        pnlNota.Paint += (s, e) =>
            e.Graphics.DrawRectangle(new Pen(Tema.CinzaBorda, 1), 0, 0, pnlNota.Width - 1, pnlNota.Height - 1);

        txtNota.Multiline = true; txtNota.ScrollBars = ScrollBars.Vertical;
        txtNota.Font = new Font("Consolas", 10); txtNota.ReadOnly = true;
        txtNota.BorderStyle = BorderStyle.None; txtNota.BackColor = Tema.Branco;
        txtNota.Dock = DockStyle.Fill;
        pnlNota.Controls.Add(txtNota);

        Panel pnlBotoes = new Panel
        {
            BackColor = Tema.AzulHeader,
            Dock = DockStyle.Bottom,
            Height = 60
        };

        Button btnSalvar = new Button { Text = "💾  Salvar TXT", Size = new Size(150, 36), Location = new Point(16, 12) };
        Tema.EstilizarBotao(btnSalvar, "branco");
        btnSalvar.ForeColor = Tema.AzulHeader;
        btnSalvar.Click += BtnSalvar_Click;

        Button btnFechar = new Button { Text = "Fechar", Size = new Size(120, 36), Name = "btnFechar" };
        Tema.EstilizarBotao(btnFechar);
        btnFechar.Click += (s, e) => Close();

        pnlBotoes.Controls.AddRange(new Control[] { btnSalvar, btnFechar });
        pnlBotoes.Resize += (s, e) =>
        {
            if (pnlBotoes.Controls["btnFechar"] is Button b)
                b.Location = new Point(pnlBotoes.Width - 136, 12);
        };

        Controls.Add(pnlNota);
        Controls.Add(pnlBotoes);

        Resize += (s, e) =>
        {
            if (Controls["pnlNota"] is Panel p)
                p.Size = new Size(ClientSize.Width - 32, ClientSize.Height - 162);
        };
        pnlNota.Size = new Size(ClientSize.Width - 32, ClientSize.Height - 162);

        GerarNota();
    }

    private void GerarNota()
    {
        using var conn = Conexao.Conectar(); conn.Open();
        var nota = new StringBuilder();
        nota.AppendLine("╔══════════════════════════════════════╗");
        nota.AppendLine("║       SUPERMERCADO EXEMPLO           ║");
        nota.AppendLine("║   Rua das Flores, 123 - Centro       ║");
        nota.AppendLine("║   CNPJ: 12.345.678/0001-90           ║");
        nota.AppendLine("╚══════════════════════════════════════╝");
        nota.AppendLine();

        var cmdV = new MySqlCommand("SELECT id, data_venda, total FROM vendas WHERE id=@id", conn);
        cmdV.Parameters.AddWithValue("@id", vendaId);
        using (var r = cmdV.ExecuteReader())
        {
            if (r.Read())
            {
                nota.AppendLine($"  Nº da Nota : {r["id"]}");
                nota.AppendLine($"  Data       : {Convert.ToDateTime(r["data_venda"]):dd/MM/yyyy HH:mm}");
            }
        }

        nota.AppendLine("  ──────────────────────────────────────");
        nota.AppendLine("  ITENS DA COMPRA");
        nota.AppendLine("  ──────────────────────────────────────");

        var cmdI = new MySqlCommand(@"SELECT p.nome, i.quantidade, i.preco_unitario, i.subtotal
            FROM itens_venda i INNER JOIN produtos p ON p.id = i.produto_id
            WHERE i.venda_id=@id ORDER BY p.nome", conn);
        cmdI.Parameters.AddWithValue("@id", vendaId);

        decimal total = 0;
        using (var r = cmdI.ExecuteReader())
        {
            while (r.Read())
            {
                string nome = r["nome"].ToString()!.PadRight(22);
                string qtd = ("x" + r["quantidade"]).PadLeft(4);
                string unit = ("R$ " + Convert.ToDecimal(r["preco_unitario"]).ToString("N2")).PadLeft(10);
                string sub = ("R$ " + Convert.ToDecimal(r["subtotal"]).ToString("N2")).PadLeft(10);
                nota.AppendLine($"  {nome}{qtd}  {unit}  {sub}");
                total += Convert.ToDecimal(r["subtotal"]);
            }
        }

        nota.AppendLine("  ──────────────────────────────────────");
        nota.AppendLine($"  TOTAL DA COMPRA       R$ {total:N2}");
        nota.AppendLine("  ──────────────────────────────────────");
        nota.AppendLine();
        nota.AppendLine("      Obrigado e volte sempre!");

        txtNota.Text = nota.ToString();
    }

    private void BtnSalvar_Click(object? sender, EventArgs e)
    {
        string arquivo = "nota_" + vendaId + ".txt";
        File.WriteAllText(arquivo, txtNota.Text);
        MessageBox.Show($"Nota salva como {arquivo}", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}