//Type Reflection, late binding, attributes, dynamic types

using ObservableExample;

namespace Ch17;

public class ReflectionExamples
{
    public static void TypeExample()
    {
        Person person = new();
        Type typeOfPerson = person.GetType();
        Console.WriteLine(typeOfPerson);
    }
}
