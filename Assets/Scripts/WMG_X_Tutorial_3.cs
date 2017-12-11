using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WMG_X_Tutorial_3 : MonoBehaviour
{
    public Object emptyGraphPrefab;
    public bool plotOnStart;
    public bool plottingData
    {
        get { return _plottingData; }
        set
        {
            if (_plottingData != value)
            {
                _plottingData = value;
                plottingDataC.Changed();
            }
        }
    }
    [SerializeField]
    private bool _plottingData;

    public float plotIntervalSeconds;       // 현재 포인트에서 다음 포인트까지의 대기시간(지수적으로 작아적야 함)
    public float plotAnimationSeconds;      // 현재 끝점에 새로운 포인트까지 라인그리는 시간 (0이면 애니없음)
    Ease plotEaseType = Ease.OutQuad;
    public float xInterval;                 // 라인길이

    float addPointAnimTimeline;
    Tween blinkingTween;
    public bool moveXaxisMinimum;

    private List<WMG_Change_Obj> changeObjs = new List<WMG_Change_Obj>();
    private WMG_Change_Obj plottingDataC = new WMG_Change_Obj();

    WMG_Axis_Graph graph;
    WMG_Series series1;  // 데이터 관리자
    GameObject graphOverlay;

    void Start()
    {
        changeObjs.Add(plottingDataC);

        GameObject graphGO = GameObject.Instantiate(emptyGraphPrefab) as GameObject;
        graphGO.transform.SetParent(this.transform, false);
        graph = graphGO.GetComponent<WMG_Axis_Graph>();
        //graph.stretchToParent(graphGO); // 화면에 맞게 늘리기

        graphOverlay = new GameObject();
        graphOverlay.AddComponent<RectTransform>();
        graphOverlay.name = "Graph Overlay";
        graphOverlay.transform.SetParent(graphGO.transform, false);

        graph.autoAnimationsEnabled = false;
        graph.yAxis.MaxAutoGrow = true; // auto increase yAxis max if a point value exceeds max
        graph.yAxis.MinAutoGrow = true; // auto decrease yAxis min if a point value exceeds min

        //graph.xAxis.AxisNumTicks = 5;
        Debug.Log("graph.xAxis.AxisMaxValue : " + graph.xAxis.AxisMaxValue);
        Debug.Log("graph.xAxis.AxisLinePadding : " + graph.xAxis.AxisLinePadding);
        Debug.Log("graph.xAxis.AxisNumTicks : " + graph.xAxis.AxisNumTicks); //

        series1 = graph.addSeries();
        //AddData();
        //if (series1Data2.Count > 0)
        //    series1.pointValues.SetList(data);

        Debug.Log("series1.UseXDistBetweenToSpace : " + series1.UseXDistBetweenToSpace);
        Debug.Log("series1.AutoUpdateXDistBetween : " + series1.AutoUpdateXDistBetween);

        //increment = 1f / 10f;// graph.xAxis.AxisMaxValue;

        plottingDataC.OnChange += PlottingDataChanged;
        if (plotOnStart)
        {
            plottingData = true;
        }
    }

    void PlottingDataChanged()
    {
        //Debug.Log("plottingData: " + plottingData);
        if (plottingData)
        {
            StartCoroutine(plotData());
        }
    }


    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //StartCoroutine("ProcXTick");
            graph.xAxis.AxisNumTicks++;
        }

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    AddData();
        //}
        
    }

    void AddData()
    {
        /*
        series1Data2.Add(series1Data2.Count);
        //series1Data2[series1Data2.Count - 1].ToString()

        List<string> groups = new List<string>();
        List<Vector2> data = new List<Vector2>();
        for (int i = 0; i < series1Data2.Count; i++)
        {
            groups.Add(series1Data2[i].ToString());

            float x = (i * increment);
            float y = (i * increment);
            data.Add(new Vector2(x, y));
        }

        graph.groups.SetList(groups);
        Debug.Log(graph.groups.Count);

        graph.useGroups = true;

        series1.pointValues.SetList(data);
        */
    }

    float deltaTime = 0;
    IEnumerator ProcXTick()
    {
        while (true)
        {
            deltaTime += Time.deltaTime;
            if (deltaTime >= 1.0f)
            {
                deltaTime = 0f;
                graph.xAxis.AxisNumTicks++;
            }

            yield return null;
        }
    }

    public IEnumerator plotData()
    {
        while (true)
        {
            yield return new WaitForSeconds(plotIntervalSeconds);
            if (!plottingData) break;

            // x값은 현재데이터 인덱스 + xInterval 만큼 계속 증가, y값은 리니어하게 같이 증가
            float x = (series1.pointValues.Count == 0 ? 0 : (series1.pointValues[series1.pointValues.Count - 1].x + xInterval));
            float y = x;

            // 데이터 한개 추가후 애니메이션하기
            animateAddPointFromEnd(new Vector2(x, y), plotAnimationSeconds);
        }
    }

    void animateAddPointFromEnd(Vector2 pointVec, float animDuration)
    {
        if (series1.pointValues.Count == 0)
        { 
            series1.pointValues.Add(pointVec);
            graph.Refresh(); // Ensures gamobject list of series points is up to date based on pointValues
        }
        else
        {
            // 애니메이션으로 그래프 증가후 데이터에 추가
            series1.pointValues.Add(series1.pointValues[series1.pointValues.Count - 1]);

            // x값이 최대치에 닿았으면
            if (pointVec.x > graph.xAxis.AxisMaxValue)
            { 
                addPointAnimTimeline = 0; // animates from 0 to 1
                Vector2 oldEnd = new Vector2(series1.pointValues[series1.pointValues.Count - 1].x, series1.pointValues[series1.pointValues.Count - 1].y);
                Vector2 newStart = new Vector2(series1.pointValues[1].x, series1.pointValues[1].y);
                Vector2 oldStart = new Vector2(series1.pointValues[0].x, series1.pointValues[0].y);
                WMG_Anim.animFloatCallbacks(() => addPointAnimTimeline, x => addPointAnimTimeline = x, animDuration, 1,
                                            () => onUpdateAnimateAddPoint(pointVec, oldEnd, newStart, oldStart),
                                            () => onCompleteAnimateAddPoint(), plotEaseType);
            }
            else
            {
                // 그냥 선그리기 애니
                WMG_Anim.animVec2CallbackU(() => series1.pointValues[series1.pointValues.Count - 1], x => series1.pointValues[series1.pointValues.Count - 1] = x, animDuration, pointVec,
                                           () => updateIndicator(), plotEaseType);
            }
        }
    }

    void onUpdateAnimateAddPoint(Vector2 newEnd, Vector2 oldEnd, Vector2 newStart, Vector2 oldStart)
    {
        series1.pointValues[series1.pointValues.Count - 1] = WMG_Util.RemapVec2(addPointAnimTimeline, 0, 1, oldEnd, newEnd);
        graph.xAxis.AxisMaxValue = WMG_Util.RemapFloat(addPointAnimTimeline, 0, 1, oldEnd.x, newEnd.x);
    }

    void onCompleteAnimateAddPoint()
    {
        if (moveXaxisMinimum)
        {
            series1.pointValues.RemoveAt(0);
        }
    }

    void updateIndicator()
    {

    }
}