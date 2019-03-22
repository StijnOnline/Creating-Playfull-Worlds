using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject UIcontinue;

    public void Awake() {
        UIcontinue.SetActive(PlayerPrefs.GetInt("Level") > 0);
    }

    public void Continue() {
        SceneManager.LoadScene(PlayerPrefs.GetInt("Level"));
    }

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
