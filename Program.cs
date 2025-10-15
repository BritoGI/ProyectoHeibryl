public class ProgramaTemporizador
{
    public static async Task Main(string[] args)
    {
        int horas = 0;
        int minutos = 0;
        TimeSpan duracionTotal;

        Console.WriteLine("--- ¡Bienvenid@ a Puro20! ---");
        Console.WriteLine("Por favor, introduce una duración entre 30 minutos y 3 horas.");

        while (true)
        {
            while (true)
            {
                Console.Write("\nIntroduce el número de horas (0-3): ");
                string horasInput = Console.ReadLine();
                if (int.TryParse(horasInput, out horas) && horas >= 0 && horas <= 3)
                {
                    break;
                }
                Console.WriteLine("Entrada inválida. Por favor, introduce un número entre 0 y 3.");
            }

            while (true)
            {
                Console.Write("Introduce el número de minutos (0-59): ");
                string minutosInput = Console.ReadLine();
                if (int.TryParse(minutosInput, out minutos) && minutos >= 0 && minutos < 60)
                {
                    break;
                }
                Console.WriteLine("Entrada inválida. Por favor, introduce un número entre 0 y 59.");
            }

            duracionTotal = new TimeSpan(horas, minutos, 0);

            if (duracionTotal.TotalMinutes >= 0 && duracionTotal.TotalMinutes <= 180)
            {
                break;
            }

            Console.WriteLine("\nLa duración total debe estar entre 30 minutos y 3 horas. Inténtalo de nuevo.");
        }

        using var cts = new CancellationTokenSource();

        Console.WriteLine($"\nEl temporizador se ejecutará durante {duracionTotal.TotalMinutes} minutos.");
        Console.WriteLine("Presiona Enter para detenerlo manualmente...");

        var tareaDelTemporizador = RunTimerAsync(duracionTotal, cts.Token);

        Console.ReadLine();

        if (!tareaDelTemporizador.IsCompleted)
        {
            cts.Cancel();
            Console.WriteLine("\nTemporizador detenido por el usuario.");
        }

        await tareaDelTemporizador;
        Console.WriteLine("\nFin del programa.");
    }

    public static async Task RunTimerAsync(TimeSpan duracion, CancellationToken token)
    {
        DateTime tiempoInicio = DateTime.Now;
        using PeriodicTimer periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(1));

        try
        {
            while (await periodicTimer.WaitForNextTickAsync(token))
            {
                TimeSpan tiempoTranscurrido = DateTime.Now - tiempoInicio;
                TimeSpan tiempoRestante = duracion - tiempoTranscurrido;

                if (tiempoRestante.TotalSeconds <= 0)
                {
                    Console.Clear();
                    DibujarArbol(10); // Dibuja el árbol completo al finalizar
                    Console.WriteLine("\n¡Tiempo terminado! ¡haz recibido un 35% de desscuento, te esperamos!");
                    break;
                }

                // Calcula la altura del árbol basada en el progreso.
                int alturaMaxima = 10;
                double progreso = tiempoTranscurrido.TotalSeconds / duracion.TotalSeconds;
                int alturaActual = (int)(progreso * alturaMaxima) + 1;

                // Limpia y muestra la línea con el tiempo restante y el árbol.
                Console.Clear();
                Console.WriteLine($"Tiempo restante: {tiempoRestante:hh\\:mm\\:ss}");
                DibujarArbol(alturaActual);
            }
        }
        catch (OperationCanceledException)
        {
            Console.Clear();
            Console.WriteLine("\nTemporizador cancelado.");
        }
        finally
        {
            Console.CursorVisible = true;
        }
    }

    /// <summary>
    /// Dibuja un árbol de asteriscos en la consola con la altura especificada.
    /// </summary>
    public static void DibujarArbol(int altura)
    {
        int anchoMaximo = (altura * 2) - 1;

        // Dibuja el follaje del árbol.
        for (int i = 0; i < altura; i++)
        {
            int numAsteriscos = (i * 2) + 1;
            string asteriscos = new string('*', numAsteriscos);
            string espacios = new string(' ', (anchoMaximo - numAsteriscos) / 2);
            Console.WriteLine($"{espacios}{asteriscos}");
        }

        // Dibuja el tronco.
        if (altura > 0)
        {
            string tronco = "*";
            string espaciosTronco = new string(' ', (anchoMaximo - 1) / 2);
            Console.WriteLine($"{espaciosTronco}{tronco}");
            Console.WriteLine($"{espaciosTronco}{tronco}");
        }
    }
}

