using System;
using System.Collections.Generic;
using UNANMovilV2.Modelos;
using UNANMovilV2.VistasModelos;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace UNANMovilV2.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Asistencia : ContentPage
    {
        private DAsignatura Asignatura;
        MModalidades mod = new MModalidades();
        int INSS = Login.INSS;
        int idAsignatura, cant, i, j;
        MAsignatura asignaturaSeleccionada;
        MAsignatura Idcont;
        private TimeSpan hora;
        private DateTime Dia;
        List<MAsignatura> datosList = new List<MAsignatura>();
        public Asistencia()
        {
            InitializeComponent();
            Asignatura = new DAsignatura();
            BindingContext = Asignatura;
            MostrarAsignaturaTurno();
            LblFecha.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Suscribir al evento PropertyChanged del TimePicker
            TPHoraE.PropertyChanged += TPHoraE_PropertyChanged;

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Desuscribir del evento PropertyChanged del TimePicker
            TPHoraE.PropertyChanged -= TPHoraE_PropertyChanged;
        }

        private async void btnCerrar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MostrarAsistencia());
        }
        private void MostrarAsignaturaTurno()
        {
            var funcion = new DAsignatura();
            var data = funcion.MostrarAsignaturaPlan(INSS);
            PcAsig.ItemsSource = data;
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
                    string gr = lblGrupo.Text;
                    var data = funcion.MostrarContenidos(asignaturaSeleccionada.IdAsig, gr, Login.INSS);
                    PcContenido.ItemsSource = data;
                    funcion.MostrarVaronesMujeres(asignaturaSeleccionada.IdAsig, Login.INSS);
                    LblMujeres.Text = funcion.mujeres.ToString();
                    LblVarones.Text = funcion.varones.ToString();
                }
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (!validar())
            {
                DisplayAlert("ERROR", "Los bloques no pueden tener un valor menor a 1", "OK");
                nudBloque.Text = "1";
            }
            else
            {
                cant = int.Parse(nudBloque.Text.ToString());
                entrada.IsEnabled = false;
                contenedor.IsEnabled = true;
                contenedor.BackgroundColor = Color.White;
                entrada.BackgroundColor = Color.FromHex("#b5b5b5");
                BtnAgregar.BackgroundColor = Color.Gray;
                j = 1;
                LblNum.Text = j.ToString();
            }
        }

        private void BtnBloque_Clicked(object sender, EventArgs e)
        {
            Comprobar();
        }
        private void Comprobar()
        {
            if (validar2())
            {
                int mujerestxt, mujereslbl;
                mujerestxt = int.Parse(TxtMujeres.Text);
                mujereslbl = int.Parse(LblMujeres.Text);
                int varonestxt, varoneslbl;
                varonestxt = int.Parse(TxtVarones.Text);
                varoneslbl = int.Parse(LblVarones.Text);
                if (mujerestxt < 0 || varonestxt < 0) 
                {
                    DisplayAlert("ERROR", "La cantidad insertada no puede ser menor a 0", "OK");
                    if(varonestxt < 0)
                    {
                        TxtVarones.BackgroundColor = Color.Red;
                        TxtVarones.Text = "0";
                    }
                    else if (mujerestxt < 0)
                    {
                        TxtMujeres.BackgroundColor = Color.Red;
                        TxtMujeres.Text = "0";
                    }
                }
                else if (mujerestxt > mujereslbl || varonestxt > varoneslbl)
                {
                    DisplayAlert("Error", "La asistencia no puede ser mayor a la registrada", "OK");
                    if (varonestxt > varoneslbl)
                    {
                        TxtVarones.BackgroundColor = Color.Red;
                        TxtVarones.Text = "0";
                    }
                    else if (mujerestxt > mujereslbl)
                    {
                        TxtMujeres.BackgroundColor = Color.Red;
                        TxtMujeres.Text = "0";
                    }
                }
                else
                {
                    ADD();
                    TxtVarones.BackgroundColor = Color.White;
                    TxtMujeres.BackgroundColor = Color.White;
                }
            }
            else
            {
                DisplayAlert("ERROR", "Debe de rellenar todos los campos", "OK");
            }
        }

        private void ADD()
        {
            var asig = PcAsig.SelectedItem as MAsignatura;
            var Cont = PcContenido.SelectedItem as MAsignatura;
            i++;
            if (i <= cant)
            {
                if (validar2())
                {
                    // Crea un objeto MAsignatura con el elemento seleccionado
                    MAsignatura LstAsis = new MAsignatura
                    {
                        IdAsig = asignaturaSeleccionada.IdAsig,
                        Asignatura = asig.Asignatura,
                        Carrera = LblCarrera.Text,
                        Grupo = lblGrupo.Text,
                        Contenido = Cont.Contenido,
                        Mujeres = int.Parse(TxtMujeres.Text),
                        Varones = int.Parse(TxtVarones.Text),
                        IdTema = Cont.IdTema
                    };

                    // Agrega el nuevo objeto a la lista global
                    datosList.Add(LstAsis);
                    // Actualiza la fuente de datos del ListView
                    Datos.ItemsSource = null; // Primero, limpia la fuente de datos existente
                    Datos.ItemsSource = datosList; // Luego, asigna la lista actualizada
                    limpiar();
                    j++;
                    LblNum.Text = j.ToString();
                }
                else
                {
                    DisplayAlert("ERROR", "Debe de rellenar todos los campos", "OK");
                }
            }
            if (i == cant)
            {
                contenedor.IsEnabled = false;
                contenedor.BackgroundColor = Color.FromHex("#b5b5b5");
                BtnGuardar.IsEnabled = true;
            }
        }

        private void limpiar()
        {
            PcContenido.SelectedItem = null;
            PcAsig.SelectedItem = null;
            lblGrupo.Text = "--";
            LblCarrera.Text = "--";
            TxtMujeres.Text = "";
            TxtVarones.Text = "";
        }

        private bool validar()
        {
            double entero;
            if (!double.TryParse(nudBloque.Text, out entero))
            {
                return false;
            }
            return !(nudBloque.Text == "");
        }

        private void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            GuardarAsistencia();
        }

        private void TxtMujeres_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtMujeres.Text != "" && TxtVarones.Text != "")
            {
                Activar();
            }
        }
        private void TxtVarones_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtVarones.Text != "" && TxtMujeres.Text != "")
            {
                Activar();
            }
            
        }
        private void Activar()
        {
            if (TxtVarones.Text != "" && TxtMujeres.Text != "")
            {
                BtnBloque.BackgroundColor = Color.Coral;
                BtnBloque.IsEnabled = true;
            }
            else
            {
                BtnBloque.BackgroundColor = Color.Gray;
                BtnBloque.IsEnabled = false;
            }
        }

        private void TPHoraE_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Time")
            {
                var timePicker = (Xamarin.Forms.TimePicker)sender;
                hora = timePicker.Time;
            }
        }

        private void DPFecha_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Date")
            {
                var DatePicker = (Xamarin.Forms.DatePicker)sender;
                Dia = DatePicker.Date;
            }
        }

        private void DPFecha_DateSelected(object sender, DateChangedEventArgs e)
        {
            Dia = e.NewDate;
        }


        private bool validar2()
        {
            double entero;
            if (!double.TryParse(TxtVarones.Text, out entero) || !double.TryParse(TxtMujeres.Text, out entero))
            {
                return false;
            }
            return !(TxtMujeres.Text == "" || TxtVarones.Text == "");
        }

        private async void GuardarAsistencia()
        {
            try
            {
                List<MAsignatura> lst = new List<MAsignatura>();

                foreach (var item in datosList)
                {
                    if (item is MAsignatura asignatura)
                    {
                        MAsignatura oConcepto = new MAsignatura();
                        oConcepto.IdAsig = asignatura.IdAsig;
                        oConcepto.IdTema = asignatura.IdTema;
                        oConcepto.Mujeres = asignatura.Mujeres;
                        oConcepto.Varones = asignatura.Varones;
                        lst.Add(oConcepto);
                    }
                }

                MAsignatura parametros = new MAsignatura();
                DateTime HoraE = DateTime.Parse(hora.ToString());
                int bloques = int.Parse(nudBloque.Text);
                DateTime HoraF = HoraE.AddMinutes(bloques * 80);
                string horaInicioFormateada = HoraE.ToString("HH:mm");
                string horaFinFormateada = HoraF.ToString("HH:mm");


                parametros.INSS = Login.INSS;
                parametros.Fecha2 =DateTime.Parse(LblFecha.Text);
                parametros.Bloques = int.Parse(nudBloque.Text);
                parametros.HoraInicio = horaInicioFormateada.ToString();
                parametros.HoraFin = horaFinFormateada.ToString();
                parametros.Observacion = "Esto es una Asistencia";

                DAsistencia funcion = new DAsistencia();
                funcion.Insertaasistencias(parametros, lst);

                await Navigation.PushAsync(new MostrarAsistencia());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                App.Current.MainPage = new Xamarin.Forms.NavigationPage(new MostrarAsistencia());
            });

            return true;
        }

    }
}