//Multi Threading, Parallel, and Async
using System.Threading;
using Ch11;

namespace Ch15;

public class ThreadingExamples
{
    public static void BasicInfoExample()
    {
        Thread current = Thread.CurrentThread;
        current.Name = "Main Thread";
        $@"
        {current.Name}
        {current.Priority}
        {current.ManagedThreadId}
        {current.IsAlive}
        {current.ThreadState}
        {current.Priority} ".Print();
    }

    private static readonly AutoResetEvent waitHandle = new(false);

    public static void WaitForThreadExample()
    {
        Thread.CurrentThread.Name = "Primary";

        $"Current Thread Name: {Thread.CurrentThread.Name}".Print();
        TwoThreads();
        // waitHandle.WaitOne();
        $"Current Thread Name: {Thread.CurrentThread.Name}".Print();
    }

    public static void TwoThreads()
    {
        var secondThread = new Thread(new ParameterizedThreadStart(Printer)) { Name = "Secondary" };
        secondThread.Start("Salem");

        static void Printer(object name)
        {
            $"Current Thread Name: {Thread.CurrentThread.Name}".Print();
            for (int i = 0; i < 10; i++)
            {
                $"{i}-{name}".Print();
                Thread.Sleep(500);
            }
            $"Current Thread Name: {Thread.CurrentThread.Name}".Print();
            waitHandle.Set();
        }
    }

    public static void BackgroundThreadExample()
    {
        var backgroundThread = new Thread(new ParameterizedThreadStart(JustPrinter))
        {
            Name = "Background",
        };
        // backgroundThread.IsBackground = true;
        "In Primary Thread".Print();
        backgroundThread.Start("Salem");
        "Still in Primary Thread".Print();
        static void JustPrinter(object name)
        {
            $"Current Thread Name: {Thread.CurrentThread.Name}".Print();
            for (int i = 0; i < 10; i++)
            {
                $"{i}-{name}".Print();
                Thread.Sleep(500);
            }
            $"Current Thread Name: {Thread.CurrentThread.Name}".Print();
        }
    }

    class SharedPrinter
    {
        public void RandomPrinter()
        {
            lock (this) //This will prevent other threads from accessing until the current thread finishes
            {
                $"\nWorker Thread: {Thread.CurrentThread.Name}".Print();
                for (int i = 0; i < 10; i++)
                {
                    Console.Write(i + ", ");
                    Random randomNumber = new();
                    Thread.Sleep(100 * randomNumber.Next(5));
                }
            }
        }
    }

    public static void ProblemOfConcurrencyExample()
    {
        Thread[] threads = new Thread[10];
        SharedPrinter printer = new();
        for (int i = 0; i < 10; i++)
        {
            threads[i] = new Thread(new ThreadStart(printer.RandomPrinter))
            {
                Name = $"(Thread{i})",
            };
        }
        foreach (var item in threads)
        {
            item.Start();
        }
    }

    public static void TimerExample()
    {
        TimerCallback timerCB = new((_) => DateTime.Now.ToLongTimeString().Print());
        _ = new Timer(timerCB, null, 0, 1000);
        Console.ReadLine();
    }

    public static void ThreadPoolExample()
    {
        Console.WriteLine(
            "Main thread started. ThreadID = {0}",
            Environment.CurrentManagedThreadId
        );
        SharedPrinter p = new();
        WaitCallback workItem = new WaitCallback(PrintTheNumbers);
        // Queue the method ten times.
        for (int i = 0; i < 10; i++)
        {
            ThreadPool.QueueUserWorkItem(workItem, p);
        }
        Console.WriteLine("All tasks queued");
        Console.ReadLine();
        static void PrintTheNumbers(object state)
        {
            SharedPrinter task = (SharedPrinter)state;
            task.RandomPrinter();
        }
    }

    public static void TLPExample()
    {
        List<(string name, int age)> people =
        [
            ("John", 32),
            ("Mary", 23),
            ("Murray", 54),
            ("Alex", 32),
            ("Andrew", 93),
        ];
        DateTime startDate = DateTime.Now;
        Parallel.ForEach(
            people,
            person =>
                $"Thread: {Environment.CurrentManagedThreadId}, name: {person.name}, age: {person.age}".Print()
        );
        $"Execution Time: {startDate.Subtract(DateTime.Now)}".Print();
    }

    public static List<(string name, int age)> people =
    [
        ("John", 32),
        ("Mary", 23),
        ("Murray", 54),
        ("Alex", 32),
        ("Andrew", 93),
    ];

