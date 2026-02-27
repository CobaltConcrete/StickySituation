using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovement : MonoBehaviour {
    Rigidbody2D playerRigidbody;

    float xAxis;
    float yAxis;

	// Use this for initialization
	void Start () {
        playerRigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");

        moveFunction3();

	}

    void moveFunction1() {
        Vector2 movementVector = new Vector2(xAxis, yAxis);
        playerRigidbody.AddForce(movementVector);
    }

    void moveFunction2() {
        Vector2 movementVector = new Vector2(xAxis, yAxis);
         movementVector = movementVector * Time.deltaTime * 30;
        playerRigidbody.MovePosition(playerRigidbody.position + movementVector);
    }

    void moveFunction3() {
        Vector2 movementVector = new Vector2(xAxis, yAxis);
        movementVector = movementVector * 5;
        playerRigidbody.linearVelocity = movementVector;
    }
}
