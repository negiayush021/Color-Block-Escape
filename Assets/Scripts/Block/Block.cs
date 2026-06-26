using Solo.MOST_IN_ONE;
using System.Collections;
using UnityEngine;


public enum BlockColor
{
    Red,
    Blue,
    Green,
    Yellow,
    Grey,
}

public class Block : MonoBehaviour
{
    public BlockData blockData;
    public Vector2Int GridPosition;

    private TrailRenderer trail;

    private bool IsMoving = false;

    private void Start()
    {
        if(this.gameObject.tag != "Obstacle")
        {
            trail = GetComponent<TrailRenderer>();
            trail.enabled = false;
        }
        
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
            StartCoroutine(shakeEffect(direction));
            if (AudioManager.instance.can_vibrate == true)
            {
                AudioManager.instance.LightImpactHaptic();
            }
            Debug.Log("Out of bounds");
            return;
        }

        if (!GridManager.instance.CanMoveToCell(targetCell.X, targetCell.Y))
        {
            StartCoroutine(shakeEffect(direction));
            if(AudioManager.instance.can_vibrate == true)
            {
                AudioManager.instance.LightImpactHaptic();
            }
            Debug.Log("Cannot move");
            return;
        }

        GridCell currentCell = GridManager.instance.GetCell(
            GridPosition.x,
            GridPosition.y);

        currentCell.isOccupied = false;
        targetCell.isOccupied = true;

        if (targetCell.cellType == CellType.Exit)
        {
            print("Found Exit cell");
            CheckExit(targetCell);
        }


        UndoManager.Instance.SaveState();
        LevelManager.Instance.UseMove();
        GridPosition = targetPosition;

        if(this.gameObject.activeSelf == true)
        {
            StartCoroutine(Moving(GridPosition));
        }
        
    }

    private void CheckExit(GridCell cell)
    {
        if (cell.ExitColor == blockData.blockColor)
        {
            cell.isOccupied = false;
            Debug.Log("Block Escaped");
            cell.gateReference.OpenGate();
            StartCoroutine(Escaping(cell.gateReference));
            LevelManager.Instance.DecreaseBlocksNumber();
            
        }
    }

    IEnumerator Escaping(Gate gate)
    {
        yield return new WaitForSeconds(0.5f);

        AudioManager.instance.PlayClip(3);
        trail.enabled = true;
        Transform wooden_gate_pivot = gate.transform.GetChild(1).transform;

        Vector3 direction = new Vector3(
            wooden_gate_pivot.position.x - transform.position.x,
            0f,
            wooden_gate_pivot.position.z - transform.position.z).normalized;

        Vector3 targetPos = wooden_gate_pivot.transform.position + direction * 5f;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                10f * Time.deltaTime);

            yield return null;
        }


        gameObject.SetActive(false);
        GameManager.instance.hammer_disable_img.fillAmount -= 0.35f;
        GameManager.instance.undo_disable_img.fillAmount -= 0.35f;

        if (GameManager.instance.hammer_disable_img.fillAmount <= 0 )
        {
            //Power active now
            GameManager.instance.hammer_disable_img.enabled = false;
            GameManager.instance.hammerPowerCount += 1;
            GameManager.instance.hammerCount_txt.text = GameManager.instance.hammerPowerCount.ToString();
        }
        
        if (GameManager.instance.undo_disable_img.fillAmount <= 0 )
        {
            //undo active now
            GameManager.instance.undo_disable_img.enabled = false;
            GameManager.instance.undoCount += 2;
            GameManager.instance.undoCount_txt.text = GameManager.instance.undoCount.ToString();
        }
        

        LevelManager.Instance.CheckLevelComplete();
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

    
}