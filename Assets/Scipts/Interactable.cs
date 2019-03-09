using UnityEngine;
using UnityEditor;

public class Interactable : MonoBehaviour
{
    [HideInInspector] public int type; //"Button", "Lever", "Gate", "Player Barrier", "Laser Barrier"
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

        if(type == 2) {
            Vector3 targetpos;
            if (GameManager.color_states[color]) {
                targetpos = new Vector3(transform.position.x, start_pos.y - 2.5f, transform.position.z);
            } else {
                targetpos = new Vector3(transform.position.x, start_pos.y, transform.position.z); ;
            }
            
            transform.position = Vector3.Lerp(transform.position, targetpos, 0.1f);
        }

    }
}



[CustomEditor(typeof(Interactable))]
public class InteractableEditor : Editor { 
    public override void OnInspectorGUI() {
        
        var myScript = target as Interactable;

        string[] types = { "Button", "Lever", "Gate", "Player Barrier", "Laser Barrier" };
        myScript.type = EditorGUILayout.Popup("Type", myScript.type, types);

        DrawDefaultInspector();
    }

}