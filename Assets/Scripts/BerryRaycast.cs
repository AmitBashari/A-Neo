using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryRaycast : MonoBehaviour
{
    //public GameObject Player;
    public BoxCollider2D PlayerBoxCollider2D;
    public FollowWP RaycastFollowWP;


    private float _distance = 1f;

    private void Awake()
    {
        PlayerBoxCollider2D = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        RaycastFollowWP = GetComponent<FollowWP>();



    }
    void Update()
    {
        /*
        Vector2 start = transform.position;
        Vector2 direction = (Player.transform.position - transform.position).normalized;

        
        Debug.DrawRay(start, direction * _distance, Color.red);

        
        RaycastHit2D[] sightTestResults = Physics2D.RaycastAll(start, direction, _distance);

        //now iterate over all results to work out what has happened
        for (int i = 0; i < sightTestResults.Length; i++)
        {
            RaycastHit2D sightTest = sightTestResults[i];

            gameObject.GetComponent<FollowWP>().enabled = false;
        }
        */

        Debug.DrawRay(transform.position, transform.InverseTransformDirection(Vector2.up) * _distance, Color.red);
        Debug.DrawRay(transform.position, transform.InverseTransformDirection(Vector2.down) * _distance, Color.red);
        Debug.DrawRay(transform.position, transform.InverseTransformDirection(Vector2.left) * _distance, Color.red);
        Debug.DrawRay(transform.position, transform.InverseTransformDirection(Vector2.right) * _distance, Color.red);

        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, transform.InverseTransformDirection(Vector2.up), _distance);
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, transform.InverseTransformDirection(Vector2.down), _distance);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, transform.InverseTransformDirection(Vector2.left), _distance);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, transform.InverseTransformDirection(Vector2.right), _distance);


        if (hitLeft.collider == PlayerBoxCollider2D)
        {
            Debug.Log("I hit Player");

            RaycastFollowWP.ReverseArray();
            //_distance = 0;
            
        }
    }

}
