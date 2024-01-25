using System;
using System.Collections.Generic;
using System.Linq;
using UNANMovilV2.Modelos;
using UNANMovilV2.VistasModelos;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UNANMovilV2.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditarAsistencia : ContentPage
    {
        DAsistencia Asis = new DAsistencia();
        int ID;
        int bl;
        int mujeres, varones;
        public EditarAsistencia(int IdAsis, string fecha, int bloque)
        {
            InitializeComponent();
            var data = Asis.MostrarDetalleAsistencia(IdAsis);
            lstAsis.ItemsSource = data;
            LblFecha.Text = fecha;
            ID = IdAsis;
            bl = bloque;
            LblBloques.Text = bloque.ToString();
        }

        private async void btnCerrar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        List<LAsistencia> datosList = new List<LAsistencia>();
        private async void btnGuardar_Clicked(object sender, EventArgs e)
        {
            DAsistencia funcion = new DAsistencia();
            funcion.FinAsistencias(datosList, ID);
            await Navigation.PushAsync(new MostrarAsistencia());
        }

        Button stackLayout;
        //private async void BtnEstado_Clicked(object sender, EventArgs e)
        //{
        //    stackLayout = (Button)sender;
        //    if (stackLayout.BindingContext is LAsistencia Asis)
        //    {
        //        var IdAsig = Asis.IdTema;
        //        var Contenido = Asis.Contenido;
        //        var Estado = Asis.Estado;
        //        var mujeres = Asis.Mujeres;
        //        var varones = Asis.Varones;
        //        if (Estado == "Proceso")
        //        {
        //            LAsistencia LstAsis = new LAsistencia
        //            {
        //                IdTema = IdAsig,
        //                Contenido = Contenido,
        //                Estado = "Finalizado",
        //                Mujeres=mujeres,
        //                Varones=varones
        //            };
        //            stackLayout.IsEnabled = false;
        //            datosList.Add(LstAsis);
        //            Datos.ItemsSource = null;
        //            Datos.ItemsSource = datosList;
        //            DAsistencia funcion = new DAsistencia();
        //            funcion.FinAsistencias(datosList, ID);
        //            await Navigation.PushAsync(new MostrarAsistencia());
        //        }
        //        else
        //        {
        //            LAsistencia LstAsis = new LAsistencia
        //            {
        //                IdTema = IdAsig,
        //                Contenido = Contenido,
        //                Estado = "Proceso",
        //                Mujeres = mujeres,
        //                Varones = varones
        //            };
        //            stackLayout.IsEnabled = false;
        //            datosList.Add(LstAsis);
        //            Datos.ItemsSource = null;
        //            Datos.ItemsSource = datosList;
        //            DAsistencia funcion = new DAsistencia();
        //            funcion.FinAsistencias(datosList, ID);
        //            await Navigation.PushAsync(new MostrarAsistencia());
        //        }
        //    }
        //}

        private async void btnSalvar_Clicked(object sender, EventArgs e)
        {
            DAsistencia funcion = new DAsistencia();
            funcion.FinAsistencias(datosList, ID);
            await Navigation.PushAsync(new MostrarAsistencia());
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            stackLayout = (Button)sender;
            if (stackLayout.BindingContext is LAsistencia Asis)
            {
                var IdAsig = Asis.IdTema;
                var Contenido = Asis.Contenido;
                var Estado = Asis.Estado;
                mujeres = Asis.Mujeres;
                varones = Asis.Varones;
                if (validar2())
                {
                    LAsistencia LstAsis = new LAsistencia
                    {
                        IdTema = IdAsig,
                        Contenido = Contenido,
                        Estado = Estado,
                        Mujeres = mujeres,
                        Varones = varones
                    };
                    if (mujeres < 0 || varones < 0)
                    {
                        DisplayAlert("ERROR", "No puede ser menor a 0", "OK");
                    }
                    else
                    {
                        stackLayout.IsEnabled = false;
                        datosList.Add(LstAsis);
                        Datos.ItemsSource = null;
                        Datos.ItemsSource = datosList;
                    }
                }
                else
                {
                    DisplayAlert("ERROR", "Mujeres y Varones deben ser números enteros.", "OK");
                }
            }
        }
        private bool validar2()
        {
            int entero;
            if (!int.TryParse(mujeres.ToString(), out entero) || !int.TryParse(varones.ToString(), out entero))
            {
                return false;
            }
            return !(mujeres.ToString() == "" || varones.ToString() == ""); 
        }
    }
}