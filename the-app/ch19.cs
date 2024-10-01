using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
using Ch11;
using JsonLib = System.Text.Json.JsonSerializer;

namespace Ch19;

public class IOExamples()
{
    public static void DirectExample()
    {
        DirectoryInfo dir = new(".");
        DirectoryInfo dir2 = new(@"../some-test-dir/");
        string[] directories = Directory.GetLogicalDrives();
        // dir.FullName.Print();
        // dir.Name.Print();
        // if (!dir2.Exists)
        // {
        //     dir2.Create();
        // }
        // dir2.CreateSubdirectory("hello");
        // dir2.CreateSubdirectory("world");
        // dir2.CreateSubdirectory("let/test/how/this/works");
        // foreach (var item in directories)
        // {
        //     item.Print();
        // }
        foreach (var item in DriveInfo.GetDrives())
        {
            $"Driver Name: {item.Name}\tis Ready? {item.IsReady}".Print();
        }
        // Directory.Delete("../some-test-dir/world");
        void ShowStats(DirectoryInfo d)
        {
            10.Separate();
            $"Full name: {d.FullName}".Print();
            $"Creation time {d.CreationTime}".Print();
            $"Parent: {d.Parent}".Print();
            $"Last access time: {d.LastAccessTime}".Print();
            $"Root {d.Root}".Print();
            var files = d.GetFiles("*.json", SearchOption.AllDirectories);
            10.Separate();
            foreach (var item in files)
            {
                item.FullName.Print();
            }
            10.Separate();
        }
    }

    public static void WorkWithFileInfoExample()
    {
        FileInfo fi = new("test.txt");
        using var createText = fi.CreateText();
        using var openFile = fi.OpenRead();
        createText.WriteLine("This is a created file");
        using var appendText = fi.AppendText();
        appendText.WriteLine("This is an appended text");
        using var myFile = fi.Open(
            FileMode.OpenOrCreate,
            FileAccess.ReadWrite,
            FileShare.ReadWrite
        );
    }

    public static void WorkingWithFileExample()
    {
        var file = File.Create("test3.txt");
        file.Close();
        using var openFile = File.Open(
            file.Name,
            FileMode.OpenOrCreate,
            FileAccess.ReadWrite,
            FileShare.ReadWrite
        );
        // File.AppendAllLines("test.txt", ["Why would you be loved", "Why would you be loving?"]);
        //    foreach (var item in  File.ReadAllLines("test.txt"))
        //    {
        //     item.Print();
        //    }
        var binaryMessage = Encoding.Default.GetBytes("Hozier is the best!");
        openFile.Write(binaryMessage);
        openFile.Position = 0;
        var byteFromFile = new byte[openFile.Length];
        for (int i = 0; i < openFile.Length; i++)
        {
            byteFromFile[i] = (byte)openFile.ReadByte();
        }
        Encoding.Default.GetString(byteFromFile).Print();
        File.Delete(openFile.Name);
    }

    public static void WorkingWithStreamWriterExample()
    {
        using StreamWriter writer = File.CreateText("baby.txt");
        writer.WriteLine("Why would you be loved");
        writer.WriteLine("For something as hollow as trust?");
        writer.WriteLine("Why would you be loving");
        writer.Write(writer.NewLine);
        writer.WriteLine("Yeh, yeah, yeah");
        writer.Close();
        using StreamReader reader = File.OpenText("baby.txt");
        string? input;
        while ((input = reader.ReadLine()) != null)
        {
            input.Print();
        }
    }

    public static void WorkingWithStringWriterExample()
    {
        StringBuilder sb = new();
        sb.AppendLine("Come and get some");
        sb.AppendLine("Burning the children");
        sb.AppendLine("Seven ways to eat your young!");
        using StringWriter sw = new();
        sw.WriteLine(sb.ToString());
        sw.Print();

        StringReader reader = new(sw.ToString());
        reader.ReadToEnd();
    }

    public static void WorkingWithBinaryWriterExample()
    {
        // Open a binary writer for a file.
        FileInfo f = new("BinFile.dat");
        using (BinaryWriter bw = new(f.OpenWrite()))
        {
            // Print out the type of BaseStream.
            // (System.IO.FileStream in this case).
            Console.WriteLine("Base stream is: {0}", bw.BaseStream);
            // Create some data to save in the file.
            const double aDouble = 1234.67;
            const int anInt = 34567;
            const string aString = "A, B, C";
            // Write the data.
            bw.Write(aDouble);
            bw.Write(anInt);
            bw.Write(aString);
        }
        Console.WriteLine("Done!");
        using BinaryReader br = new(f.OpenRead());
        Console.WriteLine(br.ReadDouble());
        Console.WriteLine(br.ReadInt32());
        Console.WriteLine(br.ReadString());
    }

