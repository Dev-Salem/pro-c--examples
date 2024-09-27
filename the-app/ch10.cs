namespace Ch10;

public interface IAnimal
{
    public abstract void MakeSound();
}

public class Dog : IAnimal, IDisposable, ICloneable, IComparable<Dog>
{
    public int Id { get; init; }
    private bool disposed;
    private readonly Lazy<string[]> DogFoodBrands =
        new(() => ["Dog", "Dog", "Dog", "Dog", "Dog", "Dog", "Dog"]);
    private readonly Lazy<List<string>> clinic = new(["Doggy", "Happy Dog"]);
    public Stack<int> Ids = [];
    public Queue<int> IdsQueue = [];
    public LinkedList<int> IdsLinkedList = [];
    public Dictionary<int, string> names = [];

    public string[] GetAllFoodBrands() => DogFoodBrands.Value;

    public List<string> GetClinic() => clinic.Value;

    public void Dispose()
    {
        CleanUp(true);
        GC.SuppressFinalize(this);
    }

    private void CleanUp(bool isCalled)
    {
        void CleanManagedCode() { }
        void CleanUnmanagedCode() { }
        if (!disposed)
        {
            if (isCalled)
            {
                CleanManagedCode();
            }
            CleanUnmanagedCode();
        }
        disposed = true;
    }

    ~Dog() => CleanUp(false);

    public void MakeSound()
    {
        Console.WriteLine("Hw Hw!");
    }

    public object Clone()
    {
        return new Dog(0);
    }

    public Dog(int id) => Id = id;

    public int CompareTo(Dog? other)
    {
        if (Id > other?.Id)
        {
            return 1;
        }
        else if (other?.Id > Id)
        {
            return -1;
        }
        return 0;
    }

    public override string ToString()
    {
        return $"(Dog {Id})";
    }
}
