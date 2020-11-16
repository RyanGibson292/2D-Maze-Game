using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class Player : MonoBehaviour {
    private Rigidbody2D body;
    private int deathTimer, hostagesSaved, lives = 1, hostageCount, wallPhaseTimer = 5000;
    public TextMeshPro uiText, livesText, wallPhaserEffectTimer, hostagesSavedText;
    private Vector3 spawnPoint;
    private long startTime;
    private bool dead, won;
    public GameObject wallPhaserEffect;

    void Start() {
        body = GetComponent<Rigidbody2D>();
        spawnPoint = transform.position;
        startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }

    void Update() {
        TransformUI();
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

        if(lives <= 0) {
            OnDeath();
        }
    }

    private void TransformUI() {
        Vector3 cameraPos = transform.GetChild(0).transform.position;
        cameraPos.x += 3;
        cameraPos.y += 6;
        cameraPos.z = 0;
        uiText.transform.position = cameraPos;
        cameraPos.x -= 12;
        cameraPos.y += 0.5f;
        livesText.transform.position = cameraPos;
        livesText.text = "Lives: " + this.lives;
        cameraPos.x -= 9;
        cameraPos.x += 30;
        cameraPos.y -= 16.5f;
        cameraPos.z = 0;
        hostagesSavedText.transform.position = cameraPos;
        hostagesSavedText.text = "Hostages saved: " + hostagesSaved.ToString() + "/" + hostageCount.ToString();
        if(wallPhaseTimer > 0) {
            cameraPos.x -= 30;
            cameraPos.y += 16.5f;
            wallPhaserEffect.transform.position = cameraPos;
            cameraPos.x += 11;
            cameraPos.y -= 1.75f;
            wallPhaserEffectTimer.transform.position = cameraPos;
            wallPhaserEffectTimer.text = ":" + wallPhaseTimer.ToString();
        } else {
            cameraPos.z = -1;
            wallPhaserEffect.transform.position = cameraPos;
            wallPhaserEffectTimer.transform.position = cameraPos;
        }
    }

    private Vector3 ApplyMovement(Vector3 velocity) {
        if (Input.GetKey("left") || Input.GetKey("a")) {
            if(velocity.x > 0) velocity.x = -0.5f; 
            body.AddForce(new Vector2(-0.5f, 0.0f));
        } else if (Input.GetKey("right") || Input.GetKey("d")) {
            if(velocity.x < 0) velocity.x = 0.5f; 
            body.AddForce(new Vector2(0.5f, 0.0f));
        } else {
            if (velocity.x > 0.5f) {
                velocity.x = 0.5f;
            } else if (velocity.x < -0.5f) {
                velocity.x = -0.5f;
            }
        }

        if (Input.GetKey("up") || Input.GetKey("w")) {
            if(velocity.y < 0) velocity.y = 0.5f; 
            body.AddForce(new Vector2(0.0f, 0.5f));
        } else if (Input.GetKey("down") || Input.GetKey("s")) {
            if(velocity.y > 0) velocity.y = -0.5f; 
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
        if(uiText.text != null || uiText.text == "") {
            if (other.gameObject.CompareTag("Maze")) {
                if(wallPhaseTimer <= 0) {
                    lives--;
                    transform.position = this.spawnPoint;
                } else {
                    Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                }
            } else if(other.gameObject.CompareTag("Enemy")) {
                Enemy enemy = other.gameObject.GetComponent<Enemy>();
                enemy.SetTarget(this.gameObject);
                this.SetLives(this.GetLives() - 1);
                Vector3 displacement = other.gameObject.transform.position - transform.position;
                displacement = displacement.normalized;
                displacement *= -1;
                enemy.gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(displacement, ForceMode2D.Force);
                enemy.SetTarget(null);
            }
        } else {
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    private void OnDeath() {
        dead = true;
        deathTimer++;
        if (uiText.text != null || uiText.text == "") {
            uiText.text = "You died!";
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
