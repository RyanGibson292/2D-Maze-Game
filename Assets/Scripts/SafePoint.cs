using UnityEngine;

public class SafePoint : MonoBehaviour {
    private GameObject capture;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Hostage") && capture == null) {
            capture = other.gameObject;
            Hostage hostage = other.gameObject.GetComponent<Hostage>();
            hostage.SetTrapped(true);
            hostage.transform.position = transform.position;
            hostage.GetPlayer().GetComponent<Player>().SaveHostage();
        }
    }
}
