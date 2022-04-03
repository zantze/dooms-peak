using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroKeys : MonoBehaviour
{

    float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        if (timer > 1.5f)
        {
            Image image = GetComponent<Image>();
            Color color = image.color;  

            color.a -= Time.deltaTime / 3;
            image.color = color;
        }



        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }      
    }

  
}
