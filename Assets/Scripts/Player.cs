using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class Player : MonoBehaviour {
    private Rigidbody2D body;
    private int deathTimer, hostagesSaved, lives = 1, hostageCount, wallPhaseTimer;
    public TextMeshPro uiText;
    private Vector3 spawnPoint;
    private long startTime;
    private bool dead, won;

    void Start() {
        body = GetComponent<Rigidbody2D>();
        spawnPoint = transform.position;
        startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }

    void Update() {
        TransformText();
        Vector3 velocity = body.velocity;

        if (uiText.text == null || uiText.text == "") {
            velocity = ApplyMovement(velocity);
        } else {
            if(dead) {
                deathTimer++;
                if (deathTimer >= 400) {
                    deathTimer = 0;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }

        Vector3 position = transform.position;
        Vector3[] boundReturn = BoundPlayer(position, velocity);

        transform.position = boundReturn[0];
        body.velocity = boundReturn[1];

        if(hostagesSaved >= hostageCount) {
            DisplayWinningScreen();
        }

        if(wallPhaseTimer > 0) {
            wallPhaseTimer--;
        }
    }

    private void TransformText() {
        Vector3 cameraPos = transform.GetChild(0).transform.position;
        cameraPos.x += 3;
        cameraPos.y += 6;
        cameraPos.z = 0;
        uiText.transform.position = cameraPos;
    }

    private Vector3 ApplyMovement(Vector3 velocity) {
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
            Debug.Log(velocity);
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

        return velocity;
    }

    private Vector3[] BoundPlayer(Vector3 position, Vector3 velocity) {
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
        return new Vector3[] {position, velocity};
    }

    private void DisplayWinningScreen() {
        if(!won) {
            long currentTimeMillis = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            double currentTimeSeconds = (currentTimeMillis - startTime) / 1000;
            double currentTimeMins = currentTimeSeconds / 60;
            double currentTimeHours = currentTimeMins / 60;
            double currentTimeDays = currentTimeHours / 24;
            double currentTimeWeeks = currentTimeDays / 7;
            double toUse = currentTimeMillis;
            string toUseString = "ms";
            if(currentTimeMillis > 1000) {
                toUse = currentTimeSeconds; 
                toUseString = "s";
            } if(currentTimeSeconds > 60) {
                toUse = currentTimeMins;
                toUseString = " mins";
            } if(currentTimeMins > 60) {
                toUse = currentTimeHours;
                toUseString = " hours";
            } if(currentTimeHours > 24) {
                toUse = currentTimeDays;
                toUseString = " days";
            } if(currentTimeDays > 7) {
                toUse = currentTimeWeeks;
                toUseString = " weeks";
            }
            uiText.text = "You Win!!!\nYou completed it in: " + toUse + toUseString + ". Poggers!!\n\nPress 'e' to restart.";
            won = true;
        } else {
            body.velocity = Vector3.zero;
            if(Input.GetKey("e")) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(uiText != null || uiText.text == "") {
            if (other.gameObject.CompareTag("Maze")) {
                if(wallPhaseTimer <= 0) {
                    lives--;
                    transform.position = this.spawnPoint;
                    if(lives <= 0) {
                        dead = true;
                        deathTimer++;
                        if (uiText != null) {
                            uiText.text = "You died!";
                        }
                    }
                } else {
                    Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                }
            }
        } else {
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    public void DoWallPhaseEffect() {
        this.wallPhaseTimer += 1000;
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
