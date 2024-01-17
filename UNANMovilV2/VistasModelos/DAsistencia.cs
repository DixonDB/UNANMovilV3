using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UNANMovilV2.Modelos;
using Xamarin.Forms;

namespace UNANMovilV2.VistasModelos
{
    public class DAsistencia
    {
        public List<LAsistencia> MostrarAsistencia(int INSS)
        {
            var LstAsis = new List<LAsistencia>();
            try
            {
                DataTable dt = new DataTable();
                // Se abre la conexión a la base de datos
                Conexion.Abrir();

                SqlDataAdapter da = new SqlDataAdapter("MostrarAsistencia", Conexion.conectar);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@INSS", INSS);
                da.Fill(dt);
                foreach (DataRow rdr in dt.Rows)
                {
                    var parametros = new LAsistencia();
                    parametros.IdAsistencia = int.Parse(rdr["IdAsistencia"].ToString());
                    parametros.Fecha = DateTime.Parse(rdr["Fecha"].ToString()).ToString("dd/MMM/yyyy");
                    //parametros.HoraInicio = DateTime.Parse(rdr["Hora de Entrada"].ToString()).ToString("HH:mm");
                    //parametros.HoraFin = DateTime.Parse(rdr["Hora de Salida"].ToString()).ToString("HH:mm");
                    parametros.Bloques = int.Parse(rdr["Bloques"].ToString());
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

        public void Insertaasistencias(MAsignatura parametros, List<MAsignatura> lst)
        {
            try
            {
                //Se crea el DataTable con los campos necesarios
                var dt = new DataTable();
                dt.Columns.Add("Id");
                dt.Columns.Add("IdAsignatura");
                dt.Columns.Add("IdTema");
                dt.Columns.Add("AsistenciaMujeres");
                dt.Columns.Add("AsistenciaVarones");
                dt.Columns.Add("Bloque");

                int i = 1;

                //Se recorre la lista agregando los parametros que seran enviados a la base de datos
                foreach (var oElement in lst)
                {
                    dt.Rows.Add(i, oElement.IdAsig, oElement.IdTema, oElement.Mujeres, oElement.Varones,oElement.Bloque);
                    i++;
                }

                //Se abre la conexión a la base de datos
                Conexion.Abrir();

                //Crear el comando para el procedimiento almacenado
                SqlCommand cmd = new SqlCommand("InsertarAsistenciaYBloques", Conexion.conectar);
                var parameterlst = new SqlParameter("@ltsAsistencia", SqlDbType.Structured)
                {
                    TypeName = "Asistencia",
                    Value = dt
                };

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(parameterlst);
                cmd.Parameters.AddWithValue("@INSS", parametros.INSS);
                cmd.Parameters.AddWithValue("@Fecha", parametros.Fecha2);
                cmd.Parameters.AddWithValue("@Bloques", parametros.Bloques);
                cmd.ExecuteNonQuery();
                Application.Current.MainPage.DisplayAlert("Éxito", "Registro realizado", "OK");
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("ERROR", ex.Message, "OK");
            }
            finally
            {
                //Se cierra la conexión a la base de datos
                Conexion.Cerrar();
            }
        }

        public List<LAsistencia> MostrarDetalleAsistencia(int idAsistencia)
        {
            var LstAsis = new List<LAsistencia>();
            try
            {
                DataTable dt = new DataTable();
                //Se abre la conexión a la base de datos
                Conexion.Abrir();

                //Crea el comando SQL para el procedimiento almacenado
                SqlDataAdapter da = new SqlDataAdapter("MostrarDetalleAsistencia", Conexion.conectar);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@IdAsistencia", idAsistencia);
                da.Fill(dt);
                foreach (DataRow item in dt.Rows)
                {
                    var parametros = new LAsistencia();
                    parametros.IdAsistencia = int.Parse(item["IdAsistencia"].ToString());
                    parametros.IdTema = int.Parse(item["IdTema"].ToString());
                    parametros.Carrera = item["Carrera"].ToString();
                    parametros.Asignatura = item["Asignatura"].ToString();
                    parametros.Contenido = item["Contenido"].ToString();
                    parametros.Estado = item["Estado"].ToString();
                    parametros.Mujeres = int.Parse(item["Mujeres Asistencia"].ToString());
                    parametros.Varones = int.Parse(item["Mujeres Asistencia"].ToString());
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
                //Se cierra la conexión a la base de datos
                Conexion.Cerrar();
            }
        }

        public void FinAsistencias(List<LAsistencia> lst, int IdAsistencia)
        {
            try
            {
                var dt = new DataTable();
                DataColumnCollection columns = dt.Columns;
                columns.Add("Id");
                columns.Add("Contenido");
                columns.Add("Estado");

                foreach (var oElement in lst)
                {
                    dt.Rows.Add(
                        oElement.IdTema,
                        oElement.Contenido,
                        oElement.Estado);
                }
                Conexion.Abrir();
                SqlCommand cmd = new SqlCommand("FinalizarAsistencia", Conexion.conectar);
                var parameterlst = new SqlParameter("@lstAsis", SqlDbType.Structured);
                parameterlst.TypeName = "FinAsistencias";
                parameterlst.Value = dt;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(parameterlst);
                cmd.Parameters.AddWithValue("@IdAsistencia", IdAsistencia);
                cmd.ExecuteReader();
               Application.Current.MainPage.DisplayAlert("Asistencia Editada", "Asistencia editada con éxito", "OK");
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("ERROR", "No se editó la asistencia", "OK");
            }
            finally
            {
                Conexion.Cerrar();
            }
        }

        public List<LAsistencia> BuscarAsistencia(int INSS, DateTime fecha)
        {
            var LstAsis = new List<LAsistencia>();
            try
            {
                DataTable dt = new DataTable();
                // Se abre la conexión a la base de datos
                Conexion.Abrir();

                SqlDataAdapter da = new SqlDataAdapter("MostrarAsistenciaFecha", Conexion.conectar);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@INSS", INSS);
                da.SelectCommand.Parameters.AddWithValue("@Fecha", fecha);
                da.Fill(dt);
                foreach (DataRow rdr in dt.Rows)
                {
                    var parametros = new LAsistencia();
                    parametros.IdAsistencia = int.Parse(rdr["IdAsistencia"].ToString());
                    parametros.Fecha = DateTime.Parse(rdr["Fecha"].ToString()).ToString("dd/MMM/yyyy");
                    //parametros.HoraInicio = DateTime.Parse(rdr["Hora de Entrada"].ToString()).ToString("HH:mm");
                    //parametros.HoraFin = DateTime.Parse(rdr["Hora de Salida"].ToString()).ToString("HH:mm");
                    parametros.Bloques = int.Parse(rdr["Bloques"].ToString());
                    LstAsis.Add(parametros);
                }
                return LstAsis;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("ERROR", ex.Message, "OK");
                return LstAsis;
            }
            finally { Conexion.Cerrar(); }
        }
    }
}
