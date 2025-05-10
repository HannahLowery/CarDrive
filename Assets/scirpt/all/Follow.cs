using System.Collections;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject target = null;
    public Transform Pos1;
    public Transform Pos2;
    public GameObject t = null;
    public float speed = 1.5f;
    public int index;

    [Header("Audio")]
    public AudioSource switchAudio; 

    void Start()
    {
        StartCoroutine(InitAfterDelay());
    }

    IEnumerator InitAfterDelay()
    {
        yield return null; // Wait one frame to ensure all objects are instantiated

        target = GameObject.FindGameObjectWithTag("Player");
        t = GameObject.FindGameObjectWithTag("target");

        GameObject p1Obj = GameObject.FindGameObjectWithTag("Pos1");
        GameObject p2Obj = GameObject.FindGameObjectWithTag("Pos2");

        if (target == null || t == null || p1Obj == null || p2Obj == null)
        {
            Debug.LogError("One or more required objects are missing. Check tags and timing.");
            yield break;
        }

        Pos1 = p1Obj.transform;
        Pos2 = p2Obj.transform;
        Pos2.SetParent(target.transform);

        index = PlayerPrefs.GetInt("save");
    }

    void Update()
{
    if (index > 1)
    {
        index = 0;
    }
    if (index < 0)
    {
        index = 1;
    }

    // 🔑 Listen for "E" key press
    if (Input.GetKeyDown(KeyCode.E))
    {
        Next();
    }
}

    void FixedUpdate()
    {
        // Only run logic if all required objects are initialized
        if (target == null || t == null || Pos1 == null || Pos2 == null) return;

        if (index == 0)
        {
            this.transform.LookAt(target.transform); // Follow the car
            t.transform.position = Pos1.position;
        }
        else if (index == 1)
        {
            // Do NOT LookAt! Just keep normal rotation
            t.transform.position = Pos2.position;
        }

        float car_move = Mathf.Abs(Vector3.Distance(this.transform.position, t.transform.position) * speed);
        this.transform.position = Vector3.MoveTowards(this.transform.position, t.transform.position, car_move * Time.deltaTime);
    }

    public void Next()
    {
        index++;
        PlayerPrefs.SetInt("save", index);
        PlayerPrefs.Save();

        // 🎧 Play the audio when switching views
        if (switchAudio != null)
        {
            switchAudio.Play();
        }
    }
}

