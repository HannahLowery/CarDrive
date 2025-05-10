using UnityEngine;

public class Game_Man : MonoBehaviour
{
    public GameObject[] cars;

    void Start()
    {
        int index = PlayerPrefs.GetInt("carIndex", 0);

        GameObject carDriveRoot = GameObject.Find("Cardrive/Car");
        if (carDriveRoot == null)
        {
            Debug.LogError("Could not find 'Cardrive/Car' in the scene.");
            return;
        }

        // Define a point above the scene to start the raycast
        Vector3 rayOrigin = carDriveRoot.transform.position + Vector3.up * 5f;

        RaycastHit hit;
        Vector3 spawnPosition;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 20f))
        {
            // Ground hit: adjust spawn to just above the surface
            float carYOffset = -0.8238831f; // Match your car prefab pivot Y offset
            spawnPosition = hit.point + new Vector3(0, carYOffset, 0);
        }
        else
        {
            // Fallback: spawn at the original position
            Debug.LogWarning("Raycast did not hit ground. Using default spawn position.");
            spawnPosition = carDriveRoot.transform.position;
        }

        GameObject car = Instantiate(cars[index], spawnPosition, carDriveRoot.transform.rotation);
        car.transform.SetParent(carDriveRoot.transform, true);

        Debug.Log("Spawned selected car as a child of Cardrive/Car.");
    }
}
