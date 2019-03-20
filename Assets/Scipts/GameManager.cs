using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GameManager : MonoBehaviour
{
    public Color[] set_colors;
    public static Color[] colors;
    public static bool[] color_states;

    public GameObject set_HUD;
    public static GameObject HUD;

    public GameObject player;
    public Transform spawn_pos;

    public GameObject topCamera;

    public float set_laserspeed;
    public static float laserspeed;

    void Awake()
    {
        colors = set_colors;
        HUD = set_HUD;
        color_states = new bool[set_colors.Length];
        laserspeed = set_laserspeed;

        player = Instantiate(player, spawn_pos.position, spawn_pos.rotation);
        topCamera = Instantiate(topCamera, spawn_pos.position + new Vector3(0,10,0), Quaternion.Euler(0,90,0));
        topCamera.SetActive(false);

    }
    
    void Update()
    {
        //Debug.Log(color_states[0] + " " + color_states[1]);
        if (Input.GetKeyDown(KeyCode.Tab)) {
            ToggleView();
        }
    }

    void ToggleView() {
        if (topCamera.activeSelf) {
            topCamera.SetActive(false);
            player.GetComponent<FirstPersonController>().enabled = true;
        } else {
            topCamera.SetActive(true);
            player.GetComponent<FirstPersonController>().enabled = false;
        }
    }

}
