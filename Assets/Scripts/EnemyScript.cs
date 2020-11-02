using UnityEngine;
using System;

public class EnemyScript : MonoBehaviour {
    public int min, max;
    public bool moveVertically, hasWeapon;
    public float moveSpeed = 0.5f;
    public GameObject weapon, target;
    private Rigidbody2D rigidBody2D;
    private int currentTicks;

    private void Start() {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        currentTicks++;
        if (target == null) {
            ApplyNormalMovement();
        } else { // they have a target
            if (Vector2.Distance(transform.position, target.transform.position) > 10f) { // if the player is too far away
                target = null; // they have no target anymore
                return;
            }

            Vector3 displacement = target.transform.position - transform.position;
            displacement = displacement.normalized;
            transform.position += (displacement * TripleSqrt(moveSpeed) * Time.deltaTime);
        }
    }

    private float TripleSqrt(float numb) {
        return (float)Math.Sqrt(Math.Sqrt(Math.Sqrt(numb)));
    }

    private void ApplyNormalMovement() {
        if (moveVertically) {
            if (currentTicks < 10) {
                rigidBody2D.AddForce(new Vector3(0f, moveSpeed));
            }

            if (transform.position.y >= max) {
                rigidBody2D.AddForce(new Vector3(0f, -moveSpeed));
            } else if (transform.position.y <= min) {
                rigidBody2D.AddForce(new Vector3(0f, moveSpeed));
            }
        } else { // move horizontal
            if (currentTicks < 10) {
                rigidBody2D.AddForce(new Vector3(moveSpeed, 0f));
            }

            if (transform.position.x >= max) {
                rigidBody2D.AddForce(new Vector3(-moveSpeed, 0f));
            } else if (transform.position.x <= min) {
                rigidBody2D.AddForce(new Vector3(moveSpeed, 0f));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) { // if they collided with a player
            target = other.gameObject;
            Debug.Log(target.name);
        }
    }
}
