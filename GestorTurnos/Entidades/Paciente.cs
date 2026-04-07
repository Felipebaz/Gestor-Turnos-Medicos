namespace GestorTurnos.Entidades;

public class Paciente
{
    public string Nombre { get; private set; }
    public string Apellido { get; private set; }
    public DateTime FechaNacimiento { get; private set; }
    public string? ObraSocial { get; private set; } // ? es porque puede ser NULL

    public Paciente(string nombre, string apellido, DateTime fechaNacimiento)
    {
        Nombre = nombre;
        Apellido = apellido;
        FechaNacimiento = fechaNacimiento;
        ObraSocial = null;
    }
    
    // Constructor secundario que llama al primer contructor con : this y recibe los mismos parámetros más ObraSocial
    public Paciente(string nombre, string apellido, DateTime fechaNacimiento, string? obraSocial)
        : this(nombre, apellido, fechaNacimiento)
       {
           ObraSocial = obraSocial;
       }
    

}