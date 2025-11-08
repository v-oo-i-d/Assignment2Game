using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MenuButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Color Settings")]
    [SerializeField] private  Color normalColor = Color.white;
    [SerializeField] private  Color highlightedColor = new(0.9f, 0.9f, 0.9f);
    [SerializeField] private  Color pressedColor = new(0.8f, 0.8f, 0.8f);

    [Header("Hover Settings")]
    private Vector3 initialPosition;
    [SerializeField] private float hoverOffset = 20f;
    [SerializeField] private  Vector3 pressedScale = new(0.95f, 0.95f, 0.95f);
    [SerializeField] private  float transitionSpeed = 10f;

    private Image image;
    private bool isPressed = false;
    private bool isHovered = false;
    
    private enum ButtonState { Normal, Hovered, Pressed }
    private ButtonState lastState = ButtonState.Normal;
    private bool suppressHoverSound = false;

    private void Start()
    {
        image = GetComponent<Image>();
        image.color = normalColor;
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        ButtonState currentState = ButtonState.Normal;

        if (isPressed) currentState = ButtonState.Pressed;
        else if (isHovered) currentState = ButtonState.Hovered;

        if (currentState != lastState)
        {
            switch (currentState)
            {
                case ButtonState.Hovered:
                    if (!suppressHoverSound)
                        SoundManager.PlaySound(SoundType.MenuHover);
                    break;
                case ButtonState.Pressed:
                    SoundManager.PlaySound(SoundType.MenuClick);
                    suppressHoverSound = true;
                    break;
            }
        }

        lastState = currentState;

        if (isPressed)
        {   
            // Clicking
            image.color = Color.Lerp(image.color, pressedColor, Time.deltaTime * transitionSpeed);
            transform.localScale = Vector3.Lerp(transform.localScale, pressedScale, Time.deltaTime * transitionSpeed);
        }
        else if (isHovered)
        {
            // Hovering
            image.color = Color.Lerp(image.color, highlightedColor, Time.deltaTime * transitionSpeed);
            transform.localPosition = Vector3.Lerp(transform.localPosition,
                                                   initialPosition + new Vector3(hoverOffset, 0, 0),
                                                   Time.deltaTime * transitionSpeed);
        }
        else
        {
            // Reset to normal
            image.color = Color.Lerp(image.color, normalColor, Time.deltaTime * transitionSpeed);
            transform.localPosition = Vector3.Lerp(transform.localPosition,
                                                   initialPosition,
                                                   Time.deltaTime * transitionSpeed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) => isHovered = true;
    public void OnPointerExit(PointerEventData eventData) => isHovered = false;
    public void OnPointerDown(PointerEventData eventData) { isPressed = true; suppressHoverSound = true;}
    public void OnPointerUp(PointerEventData eventData) => isPressed = false;
}