using System.Diagnostics;
using System.Text;

public class Grafo
{
    private List<int>[] adj;
    private bool dirigido;

    public Grafo(int vertices, bool dirigido)
    {
        this.dirigido = dirigido;

        adj = new List<int>[vertices];

        for (int i = 0; i < vertices; i++)
        {
            adj[i] = new List<int>();
        }
    }

    public static Grafo Leer(string archivo)
    {
        string[] lineas = File.ReadAllLines(archivo);

        string[] primera = lineas[0].Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries
        );

        int vertices = int.Parse(primera[0]);
        bool dirigido = primera[1] == "true";

        Grafo g = new Grafo(vertices, dirigido);

        for (int i = 1; i < lineas.Length; i++)
        {
            if (lineas[i].Trim() == "")
                continue;

            string[] p = lineas[i].Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries
            );

            int origen = int.Parse(p[0]);
            int destino = int.Parse(p[1]);

            g.AgregarArista(origen, destino);
        }

        return g;
    }

    public void AgregarArista(int origen, int destino)
    {
        adj[origen].Add(destino);

        if (!dirigido)
        {
            adj[destino].Add(origen);
        }
    }

    public void MostrarGrafo()
    {
        Console.WriteLine("\n--- LISTA DE ADYACENCIA ---");

        for (int i = 0; i < adj.Length; i++)
        {
            Console.Write($"Nodo {i}: ");

            if (adj[i].Count == 0)
            {
                Console.WriteLine("sin conexiones");
            }
            else
            {
                Console.WriteLine(
                    string.Join(" -> ", adj[i])
                );
            }
        }
    }

    public void ConsultarNodo(int nodo)
    {
        Console.WriteLine("\n--- CONSULTA DE NODO ---");

        if (nodo < 0 || nodo >= adj.Length)
        {
            Console.WriteLine("El nodo no existe.");
            return;
        }

        Console.WriteLine($"Nodo consultado: {nodo}");

        if (adj[nodo].Count == 0)
        {
            Console.WriteLine("No tiene conexiones.");
        }
        else
        {
            Console.WriteLine(
                "Conectado con: " +
                string.Join(", ", adj[nodo])
            );
        }
    }

    public void RecorridoDFS(int inicio)
    {
        Console.WriteLine("\n--- RECORRIDO DFS ---");

        if (inicio < 0 || inicio >= adj.Length)
        {
            Console.WriteLine("Nodo inicial no válido.");
            return;
        }

        bool[] visitado = new bool[adj.Length];

        DFS(inicio, visitado);

        Console.WriteLine();
    }

    private void DFS(int nodo, bool[] visitado)
    {
        if (visitado[nodo])
            return;

        visitado[nodo] = true;

        Console.Write(nodo + " ");

        foreach (int vecino in adj[nodo])
        {
            DFS(vecino, visitado);
        }
    }

    public void RecorridoBFS(int inicio)
    {
        Console.WriteLine("\n--- RECORRIDO BFS ---");

        if (inicio < 0 || inicio >= adj.Length)
        {
            Console.WriteLine("Nodo inicial no válido.");
            return;
        }

        bool[] visitado = new bool[adj.Length];

        Queue<int> cola = new Queue<int>();

        cola.Enqueue(inicio);
        visitado[inicio] = true;

        while (cola.Count > 0)
        {
            int actual = cola.Dequeue();

            Console.Write(actual + " ");

            foreach (int vecino in adj[actual])
            {
                if (!visitado[vecino])
                {
                    visitado[vecino] = true;
                    cola.Enqueue(vecino);
                }
            }
        }

        Console.WriteLine();
    }

    public int CantidadVertices()
    {
        return adj.Length;
    }

    public int CantidadAristas()
    {
        int total = 0;

        for (int i = 0; i < adj.Length; i++)
        {
            total += adj[i].Count;
        }

        if (!dirigido)
        {
            total /= 2;
        }

        return total;
    }

    public void Dibujar(string salida)
    {
        StringBuilder sb = new StringBuilder();

        // Determinar el tipo de grafo para Graphviz
        sb.AppendLine(
            dirigido ? "digraph G {" : "graph G {"
        );

        sb.AppendLine("  rankdir=LR;");

        // Generar las conexiones
        for (int v = 0; v < adj.Length; v++)
        {
            foreach (int w in adj[v])
            {
                // En grafos no dirigidos evitamos duplicar las aristas
                if (!dirigido && w < v)
                    continue;

                if (dirigido)
                {
                    sb.AppendLine(
                        $"  {v} -> {w};"
                    );
                }
                else
                {
                    sb.AppendLine(
                        $"  {v} -- {w};"
                    );
                }
            }
        }

        sb.AppendLine("}");

        // Crear un archivo diferente al archivo de entrada
        // para no sobrescribir grafo1.dot o grafo2.dot
        string archivoDot =
            Path.GetFileNameWithoutExtension(salida)
            + "_graphviz.dot";

        // Guardar el archivo compatible con Graphviz
        File.WriteAllText(
            archivoDot,
            sb.ToString()
        );

        // Ejecutar Graphviz para generar la imagen PNG
        Process.Start(
            new ProcessStartInfo
            {
                FileName = "dot",
                Arguments =
                    $"-Tpng \"{archivoDot}\" -o \"{salida}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            }
        )?.WaitForExit();
    }
}