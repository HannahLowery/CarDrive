using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SteeringWheel : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private bool wheelBeingHeld = false;
    public RectTransform wheel;
    private float wheelAngle = 0f;
    private float lastWheelAngle = 0f;
    private Vector2 center;

    [Header("Settings")]
    public float maxSteerAngle = 200f;
    public float releaseSpeed = 300f;

    [Header("Output")]
    public float output;

    void Start()
    {
        center = RectTransformUtility.WorldToScreenPoint(null, wheel.position);
    }

    void Update()
    {
        if (!wheelBeingHeld && wheelAngle != 0f)
        {
            float deltaAngle = releaseSpeed * Time.deltaTime;
            if (Mathf.Abs(deltaAngle) > Mathf.Abs(wheelAngle))
                wheelAngle = 0f;
            else if (wheelAngle > 0f)
                wheelAngle -= deltaAngle;
            else
                wheelAngle += deltaAngle;
        }

        wheel.localEulerAngles = new Vector3(0, 0, -wheelAngle);
        output = Mathf.Clamp(wheelAngle / maxSteerAngle, -1f, 1f);
    }

    public void OnPointerDown(PointerEventData data)
    {
        wheelBeingHeld = true;
        center = RectTransformUtility.WorldToScreenPoint(data.pressEventCamera, wheel.position);
        lastWheelAngle = Vector2.Angle(Vector2.up, data.position - center);
    }

    public void OnDrag(PointerEventData data)
    {
        float newAngle = Vector2.Angle(Vector2.up, data.position - center);
        if ((data.position - center).sqrMagnitude >= 400)
        {
            if (data.position.x > center.x)
                wheelAngle += newAngle - lastWheelAngle;
            else
                wheelAngle -= newAngle - lastWheelAngle;
        }

        wheelAngle = Mathf.Clamp(wheelAngle, -maxSteerAngle, maxSteerAngle);
        lastWheelAngle = newAngle;
    }

    public void OnPointerUp(PointerEventData data)
    {
        OnDrag(data);
        wheelBeingHeld = false;
    }
}
