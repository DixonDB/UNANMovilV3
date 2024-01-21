using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UNANMovilV2.Modelos;
using Xamarin.Forms;

namespace UNANMovilV2.VistasModelos
{
    public class DAvance
    {
        public string Cont;
        public string carrera;
        public string grupo;
        public string desfase;
        public string medidas;
        public List<MAsignatura> MostrarAvance(int INSS)
        {
            var lstProg = new List<MAsignatura>();
            try
            {
                DataTable dt = new DataTable();
                // Se abre la conexión a la base de datos
                Conexion.Abrir();

                SqlDataAdapter da = new SqlDataAdapter("MostrarAvacesProgramatico", Conexion.conectar);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@INSS", INSS);
                da.Fill(dt);
                foreach (DataRow rdr in dt.Rows)
                {
                    var parametros = new MAsignatura();
                    parametros.IDAP = int.Parse(rdr["IdAvanceProgramatico"].ToString());
                    parametros.IdAsig = int.Parse(rdr["IdAsignatura"].ToString());
                    parametros.Fecha = DateTime.Parse(rdr["Fecha"].ToString()).ToString("dd/MMM/yyyy");
                    parametros.Asignatura = rdr["Asignatura"].ToString();
                    parametros.Desfase = rdr["Desfase"].ToString();
                    parametros.Medidas = rdr["Medidas"].ToString();
                    lstProg.Add(parametros);
                    desfase = parametros.Desfase;
                    medidas = parametros.Medidas;
                }
                return lstProg;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("ERROR", ex.Message, "OK");
                return lstProg;
            }
            finally
            {
                Conexion.Cerrar();
            }
        }
        public void MostrarUltimoTema(MAsignatura parametros, int INSS)
        {
            try
            {
                Conexion.Abrir();
                SqlCommand da = new SqlCommand("MostrarUltimoTema", Conexion.conectar);
                da.CommandType = CommandType.StoredProcedure;
                da.Parameters.AddWithValue("@IdAsignatura", parametros.IdAsig);
                da.Parameters.AddWithValue("@INSS", INSS);
                SqlDataAdapter cb = new SqlDataAdapter(da);
                DataTable dt = new DataTable();
                cb.Fill(dt);
                parametros.Contenido = dt.Rows[0]["Contenido"].ToString();
                if (dt.Rows[0]["Contenido"].ToString()==null)
                {
                    Cont = "--";
                }
                else
                {
                    Cont = parametros.Contenido;
                }
            }
            catch (Exception ex)
            {
                //Application.Current.MainPage.DisplayAlert("ERROR", ex.Message, "OK");
            }
            finally
            {
                Conexion.Cerrar();
            }
        }

        public List<MAsignatura> MostrarTemasAtrasados(MAsignatura parametros,int INSS)
        {
            var lstProg = new List<MAsignatura>();
            try
            {
                DataTable dt = new DataTable();
                //Abrir la conexión a la base de datos
                Conexion.Abrir();

                //Cerar el comando SQL para el procedimiento almacenado
                SqlDataAdapter da = new SqlDataAdapter("MostrarTemasAtrasados", Conexion.conectar);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@INSS", INSS);
                da.SelectCommand.Parameters.AddWithValue("@IdAsignatura", parametros.IdAsig);
                da.Fill(dt);

                foreach (DataRow rdr in dt.Rows)
                {
                    var par = new MAsignatura();
                    par.Contenido = rdr["Temas Atrasados"].ToString();
                    lstProg.Add(par);
                }
                parametros.Contenido = null;
                Cont = parametros.Contenido;
                return lstProg;

            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("ERROR", ex.Message, "OK");
                return lstProg;
            }
            finally
            {
                Conexion.Cerrar();
            }
        }

        public void GuardarAP(MAsignatura parametros, List<MAsignatura> lst)
        {
            try
            {
                // Se crea el DataTable con los campos necesarios
                var dt = new DataTable();
                dt.Columns.Add("Id");
                dt.Columns.Add("TemasAtrasados");

                int i = 1;

                // Se recorre la lista agregando los parametros que serán enviados a la base de datos
                foreach (var oElement in lst)
                {
                    dt.Rows.Add(i, oElement.TemasAtrasados);
                    i++;
                }

                // Se abre la conexión a la base de datos
                Conexion.Abrir();

                // Crear el comando para el procedimiento almacenado
                SqlCommand cmd = new SqlCommand("GuardarAP", Conexion.conectar);
                var parameterlst = new SqlParameter("@ltsAvanceP", SqlDbType.Structured)
                {
                    TypeName = "AP",
                    Value = dt
                };

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(parameterlst);
                cmd.Parameters.AddWithValue("@INSS", parametros.INSS);
                cmd.Parameters.AddWithValue("@IdAsignatura", parametros.IdAsig);
                cmd.Parameters.AddWithValue("@UltimoTema", parametros.UltimoTema);
                cmd.Parameters.AddWithValue("@Fecha", parametros.Fecha2);
                cmd.Parameters.AddWithValue("@Desfase", parametros.Desfase);
                cmd.Parameters.AddWithValue("@Medidas", parametros.Medidas);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // En Xamarin.Forms, muestra un alerta en lugar de MessageBox
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                // Se cierra la conexión a la base de datos
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
                //carrera = dt.Rows[0]["Carrera"].ToString();
                parametros.Grupo = dt.Rows[0]["Grupo"].ToString();
                //grupo = dt.Rows[0]["Grupo"].ToString();
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

        public List<MAsignatura> BuscarAp(int INSS,string buscador)
        {
            var lstProg = new List<MAsignatura>();
            try
            {
                DataTable dt = new DataTable();
                // Se abre la conexión a la base de datos
                Conexion.Abrir();

                SqlDataAdapter da = new SqlDataAdapter("BuscarAvaceProgramatico", Conexion.conectar);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@INSS", INSS);
                da.SelectCommand.Parameters.AddWithValue("@Buscador", buscador);
                da.Fill(dt);
                foreach (DataRow rdr in dt.Rows)
                {
                    var parametros = new MAsignatura();
                    parametros.IDAP = int.Parse(rdr["IdAvanceProgramatico"].ToString());
                    parametros.IdAsig = int.Parse(rdr["IdAsignatura"].ToString());
                    parametros.Fecha = DateTime.Parse(rdr["Fecha"].ToString()).ToString("dd/MMM/yyyy");
                    parametros.Asignatura = rdr["Asignatura"].ToString();
                    parametros.Desfase = rdr["Desfase"].ToString();
                    parametros.Medidas = rdr["Medidas"].ToString();
                    lstProg.Add(parametros);
                    desfase = parametros.Desfase;
                    medidas = parametros.Medidas;
                }
                return lstProg;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("ERROR", ex.Message, "OK");
                return lstProg;
            }
            finally
            {
                Conexion.Cerrar();
            }
        }
    }
}
