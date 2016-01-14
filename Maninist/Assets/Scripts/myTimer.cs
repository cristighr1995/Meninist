using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class myTimer : MonoBehaviour {

    public float myCoolTimer = 3;
    public Text timerText;

	// Use this for initialization
	void Start () {
        timerText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        myCoolTimer -= Time.deltaTime;
        timerText.text = myCoolTimer.ToString("f0");
	}
}
