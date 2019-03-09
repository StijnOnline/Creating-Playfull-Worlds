using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    LineRenderer lr;
    ParticleSystem ps;

    public Vector3[] positions = new Vector3[3];

    public float speed;
    float counter, distance;
    public float length;

    bool reflect = false;


    void Start()
    {
        lr = GetComponent<LineRenderer>();
        distance = Vector3.Distance(positions[0], positions[1]);

        ps = GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startLifetime = speed / length / 100;

        if (positions[2] != Vector3.zero) { reflect = true; }

        speed = GameManager.laserspeed;
    }

    void Update() {

        counter += speed / distance / 100;

        Vector3 pointALongLine0 = Mathf.Lerp(0, distance, counter) * Vector3.Normalize(positions[1] - positions[0]) + positions[0];
        Vector3 pointALongLine1 = Mathf.Lerp(0, distance, counter - length / distance) * Vector3.Normalize(positions[1] - positions[0]) + positions[0];

        lr.SetPosition(0, pointALongLine0);
        transform.position = pointALongLine0;
        lr.SetPosition(1, pointALongLine1);

        if (counter > 1 && reflect) {
            GameObject go = Instantiate(gameObject);

            go.GetComponent<Laser>().positions[0] = positions[1];
            go.GetComponent<Laser>().positions[1] = positions[2];
            go.GetComponent<Laser>().positions[2] = Vector3.zero;
            reflect = false;
        }

        if (counter > (distance + length) / distance) {
            Destroy(gameObject);
        }



    }
}
