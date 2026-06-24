using System.Collections.Generic;
using UnityEngine;

public class UndoManager : MonoBehaviour
{
    public static UndoManager Instance;

    private Stack<MoveState> undoStack = new Stack<MoveState>();

    private void Awake()
    {
        Instance = this;
    }

    public void SaveState()
    {
        MoveState state = new MoveState();

        Block[] blocks = FindObjectsByType<Block>(FindObjectsSortMode.None);

        foreach (Block block in blocks)
        {
            BlockState blockState = new BlockState();

            blockState.block = block;
            blockState.position = block.GridPosition;
            blockState.isActive = block.gameObject.activeSelf;

            state.blockStates.Add(blockState);
        }

        undoStack.Push(state);
    }

    public void Undo()
    {
        if (undoStack.Count == 0)
            return;

        MoveState state = undoStack.Pop();
        print("Undo");
        foreach (var blockState in state.blockStates)
        {
            GridCell cell = GridManager.instance.GetCell(
                blockState.block.GridPosition.x,
                blockState.block.GridPosition.y);

            cell.isOccupied = false;

            blockState.block.GridPosition = blockState.position;
            blockState.block.gameObject.SetActive(blockState.isActive);
            blockState.block.transform.position =
                GridManager.instance.GetWorldPosition(
                    blockState.position.x,
                    blockState.position.y);
 
        }
    }
}
