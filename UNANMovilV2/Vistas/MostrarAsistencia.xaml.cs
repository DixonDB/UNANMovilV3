using System;
using UNANMovilV2.Modelos;
using UNANMovilV2.VistasModelos;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UNANMovilV2.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MostrarAsistencia : ContentPage
    {
        DateTime fecha;
        public MostrarAsistencia()
        {
            InitializeComponent();
            VerAsistencia();
        }


        private async void btnAdd_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Asistencia());
        }

        private void VerAsistencia()
        {
            int INSS = Login.INSS;

            var funcion = new DAsistencia();
            var data = funcion.MostrarAsistencia(INSS);
            lstAsis.ItemsSource = data;
        }
        private void BuscarPorFecha()
        {
            int INSS = Login.INSS;

            var funcion = new DAsistencia();
            var data = funcion.BuscarAsistencia(INSS,fecha);
            lstAsis.ItemsSource = data;
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                bool respuesta = await DisplayAlert("Cerrar Sesión", "¿Desea cerrar la sesión?", "Sí", "No");

                if (respuesta) // Si el usuario presionó "Sí"
                {
                    // Realiza la navegación a la página de inicio de sesión (LoginPage)
                    App.Current.MainPage = new NavigationPage(new Login());
                }
                else
                {
                    // El usuario eligió "No", no se realiza ninguna acción adicional
                }
            });

            return true; // Evita que la página actual aparezca en la pila de navegación
        }

        private void BtnAP_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Avance());
        }

        private async void BtnSalir_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new Login());
            bool respuesta = await DisplayAlert("Cerrar Sesión", "¿Desea cerrar la sesión?", "Sí", "No");

            if (respuesta) // Si el usuario presionó "Sí"
            {
                // Realiza la navegación a la página de inicio de sesión (LoginPage)
                App.Current.MainPage = new NavigationPage(new Login());
            }
            else
            {
                // El usuario eligió "No", no se realiza ninguna acción adicional
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var stackLayout = (StackLayout)sender;
            if (stackLayout.BindingContext is LAsistencia Asis)
            {
                // Puedes acceder a las propiedades de asignatura y hacer lo que necesites
                var IdAsis = Asis.IdAsistencia;
                var Fecha = Asis.Fecha;
                var Bloque = Asis.Bloques;
                MostrarDetalle(IdAsis,Fecha,Bloque);
            }
        }

        private void MostrarDetalle(int idAsis,string fecha, int bloque)
        {
            Navigation.PushAsync(new DetalleAsistencia(idAsis,fecha,bloque));
        }

        private void DpFecha_DateSelected(object sender, DateChangedEventArgs e)
        {

            fecha = e.NewDate;
            BuscarPorFecha();
        }
    }
}