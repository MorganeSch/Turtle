using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct FlowerDescription
{
    public float stemLength;
    public int petals;
    public float size;
    public Color color;
}

public class MyTurtle2 : TurtleBase
{
    // Turn(float degrees)
    // Advance(float distance)
    // ChangeColor(Color color)
    // PenUp()
    // PenDown()

    public List<FlowerDescription> flowers;


    // Start is called before the first frame update
    void Start()
    {
        // Exo: Replacer for par foreach
        for (int i = 0; i < flowers.Count; i++)
        {
            FlowerDescription flower = flowers[i];
            Flower(flower.stemLength, flower.petals, flower.size, flower.color);
            Turn(90);
            Advance(1);
            Turn(-90);
        }
    }

    // Exo: Surcharger (function overload) ou modifier cette méthode pour prendre FlowerDescription directement
    void Flower(float stemLength, int petals, float petalSize, Color petalColor)
    {
        float polygonAngle = FindPolygonAngle(petals);
        ChangeColor(Color.green);
        PenDown();
        Advance(stemLength);
        Turn(polygonAngle);
        ChangeColor(petalColor);
        Polygon(petals, petalSize);
        Turn(-polygonAngle);
        PenUp();
        Advance(-stemLength);
    }

    float FindPolygonAngle(int petals)
    {
        return -90 + 180f / petals;
    }

    void Polygon(int sides, float length)
    {
        float sidesPolygon = SidesPolygon(sides);
        for (int i = 0; i < sides; i++)
        {
            Advance(length);
            Turn(360f / sides);
        }
    }

    float SidesPolygon(int sides)
    {
        return 360f / sides;
    }
}
 