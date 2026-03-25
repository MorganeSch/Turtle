using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class MyTurtle3 : TurtleBase
{
    public int poly = 3;
    public float scale =5;


    // Turn(float degrees)
    // Advance(float distance)
    // ChangeColor(Color color)
    // PenUp()
    // PenDown()

    // [SerializeField]
    // public struct FlowerDescription
    // {
    //     float basestem;
    //     int basepetal;
    //     float basesize;
    //     int dir;
    //     float spacing ;
    // }
    

    void Start()
    {
        float basestem= 0.5f;
        int basepetal=3;
        float basesize= 3;
        float spacing = 1f;

        List<int> lengths = new List<int>();
        for (int i = 0; i < 80; i++)
        {
        lengths.Add(Random.Range(1,10));
        }
        
        SelectionSort(lengths);
        
        foreach (int length in lengths)
        {
            Flower(length, basepetal= 3,  basesize = 4, Color.red);
            PenUp();
            Turn(90);
            Advance(1.8f);
            Turn(-90);
            PenDown();
        }

    }
    void SelectionSort(List<int> list)
    {
        for (int sorted = 0; sorted < list.Count; sorted++)
        {
            int smallest = sorted;
            for (int i = smallest + 1; i<list.Count; i++)
            {
                if (list[i] < list[smallest])
                {
                    smallest = i;
                }
                int store = list[sorted];
                list[sorted] = list[smallest];
                list[smallest] = store;
            }
        }

    }
    void InsertionSort(List<int> list)
    {
        int i =list.Count/2;
        int a = list[i];
        list.RemoveAt(i);

        int j = list.Count /4;
        list.Insert(a,j);

    }
                //Bubblesort 
        // for (int j = 0; j<list.Count; j++)
        // {
        //     for (int i =1; i<list.Count-j; i++)
        //     {
                
        //         int a = list[i-1];
        //         int b = list[i];
        //         if (a > b)
        //         {
        //             list[i-1] = b;
        //             list[i] = a;
        //         }
        //     }
        // }
    void Flower(float stemsize, int petals, float petalsize, Color flwrClr)
    {
        PenDown();

        ChangeColor(Color.green);
        Advance(stemsize);

        ChangeColor(Color.HSVToRGB(Random.value,1,1));
        Turn(FindPolygonAngle(petals));
        Polygon(petals, petalsize);
        // Turn(180);

        PenUp();

        Turn(-FindPolygonAngle(petals));
        Advance(-stemsize);
        // Turn(180);
        
    }
    void Polygon(int sides, float scale)
    {
        float angle =  360f/ sides;
        float length = scale/sides; 
        PenDown();
        for(int i=0; i<sides; i++)
        {

            Advance(length);
            Turn(AngleTurn(sides));
            
        }

        
    }
        float FindPolygonAngle(int sides)
        {
        
         return -90+180 / sides ;   
        }
        float AngleTurn(int sides)
        {
            return 360f / sides ;
        }
}

 