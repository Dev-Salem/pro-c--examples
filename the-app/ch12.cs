//Delegates, events
using Ch11;

namespace Ch12;

public delegate int BinaryOp(int a, int b);

public class SimpleMath
{
    static int Add(int a, int b) => a + b;

    static int Multiply(int a, int b) => a * b;

    public int Divide(int a, int b) => a / b;

    public static void Example()
    {
        BinaryOp addDelegate = new(SimpleMath.Add);
        addDelegate(1, 2).Print();
        addDelegate.Invoke(4, 2).Print();

        BinaryOp multiplyDelegate = new(SimpleMath.Multiply);
        multiplyDelegate(44, 2).Print();
        multiplyDelegate(-1, 4).Print();

        BinaryOp divideDelegate = new(new SimpleMath().Divide);
        divideDelegate(12, 6).Print();
        divideDelegate.Invoke((int)3.1, 1).Print();

        BinaryOp addLambda = new((a, b) => a + b);
        addLambda.Invoke(3, 2);

        BinaryOp squareLambda = new((a, _) => a * a);
        squareLambda.Invoke(3, 2);
    }
}

public class RealTimeChat
{
    private Stack<string> messages = [];

    public delegate void NotifierUsers(string messages, string action);
    public delegate void TestDelegate<T>(T num1, T num2);
    private NotifierUsers? _notifier;

    public void RegisterNotifier(NotifierUsers notifier)
    {
        //alternative syntax
        // _notifier = (NotifierUsers)Delegate.Combine(_notifier, notifier);
        _notifier += notifier;
    }

    public void UnRegisterNotifier(NotifierUsers notifier)
    {
        _notifier -= notifier;
    }

    public void AddMessage(string message)
    {
        _notifier?.Invoke(message, "add");
        messages.Push(message);
    }

    public void ClearMessages()
    {
        _notifier?.Invoke("All Messages", "clear");
        messages.Clear();
    }

    public void PopMessage()
    {
        _notifier?.Invoke(messages.Peek(), "pop");
        messages.Pop();
    }

    //Action and Func
    public static void Example2()
    {
        static int multiplyNumbers(int a, int b)
        {
            return a * b;
        }
        Action<int, int> sumDelegate = (int a, int b) => (a + b).Print();
        Func<int, int, int> multiplyDelegate = multiplyNumbers;
        sumDelegate.Invoke(3, 2);
        multiplyDelegate.Invoke(1, 6).Print();
    }

    //custom Delegates, group conversion, generic delegates
    public static void Example()
    {
        RealTimeChat chat = new();
        TestDelegate<int> testDelegate = new(testMethod);
        NotifierUsers notifier = new(LogMessage);
        NotifierUsers emojiNotifier = new(EmojiLogger);
        chat.RegisterNotifier(notifier);
        chat.RegisterNotifier(emojiNotifier);
        chat.AddMessage("Hello");
        chat.AddMessage("Hi you!");
        // chat.UnRegisterNotifier(EmojiLogger);
        chat.AddMessage("Nothing happened");
        chat.PopMessage();
        chat.ClearMessages();
        notifier.Invoke("Just test from emoji", "test");
        void testMethod(int a, int b)
        {
            $"{a} + {b} = {a + b}, Tda!".Print();
        }
        void LogMessage(string message, string action)
        {
            $"\"{message}\" was {action}ed".Print();
        }

        void EmojiLogger(string message, string action)
        {
            string emoji = "⁉️⁉️❓";
            if (action == "add")
            {
                emoji = "💡💡";
            }
            else if (action == "pop")
            {
                emoji = "🔥🔥";
            }
            else if (action == "clear")
            {
                emoji = "💣";
            }
            $"{action}{emoji} was Performed".Print();
        }
        // testDelegate.Invoke(2, 4);
    }
}

public class StockMarket
{
    private double _currentPrice = 50;
    public delegate void StockMarketNotifier(double currentPrice);
    public event StockMarketNotifier? CrashedMarket;
    public event StockMarketNotifier? ThrivingMarket;
    public event StockMarketNotifier? StableMarket;

    public void ChangeStockPrice()
    {
        var random = new Random();
        double newPrice = double.Round((random.NextDouble() * 100));
        if (Math.Abs(newPrice - _currentPrice) < 10)
        {
            "Market is stable".Print();
            StableMarket?.Invoke(newPrice);
        }
        else if ((newPrice - _currentPrice) > 10)
        {
            $"Market is thriving (from {_currentPrice} to {newPrice})".Print();
            ThrivingMarket?.Invoke(newPrice);
        }
        else if (newPrice - _currentPrice < -10)
        {
            $"Market is crushed (from {_currentPrice} to {newPrice})".Print();
            CrashedMarket?.Invoke(newPrice);
        }
        _currentPrice = newPrice;
    }

