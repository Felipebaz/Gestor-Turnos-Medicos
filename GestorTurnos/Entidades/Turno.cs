namespace GestorTurnos.Entidades;
using System;


public class Turno
{
    public Medico Medico { get; private set; }
    public Paciente Paciente { get; private set; }
    public DateTime FechaHora { get; private set; }

    public Turno(Medico medico, Paciente paciente, DateTime fechaHora)
    {
        Medico = medico;
        Paciente = paciente;
        FechaHora = fechaHora;
    }


    public virtual string ObtenerTipo()
    {
        return "General";
    }

    public virtual decimal CalcularCostoTurno()
    {
        decimal total = Medico.ValorConsultaBase;
        if (Paciente.ObraSocial != null)
        {
            total *= 0.85m;
        }
        return total;
    }
    
}