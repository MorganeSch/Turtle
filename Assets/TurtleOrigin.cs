using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleOrigin : TurtleBase
{
    // Turn(float degrees)
    // Advance(float distance)
    // ChangeColor(Color color)
    // PenUp()
    // PenDown()


    // Start is called before the first frame update
    void Start()
    {
        Flower(1, 3, 0.5f, Color.cyan);
        Turn(90);
        Advance(1);
        Turn(-90);
        Flower(1.5f, 6, 0.5f, Color.magenta);
    }
    void Flower(float stemLength, int petals, float petalSize, Color petalColor)
    {
        ChangeColor(Color.green);
        PenDown();
        Advance(stemLength);
        Turn(-90 + 180f / petals);
        ChangeColor(petalColor);
        Polygon(petals, petalSize);
        Turn(90 - 180f / petals);
        PenUp();
        Advance(-stemLength);
    }

    void Polygon(int sides, float length)
    {
        for (int i = 0; i < sides; i++)
        {
            Advance(length);
            Turn(360f / sides);
        }
    }
}
 