    //Events Explained;
    //1- in a normal delegate, you define the delegate first, the delegate represents
    //a call back a to function with a certain return type and parameters
    //then you simple declare a member of the delegate and assign it a function name
    //then you can invoke the function indirectly through the delegate using `Invoke` as shown
    // in SimpleMath.Example()

    //2- the next enhancement is a surge syntax, instead of (define a public delegate,
    //define a member, assign a function to the member you can simple use `Action` or `Fun`
    //they combine the first and second steps of the process. as show in RealTimeChat.Example2()

    //3- the third enhancement is combing multiple delegate to one delegate += operator
    //or alternatively using Delegate.Combine(old, new).When you invoke the delegate in which
    //older delegate where combined to, all delegates fire their respective functions. also instead
    //of combining delegate you can use Group Conversion to combine functions that are turned to
    //delegate at run time, see RealTimeChat.Example()

    //4- fourth concept is Events. an Event is just an alternative to defining multiple members
    //of the same delegate and combining them together, instead you define an event inside the
    //class that contains the delegate, now instead of making instance members of the delegate you
    //can reference the events that are already defined and add them to your delegate
    public static void Example()
    {
        void LogPrices(double currentPrice)
        {
            Console.Write("Current Price {0}\t", currentPrice);
        }
        StockMarket stockMarket = new();
        var stockDelegate = new StockMarketNotifier(LogPrices);
        stockMarket.CrashedMarket += stockDelegate;
        stockMarket.StableMarket += stockDelegate;
        stockMarket.ThrivingMarket += stockDelegate;
        stockMarket.ChangeStockPrice();
        stockMarket.ChangeStockPrice();
        stockMarket.ChangeStockPrice();
        stockMarket.ChangeStockPrice();
        stockMarket.ChangeStockPrice();
        stockMarket.ChangeStockPrice();
        stockMarket.ChangeStockPrice();
        stockMarket.ChangeStockPrice();
    }

    public static void Example2()
    {
        List<int> numbers = [1, 2, 3, 4, 5, 6, 7, 9, 10, 11, 12];
        int someNumber = 0;
        var evenNumbers = numbers.FindAll(
            (x) =>
            {
                someNumber++;
                Console.Write(x + ", ");
                return x % 2 == 0;
            }
        );

        var staticLambda = numbers.FindAll(
            static (x) =>
            {
                //can't edit things outside the scope that are non static
                // someNumber++;
                return x % 2 == 0;
            }
        );
        Predicate<int> callback = IsEvenNumber;
        static bool IsEvenNumber(int i)
        {
            // Is it an even number?
            return (i % 2) == 0;
        }
        foreach (var item in evenNumbers)
        {
            Console.WriteLine(item);
        }
    }
}

public class NoteApp
{
    public delegate void noteLogger(string note);
    public AddNoteEvent addEvent;
    public RemoveNoteEvent removeEvent;
    private List<string> _notes = [];

    public void AddNote(string note)
    {
        _notes.Add(note);
    }

    public void RemoveNote(string note) => _notes.Remove(note);

    public class NoteEvent(string note) : EventArgs
    {
        public readonly string Note = note;
    }

    public class AddNoteEvent(string note) : NoteEvent(note) { }

    public class RemoveNoteEvent(string note) : NoteEvent(note) { }

    public static void Exmple()
    {
        NoteApp myApp = new();

        void LoggerHandler(string message)
        {
            Console.WriteLine("Something happened... with {0}", message);
        }
    }
}

public class Exercise
{
    public class AlarmClock
    {
        public delegate void AlarmRingsDelegate(string message);
        public AlarmRingsDelegate notifier;
        public event AlarmRingsDelegate alarmRings;

        public void RingAlarm(string message)
        {
            "AlarmRings".Print();
            alarmRings.Invoke(message);
        }
    }

    public class Person
    {
        public string WakeUp()
        {
            return "Wake up!";
        }
    }

    public static void Example()
    {
        AlarmClock clock = new();
        Person person = new();
        clock.alarmRings += clock.notifier;
        clock.RingAlarm(person.WakeUp());
    }
}
