using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using UNANMovilV2.VistasModelos;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UNANMovilV2.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConexionIP : ContentPage
    {
        public ConexionIP()
        {
            InitializeComponent();
        }
        string ruta;
        string cadena_de_conxion;
        string parte1 = "Data source =";
        string parte2;
        string indicador_de_conexion;
        private void BtnConectar_Clicked(object sender, EventArgs e)
        {
            parte2= ";Initial Catalog=" + TXTbasededatos.Text + ";Integrated Security=False;User Id=" + txtUsuario.Text + ";Password=" + txtPassword.Text + "";
            if (validar())
            {
                probarconexion();
                if (indicador_de_conexion=="HAY CONEXIÓN")
                {
                    crear_archivo();
                    System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                }
                else
                {
                    DisplayAlert("Sin conexión", "No se logró conectar al servidor", "OK");
                }
            }
            else
            {
                DisplayAlert("ERROR", "Complete lo campos", "OK");
            }
        }

        private void probarconexion()
        {
            cadena_de_conxion = parte1 + Txtconexion.Text + parte2;
            DataTable dt = new DataTable();
            SqlDataAdapter da = null;
            try
            {
                SqlConnection conexionmanual = new SqlConnection(cadena_de_conxion);
                conexionmanual.Open();
                da = new SqlDataAdapter("Select INSS from Profesores", conexionmanual);
                da.Fill(dt);
                indicador_de_conexion = "HAY CONEXIÓN";
                DisplayAlert("Listo", "Conectado al servidor vuelva a abrir la app", "OK");
            }
            catch (Exception)
            {
                indicador_de_conexion = "NO HAY CONEXIÓN";
            }
        }

        private bool validar()
        {
            return !(txtPassword.Text == "" || txtUsuario.Text == "" || TXTbasededatos.Text == "" || Txtconexion.Text == "");
        }

        private void crear_archivo()
        {
            ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "connection.txt");
            FileInfo fi = new FileInfo(ruta);
            StreamWriter sw;
            try
            {
                
                parte2= ";Initial Catalog=" + TXTbasededatos.Text + ";Integrated Security=False;User Id=" + txtUsuario.Text + ";Password=" + txtPassword.Text + "";
                if (File.Exists(ruta)==false)
                {
                    sw = File.CreateText(ruta);
                    sw.WriteLine(parte1 + Txtconexion.Text + parte2);
                    sw.Flush();
                    sw.Close();
                }
                else if (File.Exists(ruta)==true)
                {
                    File.Delete(ruta);
                    sw = File.CreateText(ruta);
                    sw.WriteLine(parte1 + Txtconexion.Text + parte2);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}