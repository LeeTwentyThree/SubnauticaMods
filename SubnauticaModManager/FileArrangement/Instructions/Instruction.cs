namespace FileArranger.Instructions;

public class Instruction
{
    public static string GetId(Type type)
    {
        return type.ToString();
    }

    public virtual Result Execute()
    {
        return Result.None;
    }
}