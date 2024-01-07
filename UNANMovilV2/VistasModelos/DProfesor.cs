using System;
using System.Data;
using System.Data.SqlClient;
using UNANMovilV2.Modelos;

namespace UNANMovilV2.VistasModelos
{
    public class DProfesor
    {
        #region Validar Usuarios para Login
        public DataTable D_Usuarios(MProfes parametros)
        {
            try
            {
                Conexion.Abrir();
                SqlCommand cmd = new SqlCommand("Login", Conexion.conectar);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@INSS", parametros.INSS);
                cmd.Parameters.AddWithValue("Password", parametros.Password);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                Conexion.Cerrar();
            }
        }
        #endregion

        public void ComprobarConexion(ref int Id)
        {
            try
            {
                Conexion.Abrir();
                SqlCommand da = new SqlCommand("Select Top 1 INSS from Profesores", Conexion.conectar);
                Id = Convert.ToInt32(da.ExecuteScalar());
            }
            catch (Exception)
            {
                Id = 0;
            }
        }
    }
}
