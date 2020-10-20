using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D body;

    void Start() {
        body = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if(Input.GetKey("left") || Input.GetKey("a")) {
            body.AddForce(new Vector2(-0.5f, 0.0f));
        } else if(Input.GetKey("up") || Input.GetKey("w")) {
            body.AddForce(new Vector2(0.0f, 0.5f));
        } else if(Input.GetKey("right") || Input.GetKey("d")) {
            body.AddForce(new Vector2(0.5f, 0.0f));
        } else if(Input.GetKey("down") || Input.GetKey("s")) {
            body.AddForce(new Vector2(0.0f, -0.5f));
        }
    }
}
