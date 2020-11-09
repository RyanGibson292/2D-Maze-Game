using UnityEngine;

public class SafePoint : MonoBehaviour {
    private GameObject capture;

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Enemy") && capture == null) {
            capture = other.gameObject;
            EnemyTest enemy = capture.GetComponent<EnemyTest>();
            enemy.SetTarget(this.gameObject);
        }
    }
}
