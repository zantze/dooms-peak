using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Animator animator;

    private CharacterController cc;
    public Transform visual;
    private Vector3 go;
    private Vector3 goDir;
    private Vector3 targetDir;
    private Vector3 currentGoDir;
    public float speed;

    public float downwardSpeed;
    public float turnSpeed = 1;

    private Vector3 startPos = Vector3.zero;
    private Vector3 howMuchMoved = Vector3.zero;

    private bool holdingjump = false;
    private bool holdSpin = false;

    private float jumpforce = 0f;
    private float maxjumpforce = 7f;

    private bool grounded = false;

    private bool landed = false;
    private bool took_off = false;
    private bool hitting = false;
    private bool losetrack = false;

    bool goinBackward = false;

    private Vector3 directMovement;
    private Vector3 reverseMovement;

    public ParticleSystem snowEffect;


    private float angles = 0f;
    private Vector3 previousAirAngle;

    public GameObject ragdoll;

    private bool death = false;
    private float deathTimer = 0f;


    public Tiles tiles;
    // Start is called before the first frame update
    void Start()
    {
        snowEffect.Stop();
        startPos = transform.position;
        cc = GetComponent<CharacterController>();
        goDir = transform.forward;
        targetDir = transform.forward;
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.2f))
        {
            grounded = true;
            if (hitting == false)
                landed = true;

            hitting = true;
        }

        else
        {
            grounded = false;

            if (hitting == true)
                took_off = true;

            hitting = false;
        }


        speed += Time.deltaTime / 4;
        downwardSpeed = speed;

        go = new Vector3(0, go.y, 0);

        go += transform.forward;

        if (speed > 10)
        {
            speed = 10;
        }


        if (grounded)
        {
            //currentGoDir = goDir2;
            holdSpin = false;
        }

        Vector3 goNormalized = go;
        goNormalized.y = 0;
        goNormalized = goNormalized.normalized;

        float angle = Vector3.Angle(transform.forward, targetDir);
        float angle2 = Vector3.Angle(goNormalized, targetDir);
        float angle3 = Vector3.Angle(transform.forward, goDir);
        float angle4 = Vector3.Angle(goNormalized, -targetDir);
        float brakeFactor = 1 - (Mathf.Abs(90 - angle) / 90);

        goDir = Vector3.Lerp(goDir, targetDir, Time.deltaTime * 3);

        if (losetrack && grounded)
        {
            snowEffect.Play();
        }

        else
        {
            snowEffect.Stop();
        }
        

        if (landed)
        {
            if (angle > 90)
            {
                goinBackward = true;
            }

            else
            {
                goinBackward = false;
            }
        }

        if (grounded)
        {
            if ((angle2 < 80 || angle4 < 80) && !losetrack)
            {

                if (goinBackward)
                {
                    reverseMovement = Vector3.Lerp(reverseMovement, -goDir, Time.deltaTime * 3);
                    directMovement = Vector3.Lerp(directMovement, Vector3.zero, Time.deltaTime * 3);
                }

                else
                {
                    directMovement = Vector3.Lerp(directMovement, goDir, Time.deltaTime * 3);
                    reverseMovement = Vector3.Lerp(reverseMovement, Vector3.zero, Time.deltaTime * 3);
                }

            }

            else
            {
                losetrack = true;

                if (Vector3.Angle(goNormalized, targetDir) < 35)
                {
                    goinBackward = false;
                    losetrack = false;
                }

                if (Vector3.Angle(-goNormalized, targetDir) < 35)
                {

                    goinBackward = true;
                    losetrack = false;

                }
            }

        }

        if (!grounded)
        {
            if (previousAirAngle == Vector3.zero)
            {
                previousAirAngle = targetDir;
            }

            angles += Vector3.Angle(previousAirAngle, targetDir);
            previousAirAngle = targetDir;
        }

        else
        {
            previousAirAngle = Vector3.zero;

            if (angles > 480)
            {
                tuomioUI.current.DisplayTrick(new Trick("540 air", 500));
            }

            if (angles > 300)
            {
                tuomioUI.current.DisplayTrick(new Trick("360 air", 200));
            }
            
            else if (angles > 120)
            {
                tuomioUI.current.DisplayTrick(new Trick("180 air", 50));
            }

            

            angles = 0;
        }

        go += (directMovement + reverseMovement) * speed;

        go.y += -9f * Time.deltaTime;

        if (!grounded)
        {
            // reset jump
            jumpforce = 0;
            holdingjump = false;
        }   
        else if (go.y < 0)
        {
            go.y = 0f;
        }

        cc.Move(go * Time.deltaTime);

        inputs();

        if (holdingjump)
        {
            if (jumpforce < maxjumpforce)
                jumpforce += Time.deltaTime * 4;
        }

        Debug.DrawRay(transform.position, goDir, Color.green);
        Debug.DrawRay(transform.position, go.normalized, Color.blue);
        Debug.DrawRay(transform.position, targetDir, Color.red);
        Debug.DrawRay(transform.position, reverseMovement, Color.black);

        howMuchMoved = transform.position - startPos;

        if (Mathf.Abs(howMuchMoved.z) > 50f)
        {
            tiles.shiftDown();
            startPos = transform.position;
        }


        Vector3 visualLook = Vector3.Lerp(visual.transform.position + visual.transform.forward, visual.transform.position + targetDir, Time.deltaTime * 10);
        visual.LookAt(visualLook);

        animations();

        took_off = false;
        landed = false;

        if (death)
        {
            deathTimer += Time.deltaTime;

            if (deathTimer > 1)
            {
                Time.timeScale = 1f;
            }
        }
    }

    void animations()
    {
        if (holdSpin)
        {
            animator.Play("duck");
            return;
        }

        if (!grounded)
        {
            animator.Play("flying");
            return;
        }

        if (holdingjump)
        {
            animator.Play("duck");
            return;
        }

        if (goDir.z >= 0)
        {
            animator.Play("fakie");
            return;
        }


        animator.Play("rolling");
        return;
    }


        void inputs()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rotate(1);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rotate(-1);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            holdjump();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            HoldSpin();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            letSpin();
        }

        if (Input.GetKey(KeyCode.W))
        {
            speed += Time.deltaTime;
        }
    }

    private void holdjump()
    {
        holdingjump = true;
        jumpforce = 1;
    }

    private void jump()
    {
        if (holdingjump)
        {
            go.y = jumpforce;
            jumpforce = 0;
        }
        
        holdingjump = false;
    }

    void HoldSpin()
    {
        holdSpin = true;
    }

    void letSpin()
    {
        holdSpin = false;
    }

    void rotate(float dire)
    {
        float holding = holdSpin ? 2 : 1;
        targetDir = Quaternion.Euler(0, dire * turnSpeed * Time.deltaTime * 144 * holding, 0) * targetDir;
    }

    public void bounce()
    {
        go.y += 7f;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Obstacle ob = hit.gameObject.GetComponent<Obstacle>();
        if (ob != null )
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Flagpole pole = other.gameObject.GetComponent<Flagpole>();
        if (pole != null )
        {
            tuomioUI.current.DisplayTrick(new Trick("Checkpoint", 500));
        }
    }

    public void Die()
    {
        Transform dude = visual.Find("radLad");
        dude.gameObject.SetActive(false);
        ragdoll.SetActive(true);
        ragdoll.transform.position = dude.transform.position;
        ragdoll.transform.rotation = dude.transform.rotation;

        Time.timeScale = 0.2f;

        GameCamera.current.deathCamTarget = ragdoll.transform.Find("Pelvis");

        death = true;
    }
}
