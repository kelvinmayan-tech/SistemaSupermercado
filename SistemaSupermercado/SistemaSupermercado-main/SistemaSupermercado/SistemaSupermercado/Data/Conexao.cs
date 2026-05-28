using MySqlConnector;

namespace SistemaSupermercado_AV1.Data;

public static class Conexao
{
    private static readonly string StringConexao =
        "Server=localhost;Port=3306;Database=supermercado;Uid=root;Pwd=;";

    public static MySqlConnection Conectar()
    {
        return new MySqlConnection(StringConexao);
    }
}
