using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadCarSelection()
    {
        SceneManager.LoadScene("CarSelection");
    }

    public void LoadHome()
    {
        SceneManager.LoadScene("HomeMenu");
    }
    public void LoadInstructions(){
         SceneManager.LoadScene("Instructions");
    }
}
