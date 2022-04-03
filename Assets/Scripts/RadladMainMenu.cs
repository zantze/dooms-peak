using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadladMainMenu : MonoBehaviour
{

    public bool radladCutscene = false;
    // Start is called before the first frame update
    void Start()
    {

        if (radladCutscene)
        {
            this.GetComponent<Animator>().Play("CutsceneFlying");
        }
       

        else
        {
            this.GetComponent<Animator>().Play("MainMenuPose");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
