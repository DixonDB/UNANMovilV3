using System;
using System.Collections.Generic;
using UNANMovilV2.Modelos;
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
        List<LAsistencia> datosList = new List<LAsistencia>();
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
        Button stackLayout;
        private async void BtnEstado_Clicked(object sender, EventArgs e)
        {
            stackLayout = (Button)sender;
            if (stackLayout.BindingContext is LAsistencia Asis)
            {
                var IdAsig = Asis.IdTema;
                var Contenido = Asis.Contenido;
                var Estado = Asis.Estado;
                int mujeres = Asis.Mujeres;
                int varones = Asis.Varones;
                if (Estado == "Proceso")
                {
                    LAsistencia LstAsis = new LAsistencia
                    {
                        IdTema = IdAsig,
                        Contenido = Contenido,
                        Estado = "Finalizado",
                        Mujeres = mujeres,
                        Varones = varones
                    };
                    stackLayout.IsEnabled = false;
                    datosList.Add(LstAsis);
                    Datos.ItemsSource = null;
                    Datos.ItemsSource = datosList;
                    DAsistencia funcion = new DAsistencia();
                    funcion.FinAsistencias(datosList, ID);
                }
                else
                {
                    LAsistencia LstAsis = new LAsistencia
                    {
                        IdTema = IdAsig,
                        Contenido = Contenido,
                        Estado = "Proceso",
                        Mujeres = mujeres,
                        Varones = varones
                    };
                    stackLayout.IsEnabled = false;
                    datosList.Add(LstAsis);
                    Datos.ItemsSource = null;
                    Datos.ItemsSource = datosList;
                    DAsistencia funcion = new DAsistencia();
                    funcion.FinAsistencias(datosList, ID);
                }
            }
        }
    }
}