using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class MyTurtle : TurtleBase
{
    public int poly = 3;
    public float scale =5;


    // Turn(float degrees)
    // Advance(float distance)
    // ChangeColor(Color color)
    // PenUp()
    // PenDown()


    // Start is called before the first frame update
    void Start()
    {
        float basestem= 0.5f;
        int basepetal=3;
        float basesize= 3;
        int dir = 1;
        float spacing = 1f;

    for (int k =0; k < 2; k++) 
{
       for (float j =0; j < 16; j += spacing) 
       {
        Flower(basestem, basepetal, basesize, Color.HSVToRGB(Random.value,1,1) );
        Turn(90*dir);
        Advance(spacing );
        Turn(90*-dir);
        basepetal= Random.Range(3, 10);
        basestem = Random.Range(1, 6);
        basesize = 4 + Random.Range(-2f, 6f ); 
        spacing = 1+ Random.Range(-0.5f, 0.5f);
        }
        
        dir *= -1;

}    
    }




    void Flower(float stemsize, int petals, float petalsize, Color flwrClr)
    {
        PenDown();

        ChangeColor(Color.green);
        Advance(stemsize);

        ChangeColor(flwrClr);
        Turn(-90+180/petals);
        Polygon(petals, petalsize);
        
        Turn(180);
        Advance(stemsize);
        Turn(180);
        
    }
    void Polygon(int sides, float scale)
    {
        float angle =  360f/ sides;
        float length = scale/sides; 
        PenDown();
        for(int i=0; i<sides; i++)
        {

            Advance(length);
            Turn(angle);
            
        }
        PenUp();
        Turn(90 + -180/sides);
        
    }
}
 