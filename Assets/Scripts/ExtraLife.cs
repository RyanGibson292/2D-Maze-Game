using UnityEngine;

public class ExtraLife : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")) {
            Player player = other.gameObject.GetComponent<Player>();
            player.SetLives(player.GetLives() + 1);
            Destroy(this.gameObject);
        }
    }
}
