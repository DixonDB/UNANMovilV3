using System;
using System.Threading.Tasks;
using UNANMovilV2.Funciones;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UNANMovilV2.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RestablecerPass : ContentPage
    {
        private int conteo;
        string contraseñaGenerada;
        public RestablecerPass()
        {
            InitializeComponent();
            conteo = 0;
        }

        private void btnCerrar_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void BtnEnviar_Clicked(object sender, EventArgs e)
        {
            lblMsj.Text = "";
            GenerarPass();
            await Enviar();
            conteo = 0;
        }

        private Task Enviar()
        {
            var result = RecuperarPass(txtUser.Text);
            lblMsj.Text = result;
            return Task.Delay(100);
        }

        public string RecuperarPass(string pass)
        {
            return new Recuperacion().RecoverPassword2(pass, contraseñaGenerada);
        }

        private void GenerarPass()
        {
            contraseñaGenerada = Validaciones.GenerarContraseñaAleatoria();
        }
    }
}