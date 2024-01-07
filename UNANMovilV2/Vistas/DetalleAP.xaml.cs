using System;
using System.Collections.Generic;
using UNANMovilV2.Modelos;
using UNANMovilV2.VistasModelos;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UNANMovilV2.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetalleAP : ContentPage
    {
       DAvance funcion=new DAvance();
        int ID, Idasig;
        List<MAsignatura> datosList = new List<MAsignatura>();

        private void BtnAtras_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        public DetalleAP(int IdAp,int IdAsig,string Fecha,string Asignatura,string Desfase,string Medidas)
        {
            InitializeComponent();
            LblAsignatura.Text = Asignatura;
            LblFecha.Text = Fecha;
            ID = IdAp;
            Idasig = IdAsig;
            var AP = new DAvance();
            var funcion = new DAsignatura();
            //var parametros = new MAsignatura();
            var parametros = new MAsignatura();
            parametros.IdAsig = IdAsig;
            funcion.MostrarCarreraGrupo(parametros, Login.INSS);
            // Mostrar la carrera en la etiqueta LblCarrera
            LblCarrera.Text = funcion.carrera;
            LblGrupo.Text = funcion.grupo;
            AP.MostrarUltimoTema(parametros, Login.INSS);
            LblUltimo.Text = AP.Cont;
            TxtDesfase.Text = Desfase;
            TxtMedidas.Text = Medidas;
            datosList = AP.MostrarTemasAtrasados(parametros, Login.INSS);
            LstTemas.ItemsSource = datosList;
        }
    }
}