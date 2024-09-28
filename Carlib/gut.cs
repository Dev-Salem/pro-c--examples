using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("the-app")]

namespace DeepGut;

internal class MyInternalClass
{
    public static void VeryPrivate() => Console.WriteLine("I'm a secret method");
}
