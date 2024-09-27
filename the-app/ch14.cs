//process, app domains, local context
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using Ch11;

namespace Ch14;

public class PrcoessExample
{
    public static void Example()
    {
        var processes = from p in Process.GetProcesses(".") orderby p.Id select p;
        foreach (var item in processes)
        {
            $"Name: {item.ProcessName}, Id: {item.Id}".Print();
        }
        Console.Write("Choose Process Id: ");
        int id = int.Parse(Console.ReadLine() ?? "");
        var process = Process.GetProcessById(id);
        $"Name: {process.ProcessName}, Id: {process.Id}, {process.Container}".Print();
        foreach (ProcessModule item in process.Modules)
        {
            $"{item.ModuleName}\t{item.FileName}\t{item.ModuleMemorySize}".Print();
        }
        // foreach (var item in processes)
        // {
        //     $"Name: {item.ProcessName}, Id: {item.Id}".Print();
        // }
        // foreach (ProcessThread item in wallPaperProcess.Threads)
        // {
        //     $"{item.Id}".Print();
        // }
    }

    public static void Example2()
    {
        // Get access to the AppDomain for the current thread.
        AppDomain defaultAD = AppDomain.CurrentDomain;
        // Print out various stats about this domain.
        Console.WriteLine("Name of this domain: {0}", defaultAD.FriendlyName);
        var assembles = defaultAD.GetAssemblies();
        foreach (var item in assembles)
        {
            $"{item.FullName}".Print();
        }
        // Console.WriteLine("ID of domain in this process: {0}", defaultAD.Id);
        // Console.WriteLine("Is this the default domain?: {0}", defaultAD.IsDefaultAppDomain());
        // Console.WriteLine("Base directory of this domain: {0}", defaultAD.BaseDirectory);
        // Console.WriteLine("Setup Information for this domain:");
        // Console.WriteLine(
        //     "\t Target Framework: {0}",
        //     defaultAD.SetupInformation.TargetFrameworkName
        // );
    }
}
