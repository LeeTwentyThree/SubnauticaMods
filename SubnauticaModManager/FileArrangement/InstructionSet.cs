using FileArranger.Instructions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FileArranger;

[Serializable]
public class InstructionSet
{
    public Instruction[] instructions;

    private static JsonSerializerSettings _serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented };

    private string _path;

    public InstructionSet(string jsonFilePath)
    {
        _path = jsonFilePath;
    }

    public Result SaveToDisk()
    {
        if (instructions == null) return Result.NoInstructions;
        if (string.IsNullOrEmpty(_path)) return Result.InvalidPath;
        if (!File.Exists(_path)) return Result.FileNotFound;

        try
        {
            File.WriteAllText(_path, SerializeToString());
        }
        catch
        {
            return Result.Exception;
        }

        return Result.Success;
    }

    public Result[] ExecuteInstructions()
    {
        if (string.IsNullOrEmpty(_path)) return new Result[] { Result.InvalidPath };
        if (!File.Exists(_path)) return new Result[] { Result.FileNotFound };

        try
        {
            var contents = File.ReadAllText(_path);
            Deserialize(contents);
        }
        catch
        {
            return new Result[] { Result.Exception };
        }

        if (instructions == null) return new Result[] { Result.NoInstructions };
        return ExecuteAll();
    }

    private string SerializeToString()
    {
        return JsonConvert.SerializeObject(instructions, _serializerSettings);
    }

    private void Deserialize(string from)
    {
        instructions = JsonConvert.DeserializeObject<Instruction[]>(from, _serializerSettings);
    }

    private Result[] ExecuteAll()
    {
        Result[] results = new Result[instructions.Length];
        for (int i = 0; i < instructions.Length; i++)
        {
            if (instructions[i] == null)
            {
                results[i] = Result.NoInstructions;
            }
            else
            {
                results[i] = instructions[i].Execute();
            }
        }
        return results;
    }
}