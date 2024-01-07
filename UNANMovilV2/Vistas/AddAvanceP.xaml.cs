using System;
using System.Collections.Generic;
using UNANMovilV2.Modelos;
using UNANMovilV2.VistasModelos;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UNANMovilV2.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddAvanceP : ContentPage
	{
        int INSS = Login.INSS;
        MAsignatura asignaturaSeleccionada;
        DAvance AP = new DAvance();
        List<MAsignatura> datosList = new List<MAsignatura>();
        public AddAvanceP ()
		{
            InitializeComponent();
            BtnGuardar.IsEnabled = false;
            MostrarAsignaturaTurno();
            LblFecha.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Activar();
            BtnGuardar.IsEnabled = false;
        }

        private async void btnCerrar_Clicked(object sender, EventArgs e)
        {
			await Navigation.PopAsync();
        }

        private void PcAsig_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PcAsig.SelectedItem != null)
            {
                asignaturaSeleccionada = PcAsig.SelectedItem as MAsignatura;

                if (asignaturaSeleccionada != null)
                {
                    var parametros = new MAsignatura();
                    parametros.IdAsig = asignaturaSeleccionada.IdAsig;

                    // Llamar al método para mostrar la carrera y grupo
                    var funcion = new DAsignatura();
                    funcion.MostrarCarreraGrupo(parametros, Login.INSS);
                    // Mostrar la carrera en la etiqueta LblCarrera
                    LblCarrera.Text = funcion.carrera;
                    lblGrupo.Text = funcion.grupo;
                    AP.MostrarUltimoTema(parametros, Login.INSS);
                    LblTema.Text = AP.Cont;

                    datosList = AP.MostrarTemasAtrasados(parametros, INSS);
                    LstTemas.ItemsSource= datosList;
                    if (datosList.Count == 0)
                    {
                        TxtMedidas.Text = "Ninguno";
                        TxtDesfase.Text = "Ninguno";
                    }
                    else
                    {
                        TxtDesfase.Text = "";
                        TxtMedidas.Text = "";
                    }
                }
            }
        }
        private void MostrarAsignaturaTurno()
        {
            var funcion = new DAsignatura();
            var data = funcion.MostrarAsignaturaPlan(INSS);
            PcAsig.ItemsSource = data;
        }

        private async void GuardarAP()
        {
            try
            {
                List<MAsignatura> lst = new List<MAsignatura>();

                // LLenar la lista
                foreach (var item in datosList) // Asegúrate de obtener los datos de la interfaz correcta
                {
                    if (item is MAsignatura avance)
                    {
                        MAsignatura oConcepto = new MAsignatura();
                        oConcepto.TemasAtrasados = avance.Contenido; // Asegúrate de ajustar según la estructura de tus datos
                        lst.Add(oConcepto);
                    }
                }

                MAsignatura parametros = new MAsignatura();
                parametros.INSS = Login.INSS;
                parametros.IdAsig = asignaturaSeleccionada.IdAsig; // Ajusta según tu lógica
                parametros.UltimoTema = LblTema.Text;
                parametros.Fecha2 = DateTime.Parse(LblFecha.Text);
                parametros.Desfase = TxtDesfase.Text;
                parametros.Medidas = TxtMedidas.Text;

                DAvance funcion = new DAvance();
                funcion.GuardarAP(parametros, lst);

                // Mostrar un mensaje de éxito (puedes ajustar esto según tus necesidades)
                await DisplayAlert("Éxito", "Registro realizado", "OK");

                // Realizar otras acciones después del éxito
                await Navigation.PushAsync(new Avance());
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje de error (puedes ajustar esto según tus necesidades)
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }


        private void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            GuardarAP();
        }

        private void TxtDesfase_TextChanged(object sender, TextChangedEventArgs e)
        {
            Activar();
        }

        private void TxtMedidas_TextChanged(object sender, TextChangedEventArgs e)
        {
            Activar();
        }

        private void Activar()
        {
            if (TxtDesfase.Text!=""&&TxtMedidas.Text!="")
            {
                BtnGuardar.IsEnabled = true;
            }
            else
            {
                BtnGuardar.IsEnabled = false;
            }
        }
    }
}