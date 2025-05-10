using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    public GameObject roadSection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other){
        Instantiate(roadSection,new Vector3(470.5f,-154f,242.9f),Quaternion.identity);
    }
}
