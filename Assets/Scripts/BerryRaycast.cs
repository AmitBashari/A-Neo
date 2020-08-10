using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryRaycast : MonoBehaviour
{
    //public GameObject Player;
    public BoxCollider2D PlayerBoxCollider2D;
    public FollowWP RaycastFollowWP;


    private float _distance = 3f;


    private void Awake()
    {
        PlayerBoxCollider2D = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        RaycastFollowWP = GetComponent<FollowWP>();
        //Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());

    }
    void Update()
    {

        Debug.DrawRay(transform.position, transform.InverseTransformDirection(Vector2.up) * _distance, Color.red);
        Debug.DrawRay(transform.position, transform.InverseTransformDirection(Vector2.down) * _distance, Color.red);
        Debug.DrawRay(transform.position, transform.InverseTransformDirection(Vector2.left) * _distance, Color.red);
        Debug.DrawRay(transform.position, transform.InverseTransformDirection(Vector2.right) * _distance, Color.red);

        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, transform.InverseTransformDirection(Vector2.up), _distance);
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, transform.InverseTransformDirection(Vector2.down), _distance);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, transform.InverseTransformDirection(Vector2.left), _distance);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, transform.InverseTransformDirection(Vector2.right), _distance);




        if (hitLeft.collider == PlayerBoxCollider2D || hitRight.collider == PlayerBoxCollider2D || hitUp.collider == PlayerBoxCollider2D || hitDown.collider == PlayerBoxCollider2D)
        {
            Debug.Log("Berry hit Player");

            RaycastFollowWP.WaypointsNumber -= 1;
            _distance = 0f;

            //gameObject.GetComponent<CircleCollider2D>().enabled = true;

        }
    }


}
