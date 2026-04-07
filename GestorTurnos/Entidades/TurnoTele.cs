namespace GestorTurnos.Entidades;

public class TurnoTele : Turno
{
    public string LinkVideollamada { get; set; }
    
    public TurnoTele( Medico medico, Paciente paciente, DateTime fechaHora, string linkV)
    : base(medico, paciente, fechaHora)
    {
        LinkVideollamada = linkV;
    }

    public override decimal CalcularCostoTurno()
    {
        decimal total = base.CalcularCostoTurno() * 0.8m;
        if (Paciente.ObraSocial != null)
        {
            total *= 0.85m;
        }
        return total;
        // la m es para indicar que es tipo decimal y es obligatorio trabajando con este tipo de dato(tipo recomendado para valores monetarios)
    }
    
}