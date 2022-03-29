using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ScreenJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public UnityEvent<float> HorizontalInput;

    public UnityEvent<float> VerticalInput;

    public bool Horizontal = true;
    
    public bool Vertical = true;

    public RectTransform JoystickBody;
    public RectTransform Joystick;

    RectTransform _rectTransform;
    Canvas _canvas;

    Vector2? _input;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        JoystickBody.localScale = Vector3.zero;
    }

    void Update()
    {
        if (!_input.HasValue)
            return;
        if (Horizontal)
            HorizontalInput?.Invoke(_input.Value.x);
        if (Vertical)
            VerticalInput?.Invoke(_input.Value.y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, Input.mousePosition, _canvas.worldCamera, out var mousePos);
        JoystickBody.anchoredPosition = mousePos;
        JoystickBody.localScale = Vector3.one;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(JoystickBody, Input.mousePosition, _canvas.worldCamera, out var mousePos);
      
        JoystickBody.localScale = Vector3.one;

        if (mousePos.magnitude > JoystickBody.sizeDelta.x / 2f)
            mousePos = mousePos.normalized * JoystickBody.sizeDelta.x / 2f;

        Joystick.anchoredPosition = new Vector2(Horizontal ? mousePos.x : Joystick.anchoredPosition.x, Vertical ? mousePos.y : Joystick.anchoredPosition.y);

        _input = new Vector2(mousePos.x / JoystickBody.sizeDelta.x * 2f, mousePos.y / JoystickBody.sizeDelta.y * 2f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        JoystickBody.localScale = Vector3.zero;
        _input = null;
    }
}
