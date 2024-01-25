using System;
using System.Collections.Generic;
using System.Linq;
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
        private DAsignatura funcion;
        MModalidades mod = new MModalidades();
        int INSS = Login.INSS;
        int idAsignatura, i, j;
        MAsignatura asignaturaSeleccionada;
        MAsignatura Idcont;
        private TimeSpan hora;
        private DateTime Dia;
        List<MAsignatura> datosList = new List<MAsignatura>();
        public Asistencia()
        {
            InitializeComponent();
            funcion = new DAsignatura();
            BindingContext = funcion;
            MostrarAsignaturaTurno();
            LblFecha.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            TxtMujeres.IsEnabled = false;
            TxtVarones.IsEnabled = false;
            PcBloque.IsEnabled = false;
            PcContenido.IsEnabled = false;
        }

        private async void btnCerrar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MostrarAsistencia());
        }
        private void MostrarAsignaturaTurno()
        {
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

                    funcion.MostrarCarreraGrupo(parametros, INSS);
                    // Mostrar la carrera en la etiqueta LblCarrera
                    LblCarrera.Text = funcion.carrera;
                    lblGrupo.Text = funcion.grupo;
                    LblTurno.Text = funcion.turno;
                    PcContenido.IsEnabled = true;
                    var data = funcion.MostrarContenidos(asignaturaSeleccionada.IdAsig, funcion.grupo, Login.INSS);
                    PcContenido.ItemsSource = data;
                    funcion.MostrarVaronesMujeres(asignaturaSeleccionada.IdAsig,INSS);
                    LblMujeres.Text = funcion.mujeres.ToString();
                    LblVarones.Text = funcion.varones.ToString();
                    var bl = funcion.MostrarBloquesCombo(funcion.turno);
                    PcBloque.ItemsSource = bl;
                }
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
            var bloque = PcBloque.SelectedItem as MAsignatura;
            i++;

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
                    IdTema = Cont.IdTema,
                    Bloque = bloque.Bloque
                };

                bool bloqueExiste = datosList.Any(a => a.Bloque == LstAsis.Bloque);
                if (!bloqueExiste)
                {
                    // Agrega el nuevo objeto a la lista global
                    datosList.Add(LstAsis);
                    // Actualiza la fuente de datos del ListView
                    Datos.ItemsSource = null; // Primero, limpia la fuente de datos existente
                    Datos.ItemsSource = datosList; // Luego, asigna la lista actualizada
                    limpiar();
                    j++;
                    LblNum.Text = j.ToString();
                    BtnGuardar.IsEnabled = true;
                }
                else
                {
                    DisplayAlert("ERROR", "El bloque ya está ocupado", "OK");
                }
            }
            else
            {
                DisplayAlert("ERROR", "Debe de rellenar todos los campos", "OK");
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
            PcBloque.SelectedItem = null;
            PcContenido.IsEnabled = false;
            PcBloque.IsEnabled = false;
            TxtMujeres.IsEnabled = false;
            TxtVarones.IsEnabled = false;
            LblMujeres.Text = "";
            LblVarones.Text = "";
            BtnBloque.IsEnabled = false;
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

        private void PcBloque_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PcBloque.SelectedItem!=null)
            {
                TxtVarones.IsEnabled = true;
                TxtMujeres.IsEnabled = true;
            }
            else
            {
                TxtMujeres.IsEnabled = false;
                TxtVarones.IsEnabled = false;
            }
        }

        private void PcContenido_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PcContenido.SelectedItem!=null)
            {
                PcBloque.IsEnabled = true;
            }
            else
            {
                PcBloque.IsEnabled = false;
            }
        }

        private bool validar2()
        {
            int entero;
            if (!int.TryParse(TxtVarones.Text, out entero) || !int.TryParse(TxtMujeres.Text, out entero))
            {
                return false;
            }
            return !(TxtMujeres.Text == "" || TxtVarones.Text == "" || PcBloque == null || PcContenido == null);
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
                        oConcepto.Bloque = asignatura.Bloque;
                        lst.Add(oConcepto);
                    }
                }

                MAsignatura parametros = new MAsignatura();

                parametros.INSS = Login.INSS;
                parametros.Fecha2 =DateTime.Parse(LblFecha.Text);
                parametros.Bloques = datosList.Count;

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