using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    private int ticksExisted;
    private GameObject target;
    private Rigidbody2D rigidbody;
    public bool hasWeapon;
    public GameObject weapon;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
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
                transform.position += (displacement * 5f * Time.deltaTime);
            }
        }
    }
}
