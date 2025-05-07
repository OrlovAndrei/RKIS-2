using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class ListModel<TItem>
{
    public List<TItem> Items { get; }
    public CommandBuilder<TItem> CommandBuilder { get; }
    public int UndoLimit { get; }

    private readonly LimitedSizeStack<ICommand> HistoryStack;

    public ListModel(int undoLimit)
    {
        Items = new List<TItem>();
        UndoLimit = undoLimit;
        HistoryStack = new LimitedSizeStack<ICommand>(UndoLimit);
        CommandBuilder = new CommandBuilder<TItem>(this);
    }

    public void AddItem(TItem item)
    {
        var command = CommandBuilder.CreateAddItemCommand(item);
        ExecuteCommand(command);
    }

    public void RemoveItem(int index)
    {
        var command = CommandBuilder.CreateRemoveItemCommand(index);
        ExecuteCommand(command);
    }

    public bool CanUndo()
    {
        return HistoryStack.Count > 0;
    }

    public void Undo()
    {
        if (CanUndo())
        {
            var command = HistoryStack.Pop();
            try
            {
                command.Undo();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error during Undo: {e}");
            }
        }
    }
