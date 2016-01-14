using UnityEngine;
using System.Collections;

public class Emitter : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
	
	}

    void Awake() {

    }

    IEnumerator Start() {

        //Loop indefinitely
        while (true) {
            //If the player is currently not playing then wait
            while (!Manager.current.IsPlaying()) {
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
