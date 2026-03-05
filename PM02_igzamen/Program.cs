using System;
using System.IO;
using System.Linq;

class Program
{

    public const int MAX_NODES = 100;





    public static int numNodes;


    public static double[,] weightsMatrix;

    public static void ReadGraphFromFile(string fileName)
    {
        try
        {
            string[] lines = File.ReadAllLines(fileName);

            numNodes = Convert.ToInt32(lines[0]);


            weightsMatrix = new double[numNodes, numNodes];

            for (int i = 0; i < numNodes; i++)
            {
                for (int j = 0; j < numNodes; j++)
                {
                    weightsMatrix[i, j] = Double.PositiveInfinity;
                }
                weightsMatrix[i, i] = 0;
            }


            foreach (var line in lines.Skip(1))
            {
                var parts = line.Split(' ');
                int fromNode = Convert.ToInt32(parts[0]) - 1;
                int toNode = Convert.ToInt32(parts[1]) - 1;
                double weight = Convert.ToDouble(parts[2]);

                weightsMatrix[fromNode, toNode] = weight;
                weightsMatrix[toNode, fromNode] = weight;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка чтения файла: {0}", ex.Message);
            Console.ReadLine();
            Environment.Exit(-1);
        }
    }

    // Модуль реализации алгоритма Флойда
    private static double[,] Floyd(double[,] a)
    {
        double[,] d = new double[numNodes, numNodes];

        Array.Copy(a, d, a.Length);

        for (int i = 0; i < numNodes; i++)
        {
            for (int j = 0; j < numNodes; j++)
            {
                for (int k = 0; k < numNodes; k++)
                {
                    if (d[j, k] > d[j, i] + d[i, k])
                    {
                        d[j, k] = d[j, i] + d[i, k];
                    }
                }
            }
        }
        return d;
    }
    // коментарий для комита

    public static void FindShortestPathForThreePoints()
    {
        while (true)
        {
            Console.Write("Введите номер первой точки (или 0 для выхода): ");
            int pointA = Convert.ToInt32(Console.ReadLine());

            if (pointA == 0) break;

            Console.Write("Введите номер второй точки: ");
            int pointB = Convert.ToInt32(Console.ReadLine());

            Console.Write("Введите номер третьей точки: ");
            int pointC = Convert.ToInt32(Console.ReadLine());

            if (pointB == 0 || pointC == 0) break;


            double[,] shortestPaths = Floyd(weightsMatrix);


            int startPoint = Math.Min(Math.Min(pointA, pointB), pointC) - 1;
            int endPoint = Math.Max(Math.Max(pointA, pointB), pointC) - 1;
            int middlePoint = (pointA != startPoint && pointA != endPoint) ? pointA : ((pointB != startPoint && pointB != endPoint) ? pointB : pointC) - 1;


            double totalDistance = shortestPaths[startPoint, middlePoint] + shortestPaths[middlePoint, endPoint];

            Console.WriteLine($"Кратчайший маршрут проходит через точки: {startPoint + 2}, {middlePoint + 2}, {endPoint + 2}");
            Console.WriteLine($"Общая длина маршрута: {totalDistance:F2}\n");
        }
    }

    static void Main(string[] args)
    {
        Console.Write("Введите имя файла с картой города: ");
        string filename = $"{Console.ReadLine()}.txt";

        ReadGraphFromFile(filename);

        FindShortestPathForThreePoints();
    }
}