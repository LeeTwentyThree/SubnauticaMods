namespace FileArranger.Instructions;

[Serializable]
public class Move : Instruction
{
    public string target;

    public string destination;

    public override Result Execute()
    {
        File.Move(target, destination);
        return Result.Success;
    }

    public Move(string target, string destination)
    {
        this.target = target;
        this.destination = destination;
    }
}
