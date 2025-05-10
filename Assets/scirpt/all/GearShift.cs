using UnityEngine;
using UnityEngine.EventSystems;

public class GearShift : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public enum Gear { Reverse, Park, Drive }
    public Gear currentGear = Gear.Park;

    public float shiftSpeed = 150f;

    public Vector2 drivePos = new Vector2(0, -21f);
    public Vector2 reversePos = new Vector2(0, 11.4f);
    public Vector2 parkPos = new Vector2(0, 36.4f);  // Highest Y

    private RectTransform rectTransform;
    private float minY, maxY;
    private bool isDragging = false;

    public AudioSource gearShiftAudio;  // AudioSource for playing the gear shift sound

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        SetGear(currentGear);

        // Correct range: Drive is lowest, Park is highest
        minY = drivePos.y;
        maxY = parkPos.y;
    }

    private void Update()
    {
        if (!isDragging)
        {
            Vector2 targetPos = GetGearPosition(currentGear);
            rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, targetPos, shiftSpeed * Time.deltaTime);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentPos = rectTransform.anchoredPosition;
        float newY = Mathf.Clamp(currentPos.y + eventData.delta.y, minY, maxY);
        rectTransform.anchoredPosition = new Vector2(currentPos.x, newY);
        
        // Play the sound when the knob is moved
        if (!gearShiftAudio.isPlaying) // Only play if it's not already playing
        {
            gearShiftAudio.Play();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        float y = rectTransform.anchoredPosition.y;
        float distToDrive = Mathf.Abs(y - drivePos.y);
        float distToReverse = Mathf.Abs(y - reversePos.y);
        float distToPark = Mathf.Abs(y - parkPos.y);

        if (distToDrive <= distToReverse && distToDrive <= distToPark)
            SetGear(Gear.Drive);
        else if (distToReverse <= distToDrive && distToReverse <= distToPark)
            SetGear(Gear.Reverse);
        else
            SetGear(Gear.Park);

        // Stop the sound when the dragging ends (you can adjust this to stop at specific conditions if needed)
        gearShiftAudio.Stop();
    }

    public void SetGear(Gear gear)
    {
        currentGear = gear;
        rectTransform.anchoredPosition = GetGearPosition(gear);

        // Optionally, you can play a sound when the gear is set (if you want this behavior)
        // gearShiftAudio.Play();
    }

    public Vector2 GetGearPosition(Gear gear)
    {
        return gear switch
        {
            Gear.Drive => drivePos,
            Gear.Reverse => reversePos,
            Gear.Park => parkPos,
            _ => parkPos
        };
    }
}
