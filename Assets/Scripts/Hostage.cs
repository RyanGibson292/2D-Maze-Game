using UnityEngine;

public class Hostage : MonoBehaviour
{
    private int ticksExisted;
    private GameObject target;
    private Rigidbody2D newRigidbody;
    public bool isTrapped;
    public GameObject player;

    void Start()
    {
        newRigidbody = GetComponent<Rigidbody2D>();
        player.GetComponent<Player>().AddHostages(1);
    }

    void OnDestroy() {
        player.GetComponent<Player>().AddHostages(-1);
    }

    void Update()
    {
        ticksExisted++;
        if(!isTrapped) {
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
    }

    private void ApplyNormalMovement() {
        newRigidbody.AddForce(new Vector2(Random.Range(-50f, 50f), Random.Range(-50f, 50f)));
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player") && !isTrapped) {
            SetTarget(other.gameObject);
        }
    }

    public void SetTarget(GameObject targetIn) {
        this.target = targetIn;
    }

    public void SetTrapped(bool trapped) {
        this.isTrapped = trapped;
        this.SetTarget(null);
        Destroy(newRigidbody);
    }

    public GameObject GetPlayer() {
        return this.player;
    }
}
