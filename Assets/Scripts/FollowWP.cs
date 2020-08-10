using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWP : MonoBehaviour
{
    public Transform[] Waypoints;
    public int WaypointsNumber = 0;

    private float Movespeed = 2f;

    void Start()
    {
        transform.position = Waypoints[WaypointsNumber].transform.position;
    }

    void Update()
    {
        Move();
    }

    public void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position,
        Waypoints[WaypointsNumber].transform.position,
        Movespeed * Time.deltaTime);

        if (transform.position == Waypoints[WaypointsNumber].transform.position)
        {
            WaypointsNumber += 1;
        }

        if (WaypointsNumber == Waypoints.Length)
            WaypointsNumber = 0;
    }

}
