namespace FileArranger.Instructions;

[Serializable]
public class Delete : Instruction
{
    public string fileToDelete;

    public override Result Execute()
    {
        File.Delete(fileToDelete);
        return Result.Success;
    }

    public Delete(string fileToDelete)
    {
        this.fileToDelete = fileToDelete;
    }
}
