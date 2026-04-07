namespace GestorTurnos.Entidades;

public class Medico
{
    public string Nombre  { get; private set; }
    public string Especialidad { get; private set; }
    public decimal ValorConsultaBase { get; private set; }

    public Medico(string nombre, string especialidad, decimal valorConsultaBase)
    {
        Nombre = nombre;
        Especialidad = especialidad;
        ValorConsultaBase = valorConsultaBase;
    }
    
    
}