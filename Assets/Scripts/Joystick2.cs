using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick2 : MonoBehaviour
{
    public RectTransform background;
    public RectTransform handle;
    public float handleLimit = 1f;

    private Vector2 inputVector;
    private bool joystickActive = false;

    void Start()
    {
        HideJoystick();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        background.position = eventData.position;
        ShowJoystick();
        joystickActive = true;
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        HideJoystick();
        joystickActive = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (joystickActive)
        {
            Vector2 direction = eventData.position - (Vector2)background.position;
            inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
            handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
        }
    }

    private void ShowJoystick()
    {
        background.gameObject.SetActive(true);
    }

    private void HideJoystick()
    {
        background.gameObject.SetActive(false);
    }

    public float Horizontal()
    {
        return inputVector.x;
    }

    public float Vertical()
    {
        return inputVector.y;
    }

    public Vector2 Direction()
    {
        return new Vector2(Horizontal(), Vertical());
    }
}
