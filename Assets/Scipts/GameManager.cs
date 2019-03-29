using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Color[] set_colors;
    public static Color[] colors;
    public static bool[] color_states;    

    public GameObject HUD;

    public GameObject player;

    public float set_laserspeed;
    public static float laserspeed;

    void Awake()
    {
        GameObject start = GameObject.Find("Start");
        player = Instantiate(player, start.transform.position,Quaternion.LookRotation(start.transform.right,Vector3.up));

        colors = set_colors;
        Instantiate(HUD);
        color_states = new bool[set_colors.Length];
        laserspeed = set_laserspeed;
    }

    private void Update() {
        
    }    
}
