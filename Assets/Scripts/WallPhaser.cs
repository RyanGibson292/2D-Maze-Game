using UnityEngine;

public class WallPhaser : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            Player player = other.gameObject.GetComponent<Player>();
            player.DoWallPhaseEffect();
            Destroy(this.gameObject);
        }
    }
}