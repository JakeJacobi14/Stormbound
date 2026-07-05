using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimationFix : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Button button;
    private Animator animator;
    private bool isPressed = false;

    void Awake()
    {
        button = GetComponent<Button>();
        animator = GetComponent<Animator>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        animator.SetBool("Pressed", true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        animator.SetBool("Pressed", false);
    }

    void Update()
    {
        if (isPressed)
        {
            animator.SetBool("Pressed", true);
        }
    }
}