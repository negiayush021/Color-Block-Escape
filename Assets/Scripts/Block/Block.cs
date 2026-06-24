using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
    public BlockData blockData;
    public Vector2Int GridPosition;

    private bool IsMoving = false;

    private void Start()
    {
        GetComponent<Renderer>().material.color = blockData.color;
    }
    public void Move(Vector2Int direction)
    {
        if (IsMoving) return;

        Vector2Int targetPosition = GridPosition + direction;

        GridCell targetCell = GridManager.instance.GetCell(
            targetPosition.x,
            targetPosition.y);

        if (targetCell == null)
        {
            Debug.Log("Out of bounds");
            return;
        }

        if (!GridManager.instance.CanMoveToCell(targetCell.X, targetCell.Y))
        {
            Debug.Log("Cannot move");
            return;
        }

        GridCell currentCell = GridManager.instance.GetCell(
            GridPosition.x,
            GridPosition.y);

        currentCell.isOccupied = false;
        targetCell.isOccupied = true;

        GridPosition = targetPosition;
        StartCoroutine(Moving(GridPosition));
    }

    IEnumerator Moving(Vector2Int GridPos)
    {
        IsMoving = true;
        Vector3 startPos = transform.position;
        Vector3 targetPos = GridManager.instance.GetWorldPosition(GridPos.x, GridPos.y);

        float duration = 0.1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            transform.position = Vector3.Lerp(
                startPos,
                targetPos,
                elapsed / duration);

            yield return null;
        }

        transform.position = targetPos;
        IsMoving = false;

    }

}