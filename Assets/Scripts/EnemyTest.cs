using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    private int ticksExisted;
    private GameObject target;
    private Rigidbody2D newRigidbody;
    public bool hasWeapon;
    public GameObject weapon;

    void Start()
    {
        newRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ticksExisted++;
        if(target != null) {
            if(Vector2.Distance(transform.position, target.transform.position) > 10f) {
                target = null;
            } else {
                Vector3 displacement = target.transform.position - transform.position;
                displacement = displacement.normalized;
                transform.position += (displacement * 2.5f * Time.deltaTime);
            }
        } else {
            ApplyNormalMovement();
        }
    }

    private void ApplyNormalMovement() {
        newRigidbody.AddForce(new Vector2(Random.Range(-50f, 50f), Random.Range(-50f, 50f)));
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")) {
            SetTarget(other.gameObject);
        }
    }

    public void SetTarget(GameObject targetIn) {
        this.target = targetIn;
    }
}
