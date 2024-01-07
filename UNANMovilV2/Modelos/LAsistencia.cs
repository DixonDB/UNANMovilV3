namespace UNANMovilV2.Modelos
{
    public class LAsistencia
    {
        public int IdAsistencia { get; set; }
        public int INSS { get; set; }
        public string Fecha { get; set; }
        public int Bloques { get; set; }
        public int IdDetalleAsistencia { get; set; }
        public int IdAsignatura { get; set; }
        public int IdTema { get; set; }
        public int Mujeres { get; set; }
        public int Varones { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public string Observacion { get; set; }
        public string Contenido { get; set; }
        public string Estado { get; set; }
        public string Carrera { get; set; }
        public string Grupo { get; set; }
        public string Turno { get; set; }
        public string Asignatura { get; set; }
        public int IDAP { get; set; }
        public string Medidas { get; set; }
        public string Desfase { get; set; }
        public string UltimoTema { get; set; }
        public string TemasAtrasados { get; set; }
    }
}
