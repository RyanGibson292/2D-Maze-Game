﻿using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour {
    private Rigidbody2D body;
    private int deathTimer, hostagesSaved, lives = 1, hostageCount;
    public TextMeshPro deathText;
    private Vector3 spawnPoint;

    void Start() {
        body = GetComponent<Rigidbody2D>();
        spawnPoint = transform.position;
    }

    void Update() {
        Vector3 velocity = body.velocity;

        if (deathTimer < 1) {
            if (Input.GetKey("left") || Input.GetKey("a")) {
                body.AddForce(new Vector2(-0.5f, 0.0f));
            } else if (Input.GetKey("right") || Input.GetKey("d")) {
                body.AddForce(new Vector2(0.5f, 0.0f));
            } else {
                if (velocity.x > 0.5f) {
                    velocity.x = 0.5f;
                } else if (velocity.x < -0.5f) {
                    velocity.x = -0.5f;
                }
            }

            if (Input.GetKey("up") || Input.GetKey("w")) {
                body.AddForce(new Vector2(0.0f, 0.5f));
            } else if (Input.GetKey("down") || Input.GetKey("s")) {
                body.AddForce(new Vector2(0.0f, -0.5f));
            } else {
                if (velocity.y > 0.5f) {
                    velocity.y = 0.5f;
                } else if (velocity.y < -0.5f) {
                    velocity.y = -0.5f;
                }
            }
        } else {
            deathTimer++;
            if (deathTimer >= 400) {
                deathTimer = 0;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        Vector3 position = transform.position;

        if (position.x > 36f) {
            position.x = 36f;
            velocity.x = 0;
        } else if (position.x < -36f) {
            position.x = -36f;
            velocity.x = 0;
        }

        if (position.y > 36f) {
            position.y = 36f;
            velocity.y = 0;
        } else if (position.y < -36f) {
            position.y = -36f;
            velocity.y = 0;
        }

        transform.position = position;
        body.velocity = velocity;

        if(hostagesSaved >= hostageCount) {
            deathText.text = "You Win!!!";
            SceneManager.LoadScene("Winning Screen");
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Maze")) {
            lives--;
            transform.position = this.spawnPoint;
            if(lives <= 0) {
                deathTimer++;
                if (deathText != null) {
                    deathText.text = "You died!";
                }
            }
        }
    }

    public void SaveHostage() {
        this.hostagesSaved++;
    }

    public void SetLives(int livesIn) {
        this.lives = livesIn;
    }

    public int GetLives() {
        return this.lives;
    }

    public Vector3 GetSpawnPoint() {
        return this.spawnPoint;
    }

    public void AddHostages(int amount) {
        this.hostageCount += amount;
    }
}
