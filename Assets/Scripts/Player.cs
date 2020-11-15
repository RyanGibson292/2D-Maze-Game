using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour {
    private Rigidbody2D body;
    private int deathTimer, hostagesSaved, lives = 1, hostageCount, wallPhaseTimer, gameTimer;
    public TextMeshPro uiText;
    private Vector3 spawnPoint;

    void Start() {
        body = GetComponent<Rigidbody2D>();
        spawnPoint = transform.position;
    }

    void Update() {
        Vector3 cameraPos = transform.GetChild(0).transform.position;
        cameraPos.x += 3;
        cameraPos.y += 6;
        cameraPos.z = 0;
        uiText.transform.position = cameraPos;
        Vector3 velocity = body.velocity;

        if (uiText.text == null || uiText.text == "") {
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
            uiText.text = "You Win!!!\nYou completed it in: " + (gameTimer / 350) + " seconds\n\nPress 'e' to restart.";
            if(Input.GetKey("e")) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        if(wallPhaseTimer > 0) {
            wallPhaseTimer--;
        }

        if(uiText.text == null || uiText.text == "") {
            gameTimer++;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Maze")) {
            if(wallPhaseTimer <= 0) {
                lives--;
                transform.position = this.spawnPoint;
                if(lives <= 0) {
                    deathTimer++;
                    if (uiText != null) {
                        uiText.text = "You died!";
                    }
                }
            } else {
                Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
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
