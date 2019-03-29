using UnityEngine;
using UnityEngine.SceneManagement;




using UnityStandardAssets.Characters.FirstPerson;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex + 1);
            } else {
                PlayerPrefs.SetInt("Level", 0);
                
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;

                SceneManager.LoadScene(0);
            }
            
            
        }
    }
}
