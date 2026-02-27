using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {
    Rigidbody2D rb;
    Vector2 movement;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        movement = new Vector2(500, 0);
        rb.AddForce(movement);

    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
