using Microsoft.Data.Sqlite;

namespace SoftCep.Infra.Data
{
    public static class DbInitializer
    {
        public static void Initialize(string dbPath, string seedPath)
        {
            // Se o banco já existir, não precisa recriar
            if (File.Exists(dbPath))
                return;

            Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            // Cria tabela
            var createTableSql = @"
                CREATE TABLE IF NOT EXISTS Enderecos (
                    Id TEXT PRIMARY KEY,
                    Cep TEXT NOT NULL,
                    Logradouro TEXT,
                    Bairro TEXT,
                    Cidade TEXT,
                    Uf TEXT
                );
            ";
            using var cmd = connection.CreateCommand();
            cmd.CommandText = createTableSql;
            cmd.ExecuteNonQuery();

            // Executa script de seed
            if (File.Exists(seedPath))
            {
                var sql = File.ReadAllText(seedPath, System.Text.Encoding.UTF8);
                using var seedCmd = connection.CreateCommand();
                seedCmd.CommandText = sql;
                seedCmd.ExecuteNonQuery();
            }
        }
    }
}
