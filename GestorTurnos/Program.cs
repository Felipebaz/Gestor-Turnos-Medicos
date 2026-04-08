using GestorTurnos.Entidades;
using GestorTurnos.Enums;
using GestorTurnos.Negocio;

GestorDeTurnos gestor = new();
bool ejecutando = true;

while (ejecutando)
{
    Console.WriteLine();
    Console.WriteLine("=== Sistema de Gestión de Turnos Médicos ===");
    Console.WriteLine("1. Registrar médico");
    Console.WriteLine("2. Registrar paciente");
    Console.WriteLine("3. Registrar turno");
    Console.WriteLine("4. Listar turnos del día");
    Console.WriteLine("5. Listar turnos por médico");
    Console.WriteLine("6. Atender próximo turno");
    Console.WriteLine("7. Ver historial");
    Console.WriteLine("8. Recaudación del día");
    Console.WriteLine("0. Salir");
    Console.Write("Opción: ");

    string? opcion = Console.ReadLine();

    try
    {
        switch (opcion)
        {
            case "1":
                RegistrarMedico();
                break;
            case "2":
                RegistrarPaciente();
                break;
            case "3":
                RegistrarTurno();
                break;
            case "4":
                ListarTurnosDelDia();
                break;
            case "5":
                TurnoDelMedico();
                break;
            case "6":
                AtenderProximoTurno();
                break;
            case "7":
                VerHistorial();
                break;
            case "8":
                VerRecaudacion();
                break;
            case "0":
                ejecutando = false;
                Console.WriteLine("¡Hasta luego!");
                break;
            default:
                Console.WriteLine("Opción no válida.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

// ---- Funciones del menú ----

void TurnoDelMedico()
{
    IReadOnlyList<Medico> medicos = gestor.ObtenerMedicos();
    Console.WriteLine("--- Médicos disponibles ---");
    for (int i = 0; i < medicos.Count; i++)
    {
        Console.WriteLine($"  {i + 1}. {medicos[i].Nombre} - {medicos[i].Especialidad}");
    }
    Console.Write("Seleccione médico: ");
    int idx = LeerEntero(1, medicos.Count) - 1;
    Medico medico = medicos[idx];

    List<Turno> turnos = gestor.TurnoPorMedico(medico);

    if (turnos.Count == 0)
    {
        Console.WriteLine($"No hay turnos pendientes para {medico.Nombre}.");
        return;
    }

    Console.WriteLine($"--- Turnos pendientes de {medico.Nombre} ---");
    for (int i = 0; i < turnos.Count; i++)
    {
        Turno t = turnos[i];

        string tipo = t.ObtenerTipo();

        Console.WriteLine($"  {t.FechaHora:dd/MM/yyyy HH:mm} | {tipo} | {t.Paciente.Nombre} {t.Paciente.Apellido} | ${t.CalcularCostoTurno():F2}");
    }
}

void AtenderProximoTurno()
{
    Turno atendido = gestor.AtenderProximoTurno();

    Console.WriteLine("--- Turno atendido ---");
    Console.WriteLine($"  Tipo: {atendido.ObtenerTipo()}");
    if (atendido is TurnoUrgencia u)
        Console.WriteLine($"  Prioridad: {u.Prioridad}");
    Console.WriteLine($"  Paciente: {atendido.Paciente.Nombre} {atendido.Paciente.Apellido}");
    Console.WriteLine($"  Médico: {atendido.Medico.Nombre}");
    Console.WriteLine($"  Fecha: {atendido.FechaHora:dd/MM/yyyy HH:mm}");
    Console.WriteLine($"  Costo: ${atendido.CalcularCostoTurno():F2}");
}

void VerHistorial()
{
    IReadOnlyList<Turno> historial = gestor.ObtenerHistorial();

    Console.WriteLine("--- Historial de turnos atendidos ---");
    foreach (Turno t in historial)
    {
        string tipo = t.ObtenerTipo();
        if (t is TurnoUrgencia u)
            tipo += $" ({u.Prioridad})";

        Console.WriteLine($"  {t.FechaHora:dd/MM/yyyy HH:mm} | {tipo} | {t.Paciente.Nombre} {t.Paciente.Apellido} | {t.Medico.Nombre} | ${t.CalcularCostoTurno():F2}");
    }
}

void VerRecaudacion()
{
    Console.Write("Ingrese la fecha (dd/MM/yyyy): ");
    DateTime fecha = LeerFecha();

    decimal total = gestor.RecaudacionDelDia(fecha);
    Console.WriteLine($"Recaudación del {fecha:dd/MM/yyyy}: ${total:F2}");
}

void ListarTurnosDelDia()
{
    Console.Write("Ingrese la fecha (dd/MM/yyyy): ");
    DateTime fecha = LeerFecha();

    List<Turno> turnos = gestor.ObtenerTurnosDelDia(fecha);

    if (turnos.Count == 0)
    {
        Console.WriteLine("No hay turnos para esa fecha.");
        return;
    }

    Console.WriteLine($"--- Turnos del {fecha:dd/MM/yyyy} ---");
    for (int i = 0; i < turnos.Count; i++)
    {
        Turno t = turnos[i];

        string tipo = t.ObtenerTipo();

        Console.WriteLine($"  {t.FechaHora:HH:mm} | {tipo} | {t.Paciente.Nombre} {t.Paciente.Apellido} | {t.Medico.Nombre} | ${t.CalcularCostoTurno():F2}");
    }
}

void RegistrarMedico()
{
    Console.Write("Nombre del médico: ");
    string nombre = LeerTexto();

    Console.Write("Especialidad: ");
    string especialidad = LeerTexto();

    Console.Write("Valor consulta base: ");
    decimal valor = LeerDecimal();

    Medico medico = new(nombre, especialidad, valor);
    gestor.AgregarMedico(medico);
    Console.WriteLine($"Médico '{nombre}' registrado.");
}

void RegistrarPaciente()
{
    Console.Write("Nombre del paciente: ");
    string nombre = LeerTexto();

    Console.Write("Apellido: ");
    string apellido = LeerTexto();

    Console.Write("Fecha de nacimiento (dd/MM/yyyy): ");
    DateTime fechaNac = LeerFecha();

    Console.Write("¿Tiene obra social? (s/n): ");
    string? respuesta = Console.ReadLine()?.Trim().ToLower();

    Paciente paciente;
    if (respuesta == "s")
    {
        Console.Write("Nombre de la obra social: ");
        string obraSocial = LeerTexto();
        paciente = new Paciente(nombre, apellido, fechaNac, obraSocial);
    }
    else
    {
        paciente = new Paciente(nombre, apellido, fechaNac);
    }

    gestor.AgregarPaciente(paciente);
    Console.WriteLine($"Paciente '{nombre} {apellido}' registrado.");
}

void RegistrarTurno()
{
    // Seleccionar médico
    IReadOnlyList<Medico> medicos = gestor.ObtenerMedicos();
    Console.WriteLine("--- Médicos disponibles ---");
    for (int i = 0; i < medicos.Count; i++)
    {
        Console.WriteLine($"  {i + 1}. {medicos[i].Nombre} - {medicos[i].Especialidad} (${medicos[i].ValorConsultaBase})");
    }
    Console.Write("Seleccione médico: ");
    int idxMedico = LeerEntero(1, medicos.Count) - 1;
    Medico medico = medicos[idxMedico];

    // Seleccionar paciente
    IReadOnlyList<Paciente> pacientes = gestor.ObtenerPacientes();
    Console.WriteLine("--- Pacientes registrados ---");
    for (int i = 0; i < pacientes.Count; i++)
    {
        string os = pacientes[i].ObraSocial != null ? $" (OS: {pacientes[i].ObraSocial})" : "";
        Console.WriteLine($"  {i + 1}. {pacientes[i].Nombre} {pacientes[i].Apellido}{os}");
    }
    Console.Write("Seleccione paciente: ");
    int idxPaciente = LeerEntero(1, pacientes.Count) - 1;
    Paciente paciente = pacientes[idxPaciente];

    // Fecha y hora
    Console.Write("Fecha y hora del turno (dd/MM/yyyy HH:mm): ");
    DateTime fechaHora = LeerFechaHora();

    // Tipo de turno
    Console.WriteLine("Tipo de consulta:");
    Console.WriteLine("  1. General");
    Console.WriteLine("  2. Urgencia");
    Console.WriteLine("  3. Telemedicina");
    Console.Write("Seleccione tipo: ");
    int tipo = LeerEntero(1, 3);

    Turno turno;
    switch (tipo)
    {
        case 2:
            Console.WriteLine("Prioridad:");
            Console.WriteLine("  1. Alta");
            Console.WriteLine("  2. Media");
            Console.WriteLine("  3. Baja");
            Console.Write("Seleccione prioridad: ");
            int prio = LeerEntero(1, 3);
            Prioridad prioridad = (Prioridad)(prio - 1);
            turno = new TurnoUrgencia(medico, paciente, fechaHora, prioridad);
            break;
        case 3:
            Console.Write("Link de videollamada: ");
            string link = LeerTexto();
            turno = new TurnoTele(medico, paciente, fechaHora, link);
            break;
        default:
            turno = new Turno(medico, paciente, fechaHora);
            break;
    }

    gestor.RegistrarTurno(turno);
    Console.WriteLine($"Turno registrado. Costo: ${turno.CalcularCostoTurno():F2}");
}

// ---- Helpers de lectura con validación ----

string LeerTexto()
{
    string? input = Console.ReadLine()?.Trim();
    while (string.IsNullOrEmpty(input))
    {
        Console.Write("Entrada no válida, intente de nuevo: ");
        input = Console.ReadLine()?.Trim();
    }
    return input;
}

decimal LeerDecimal()
{
    while (true)
    {
        if (decimal.TryParse(Console.ReadLine()?.Trim(), out decimal valor) && valor > 0)
            return valor;
        Console.Write("Valor no válido, intente de nuevo: ");
    }
}

int LeerEntero(int min, int max)
{
    while (true)
    {
        if (int.TryParse(Console.ReadLine()?.Trim(), out int valor) && valor >= min && valor <= max)
            return valor;
        Console.Write($"Ingrese un número entre {min} y {max}: ");
    }
}

DateTime LeerFecha()
{
    while (true)
    {
        if (DateTime.TryParseExact(Console.ReadLine()?.Trim(), "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime fecha))
            return fecha;
        Console.Write("Formato no válido (dd/MM/yyyy), intente de nuevo: ");
    }
}

DateTime LeerFechaHora()
{
    while (true)
    {
        if (DateTime.TryParseExact(Console.ReadLine()?.Trim(), "dd/MM/yyyy HH:mm",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime fecha))
            return fecha;
        Console.Write("Formato no válido (dd/MM/yyyy HH:mm), intente de nuevo: ");
    }
}