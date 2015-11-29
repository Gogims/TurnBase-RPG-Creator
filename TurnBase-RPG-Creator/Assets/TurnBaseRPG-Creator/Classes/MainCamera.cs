using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {
    GameObject camera;
    GameObject player;
	// Use this for initialization
	void Start () {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.FindGameObjectWithTag("RPG-PLAYER"); 
	}
	
	// Update is called once per frame
	void Update () {
        if (player != null)
            camera.transform.position = new Vector3(player.transform.position.x,player.transform.position.y,-10);

	}
}
