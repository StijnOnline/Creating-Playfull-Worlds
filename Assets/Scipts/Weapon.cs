using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


using UnityStandardAssets.Characters.FirstPerson;

public class Weapon : MonoBehaviour {

    public int color = 0;

    Camera fpscam;
    Material change_material;
    public Transform muzzle_pos;
    public GameObject muzzleflash;

    public GameObject laser;
    LineRenderer lr;
    int layerMask;
    float counter, lineDrawSpeed;
    
    public AudioSource SFXlaser;
    public AudioSource SFXgunswitch;

    private IEnumerator coroutine;

    public AudioSource interactsound;

    void Start(){
		fpscam = GetComponent<Camera> ();

        foreach (Material mat in GetComponentInChildren<Renderer>().materials)
        {
            if (mat.name.Contains("ChangeMaterial"))
            {
                change_material = mat;
                
            }
        }
        change_material.SetColor("_Color", GameManager.colors[color]);


        layerMask = 1 << LayerMask.NameToLayer("RayCastIgnore");
        layerMask = ~layerMask;
        
    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.M)) {

            //disable mouselock (made m_MouseLook public in FirstPersonController because i couldn't get a reference otherwise)
            transform.parent.GetComponent<FirstPersonController>().m_MouseLook.SetCursorLock(false);

            SceneManager.LoadScene(0);
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f) {
            color++;
            if (color > GameManager.colors.Length - 1) {
                color = 0;
            }
            change_material.SetColor("_Color", GameManager.colors[color]);
            SFXgunswitch.Play();
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f) {
            color--;
            if (color < 0) {
                color = GameManager.colors.Length - 1;
            }
            change_material.SetColor("_Color", GameManager.colors[color]);
            SFXgunswitch.Play();
        }
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        
        

    }

    void Shoot() {
        SFXlaser.Play();

        if (muzzleflash != null && muzzle_pos != null) {
            GameObject ob = Instantiate(muzzleflash, muzzle_pos);
            var ps = ob.GetComponent<ParticleSystem>().main;
            ps.startColor = GameManager.colors[color];
            Destroy(ob, 1f);
        }

        RaycastHit hit;
        if (Physics.Raycast(fpscam.transform.position, fpscam.transform.forward, out hit,Mathf.Infinity, layerMask))
        {
            GameObject lsr = Instantiate(laser, muzzle_pos.transform.position, muzzle_pos.transform.rotation);
            lr = lsr.GetComponent<LineRenderer>();

            lsr.GetComponent<Laser>().positions[0] = muzzle_pos.position;
            lsr.GetComponent<Laser>().positions[1] = hit.point;
            
            if (lsr.GetComponent<ParticleSystem>()) {
                var ps = lsr.GetComponent<ParticleSystem>().main;
                ps.startColor = GameManager.colors[color];
            }

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
            
            //set colors
            lr.startColor = GameManager.colors[color];
            lr.endColor = GameManager.colors[color];           

        }
    }

   

    private IEnumerator Interact(float waitTime,Interactable hit_interactable)
    {
        yield return new WaitForSeconds(waitTime);
        if (hit_interactable.type == Interactable.Type.Button) { GameManager.color_states[hit_interactable.color] = true; }
        if (hit_interactable.type == Interactable.Type.Lever) { GameManager.color_states[hit_interactable.color] = !GameManager.color_states[hit_interactable.color]; }

        if (hit_interactable.type == Interactable.Type.Button || hit_interactable.type == Interactable.Type.Lever) {
            interactsound.Play();
        }
    }

}
