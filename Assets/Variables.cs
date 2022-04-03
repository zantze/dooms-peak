using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variables
{
    public static Variables current;

    public int score = 0;

    public Variables()
    {
        current = this;
    }
}