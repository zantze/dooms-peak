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


    public Tiles tiles;
    // Start is called before the first frame update
    void Start()
    {
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
            landed = true;
        }

        else
        {
            grounded = false;
            took_off = true;
        }


        speed += Time.deltaTime / 4;
        downwardSpeed = speed;

        go = new Vector3(0, go.y, 0);

        go += transform.forward * downwardSpeed;


        if (grounded)
        {
            //currentGoDir = goDir2;
            holdSpin = false;
        }


        float angle = Vector3.Angle(transform.forward, targetDir);
        float angle2 = Vector3.Angle(goDir, targetDir);
        float angle3 = Vector3.Angle(transform.forward, goDir);
        float brakeFactor = 1 - (Mathf.Abs(90 - angle) / 90);

        if (angle2 < 90f && grounded)
        {
            goDir = Vector3.Lerp(goDir, targetDir, Time.deltaTime * 3);
        }

        else
        {
            if (speed > 0)
                speed -= Time.deltaTime / 2;

            if (grounded)
            {
                goDir = Vector3.Lerp(goDir, targetDir, Time.deltaTime * 3 * (1 - brakeFactor));
            }
        }

        Vector3 actualGoDir = goDir;

        if (angle3 > 90)
        {
            actualGoDir = -actualGoDir;
        }

        Debug.Log(actualGoDir);

        if (angle > 80 && angle < 100 && grounded)
            actualGoDir.x *= (1 - brakeFactor);

        go += actualGoDir * speed;



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
        Debug.DrawRay(transform.position, actualGoDir, Color.blue);
        Debug.DrawRay(transform.position, targetDir, Color.red);

        howMuchMoved = transform.position - startPos;

        if (Mathf.Abs(howMuchMoved.z) > 50f)
        {
            tiles.shiftDown();
            startPos = transform.position;
        }

        visual.LookAt(visual.transform.position + targetDir);

        animations();

        took_off = false;
        landed = false;
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
    }

    private void holdjump()
    {
        holdingjump = true;
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
        targetDir = Quaternion.Euler(0, dire * turnSpeed * Time.deltaTime * 144, 0) * targetDir;
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
            Debug.Log("Die");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Flagpole pole = other.gameObject.GetComponent<Flagpole>();
        if (pole != null )
        {
            Debug.Log("points");
        }
    }
}
