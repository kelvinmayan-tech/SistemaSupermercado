using SistemaSupermercado_AV1.Forms;

namespace SistemaSupermercado_AV1;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        Application.Run(new FrmMenu());
    }
}