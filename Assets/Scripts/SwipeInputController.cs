using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeInputController : MonoBehaviour
{
    private Block selectedBlock;

    private Vector2 startPos;
    private Vector2 endPos;

    [SerializeField] private float swipeThreshold = 50f;

    
    Vector3 GetPointerPosition()
    {
        if (Touchscreen.current != null)
            return Touchscreen.current.primaryTouch.position.ReadValue();

        if (Mouse.current != null)
            return Mouse.current.position.ReadValue();

        return Vector2.zero;
    }

    bool PointerPressedThisFrame()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            return true;
        if (Mouse.current != null)
            return Mouse.current.press.wasPressedThisFrame;

        return false;
    }

    bool PointerReleasedThisFrame()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasReleasedThisFrame)
            return true;
        if (Mouse.current != null)
            return Mouse.current.press.wasReleasedThisFrame;

        return false;
    }

    private void Update()
    {
        SelectBlock();

        if (selectedBlock == null)
            return;

        DetectSwipe();
    }

    private void SelectBlock()
    {
        if (PointerPressedThisFrame())
        {
            Ray ray = Camera.main.ScreenPointToRay(GetPointerPosition());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Block block = hit.collider.GetComponent<Block>();

                if (block != null)
                {
                    selectedBlock = block;

                    if (GameManager.instance.PowerInUse == true)
                    {
                        GameManager.instance.destroyObstacle(selectedBlock.gameObject);
                    }

                    startPos = GetPointerPosition();
                }
            }
        }
    }

    private void DetectSwipe()
    {

        if (PointerReleasedThisFrame())
        {
            endPos = GetPointerPosition();

            Vector2 delta = endPos - startPos;

            if (delta.magnitude < swipeThreshold)
                return;

            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                if (delta.x > 0)
                    selectedBlock.Move(Vector2Int.right);
                else
                    selectedBlock.Move(Vector2Int.left);
            }
            else
            {
                if (delta.y > 0)
                    selectedBlock.Move(Vector2Int.up);
                else
                    selectedBlock.Move(Vector2Int.down);
            }
            selectedBlock = null;
        }
    }
}