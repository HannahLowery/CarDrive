using UnityEngine;

public class Horn : MonoBehaviour
{
    [Header("Horn Audio")]
    [SerializeField] private AudioSource hornAudioSource;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayHorn();
        }
    }

    public void PlayHorn()
    {
        if (hornAudioSource != null && !hornAudioSource.isPlaying)
        {
            hornAudioSource.Play();
        }
    }
}
