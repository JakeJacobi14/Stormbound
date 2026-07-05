using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class UIButtonPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // [Header("References")]
    // The RectTransform of the button. If not set, the script uses the GameObject's own RectTransform.
    private RectTransform buttonRect;

    // The Shadow component attached to this button.
    private Shadow buttonShadow;

    [Header("Settings")]
    // The pixel offset by which the button moves when pressed.
    public float pressOffset = 20f;
    // The duration over which the movement is animated.
    public float moveDuration = 0.1f;

    // Cache original states.
    private Vector2 buttonOriginalPos;
    private Vector2 shadowOriginalEffectDistance;

    private Coroutine moveCoroutine;

    void Start()
    {
        
        buttonRect = GetComponent<RectTransform>();
        
        buttonOriginalPos = buttonRect.anchoredPosition;

        // Get the Shadow component (it must be on the same GameObject).
        buttonShadow = GetComponent<Shadow>();
        if (buttonShadow != null)
        {
            shadowOriginalEffectDistance = buttonShadow.effectDistance;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(AnimateButtonAndShadow(pressed: true));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(AnimateButtonAndShadow(pressed: false));
    }

    private IEnumerator AnimateButtonAndShadow(bool pressed)
    {
        float elapsedTime = 0f;
        Vector2 startButtonPos = buttonRect.anchoredPosition;
        Vector2 targetButtonPos = pressed ? buttonOriginalPos + new Vector2(0, -pressOffset) : buttonOriginalPos;

        // For the shadow effect, adjust the effectDistance so that when the button moves down,
        // the shadow's offset is increased upward to counteract that movement.
        Vector2 startShadowEffect = buttonShadow != null ? buttonShadow.effectDistance : Vector2.zero;
        Vector2 targetShadowEffect = buttonShadow != null 
            ? (pressed ? shadowOriginalEffectDistance + new Vector2(0, pressOffset) : shadowOriginalEffectDistance)
            : Vector2.zero;

        while (elapsedTime < moveDuration)
        {
            // Use unscaledDeltaTime instead of deltaTime to be independent of timeScale
            elapsedTime += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsedTime / moveDuration);

            // Lerp the button's anchored position.
            buttonRect.anchoredPosition = Vector2.Lerp(startButtonPos, targetButtonPos, t);

            // Lerp the shadow's effectDistance so its visual remains fixed.
            if (buttonShadow != null)
            {
                buttonShadow.effectDistance = Vector2.Lerp(startShadowEffect, targetShadowEffect, t);
            }
            yield return null;
        }

        // Ensure final values are set.
        buttonRect.anchoredPosition = targetButtonPos;
        if (buttonShadow != null)
        {
            buttonShadow.effectDistance = targetShadowEffect;
        }
    }
}
