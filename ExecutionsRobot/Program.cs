using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading;

namespace ExecutionsRobot
{
    class Program
    {
        private static readonly string connectionString = "Data Source=ALEXANDRE\\SQLEXPRESS;Initial Catalog=CadastroDB;User ID=prd_1;Password=Cebol@24";
        private static readonly string logFile = "C:\\Logs\\ExecutionsRobot.log";  // Caminho do log
        private static readonly CancellationTokenSource cts = new CancellationTokenSource();

        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando o robô de execução... Pressione Ctrl+C para parar.");

            // Capturar Ctrl+C para parada segura
            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
                Console.WriteLine("Encerrando o robô de execução...");
            };

            // Rodar enquanto não for cancelado
            while (!cts.Token.IsCancellationRequested)
            {
                InvocarProcedure();
                Thread.Sleep(30000);  // Espera 30 segundos entre as execuções
            }
        }

        static void InvocarProcedure()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("PRY9_VERIFICAPENDENCIA", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }

                    Log("Procedure executada com sucesso.");
                }
            }
            catch (Exception ex)
            {
                Log($"Erro ao executar a procedure: {ex.Message}");
            }
        }

        static void Log(string message)
        {
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            Console.WriteLine(logMessage);
            File.AppendAllText(logFile, logMessage + Environment.NewLine);
        }
    }
}
