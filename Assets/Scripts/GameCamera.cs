using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float speed = target.gameObject.GetComponent<Player>().speed / 3;

        speed = Mathf.Clamp(speed, 0, 2);
        Vector3 pos = target.position;

        //pos.y = + Mathf.Sin(Time.deltaTime);
        //pos.z =+ Mathf.Sin(Time.deltaTime);

        Vector3 newTarget = pos + (new Vector3(0, 23, -20).normalized * 23 * speed);
        newTarget.z -= Mathf.Clamp(speed, 0, 5) * 3;


        transform.position = Vector3.Lerp(transform.position, newTarget, Time.deltaTime * 3);
    }
}
