using MPO.Project1;
using System.Diagnostics.Metrics;
using System.Globalization;

namespace paralell1_not_derevo;

class Program
{
    private const bool WITH_LOCK = true;
    private const int WORKING_TIME = 10;
    private const int PRINT_TIME = 2;

    static void Main()
    {
        int N, K;
        double p = 0;

        Console.Write("Enter N: ");
        N = Int32.Parse(Console.ReadLine());
        Console.Write("Enter K: ");
        K = Int32.Parse(Console.ReadLine());
        while (!(p > 0 && p < 1))
        {
            Console.Write("Enter p: ");
            p = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
        }

        var crystal = new Crystal(N, K, p, WITH_LOCK);

        var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        TimerCallback callback = new TimerCallback(PrintInfo);

        crystal.Start(token);
        Timer timer = new Timer(callback, crystal, TimeSpan.Zero, TimeSpan.FromSeconds(PRINT_TIME));
        Thread.Sleep(TimeSpan.FromSeconds(WORKING_TIME));

        tokenSource.Cancel();

        timer.Change(Timeout.Infinite, Timeout.Infinite);

        Console.WriteLine("");
        Console.WriteLine("");
        Console.WriteLine("============= Final Result is ===============");
        PrintInfo(crystal);
    }

    public static void PrintInfo(object crystalObj)
    {
        var crystal = (Crystal)crystalObj;
        var cells = crystal.Cells.ToList();
        var sum = cells.Sum();

        Console.WriteLine("=============================================");
        for (int i = 0; i < cells.Count; i++)
            Console.Write(cells[i] + "   ");
        Console.WriteLine("");
        Console.WriteLine($"----------- Sum   = {sum} --------------");
    }
}