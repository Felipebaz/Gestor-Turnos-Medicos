namespace GestorTurnos.Entidades;

public class Medico
{
    private string Nombre  { get; set; }
    private string Especialidad { get; set; }
    private float ValorConsultaBase { get; set; }

    public Medico(string nombre, string especialidad, float valorConsultaBase)
    {
        Nombre = nombre;
        Especialidad = especialidad;
        ValorConsultaBase = valorConsultaBase;
    }
    
    
}