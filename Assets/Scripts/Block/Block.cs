using Solo.MOST_IN_ONE;
using System.Collections;
using UnityEngine;
using static UnityEditor.Progress;


public enum BlockColor
{
    Red,
    Blue,
    Green,
    Yellow,
    Grey,
    GateColor
}

public class Block : MonoBehaviour
{
    public BlockData blockData;
    public Vector2Int GridPosition;

    private bool IsMoving = false;
    

    public void Move(Vector2Int direction)
    {
        if (IsMoving) return;

        Vector2Int targetPosition = GridPosition + direction;

        GridCell targetCell = GridManager.instance.GetCell(
            targetPosition.x,
            targetPosition.y);

        if (targetCell == null)
        {
            StartCoroutine(shakeEffect(direction));
            LightImpactHaptic();
            Debug.Log("Out of bounds");
            return;
        }

        if (!GridManager.instance.CanMoveToCell(targetCell.X, targetCell.Y))
        {
            StartCoroutine(shakeEffect(direction));
            LightImpactHaptic();
            Debug.Log("Cannot move");
            return;
        }

        GridCell currentCell = GridManager.instance.GetCell(
            GridPosition.x,
            GridPosition.y);

        if(targetCell.cellType == CellType.Exit)
        {
            print("Found Exit cell");
            CheckExit(currentCell);
        }

        currentCell.isOccupied = false;
        targetCell.isOccupied = true;

        GridPosition = targetPosition;
        StartCoroutine(Moving(GridPosition));
    }

    private void CheckExit(GridCell cell)
    {
        if (cell.ExitColor == blockData.blockColor)
        {
            Debug.Log("Block Escaped");

            Destroy(gameObject);

            //LevelManager.Instance.CheckLevelComplete();
        }
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

    IEnumerator shakeEffect(Vector2Int direction)
    {
        float shakeDuration = 0.2f;
        float elapsed = 0f;
        float shakeMagnitude = 0.08f;
        float shakeSpeed = 40f;
        Vector3 originalPos = transform.position;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            float offset = Mathf.Sin(elapsed * shakeSpeed) * shakeMagnitude;
            if(direction == Vector2Int.right || direction == Vector2Int.left)
            {
                transform.position = originalPos + new Vector3(0f, 0f, offset);
            }
            else
            {
                transform.position = originalPos + new Vector3(offset, 0f, 0f);
            }
            
            yield return null;
        }
        transform.position = originalPos;
    }

    public void LightImpactHaptic()
    {
        MOST_HapticFeedback.Generate(MOST_HapticFeedback.HapticTypes.LightImpact);
    }
}