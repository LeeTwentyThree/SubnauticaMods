namespace FileArranger;

public enum Result : byte
{
    None,
    Success,
    Exception,
    FileNotFound,
    InvalidPath,
    NoInstructions
}