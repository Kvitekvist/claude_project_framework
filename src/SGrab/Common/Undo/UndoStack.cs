namespace SGrab.Common.Undo;

/// <summary>A single reversible edit.</summary>
public interface IUndoableAction
{
    void Undo();

    void Redo();
}

/// <summary>Two-stack undo/redo history.</summary>
public sealed class UndoStack
{
    private readonly Stack<IUndoableAction> _undo = new();
    private readonly Stack<IUndoableAction> _redo = new();

    public event EventHandler? Changed;

    public bool CanUndo => _undo.Count > 0;

    public bool CanRedo => _redo.Count > 0;

    /// <summary>Records an action that has already been applied.</summary>
    public void Push(IUndoableAction action)
    {
        _undo.Push(action);
        _redo.Clear();
        Changed?.Invoke(this, EventArgs.Empty);
    }

    public void Undo()
    {
        if (_undo.Count == 0)
        {
            return;
        }

        var action = _undo.Pop();
        action.Undo();
        _redo.Push(action);
        Changed?.Invoke(this, EventArgs.Empty);
    }

    public void Redo()
    {
        if (_redo.Count == 0)
        {
            return;
        }

        var action = _redo.Pop();
        action.Redo();
        _undo.Push(action);
        Changed?.Invoke(this, EventArgs.Empty);
    }
}

/// <summary>Runs the supplied undo/redo delegates.</summary>
public sealed class DelegateAction : IUndoableAction
{
    private readonly Action _undo;
    private readonly Action _redo;

    public DelegateAction(Action undo, Action redo)
    {
        _undo = undo;
        _redo = redo;
    }

    public void Undo() => _undo();

    public void Redo() => _redo();
}
