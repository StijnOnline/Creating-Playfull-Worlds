using UnityEngine;
using UnityEditor;

public class Interactable : MonoBehaviour
{
    public enum Type { Button, Lever, Gate, inverse_Gate, Player_Barrier, Laser_Barrier };
    public Type type;
    public int color = 0;
    public Material change_material;
    [HideInInspector] private float barrier_alpha = 0.5f;
    Vector3 start_pos;

    void Start() {

        start_pos = transform.position;
        
        Color setcolor = GameManager.colors[color];
        if (tag == "Barrier") { setcolor.a = barrier_alpha; }
        change_material.SetColor("_Color", setcolor);
    }

    void Update() {

        if(type == Type.Gate || type == Type.inverse_Gate) {
            bool state = (GameManager.color_states[color] || type == Type.inverse_Gate);
            Vector3 targetpos;
            if (state) {
                targetpos = new Vector3(transform.position.x, start_pos.y - 2.5f, transform.position.z);
            } else {
                targetpos = new Vector3(transform.position.x, start_pos.y, transform.position.z); ;
            }
            
            transform.position = Vector3.Lerp(transform.position, targetpos, 0.1f);
        }

    }
}