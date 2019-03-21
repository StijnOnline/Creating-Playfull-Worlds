using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Finish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log(transform.position - other.transform.position);
            GameManager.spawn_ob.transform.position = transform.position - other.transform.position;
            GameManager.spawn_ob.transform.rotation = other.transform.rotation;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
