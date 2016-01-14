using UnityEngine;
using System.Collections;

public class spawner2 : MonoBehaviour {

    Vector3 spawn = new Vector3(0.02f, 0.43f, 0); //hardcodat

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter2D(Collider2D target) {
        if (target.tag == "Ball") {
            target.transform.position = spawn;
            FindObjectOfType<Manager>().AddPointToPlayer2(1);
            FindObjectOfType<Manager>().RoundEnded();
        }
    }
}

