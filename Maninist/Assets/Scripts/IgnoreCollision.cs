using UnityEngine;
using System.Collections;

public class IgnoreCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //   Physics.IgnoreCollision(GameObject.Find("hero").GetComponent<CircleCollider2D>(), GetComponent<Collider>());
        Physics.IgnoreLayerCollision( GameObject.Find("Bazooka").layer, GameObject.Find("hero").layer);
        Physics.IgnoreLayerCollision(GameObject.Find("Bazooka2").layer, GameObject.Find("hero2").layer);

    }
	
	// Update is called once per frame
	void Update () {

    }
}
