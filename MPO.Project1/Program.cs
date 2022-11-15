using System.Globalization;

namespace paralell1_not_derevo;

class Program
{
    static int[] array;
    static Random rand = new Random();
    static int Counter = 0;

    const int TIMER_PERIOD = 10000; //10 sec.

    static void Main(string[] args)
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

        TimerCallback tm = new TimerCallback(PrintInfo);
        Timer timer = new Timer(tm, null, TIMER_PERIOD, TIMER_PERIOD);

        array = new int[N];
        array[0] = K;

        Thread[] threads = new Thread[K];
        for (int i = 0; i < K; i++)
        {
            threads[i] = new Thread(new ParameterizedThreadStart(Cells0));
            //threads[i] = new Thread(new ParameterizedThreadStart(Cells1));
        }

        for (int i = 0; i < K; i++)
        {
            threads[i].Start(p);
        }

        for (int i = 0; i < K; i++)
        {
            threads[i].Join();
        }

        Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
        Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
        Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
        PrintInfo(null);

    }

    public static void Cells0(object P)
    {
        double p = (Double)P;
        double m;
        int posInGeneralArray = 0;

        while (!Console.KeyAvailable)
        {
            m = rand.NextDouble();
            if (m > p && posInGeneralArray < array.Length - 1)
            {
                posInGeneralArray += 1;
                lock (array.SyncRoot)
                {
                    array[posInGeneralArray - 1] -= 1;
                    array[posInGeneralArray] += 1;
                }
            }
            if (m < p && posInGeneralArray > 0)
            {
                posInGeneralArray -= 1;
                lock (array.SyncRoot)
                {
                    array[posInGeneralArray] += 1;
                    array[posInGeneralArray + 1] -= 1;
                }
            }
            Counter++;
        }

    }


    public static void Cells1(object P)
    {
        double p = (Double)P;
        double m;
        int posInGeneralArray = 0;

        while (!Console.KeyAvailable)
        {
            m = rand.NextDouble();
            if (m > p && posInGeneralArray < array.Length - 1)
            {
                posInGeneralArray += 1;
                lock ((object)array[posInGeneralArray - 1])
                {
                    array[posInGeneralArray - 1] -= 1;
                }
                lock ((object)array[posInGeneralArray])
                {
                    array[posInGeneralArray] += 1;
                }
            }
            if (m < p && posInGeneralArray > 0)
            {
                posInGeneralArray -= 1;
                lock ((object)array[posInGeneralArray])
                {
                    array[posInGeneralArray] += 1;
                }
                lock ((object)array[posInGeneralArray + 1])
                {
                    array[posInGeneralArray + 1] -= 1;
                }
            }
            Counter++;
        }
    }


    static void PrintInfo(object o)
    {
        Console.WriteLine("=============================================");
        for (int i = 0; i < array.Length; i++)
            Console.Write(array[i] + "   ");

        int sum = 0;
        for (int i = 0; i < array.Length; i++)
            sum += array[i];
        Console.WriteLine("");
        Console.WriteLine("----------- Sum   = {0} --------------", sum);
        Console.WriteLine("----------- Count = {0} -----------", Counter);
        Counter = 0;
        Console.WriteLine("");
    }

}
