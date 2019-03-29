using UnityEngine;
using UnityEngine.SceneManagement;

<<<<<<< HEAD



=======
>>>>>>> d93d76ee93e6361f820c05667a4112a3808649e1
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

                other.gameObject.GetComponent<FirstPersonController>().m_MouseLook.SetCursorLock(false);

                SceneManager.LoadScene(0);
            }
            
            
        }
    }
}
