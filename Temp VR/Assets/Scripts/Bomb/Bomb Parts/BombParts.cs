using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombParts : MonoBehaviour
{
    //Parent Class with a very basic job, places the object at the Bomb Placement location
    public Transform placementLoc;
    //Used to rotate Timer so the time faces up
    public bool isInTrigger;

    //MAYBE A GLOBAL VELOCITY THING SO IF ANY OF THEM HIT THE GROUND TOO FAST IT WILL ALET GUARD
    public Rigidbody bombPartRigid;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BombPlacement")
        {
            gameObject.transform.position = placementLoc.position;
            isInTrigger = true;
            bombPartRigid.isKinematic = true;
            bombPartRigid.useGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "BombPlacement")
        {
            isInTrigger = false;
            bombPartRigid.isKinematic = false;
            bombPartRigid.useGravity = true;
        }
    }
}