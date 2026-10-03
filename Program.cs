using System.Diagnostics;

Console.WriteLine("======================================");
Console.WriteLine(" UNIVERSIDAD ESTATAL AMAZONICA");
Console.WriteLine(" PRACTICA DE GRAFOS");
Console.WriteLine("======================================");

Stopwatch tiempoTotal = Stopwatch.StartNew();

ProcesarGrafo(
    "grafo1.dot",
    "grafo1.png",
    0
);

ProcesarGrafo(
    "grafo2.dot",
    "grafo2.png",
    0
);

tiempoTotal.Stop();

Console.WriteLine("\n======================================");
Console.WriteLine(
    $"Tiempo total de ejecución: " +
    $"{tiempoTotal.Elapsed.TotalMilliseconds:F4} ms"
);
Console.WriteLine("======================================");


static void ProcesarGrafo(
    string archivo,
    string imagen,
    int nodoConsulta)
{
    Console.WriteLine("\n\n======================================");
    Console.WriteLine($" PROCESANDO: {archivo}");
    Console.WriteLine("======================================");

    Stopwatch tiempo = Stopwatch.StartNew();

    Grafo g = Grafo.Leer(archivo);

    tiempo.Stop();

    Console.WriteLine(
        $"Vértices: {g.CantidadVertices()}"
    );

    Console.WriteLine(
        $"Aristas: {g.CantidadAristas()}"
    );

    g.MostrarGrafo();

    g.ConsultarNodo(nodoConsulta);

    g.RecorridoDFS(0);

    g.RecorridoBFS(0);

    g.Dibujar(imagen);

    Console.WriteLine(
        $"Imagen creada: {imagen}"
    );

    Console.WriteLine(
        $"Tiempo de lectura y procesamiento: " +
        $"{tiempo.Elapsed.TotalMilliseconds:F4} ms"
    );
}

