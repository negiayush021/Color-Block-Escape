using UnityEngine;

public class Gate : MonoBehaviour
{
    public BlockColor gateColor;

    [SerializeField] private Animator animator;

    public void OpenGate()
    {
        animator.SetTrigger("Open");
    }
}