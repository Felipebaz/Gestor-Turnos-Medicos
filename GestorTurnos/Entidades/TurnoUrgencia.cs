namespace GestorTurnos.Entidades;
using GestorTurnos.Enums;


public class TurnoUrgencia : Turno
{
    public Prioridad Prioridad { get; private set; }
    
    public TurnoUrgencia(Medico medico, Paciente paciente, DateTime fechaHora, Prioridad prioridad)
        : base(medico, paciente, fechaHora)
    {
        Prioridad = prioridad;
    }

    public override decimal CalcularCostoTurno()
    {
        return base.CalcularCostoTurno() * 1.5m;
    }

    
}