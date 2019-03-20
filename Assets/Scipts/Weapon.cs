using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class Weapon : MonoBehaviour {

    public int color = 0;

    public GameObject laser;
    public Transform muzzle_pos;

    public GameObject muzzleflash;
    
	Camera fpscam;
	AudioSource audiosource;
    int layerMask;

    LineRenderer lr;
    float counter, lineDrawSpeed;

    private IEnumerator coroutine;

    public GameObject preview;


    void Awake(){
		fpscam = GetComponent<Camera> ();
		audiosource = GetComponent<AudioSource> ();

        UpdateHUD();

        layerMask = 1 << LayerMask.NameToLayer("RayCastIgnore");
        layerMask = ~layerMask;

        preview = Instantiate(preview);
        preview.SetActive(false);
    }

    void Update() {

        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0f) {
            color++;
            if (color > GameManager.colors.Length - 1) {
                color = 0;
            }
            UpdateHUD();
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f) {
            color--;
            if (color < 0) {
                color = GameManager.colors.Length - 1;
            }
            UpdateHUD();
        }
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        
        if (Input.GetMouseButton(1)) {
            preview.SetActive(true);
            Preview();
        } else {
            preview.SetActive(false);
        }
        

    }

    void UpdateHUD() {
        Image HUD_color = GameManager.HUD.transform.Find("Color").GetComponent<Image>();
        HUD_color.color = GameManager.colors[color];
    }


    void Shoot() { 
        if (muzzleflash != null && muzzle_pos != null) { Destroy(Instantiate(muzzleflash, muzzle_pos),1f); }
        
        RaycastHit hit;
        if (Physics.Raycast(fpscam.transform.position, fpscam.transform.forward, out hit,Mathf.Infinity, layerMask))
        {
            GameObject lsr = Instantiate(laser, muzzle_pos.transform.position, muzzle_pos.transform.rotation);
            lr = lsr.GetComponent<LineRenderer>();

            lsr.GetComponent<Laser>().positions[0] = muzzle_pos.position;
            lsr.GetComponent<Laser>().positions[1] = hit.point;

            //Destroy(lsr, 1f);

            
            //check for barriers before mirror
            while (hit.collider.tag == "Barrier") {
                GameObject ob = hit.collider.gameObject;
                Interactable hit_interactable = hit.collider.gameObject.GetComponent<Interactable>();
                if ((hit_interactable.type == Interactable.Type.Laser_Barrier && hit_interactable.color == color) || hit_interactable.type == Interactable.Type.Player_Barrier)
                {
                    ob.layer = LayerMask.NameToLayer("RayCastIgnore");

                    Vector3 incomingVec = fpscam.transform.forward;
                    if (Physics.Raycast(hit.point, incomingVec, out hit, Mathf.Infinity, layerMask))
                    {
                        lsr.GetComponent<Laser>().positions[1] = hit.point;
                    }
                    ob.layer = 0;
                }
                else
                {
                    break;
                }
            }

            //check for mirror
            if (hit.collider.tag == "Mirror")
            {
                Vector3 incomingVec = fpscam.transform.forward;
                Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);

                if (Physics.Raycast(hit.point, reflectVec, out hit))
                {
                    
                    lsr.GetComponent<Laser>().positions[2] = hit.point;

                    //check for barriers after mirror
                    while (hit.collider.tag == "Barrier") {
                        GameObject ob = hit.collider.gameObject;
                        Interactable hit_interactable = hit.collider.gameObject.GetComponent<Interactable>();
                        if ((hit_interactable.type == Interactable.Type.Laser_Barrier && hit_interactable.color == color) || hit_interactable.type == Interactable.Type.Player_Barrier) {
                            ob.layer = LayerMask.NameToLayer("RayCastIgnore");

                            if (Physics.Raycast(hit.point, reflectVec, out hit, Mathf.Infinity, layerMask)) {
                                lsr.GetComponent<Laser>().positions[2] = hit.point;
                            }

                            ob.layer = 0;
                        }
                        else
                        {
                            break;
                        }
                    }


                    
                }
            }


            

            ////check for Interactables
            if (hit.collider.tag == "Interactable") {
                Interactable hit_interactable = hit.collider.gameObject.GetComponent<Interactable>();
                if (hit_interactable.color == color) {
                    Laser lsr_script = lsr.GetComponent<Laser>();
                    float distance = Vector3.Distance(lsr_script.positions[0], lsr_script.positions[1]) ;
                    distance += Vector3.Distance(lsr_script.positions[1], lsr_script.positions[2]);
                    distance -= lsr_script.length * 2;
                    coroutine = Interact(distance / GameManager.laserspeed,hit_interactable);
                    StartCoroutine(coroutine);
                }

            }
            

            //place laser impact effect
            /*
            Transform laser_impact = lsr.transform.Find("LaserImpact");
            laser_impact.rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
            laser_impact.position = lr.GetPosition(lr.positionCount - 1);
            */


            //set colors
            lr.startColor = GameManager.colors[color];
            lr.endColor = GameManager.colors[color];           

        }
    }

    void Preview() {
        LineRenderer lr = preview.GetComponent<LineRenderer>();
        lr.SetPosition(0, muzzle_pos.transform.position);

        RaycastHit hit;
        if (Physics.Raycast(fpscam.transform.position, fpscam.transform.forward, out hit, Mathf.Infinity, layerMask)) {
            
            lr.SetPosition(1, hit.point);
            Debug.Log(hit.point);
            //check for barriers before mirror
            while (hit.collider.tag == "Barrier") {
                GameObject ob = hit.collider.gameObject;
                Interactable hit_interactable = hit.collider.gameObject.GetComponent<Interactable>();
                if ((hit_interactable.type == Interactable.Type.Laser_Barrier && hit_interactable.color == color) || hit_interactable.type == Interactable.Type.Player_Barrier) {
                    ob.layer = LayerMask.NameToLayer("RayCastIgnore");

                    Vector3 incomingVec = fpscam.transform.forward;
                    if (Physics.Raycast(hit.point, incomingVec, out hit, Mathf.Infinity, layerMask)) {
                        lr.SetPosition(1, hit.point);
                    }
                    ob.layer = 0;
                } else {
                    break;
                }
            }

            //check for mirror
            if (hit.collider.tag == "Mirror") {
                Vector3 incomingVec = fpscam.transform.forward;
                Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);

                if (Physics.Raycast(hit.point, reflectVec, out hit)) {

                    lr.positionCount = 3;
                    lr.SetPosition(2, hit.point);

                    //check for barriers after mirror
                    while (hit.collider.tag == "Barrier") {
                        GameObject ob = hit.collider.gameObject;
                        Interactable hit_interactable = hit.collider.gameObject.GetComponent<Interactable>();
                        if ((hit_interactable.type == Interactable.Type.Laser_Barrier && hit_interactable.color == color) || hit_interactable.type == Interactable.Type.Player_Barrier) {
                            ob.layer = LayerMask.NameToLayer("RayCastIgnore");

                            if (Physics.Raycast(hit.point, reflectVec, out hit, Mathf.Infinity, layerMask)) {
                                lr.SetPosition(2, hit.point);
                            }

                            ob.layer = 0;
                        } else {
                            break;
                        }
                    }



                }
            }

            //set colors
            lr.startColor = GameManager.colors[color];
            lr.endColor = GameManager.colors[color];

        } else {
            lr.SetPosition(1, fpscam.transform.position + fpscam.transform.forward * 500);
        }
    }

    private IEnumerator Interact(float waitTime,Interactable hit_interactable)
    {
        yield return new WaitForSeconds(waitTime);
        if (hit_interactable.type == Interactable.Type.Button) { GameManager.color_states[hit_interactable.color] = true; }
        if (hit_interactable.type == Interactable.Type.Lever) { GameManager.color_states[hit_interactable.color] = !GameManager.color_states[hit_interactable.color]; }
    }

}
