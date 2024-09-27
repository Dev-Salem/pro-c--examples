using CarType;

namespace CarLibrary;

// The abstract base class in the hierarchy.
public abstract class Car
{
    public string PetName { get; set; }
    public int CurrentSpeed { get; set; }
    public int MaxSpeed { get; set; }
    protected CarTypeEnum carType = CarTypeEnum.Automatic;
    public CarTypeEnum type => carType;
    public abstract void TurboBoost();

    protected Car() { }

    protected Car(string name, int maxSpeed, int currentSpeed)
    {
        PetName = name;
        MaxSpeed = maxSpeed;
        CurrentSpeed = currentSpeed;
    }
}
