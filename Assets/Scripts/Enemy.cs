using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int ticksExisted;
    private GameObject target;
    private Rigidbody2D newRigidbody;

    void Start() {
        newRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ticksExisted++;
        if(target != null) {
            if(Vector2.Distance(transform.position, target.transform.position) > 20f) {
                target = null;
            } else {
                Vector3 displacement = target.transform.position - transform.position;
                displacement = displacement.normalized;

                newRigidbody.AddRelativeForce(displacement * 0.25f, ForceMode2D.Force);
            }
        } else {
            ApplyNormalMovement();
        }
    }

    private void ApplyNormalMovement() {
        newRigidbody.AddForce(new Vector2(Random.Range(-25f, 25f), Random.Range(-25f, 25f)));
    }

    public void SetTarget(GameObject targetIn) {
        this.target = targetIn;
    }
}
