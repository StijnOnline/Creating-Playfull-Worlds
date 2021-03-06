﻿using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum Type { Button, Lever, Gate, inverse_Gate, Player_Barrier, Laser_Barrier };
    public Type type;

    public int color = 0;

    //barrier
    [HideInInspector] private float barrier_alpha = 0.25f;
    //door
    Vector3 start_pos;
    Transform door;
    

    void Start() {
        if (type == Type.Gate || type == Type.inverse_Gate) {
            door = transform.GetChild(0);
            start_pos = door.position;
        }

        Color setcolor = GameManager.colors[color];
        if (tag == "Barrier") { setcolor.a = barrier_alpha; }

        foreach (Renderer rd in GetComponentsInChildren<Renderer>()) {
            foreach (Material mat in rd.materials) {
                if (mat.name.Contains("ChangeMaterial")) { mat.SetColor("_Color", setcolor); }
            }
        }
    }

    void Update() {

        if(type == Type.Gate || type == Type.inverse_Gate) {



            bool state = (GameManager.color_states[color] ^ type == Type.inverse_Gate); // ^ = XOR
            Vector3 targetpos;
            if (state) {
                targetpos = new Vector3(door.position.x, start_pos.y - 2.5f, door.position.z);
            } else {
                targetpos = new Vector3(door.position.x, start_pos.y, door.position.z); ;
            }

            door.position = Vector3.Lerp(door.position, targetpos, 0.1f);
        }

        if (type == Type.Lever) {
            if (GameManager.color_states[color]) {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);                
            } else {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 180);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(type == Type.Laser_Barrier || other.tag == "Player")
        {
            GameManager.color_states[color] = false;
        }
    }
}