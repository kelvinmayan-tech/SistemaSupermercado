using SistemaSupermercado_AV1.Models;
using System.Drawing;

namespace SistemaSupermercado_AV1.Forms;

public class FrmMenu : Form
{
    Panel pnlCard = new();

    public FrmMenu()
    {
        Text = "Supermercado - Sistema de Gestão";
        Size = new Size(480, 560);
        MinimumSize = new Size(400, 480);
        StartPosition = FormStartPosition.CenterScreen;

        AutoScaleMode = AutoScaleMode.None;

        BackColor = Tema.FundoApp;
        FormBorderStyle = FormBorderStyle.Sizable;

        CriarTela();

        Resize += (s, e) => AjustarLayout();
    }

    private void CriarTela()
    {
        Panel topo = new Panel
        {
            BackColor = Tema.AzulHeader,
            Dock = DockStyle.Top,
            Height = 160,
            Name = "topo"
        };

        Panel faixaDeco = new Panel
        {
            BackColor = Tema.AzulSecundario,
            Dock = DockStyle.Bottom,
            Height = 4
        };

        Label lblTitulo = new Label
        {
            Text = "Supermercado",
            Font = new Font("Segoe UI", 22, FontStyle.Bold),
            ForeColor = Tema.Branco,
            Dock = DockStyle.None,
            Location = new Point(0, 58),
            Size = new Size(480, 44),
            TextAlign = ContentAlignment.MiddleCenter,
            Name = "lblTitulo"
        };
        Label lblSub = new Label
        {
            Text = "Sistema de Gestão",
            Font = new Font("Segoe UI", 10),
            ForeColor = Tema.AzulClaro,
            Location = new Point(0, 102),
            Size = new Size(480, 28),
            TextAlign = ContentAlignment.MiddleCenter,
            Name = "lblSub"
        };

        Label lblIcone = new Label
        {
            Text = "🛒",
            Font = new Font("Segoe UI Emoji", 22),
            ForeColor = Tema.Branco,
            Location = new Point(0, 10),
            Size = new Size(480, 40),
            TextAlign = ContentAlignment.MiddleCenter,
            Name = "lblIcone"
        };

        topo.Controls.AddRange(new Control[] { faixaDeco, lblIcone, lblTitulo, lblSub });

        pnlCard = Tema.CriarCard(60, 180, 360, 296);
        pnlCard.Name = "pnlCard";

        Label lblMenu = new Label
        {
            Text = "MENU PRINCIPAL",
            Font = new Font("Segoe UI", 8, FontStyle.Bold),
            ForeColor = Tema.CinzaSecundario,
            Location = new Point(16, 16),
            AutoSize = true
        };

        Panel sep = new Panel { BackColor = Tema.CinzaBorda, Location = new Point(16, 36), Size = new Size(328, 1), Name = "sep" };

        Button btnProdutos = new Button
        {
            Text = "  Cadastro de Produtos",
            Size = new Size(328, 52),
            Location = new Point(16, 50),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(14, 0, 0, 0),
            Name = "btnProdutos"
        };
        Tema.EstilizarBotao(btnProdutos);
        btnProdutos.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        btnProdutos.Click += (s, e) => new FrmProdutos().ShowDialog();

        Button btnCompras = new Button
        {
            Text = "  Nova Compra",
            Size = new Size(328, 52),
            Location = new Point(16, 114),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(14, 0, 0, 0),
            Name = "btnCompras"
        };
        Tema.EstilizarBotao(btnCompras);
        btnCompras.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        btnCompras.Click += (s, e) => new FrmCompras().ShowDialog();

        Panel sep2 = new Panel { BackColor = Tema.CinzaBorda, Location = new Point(16, 182), Size = new Size(328, 1), Name = "sep2" };

        Button btnSair = new Button { Text = "Sair do sistema", Size = new Size(328, 40), Location = new Point(16, 196), Name = "btnSair" };
        Tema.EstilizarBotao(btnSair, "branco");
        btnSair.Click += (s, e) => Close();

        pnlCard.Controls.AddRange(new Control[] { lblMenu, sep, btnProdutos, btnCompras, sep2, btnSair });

        Panel rodape = new Panel { BackColor = Tema.AzulHeader, Dock = DockStyle.Bottom, Height = 32 };
        Label lblRodape = new Label
        {
            Text = "© 2026 Sistema de Supermercado",
            Font = Tema.FontePequena,
            ForeColor = Tema.AzulClaro,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter
        };
        rodape.Controls.Add(lblRodape);

        Controls.Add(topo);
        Controls.Add(pnlCard);
        Controls.Add(rodape);

        AjustarLayout();
    }

    private void AjustarLayout()
    {
        int w = ClientSize.Width;

        foreach (Control c in Controls)
            if (c.Name == "topo")
                foreach (Control inner in c.Controls)
                    if (inner.Name is "lblIcone" or "lblTitulo" or "lblSub")
                        inner.Width = w;

        if (Controls["pnlCard"] is Panel card)
        {
            int cw = Math.Min(380, w - 60);
            card.Width = cw;
            card.Location = new Point((w - cw) / 2, 180);
            int bw = cw - 32;
            foreach (Control c in card.Controls)
            {
                if (c.Name is "btnProdutos" or "btnCompras" or "btnSair") c.Width = bw;
                if (c.Name is "sep" or "sep2") c.Width = bw;
            }
        }
    }
}