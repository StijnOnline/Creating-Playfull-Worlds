using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Color[] set_colors;
    public static Color[] colors;
    public static bool[] color_states;    

    public GameObject HUD;

    public GameObject player;
    public static GameObject spawn_ob;

    public float set_laserspeed;
    public static float laserspeed;

    void Awake()
    {
        if (spawn_ob == null){
            spawn_ob = new GameObject();
            spawn_ob.transform.Rotate(0,90,0,Space.Self);
        }
        GameObject start = GameObject.Find("Start");
        DontDestroyOnLoad(spawn_ob);
        player = Instantiate(player, start.transform.position - spawn_ob.transform.position, spawn_ob.transform.rotation);

        colors = set_colors;
        Instantiate(HUD);
        color_states = new bool[set_colors.Length];
        laserspeed = set_laserspeed;


    }

    private void Update() {
        
    }    
}
