using System;
using UNANMovilV2.VistasModelos;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UNANMovilV2.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetalleAsistencia : ContentPage
    {
        DAsistencia Asis = new DAsistencia();
        int ID;
        string Fecha;
        int bl;
        public DetalleAsistencia(int IdAsis, string fecha,int bloque)
        {
            InitializeComponent();
            ID = IdAsis;
            Fecha = fecha;
            bl=bloque;
            Mostrar();
        }
        private void Mostrar()
        {
            var data = Asis.MostrarDetalleAsistencia(ID);
            lstAsis.ItemsSource = data;
            LblFecha.Text = Fecha.ToString();
            LblBloque.Text= bl.ToString();

        }
        private async void Editar(int IdAsis, string fecha,int bloque)
        {
            await Navigation.PushAsync(new EditarAsistencia(IdAsis, fecha, bloque));
        }

        private void BtnEditar_Clicked(object sender, EventArgs e)
        {
            var Fecha = LblFecha.Text;
            var Bloque = int.Parse(LblBloque.Text);
            Editar(ID, Fecha,Bloque);
        }

        protected override void OnAppearing()
        {
            Mostrar();
        }
    }
}