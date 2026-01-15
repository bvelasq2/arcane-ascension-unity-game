using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyCollectible : MonoBehaviour
{
    [Tooltip("Name of the scene to load after collecting the key.")]
    public string nextSceneName = "Level 3_Tower Ascension"; // Scene to load after collecting the key

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player touched the key
        if (other.CompareTag("Player"))
        {
            Debug.Log(" Key collected!");
            
            // Remove the key object from the scene
            Destroy(gameObject);

            // Load the next scene if one is set
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning("No scene name set in KeyCollectible!");
            }
        }
    }
}