    public static void WorkingWithFileWatcher()
    {
        FileSystemWatcher watcher =
            new()
            {
                Path = ".",
                Filter = "*.txt",
                NotifyFilter = NotifyFilters.LastAccess,
            };
        watcher.Changed += (s, e) => $"{e.FullPath}, changed to {e.FullPath}".Print();
        watcher.Created += (s, e) => $"{e.FullPath}, created {e.FullPath}".Print();
        watcher.Deleted += (s, e) => $"{e.FullPath}, deleted {e.FullPath}".Print();
        watcher.EnableRaisingEvents = true;
        using var textReader = File.CreateText("text.txt");
        textReader.WriteLine("I'm changed mom");
        textReader.Close();
        File.Delete("text.txt");
        while (Console.Read() != 'q') { }
    }
}

public class SerializationExample()
{
    static readonly List<Person> people =
    [
        new Person() { Name = "John", Age = 32 },
        new Person()
        {
            Name = "Ann",
            Age = 22,
            Pets = [new Pet() { Name = "Rocky", AnimalType = Pet.Animal.Dog }],
        },
        new Person()
        {
            Name = "Bruce",
            Age = 27,
            Pets =
            [
                new Pet() { Name = "Kitty", AnimalType = Pet.Animal.Cat },
                new Pet() { Name = "Alex", AnimalType = Pet.Animal.Cat },
            ],
        },
    ];

    public record Pet
    {
        [JsonPropertyOrder(2)]
        public string Name { get; set; } = "pet";

        [JsonPropertyOrder(1)]
        public Animal AnimalType { get; set; }

        public enum Animal
        {
            Dog,
            Cat,
            Bird,
            Rat,
            Other,
        }

        public Pet(string name, Animal type)
        {
            Name = name;
            AnimalType = type;
        }

        public Pet() { }
    }

    public record Person
    {
        public string Name { get; set; } = "name";
        public int Age { get; set; }
        public List<Pet> Pets { get; set; } = [];

        public Person(string name, int age, List<Pet> pets)
        {
            Name = name;
            Age = age;
            Pets = pets;
        }

        public Person() { }
    }

    public static void XMLSerializerExample()
    {
        SaveToXML(people, "people.xml");
        10.Separate();
        var objects = ReadFromXML<List<Person>>("people.xml");
        10.Separate();
        objects.Print();
        void SaveToXML<T>(T obj, string name)
        {
            XmlSerializer xml = new(typeof(T));
            using Stream fStream = new FileStream(
                name,
                FileMode.Create,
                FileAccess.ReadWrite,
                FileShare.ReadWrite
            );
            xml.Serialize(fStream, obj);
        }

        T? ReadFromXML<T>(string fileName)
        {
            XmlSerializer xml = new(typeof(T));
            using Stream fStream = new FileStream(fileName, FileMode.Open);
            return (T?)xml.Deserialize(fStream);
        }
    }

    public static void JsonSerializerExample()
    {
        JsonSerializerOptions options =
            new()
            {
                IncludeFields = true,
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                Converters = { new JsonStringNullToEmptyConverter() },
            };
        // JsonSerializerOptions options =
        //     new()
        //     {
        //         PropertyNameCaseInsensitive = true,
        //         //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        //         PropertyNamingPolicy = null, //Pascal casing
        //         IncludeFields = true,
        //         WriteIndented = true,
        //         NumberHandling =
        //             JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString,
        //     };
        // SaveToJson(people, "people.json");
        List<Person>? newPeople = ReadFromJson<List<Person>>("people.json");
        foreach (var item in newPeople ?? [])
        {
            item.Print();
        }
        void SaveToJson<T>(T obj, string fileName)
        {
            File.WriteAllText(fileName, JsonLib.Serialize(obj, options));
        }
        T? ReadFromJson<T>(string fileName)
        {
            // using FileStream fStream = new(fileName, FileMode.Open);
            return (T)JsonLib.Deserialize<T>(File.ReadAllText(fileName));
        }
    }
}

public class JsonStringNullToEmptyConverter : JsonConverter<string>
{
    public override bool HandleNull => true;

    public override string? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var value = reader.GetString();
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }
        return value;
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        value ??= string.Empty;
        writer.WriteStringValue(value);
    }
}
