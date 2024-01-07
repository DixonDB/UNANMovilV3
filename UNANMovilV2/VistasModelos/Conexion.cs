using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace UNANMovilV2.VistasModelos
{
    public class Conexion
    {
        public static string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "connection.txt");
        public static string text = File.ReadAllText(ruta);
        public static string conexion = text;
        //public static string conexion = "Data source =192.168.159.84; ;Initial Catalog=UNAN1;Integrated Security=False;User Id=FelixL;Password=1316";

        public static SqlConnection conectar = new SqlConnection(conexion);

        public static void Abrir()
        {
            if (conectar.State == ConnectionState.Closed)
            {
                conectar.Open();
            }
        }

        public static void Cerrar()
        {
            if (conectar.State == ConnectionState.Open)
            {
                conectar.Close();
            }
        }
    }
}
