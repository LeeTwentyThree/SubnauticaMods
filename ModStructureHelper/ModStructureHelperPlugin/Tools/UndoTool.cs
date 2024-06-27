using System.Collections;

namespace ModStructureHelperPlugin.Tools;

public class UndoTool : ToolBase
{
    public override ToolType Type => ToolType.Undo;
    public override bool MultitaskTool => true;
    public override bool PairedWithControl => true;

    private bool _coroutineRunning;
    
    protected override void OnToolEnabled()
    {
        if (_coroutineRunning)
        {
            ErrorMessage.AddMessage("Cannot undo until all undo processes are complete.");
            return;
        }

        StartCoroutine(Undo());
    }

    protected override void OnToolDisabled()
    {
        
    }

    private IEnumerator Undo()
    {
        _coroutineRunning = true;
        yield return manager.undoHistory.Undo();
        _coroutineRunning = false;
        DisableTool();
    }
}