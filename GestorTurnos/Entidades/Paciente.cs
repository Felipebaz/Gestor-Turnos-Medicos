namespace GestorTurnos.Entidades;

public class Paciente
{
    private string Nombre { get; set; }
    private string Apellido { get; set; }
    private DateTime FechaNacimiento { get; set; }
    private string? ObraSocial { get; set; } // ? es porque puede ser NULL

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