using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public List<Transform> lstBack;

    public List<Transform> lstFloor;
    public Transform player;

    public float blockWidth;
    public float blockPadding;

    private int headBlockIdx;
    private int tailBlockIdx;
    private int currBlockIdx;        // 플레이어가 위치해 있는 블럭 위치

	void Start () {
        tailBlockIdx = 0;
        currBlockIdx = 4;
        headBlockIdx = 4;

        //Debug.Log("Start tailBlockIdx : " + tailBlockIdx + ", currBlockIdx : " + currBlockIdx + ", headBlockIdx : " + headBlockIdx);
    }
		
	void Update () {
        if (player.position.x > GetCurrBlockXPos())
        {
            MoveBeforeBlockToNextAfterBloc();
        }
    }

    float GetCurrBlockXPos()
    {
        return lstFloor[currBlockIdx].position.x;
    }

    void MoveBeforeBlockToNextAfterBloc()
    {
        
        Transform tailBlock = lstFloor[tailBlockIdx];
        Transform headBlock = lstFloor[headBlockIdx];

        tailBlock.position = new Vector3(headBlock.position.x + blockWidth + blockPadding, 0f, 0f);
        
        tailBlockIdx = tailBlockIdx + 1;
        if (tailBlockIdx > lstFloor.Count - 1)
            tailBlockIdx = 0;
        currBlockIdx = currBlockIdx + 1;
        if (currBlockIdx > lstFloor.Count - 1)
            currBlockIdx = 0;
        headBlockIdx = headBlockIdx + 1;
        if (headBlockIdx > lstFloor.Count - 1)
            headBlockIdx = 0;
        
        //Debug.Log("beforeBlockIdx : " + tailBlockIdx + ", currBlockIdx : " + currBlockIdx + ", headBlockIdx : " + headBlockIdx);
    }
}
