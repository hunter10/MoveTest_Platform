using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameMng : MonoBehaviour {

    public Text resultLabel;
    public float endTime;
    public TimeSpan startTime;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            PowTest2();
        }
	}

    void PowTest()
    {
        int value = 2;
        for (int i = 0; i < 32; i++)
        {
            Debug.Log((long)Mathf.Pow(value, i));
        }
    }

    void PowTest2()
    {
        StartCoroutine("ProcPowTest2");
    }

    IEnumerator ProcPowTest2()
    {
        float deltaTime = 0;
        int value = 2; // Math.E
        double r = 0.00006f;
        DateTime startTime = DateTime.Now;
        Debug.Log(startTime.Ticks);
        while (true)
        {
            deltaTime += Time.deltaTime;
            if(deltaTime > 10) //endTime)
            {
                break;
            }

            long elapsed = DateTime.Now.Ticks - startTime.Ticks;
            TimeSpan tempTime = new TimeSpan(elapsed);
            Debug.Log(elapsed + ", " + elapsed /1000 + ", " + tempTime.TotalSeconds + ", " + tempTime.TotalMilliseconds + ", " + CalcPow(elapsed / 1000));

            resultLabel.text = string.Format("{0:N2}", CalcPow(elapsed / 1000));
            yield return null;
        }

        yield break;
    }

    double CalcPow(double elaped)
    {
        double r = 0.00006f; // 부스타빗에 있던 수치
        return Math.Exp(r * elaped);
    }
}
