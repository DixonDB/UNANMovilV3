using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UNANMovilV2.Modelos;
using Xamarin.Forms;

namespace UNANMovilV2.VistasModelos
{
    public class DAsignatura
    {
        public string carrera;
        public string grupo;
        public int varones, mujeres;
        public void MostrarVaronesMujeres(int IdAsignatura, int INSS)
        {
            try
            {
                Conexion.Abrir();
                SqlCommand da = new SqlCommand("MostrarMV", Conexion.conectar);
                da.CommandType = CommandType.StoredProcedure;
                da.Parameters.AddWithValue("@IdAsignatura", IdAsignatura);
                da.Parameters.AddWithValue("@INSS", INSS);
                SqlDataAdapter cb = new SqlDataAdapter(da);
                DataTable dt = new DataTable();
                cb.Fill(dt);
                mujeres = int.Parse(dt.Rows[0]["Mujeres"].ToString());
                varones = int.Parse(dt.Rows[0]["Varones"].ToString());
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("ERROR", "Ocurrió un error al cargar los datos", "OK");
            }
            finally
            {
                Conexion.Cerrar();
            }
        }
        public List<MAsignatura> MostrarAsignaturaPlan(int INSS)
        {
            var lstAsig = new List<MAsignatura>();
            try
            {
                Conexion.Abrir();
                SqlCommand da = new SqlCommand("MostrarAsignaturaPlan", Conexion.conectar);
                da.CommandType = CommandType.StoredProcedure;
                da.Parameters.AddWithValue("@INSS", INSS);
                SqlDataAdapter cb = new SqlDataAdapter(da);
                DataTable dt = new DataTable();
                cb.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int id = Convert.ToInt32(dt.Rows[i]["IdAsignatura"]);
                    string Asig = dt.Rows[i]["Asig"].ToString();
                    lstAsig.Add(new MAsignatura { IdAsig = id, Asignatura = Asig });
                }
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("ERROR", ex.Message, "OK");
            }
            finally
            {
                Conexion.Cerrar();
            }

            return lstAsig; // Devuelve la lista de asignaturas
        }

        public List<MAsignatura> MostrarAsignturas(int INSS)
        {
            var LstAsis = new List<MAsignatura>();
            try
            {
                DataTable dt = new DataTable();
                // Se abre la conexión a la base de datos
                Conexion.Abrir();

                SqlDataAdapter da = new SqlDataAdapter("MostrarAsignaturaPlan", Conexion.conectar);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@INSS", INSS);
                da.Fill(dt);
                foreach (DataRow rdr in dt.Rows)
                {
                    var parametros = new MAsignatura();
                    parametros.IdAsig = int.Parse(rdr["IdAsignatura"].ToString());
                    parametros.Asignatura = rdr["Asig"].ToString();
                    LstAsis.Add(parametros);
                }
                return LstAsis;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("ERROR", ex.Message, "OK");
                return LstAsis;
            }
            finally
            {
                Conexion.Cerrar();
            }
        }
        public List<MAsignatura> MostrarContenidos(int IdAsig, string Grupo, int INSS)
        {
            var LstCont = new List<MAsignatura>();
            try
            {
                DataTable dt = new DataTable();
                //Abrir la conexión a la base de datos
                Conexion.Abrir();

                //Crear el comando SQL para el procedimiento almacenado
                SqlDataAdapter da = new SqlDataAdapter("MostrarContenidos", Conexion.conectar);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@INSS", INSS);
                da.SelectCommand.Parameters.AddWithValue("@Asignatura", IdAsig);
                da.SelectCommand.Parameters.AddWithValue("@Grupo", Grupo);
                da.Fill(dt);

                foreach (DataRow rdr in dt.Rows)
                {
                    MAsignatura parametros = new MAsignatura();
                    parametros.IdTema = int.Parse(rdr["IdTema"].ToString());
                    parametros.Contenido = rdr["Contenido"].ToString();
                    LstCont.Add(parametros);
                }
                return LstCont;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("ERROR", ex.Message, "OK");
                return LstCont;
            }
            finally
            {
                //Cerrar la conexión a la base de datos
                Conexion.Cerrar();
            }
        }
        public void MostrarCarreraGrupo(MAsignatura parametros, int INSS)
        {
            try
            {
                Conexion.Abrir();
                SqlCommand da = new SqlCommand("MostrarCarreraGrupo", Conexion.conectar);
                da.CommandType = CommandType.StoredProcedure;
                da.Parameters.AddWithValue("@IdAsignatura", parametros.IdAsig);
                da.Parameters.AddWithValue("@INSS", INSS);
                SqlDataAdapter cb = new SqlDataAdapter(da);
                DataTable dt = new DataTable();
                cb.Fill(dt);
                parametros.Carrera = dt.Rows[0]["Carrera"].ToString();
                parametros.Grupo = dt.Rows[0]["Grupo"].ToString();
                carrera = parametros.Carrera;
                grupo = parametros.Grupo;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("ERROR", ex.Message, "OK");
            }
            finally
            {
                Conexion.Cerrar();
            }
        }
    }
}
