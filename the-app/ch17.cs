//Type Reflection, late binding, attributes, dynamic types

using System.Reflection;
// using Cars;
using Ch11;
using ObservableExample;

namespace Ch17;

public class ReflectionExamples
{
    [JsonIgnore]
    public String Name { get; set; }

    [HeavyInitialization("This is a very long list, be careful when initializing it")]
    public static int[] longList = Enumerable.Range(0, 1000000).ToArray();

    public static void TypeExample()
    {
        Person person = new();
        Type typeOfPerson = person.GetType();
        Type typeOfPerson2 = typeof(Person);
        Type typeOfPerson3 = Type.GetType("ObservableExample.Person", true, true);
        Type externalType = Type.GetType("Car.MiniCar, Carlib");
        Console.WriteLine(typeOfPerson);
        Console.WriteLine(typeOfPerson2);
        Console.WriteLine(typeOfPerson3);
        Console.WriteLine(externalType);
        GetMethods(typeOfPerson);
        static void GetMethods(Type t)
        {
            (from method in t.GetMethods() select method)
                .ToList()
                .ForEach(x =>
                    Console.WriteLine($"{x.ReturnType.FullName}\t{x.Name}\t{x.GetParameters}")
                );
            "\n====\n".Print();
            (from method in t.GetFields() select method.Name)
                .ToList()
                .ForEach(x => Console.Write(x + ", "));
            "\n====\n".Print();
            (from method in t.GetInterfaces() select method.Name)
                .ToList()
                .ForEach(x => Console.Write(x + ", "));
        }
    }

    public static Assembly LoadExternalAssemblyExample()
    {
        Assembly asm = Assembly.LoadFrom(
            "/Users/devsalem/Desktop/dot-net-lab/Carlib/obj/Debug/net8.0/Carlib.dll"
        );
        Console.WriteLine("\n***** Types in Assembly *****");
        Console.WriteLine("->{0}", asm.FullName);
        Type[] types = asm.GetTypes();
        foreach (Type t in types)
        {
            Console.WriteLine("Type: {0}", t);
        }
        return asm;
    }

    public static void LateBindingExample()
    {
        Assembly asm = LoadExternalAssemblyExample();
        Type miniVan = asm.GetType("Cars.MiniCar");
        object obj = Activator.CreateInstance(miniVan);
        MethodInfo method = miniVan.GetMethod("TurboBoost");
        method?.Invoke(obj, null);
    }

    [Obsolete("This is an example of an obsolete method")]
    public static void AttributesExample()
    {
        "I'm Obsolete!!!".Print();
        var a = longList;
        var largestNumber = (from i in longList orderby i descending select i).First();
        $"Longest Number in list: ${largestNumber}".Print();
    }

    public static void DynamicKeywordExample()
    {
        int x = 32;
        var a = 3;
        object b = 4;
        dynamic c = 325;
        b = "Hell world";
        325.ReverseNumber().Print();
        Assembly asm = LoadExternalAssemblyExample();
        Type miniVan = asm.GetType("Cars.MiniCar");
        dynamic obj = Activator.CreateInstance(miniVan);
        obj.TurboBoost();
    }
}

[AttributeUsage(AttributeTargets.Field)]
public sealed class HeavyInitializationAttribute : Attribute
{
    public string? Description { get; set; }

    public HeavyInitializationAttribute() { }

    public HeavyInitializationAttribute(string des) => Description = des;
}
