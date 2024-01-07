using System.Data;
using UNANMovilV2.VistasModelos;

namespace UNANMovilV2.Modelos
{
    public class NProfes
    {
        DProfesor objd = new DProfesor();
        public DataTable Nprofe(MProfes parametros)
        {
            return objd.D_Usuarios(parametros);
        }
    }
}
