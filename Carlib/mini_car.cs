using CarLibrary;
using CarType;

namespace Cars;

public class MiniCar : Car
{
    public override void TurboBoost()
    {
        Console.WriteLine("Im so mini");
    }
}

public class SportCar : Car
{
    public override void TurboBoost()
    {
        Console.WriteLine("Im a total beast");
    }
}
