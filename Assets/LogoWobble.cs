using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoWobble : MonoBehaviour
{
    Vector3 start;

    float x = 2, y = 2.5f, z = 1.25f;
    // Start is called before the first frame update
    void Start()
    {
        start = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        x += Time.deltaTime / 4;
        y += Time.deltaTime / 5;
        z += Time.deltaTime / 7;
        transform.position = start + new Vector3(Mathf.Sin(x) / 3, Mathf.Sin(y) / 3, Mathf.Sin(z) / 3);

    }
}
