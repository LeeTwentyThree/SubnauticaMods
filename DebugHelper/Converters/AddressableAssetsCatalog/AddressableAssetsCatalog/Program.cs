using System.Reflection;
using System.Text;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Press enter to continue.");
Console.ReadLine();

var directory = Directory.GetCurrentDirectory();
var textFile = Path.Combine(directory, "addressables-list.json");
var outputFile = Path.Combine(directory, "addressables-formatted.json");
var text = File.ReadAllText(textFile);
var length = text.Length;

Console.WriteLine("Creating StringBuilder...");

StringBuilder output = new StringBuilder(4000000);

Console.WriteLine("StringBuilder created!");

output.AppendLine("[");

const string assetsPrefix = "\"Assets";

// code goes here
int next = text.IndexOf(assetsPrefix);
int timeNextStatusUpdate = 50;
while (next >= 0)
{
    int endIndex = text.IndexOf("\"", next + 1);
    string line = text.Substring(next, endIndex - next + 1);
    output.Append("\t");
    Console.WriteLine(line);
    output.Append(line);
    next = text.IndexOf(assetsPrefix, next + 1);
    if (next >= 0)
        output.AppendLine(",");
    else output.AppendLine();
    timeNextStatusUpdate--;
    if (timeNextStatusUpdate == 0)
    {
        timeNextStatusUpdate = 50;
        Console.WriteLine((float)next / length * 100f + "%");
    }
}

output.Append("]");

File.WriteAllText(outputFile, output.ToString());
Console.WriteLine("Saved!");
Console.WriteLine("Press enter to close.");
Console.ReadLine();