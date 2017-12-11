using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private Transform tr;
    public Transform player;
    private Vector3 offset;

	// Use this for initialization
	void Start () {
        tr = GetComponent<Transform>();
        offset = transform.position - player.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = player.position + offset;
	}
}
