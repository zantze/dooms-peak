using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnding : MonoBehaviour
{

    public bool second;
    // Start is called before the first frame update
    void Start()
    {
        int score = Variables.current.score;

        if (second)
        {
            if (score > 2000)
            {
                this.GetComponent<Text>().text = "Light travels, you legend";
            }

            else if (score > 1000)
            {
                this.GetComponent<Text>().text = "You lived as you died, you crazy bastard";
            }

            else if (score > 500)
            {
                this.GetComponent<Text>().text = "You will be remembered as a hero";
            }

            else
            {
                this.GetComponent<Text>().text = "Light travels";
            }

        }
        else
        {
            this.GetComponent<Text>().text = "Score :" + Variables.current.score;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
