﻿using System;
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
        private async void BtnEstado_Clicked(object sender, EventArgs e)
        {
            stackLayout = (Button)sender;
            if (stackLayout.BindingContext is LAsistencia Asis)
            {
                // Puedes acceder a las propiedades de asignatura y hacer lo que necesites
                //var IdAsis = Asis.IdAsistencia;
                //var Fecha = Asis.Fecha;
                var IdAsig = Asis.IdTema;
                //var Asignatura = Asis.Asignatura;
                //var Carrera = Asis.Carrera;
                //var Grupo = Asis.Grupo;
                var Contenido = Asis.Contenido;
                //var Mujeres = Asis.Mujeres;
                //var Varones = Asis.Varones;
                var Estado = Asis.Estado;
                if (Estado=="Proceso")
                {
                    LAsistencia LstAsis = new LAsistencia
                    {
                        IdTema = IdAsig,
                        Contenido = Contenido,
                        Estado = "Finalizado"
                    };
                    stackLayout.IsEnabled = false;
                    datosList.Add(LstAsis);
                    Datos.ItemsSource = null; // Primero, limpia la fuente de datos existente
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
                        Estado = "Proceso"
                    };
                    stackLayout.IsEnabled = false;
                    datosList.Add(LstAsis);
                    Datos.ItemsSource = null; // Primero, limpia la fuente de datos existente
                    Datos.ItemsSource = datosList;
                    DAsistencia funcion = new DAsistencia();
                    funcion.FinAsistencias(datosList, ID);
                }
            }
        }
    }
}