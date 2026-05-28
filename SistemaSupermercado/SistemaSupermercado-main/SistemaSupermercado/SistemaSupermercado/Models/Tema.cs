using System.Drawing;
using System.Windows.Forms;

namespace SistemaSupermercado_AV1.Models
{
    public static class Tema
    {
        // CORES
        // =========================
        public static Color AzulPrimario =
            ColorTranslator.FromHtml("#1565C0");

        public static Color AzulSecundario =
            ColorTranslator.FromHtml("#1E88E5");

        public static Color AzulSuave =
            ColorTranslator.FromHtml("#E3F2FD");

        public static Color AzulClaro =
            ColorTranslator.FromHtml("#BBDEFB");

        public static Color AzulHeader =
            ColorTranslator.FromHtml("#0D47A1");

        public static Color AzulMenta =
            ColorTranslator.FromHtml("#EEF4FB");

        public static Color Vermelho =
            ColorTranslator.FromHtml("#E53935");

        public static Color Branco = Color.White;

        public static Color FundoApp =
            ColorTranslator.FromHtml("#F4F7FB");

        public static Color CinzaTexto =
            ColorTranslator.FromHtml("#212121");

        public static Color CinzaSecundario =
            ColorTranslator.FromHtml("#757575");

        public static Color CinzaBorda =
            ColorTranslator.FromHtml("#BBDEFB");

        // FONTES
        // =========================
        public static Font FonteTitulo =
            new Font("Segoe UI Emoji", 20, FontStyle.Bold);

        public static Font FonteSubtitulo =
            new Font("Segoe UI", 10, FontStyle.Bold);

        public static Font FontePadrao =
            new Font("Segoe UI", 10);

        public static Font FonteBotao =
            new Font("Segoe UI", 9, FontStyle.Bold);

        public static Font FonteGrid =
            new Font("Segoe UI", 9);

        public static Font FontePequena =
            new Font("Segoe UI", 8);

        // BOTÕES
        // =========================
        public static void EstilizarBotao(Button btn, string cor = "azul")
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;

            btn.Font = FonteBotao;
            btn.Cursor = Cursors.Hand;

            btn.Height = 38;

            switch (cor)
            {
                case "vermelho":
                    btn.BackColor = Vermelho;
                    btn.ForeColor = Branco;
                    break;

                case "branco":
                    btn.BackColor = Branco;
                    btn.ForeColor = AzulPrimario;

                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = AzulClaro;
                    break;

                case "cinza":
                    btn.BackColor =
                        ColorTranslator.FromHtml("#EEEEEE");

                    btn.ForeColor = CinzaTexto;

                    btn.FlatAppearance.BorderSize = 1;

                    btn.FlatAppearance.BorderColor =
                        ColorTranslator.FromHtml("#BDBDBD");
                    break;

                default:
                    btn.BackColor = AzulPrimario;
                    btn.ForeColor = Branco;
                    break;
            }
        }

        // CARD
        // =========================
        public static Panel CriarCard(int x, int y, int w, int h)
        {
            var p = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(w, h),
                BackColor = Branco,
                Padding = new Padding(16)
            };

            p.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(CinzaBorda, 1))
                {
                    e.Graphics.DrawRectangle(
                        pen,
                        0,
                        0,
                        p.Width - 1,
                        p.Height - 1
                    );
                }
            };

            return p;
        }

        // HEADER
        // =========================
        public static Panel CriarHeader(string titulo)
        {
            var header = new Panel
            {
                BackColor = AzulHeader,
                Dock = DockStyle.Top,
                Height = 84
            };

            Panel faixa = new Panel
            {
                BackColor = AzulSecundario,
                Dock = DockStyle.Bottom,
                Height = 4
            };

            Label lblTitulo = new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Branco,
                AutoSize = true,
                Location = new Point(20, 18)
            };

            header.Controls.Add(faixa);
            header.Controls.Add(lblTitulo);

            return header;
        }
        public static void EstilizarGrid(DataGridView grid)
        {
            grid.BackgroundColor = Branco;

            grid.BorderStyle = BorderStyle.None;

            grid.GridColor =
                ColorTranslator.FromHtml("#F0F0F0");

            grid.CellBorderStyle =
                DataGridViewCellBorderStyle.SingleHorizontal;

            grid.ColumnHeadersDefaultCellStyle.BackColor =
                AzulHeader;

            grid.ColumnHeadersDefaultCellStyle.ForeColor =
                Branco;

            grid.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9, FontStyle.Bold);

            grid.ColumnHeadersDefaultCellStyle.Padding =
                new Padding(10, 0, 0, 0);

            grid.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleLeft;

            grid.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.None;

            grid.ColumnHeadersHeight = 42;

            grid.DefaultCellStyle.Font = FonteGrid;

            grid.DefaultCellStyle.ForeColor =
                CinzaTexto;

            grid.DefaultCellStyle.Padding =
                new Padding(10, 0, 0, 0);

            grid.DefaultCellStyle.SelectionBackColor =
                AzulClaro;

            grid.DefaultCellStyle.SelectionForeColor =
                CinzaTexto;

            grid.DefaultCellStyle.BackColor =
                Branco;

            grid.AlternatingRowsDefaultCellStyle.BackColor =
                AzulMenta;

            grid.RowHeadersVisible = false;

            grid.EnableHeadersVisualStyles = false;

            grid.RowTemplate.Height = 38;

            grid.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            grid.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            grid.MultiSelect = false;
        }
    }
}