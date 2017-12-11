using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour {

    public double speed = 1f;
    private Transform tr;

    public float endTime = 10f;     // 30초이상 비추
    public Text speedLabel;

    [Range(0.1f, 1.0f)]
    public float userTrim;          // 0.1 - 1.0
    private double trim;            // 가속 조정값(0.000001f - 0.00001f)

    ParticleSystem speedEffect;

    private void Awake()
    {
        speedEffect = GetComponentInChildren<ParticleSystem>();
        speedEffect.Stop();
    }

    void Start () {
        tr = GetComponent<Transform>();
        CalcSpeed();

        trim = userTrim / 100000f;
    }

	void Update () {
        double x = speed * Time.deltaTime;
        tr.Translate(new Vector3((float)x, 0f, 0f), Space.World);
	}

    // 지수적으로 빨라지게
    void CalcSpeed()
    {
        StartCoroutine(ProcCalcSpeed());
    }

    IEnumerator ProcCalcSpeed()
    {
        yield return new WaitForSeconds(1f); // 1초 후에 가속

        float deltaTime = 0;
        DateTime startTiem = DateTime.Now;

        while(true)
        {
            deltaTime += Time.deltaTime;
            if(deltaTime > endTime)
            {
                break;
            }

            long elapsed = DateTime.Now.Ticks - startTiem.Ticks;
            speed = CalcPow(elapsed / 1000);
            speedLabel.text = string.Format("Speed : {0:N2}", speed);

            if(speed > 5.0f)
            {
                speedEffect.Play();
            }

            yield return null;
        }

        yield break;
    }

    double CalcPow(double elaped)
    {
        // 가속비율(값이 클수록 수치가 빨리 올라감)
        // double trim = 0.00006f;        // 부스타빗에 있던 수치 - 틱당 간격은 150
        // double trim = 0.000006f;       // 현재 FPS하에서 적당한 수치
        // double trim = 0.000001f;       // 거의 속도감이 안느껴지는 수치

        //double r = 0.000005f;          

        return Math.Exp(trim * elaped);
    }
}
