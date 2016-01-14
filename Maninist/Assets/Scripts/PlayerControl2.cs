using UnityEngine;
using System.Collections;

public class PlayerControl2 : MonoBehaviour
{
    [HideInInspector]
    public bool facingRight = true;         // For determining which way the player is currently facing.
    [HideInInspector]
    public bool jump = false;               // Condition for whether the player should jump.


    public float moveForce = 365f;          // Amount of force added to move the player left and right.
    public float maxSpeed = 5f;             // The fastest the player can travel in the x axis.
    public AudioClip[] jumpClips;           // Array of clips for when the player jumps.
    public float jumpForce = 1000f;         // Amount of force added when the player jumps.
    public AudioClip[] taunts;              // Array of clips for when the player taunts.
    public float tauntProbability = 50f;    // Chance of a taunt happening.
    public float tauntDelay = 1f;           // Delay for when the taunt should happen.


    private int tauntIndex;                 // The index of the taunts array indicating the most recent taunt.
    private Transform groundCheck;          // A position marking where to check if the player is grounded.
    private bool grounded = false;          // Whether or not the player is grounded.
    private Animator anim;
    private GameObject bazooka;            // Reference to the player's animator component.


    float time;
    Vector3 start, startt, end;
    int move, idle;
    float myAngle = -300;
    bool facingRightBaz;
  //  PlayerControl pc = new PlayerControl();


    GameObject obj;
    public PlayerControl script;
    int cnt,sgn;

    void rotate()
    {
        bazooka.transform.Rotate(0, 0, myAngle * cnt * Time.deltaTime);
        move--;
        if (move == 0 && myAngle == -300)
        {
            myAngle *= -1;
            move = 20;
        }
    }

    void reset_rotation()
    {
        bazooka.transform.eulerAngles = start;
        myAngle = -300;
        move = 20;
        idle = 0;
    }


    void Awake()
    {

        // Setting up references.
        groundCheck = transform.Find("groundCheck");
        anim = GetComponent<Animator>();

        bazooka = GameObject.FindGameObjectWithTag("bazooka2");
        facingRight = true;

        sgn = 1;
        time = 30;
        move = 20;
        idle = 0;

        obj = GameObject.Find("hero");
        script = obj.GetComponent<PlayerControl>();
        cnt = script.sgn;

        startt = bazooka.transform.localEulerAngles;
        bazooka.transform.Rotate(-180, 0, 0);
        end = bazooka.transform.localEulerAngles;

        bazooka.transform.Rotate(180, 0, 0);
    }


    void Update()
    {
        if (idle == 0)
            if (sgn == 1) bazooka.transform.eulerAngles = startt;
            else bazooka.transform.eulerAngles = end;

        // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (Input.GetKey(KeyCode.X)) {
            Application.LoadLevel("scene3");
        }
        if (Input.GetKey(KeyCode.C)) {
            Application.LoadLevel("scene4");
        }
        if (Input.GetKey(KeyCode.Z)) {
            Application.LoadLevel("scene2");
        }
        if (Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
        }

        // If the jump button is pressed and the player is grounded then the player should jump.
        if (Input.GetButtonDown("Jump2") && grounded)
            jump = true;

        bazooka.transform.position = new Vector2(transform.localPosition.x + sgn*0.35f, transform.localPosition.y -0.3f);


        time -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.P) && idle == 0)
        {
            start = bazooka.transform.localEulerAngles;
            idle = 1;
        }

        if (idle == 0)
            return;

        if (move > 0)
            rotate();

        if (move == 0 && myAngle == 300)
            reset_rotation();
    }
    


    void FixedUpdate()
    {
        // Cache the horizontal input.
        float h = Input.GetAxis("Horizontal2");

        // The Speed animator parameter is set to the absolute value of the horizontal input.
        anim.SetFloat("Speed", Mathf.Abs(h));

        // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
        if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
            // ... add a force to the player.
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);

        // If the player's horizontal velocity is greater than the maxSpeed...
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
            // ... set the player's velocity to the maxSpeed in the x axis.
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

        // If the input is moving the player right and the player is facing left...
        if (h > 0 && !facingRight)
            // ... flip the player.
            Flip();
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (h < 0 && facingRight)
            // ... flip the player.
            Flip();

        // If the player should jump...
        if (jump)
        {
            // Set the Jump animator trigger parameter.
            anim.SetTrigger("Jump2");

            // Play a random jump audio clip.
            int i = Random.Range(0, jumpClips.Length);
            AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);

            // Add a vertical force to the player.
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));

            // Make sure the player can't jump again until the jump conditions from Update are satisfied.
            jump = false;
        }
    }


    void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        sgn = -sgn;
        if (sgn == 1) bazooka.transform.eulerAngles = startt;
        else bazooka.transform.eulerAngles = end;
    }


    public IEnumerator Taunt()
    {
        // Check the random chance of taunting.
        float tauntChance = Random.Range(0f, 100f);
        if (tauntChance > tauntProbability)
        {
            // Wait for tauntDelay number of seconds.
            yield return new WaitForSeconds(tauntDelay);

            // If there is no clip currently playing.
            if (!GetComponent<AudioSource>().isPlaying)
            {
                // Choose a random, but different taunt.
                tauntIndex = TauntRandom();

                // Play the new taunt.
                GetComponent<AudioSource>().clip = taunts[tauntIndex];
                GetComponent<AudioSource>().Play();
            }
        }
    }


    int TauntRandom()
    {
        // Choose a random index of the taunts array.
        int i = Random.Range(0, taunts.Length);

        // If it's the same as the previous taunt...
        if (i == tauntIndex)
            // ... try another random taunt.
            return TauntRandom();
        else
            // Otherwise return this index.
            return i;
    }
}
