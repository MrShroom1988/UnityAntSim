﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{

    // Movement variables
    float speed = 3;
    Vector3 direction = Vector3.forward;
    float wiggleSpeed = 1;
    float wiggleAngle = 45;
    float newMovementAngle;

    // Sense variables
    public float coneWidth = 120;
    public float coneRadius = 10;
    public float smallestToBeSensedObjectWidth = 1;
    float coneRayInterval;

    // Debug stuff
    public LineRenderer lineRenderer;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Some math that makes sure that we are using the minimal amount of rays possible
        // while not missing any objects larger than smallestToBeSensedObjectWidth
        coneRayInterval = Mathf.Atan(smallestToBeSensedObjectWidth/coneRadius) * Mathf.Rad2Deg;
        
    }

    // Wiggle by choosing a new direction and turning by wigglespeed towards that direction
    // If the direction is achieved, simply choose a new direction.
    void Wiggle()
    {
        if (newMovementAngle == 0){
            newMovementAngle = Random.Range(-wiggleAngle, wiggleAngle);
        }

        // The amount we need to turn this frame
        float directionRotation = 0;
        if (newMovementAngle > 0){
            directionRotation = Mathf.Min(wiggleSpeed, newMovementAngle);
        } else {
            directionRotation = Mathf.Max(-wiggleSpeed, newMovementAngle);
        }

        // Rotate
        direction = Quaternion.Euler(0, directionRotation, 0) * direction;
        direction.Normalize();

        // Update how far we still have to rotate
        newMovementAngle -= directionRotation;
    }

    void Move()
    {
        Wiggle();
        transform.position += speed * direction * Time.deltaTime;
    }

    void CalculateConeRayInterval(){
        // Delen door nul etc voorkomen
        if (coneRadius < 0.05f) coneRadius = 0.05f;
        if (smallestToBeSensedObjectWidth < 0.05f) smallestToBeSensedObjectWidth = 0.05f;
        coneRayInterval = Mathf.Atan(smallestToBeSensedObjectWidth/coneRadius) * Mathf.Rad2Deg;
    }

    void See()
    {
        List<Vector3> rayDirections = AntSenses.GenerateSenseRayDirections(coneWidth, coneRadius, coneRayInterval);

        // Make the lines visible
        
        foreach (Vector3 ray in rayDirections){
            // Rotate the rays to face the ant's direction
            float directionAngle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
            Vector3 rotatedRay = Quaternion.Euler(0, directionAngle, 0) * ray;
            rotatedRay.Normalize();

            // Makes the lines visible for debug purposes
            if (true){
                Debug.DrawLine(transform.position, transform.position + (rotatedRay * coneRadius), Color.white);
            }
        }
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CalculateConeRayInterval();
        
        See();
        Move();
    }
}
