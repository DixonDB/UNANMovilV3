using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UNANMovilV2.Modelos;
using UNANMovilV2.VistasModelos;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UNANMovilV2.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        MProfes mprofes = new MProfes();
        NProfes nprofes = new NProfes();
        public static string nombreprofe;
        public static int idprofesor;
        public static string correo;
        public static string Tusuario;
        public static int INSS;
        int IdUser = 0;
        public Login()
        {
            InitializeComponent();;
        }

        private async void ProbarConexion()
        {
            try
            {
                var funcion = new DProfesor();
                funcion.ComprobarConexion(ref IdUser);
            }
            catch (Exception)
            {
                IdUser = 0;
            }
            if (IdUser > 0)
            {
                await Logueo();
            }
            else
            {
                await DisplayAlert("Sin conexión", "No se logró conectar al servidor", "OK");
                Application.Current.MainPage = new ConexionIP();
            }
        }

        private void BtnIniciarSesion_Clicked(object sender, EventArgs e)
        {
            ProbarConexion();
        }

        private async Task Logueo()
        {
            if (string.IsNullOrEmpty(txtINSS.Text) || string.IsNullOrEmpty(txtPass.Text))
            {
                await DisplayAlert("ERROR", "Los campos estan vacios", "OK");
            }
            else
            {
                DataTable dt = new DataTable();
                mprofes.INSS = int.Parse(txtINSS.Text);
                mprofes.Password = Encrip.Encriptar(Encrip.Encriptar(txtPass.Text));
                try
                {
                    dt = nprofes.Nprofe(mprofes);
                    if (dt.Rows.Count > 0)
                    {
                        idprofesor = Convert.ToInt32(dt.Rows[0][0]);
                        nombreprofe = dt.Rows[0][1].ToString();
                        //Icono = (byte[])dt.Rows[0][3];
                        correo = dt.Rows[0][5].ToString();
                        Tusuario = dt.Rows[0][6].ToString();
                        INSS = int.Parse(dt.Rows[0][2].ToString());
                        await Navigation.PushAsync(new Asistencia());
                        txtINSS.Text = "";
                        txtPass.Text = "";
                    }
                    else
                    {
                        await DisplayAlert("ERROR", "INSS o Contraseña Incorrectos", "Ok");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("ERROR", ex.Message, "OK");
                }
            }
        }

        private async void BtnRestablecer_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RestablecerPass());
        }
    }
}