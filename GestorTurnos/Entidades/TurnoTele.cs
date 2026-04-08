namespace GestorTurnos.Entidades;

public class TurnoTele : Turno
{
    public string LinkVideollamada { get; private set; }
    
    public TurnoTele( Medico medico, Paciente paciente, DateTime fechaHora, string linkV)
    : base(medico, paciente, fechaHora)
    {
        LinkVideollamada = linkV;
    }

    public override string ObtenerTipo()
    {
        return "Telemedicina";
    }

    public override decimal CalcularCostoTurno()
    {
        decimal total = Medico.ValorConsultaBase * 0.8m;
        if (Paciente.ObraSocial != null)
        {
            total *= 0.85m;
        }
        return total;
    }
    
}