using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BazookaRotate : MonoBehaviour
{

    float time;

    Vector3 start, end;
    int move, idle;
    float myAngle = -300;
    bool facingRightBaz;
    PlayerControl pc = new PlayerControl();

    GameObject obj;
    public PlayerControl script;
    int cnt;

    void rotate()
    {
        transform.Rotate(0, 0, myAngle *cnt*Time.deltaTime);
        move--;
        if (move == 0 && myAngle == -300)
        {
            myAngle *= -1;
            move = 13;
        }
    }

    void reset_rotation()
    {
        transform.eulerAngles = start;
        myAngle = -300;
        move = 13;
        idle = 0;
    }

    // Use this for initialization
    void Start()
    {
        time = 30;
        move = 13;
        idle = 0;

        obj = GameObject.Find("hero");
        script = obj.GetComponent<PlayerControl>();
        cnt = script.sgn;

       
    }

    // Update is called once per frame
    void Update()
    {

        time -= Time.deltaTime;

        if (Input.GetKeyDown("space") && idle == 0)
        {
            start = transform.localEulerAngles;
            idle = 1;
        }

        if (idle == 0)
            return;

        if (move > 0)
            rotate();

        if (move == 0 && myAngle == 300)
            reset_rotation();
    }

}