using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class Vectrosity_Test : MonoBehaviour {

    private VectorLine myLine;
    private VectorLine myLine2;
    List<Vector2> linePoints = new List<Vector2>() { new Vector2(20, 30), new Vector2(100, 50) };
	
	void Start () {
        //VectorLine.SetLine(Color.green, new Vector2(0, 0), new Vector2(Screen.width - 1, Screen.height - 1));
        //VectorLine.SetRay(Color.green, transform.position, transform.forward * 5.0f);
        //myLine = VectorLine.SetLine(Color.green, RandomPoint(), RandomPoint());
        myLine2 = new VectorLine("Line", linePoints, 10.0f, LineType.Continuous, Joins.Weld);
        myLine2.maxWeldDistance = 20;
    }

    Vector2 RandomPoint()
    {
        return new Vector2(Random.RandomRange(0, Screen.width), Random.RandomRange(0, Screen.height));
    }
	
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //myLine.points2.Add(RandomPoint());
            //myLine.Draw();

            //linePoints.Add(RandomPoint());
            myLine2.points2.Add(RandomPoint());
            myLine2.Draw();

        }
	}


}
