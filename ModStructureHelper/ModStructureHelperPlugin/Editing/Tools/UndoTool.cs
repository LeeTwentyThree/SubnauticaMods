using System.Collections;
using UnityEngine;

namespace ModStructureHelperPlugin.Editing.Tools;

public class UndoTool : ToolBase
{
    public override ToolType Type => ToolType.Undo;
    public override bool MultitaskTool => true;
    public override bool PairedWithControl => true;

    private bool _coroutineRunning;
    
    public delegate void OnUndo();

    public OnUndo OnUndoHandler;
    
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
        OnUndoHandler?.Invoke();
        yield return manager.undoHistory.Undo();
        _coroutineRunning = false;
        yield return new WaitForSeconds(0.1f);
        DisableTool();
    }
}