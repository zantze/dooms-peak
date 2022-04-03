using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiTrickText : MonoBehaviour
{
    float startTimer = 1f;
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 50 * Time.deltaTime, 0);
        startTimer -= Time.deltaTime;
        if (startTimer < 0f)
        {

            Color color = text.color;
            color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * 1.5f);
            text.color = color;

            if (color.a < 0.1f)
            {
                Destroy(this.gameObject);
            }

        }
    }
}
