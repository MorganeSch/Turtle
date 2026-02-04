using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class MyTurtle1 : TurtleBase
{
    public int poly = 3;
    public float scale =5;


    // Turn(float degrees)
    // Advance(float distance)
    // ChangeColor(Color color)
    // PenUp()
    // PenDown()
    void Start()
    {
        PenDown();
        Advance(4);
        Polygon(Random.Range(3, 9));
        PenUp();
        Turn(180);
        Advance(4);
        Turn(-90);
        Advance(3);
        Turn(-90);

    }

    // Start is called before the first frame update

    void Polygon(int cotes)
    {
        poly = cotes;
        Turn(-90 +180/cotes);
        for (int i =0; i < poly; i++)
        {
        PenDown();
        Advance(1);
        Turn(360f/poly);

        }
        Turn(90 + -180/cotes);
    }

}
 