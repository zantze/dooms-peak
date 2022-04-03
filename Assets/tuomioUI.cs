using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tuomioUI : MonoBehaviour
{
    public static tuomioUI current;
    public Text trickDisplay;
    public GameObject trickPrefab;

    public int score;

    // Start is called before the first frame update
    void Start()
    {
        current = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
        
    public void DisplayTrick(Trick trick)
    {
        GameObject text = Instantiate(trickPrefab, transform.position, Quaternion.identity, this.transform);
        text.GetComponent<Text>().text = trick.name + " - " + trick.points;
        score += trick.points;
    }
}