    public static void NoTLPExample()
    {
        DateTime startDate = DateTime.Now;
        foreach (var (name, age) in people)
        {
            $"Thread: {Environment.CurrentManagedThreadId}, name: {name}, age: {age}".Print();
        }
        $"Execution Time: {startDate.Subtract(DateTime.Now)}".Print();
    }

    public static void PLinqExample()
    {
        CancellationTokenSource cancelToken = new();
        DateTime startDate = DateTime.Now;
        int[] numbers = Enumerable.Range(1, 10_000_000).ToArray();
        int[] dividerByThree = (
            from i in numbers.AsParallel().WithCancellation(cancelToken.Token)
            where i % 12 == 0
            select i
        ).ToArray();
        dividerByThree.Length.Print();
        $"Execution Time: {startDate.Subtract(DateTime.Now)}".Print();
    }

    public static async Task AsyncExampleAsync()
    {
        string message = await DoWork();
        message.Print();
        async Task<string> DoWork()
        {
            return await Task.Run(() =>
            {
                Thread.Sleep(3000);
                return "Finished Working";
            });
        }
    }

    public static async Task AsyncVoidMethod()
    {
        "Fire and forget 1".Print();
        // await Task.Run(() =>
        // {
        //     Thread.Sleep(1000);
        //     "Fire and forget From task 1".Print();
        //     // throw new Exception("Can't see this exception");
        // });
        // await Task.Run(() =>
        // {
        //     Thread.Sleep(1000);
        //     "Fire and forget from task 2".Print();
        //     // throw new Exception("Can't see this exception");
        // });
        // await Task.Run(() =>
        // {
        //     Thread.Sleep(1000);
        //     "Fire and forget from task 3".Print();
        //     // throw new Exception("Can't see this exception");
        // });

        await Task.WhenAll(
            Task.Run(() =>
            {
                Thread.Sleep(900);
                "Fire and forget From task 1".Print();
                // throw new Exception("Can't see this exception");
            }),
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                "Fire and forget from task 2".Print();
                // throw new Exception("Can't see this exception");
            }),
            Task.Run(() =>
            {
                Thread.Sleep(1100);
                "Fire and forget from task 3".Print();
                // throw new Exception("Can't see this exception");
            })
        );
        "Fire and forget 3".Print();
    }

    public static async Task CancelAwaitExample()
    {
        CancellationTokenSource cancelToken = new();
        // // cancelToken.Cancel();
        await AsyncVoidMethod().WaitAsync(TimeSpan.FromSeconds(1), cancelToken.Token);

        AsyncVoidMethod().Wait(500); //cancel without waiting
    }

    public static async ValueTask<int> ValueTaskExample()
    {
        await Task.Delay(100);
        return 29;
    }

    public static async Task AsyncStreamExample()
    {
        await foreach (var item in EnumerableAsyncExample())
        {
            item.Print();
        }
        static async IAsyncEnumerable<(string name, int age)> EnumerableAsyncExample()
        {
            foreach (var item in people)
            {
                await Task.Delay(1000);
                yield return item;
            }
        }
    }

    public static async Task TLPExample2()
    {
        List<(string name, int age)> people =
        [
            ("John", 32),
            ("Mary", 23),
            ("Murray", 54),
            ("Alex", 32),
            ("Andrew", 93),
        ];
        CancellationTokenSource tokenSource = new();
        ParallelOptions options = new() { CancellationToken = tokenSource.Token };
        DateTime startDate = DateTime.Now;
        await Parallel.ForEachAsync(
            people,
            options,
            async (person, token) =>
            {
                token.ThrowIfCancellationRequested();
                // if (person.age > 70)
                // {
                //     tokenSource.Cancel();
                // }

                await Task.Delay(1000, token);
                $"Thread: {Environment.CurrentManagedThreadId}, name: {person.name}, age: {person.age}".Print();
            }
        );
        $"Execution Time: {startDate.Subtract(DateTime.Now)}".Print();
    }

    public static async Task BookReaderExample()
    {
        HttpClient client = new();
        string? ebook = await client.GetStringAsync("http://www.gutenberg.org/files/98/98-0.txt");
        Console.WriteLine("Download complete.");
        GetStats(ebook);
        static void GetStats(string? text)
        {
            if (text is string book)
            {
                var splittedText = text.Split([',', ' ', '.', '-', ';']).AsParallel();
                var longestWord = (
                    from word in splittedText
                    where word.Length > 5
                    orderby word descending
                    select word
                ).AsParallel();
                Console.WriteLine($"Longest word is {longestWord.ToList()[0]}");
            }
            else
            {
                "Received Empty Book".Print();
            }
        }
    }
}
