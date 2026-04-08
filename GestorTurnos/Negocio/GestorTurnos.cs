namespace GestorTurnos.Negocio;

using GestorTurnos.Entidades;
using GestorTurnos.Enums;

public class GestorDeTurnos
{
    private readonly List<Medico> _medicos = new();
    private readonly List<Paciente> _pacientes = new();
    private readonly List<Turno> _turnos = new();
    private readonly List<Turno> _historial = new();

    // ---- Médicos ----

    public void AgregarMedico(Medico medico)
    {
        _medicos.Add(medico);
    }

    public IReadOnlyList<Medico> ObtenerMedicos()
    {
        if (_medicos.Count == 0)
            throw new InvalidOperationException("No hay médicos registrados.");
        return _medicos.AsReadOnly();
    }

    // ---- Pacientes ----

    public void AgregarPaciente(Paciente paciente)
    {
        _pacientes.Add(paciente);
    }

    public IReadOnlyList<Paciente> ObtenerPacientes()
    {
        if (_pacientes.Count == 0)
            throw new InvalidOperationException("No hay pacientes registrados.");
        return _pacientes.AsReadOnly();
    }

    // ---- Turnos ----

    public List<Turno> ObtenerTurnosDelDia(DateTime fecha)
    {
        List<Turno> turnosDelDia = new();

        foreach (Turno turno in _turnos)
        {
            if (turno.FechaHora.Date == fecha.Date)
            {
                turnosDelDia.Add(turno);
            }
        }

        // Ordenar por hora usando Sort con comparación
        turnosDelDia.Sort((a, b) => a.FechaHora.CompareTo(b.FechaHora));

        return turnosDelDia;
    }

    
    
    
    // Agrego logica para listar todos los tunros pendientes de un medico
    public List<Turno> TurnoPorMedico(Medico medico)
    {
        List<Turno> turnosPorMedico = new();
        foreach ( Turno turno in _turnos)
        {
            if (turno.Medico == medico)
            {
                turnosPorMedico.Add(turno);
            }
        }
        return turnosPorMedico;
    }

    public Turno AtenderProximoTurno()
    {
        if (_turnos.Count == 0)
            throw new InvalidOperationException("No hay turnos pendientes.");

        // Buscar si hay urgencias
        TurnoUrgencia? urgenciaMayor = null;
        foreach (Turno turno in _turnos)
        {
            if (turno is TurnoUrgencia urgencia)
            {
                if (urgenciaMayor == null || urgencia.Prioridad < urgenciaMayor.Prioridad)
                {
                    urgenciaMayor = urgencia;
                }
            }
        }

        // Si hay urgencia, se atiende esa. Si no, el primero de la lista (orden de llegada)
        Turno atendido;
        if (urgenciaMayor != null)
        {
            atendido = urgenciaMayor;
        }
        else
        {
            atendido = _turnos[0];
        }

        _turnos.Remove(atendido);
        _historial.Add(atendido);

        return atendido;
    }

    public IReadOnlyList<Turno> ObtenerHistorial()
    {
        if (_historial.Count == 0)
            throw new InvalidOperationException("No hay turnos atendidos.");
        return _historial.AsReadOnly();
    }

    public decimal RecaudacionDelDia(DateTime fecha)
    {
        decimal total = 0;
        foreach (Turno turno in _historial)
        {
            if (turno.FechaHora.Date == fecha.Date)
            {
                total += turno.CalcularCostoTurno();
            }
        }
        return total;
    }

    public void RegistrarTurno(Turno turno)
    {
        bool conflicto = false;
        foreach (Turno t in _turnos)
        {
            if (t.Medico == turno.Medico && t.FechaHora == turno.FechaHora)
            {
                conflicto = true;
                break;
            }
        }

        if (conflicto)
            throw new InvalidOperationException("El médico ya tiene un turno en ese horario.");

        _turnos.Add(turno);
    }
}