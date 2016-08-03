using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float moveSpeed = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetAxis("Horizontal") > 0.1f)
            transform.position += Vector3.right * Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        if(Input.GetAxis("Horizontal") < 0.1f)
            transform.position -= Vector3.left * Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

    }
}
