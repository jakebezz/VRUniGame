using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BombParts : MonoBehaviour
{
    [NonSerialized] public bool isPlaced;                                                   //Check if Object is placed

    public Rigidbody bombPartRigid;                                                         //Rigidbody of the Bomb Parts

    [Header("Placement References")]
    [SerializeField] private CheckAllParts checkPlacement;                                  //Reference to Check if all parts are placed
    [SerializeField] private GameObject hologram;                                           //HologramObject to move Bomb part to

    private string bombPlacementTag = "BombPlacement";                                      //Tag to compare

    /// <summary>
    /// Virtual start iherited by the children, deactivates the placement hologram for the object, gets the rigidbody from the object
    /// </summary>
    protected virtual void Start()
    {
        bombPartRigid = gameObject.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Activate the hologram when object enters box
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(bombPlacementTag))
        {
            hologram.SetActive(false);
            MoveToPlacement();

            //Checks if all parts have been placed when an object is placed
            if (checkPlacement.CheckAllPlaced() == true)
            {
                Debug.Log("All Parts Placed");
                GameManager.Instance.PlacedAllBombParts();

            }
        }
    }

    /// <summary>
    /// Resets Variables when exiting trigger
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(bombPlacementTag))
        {
            bombPartRigid.isKinematic = false;
            bombPartRigid.useGravity = true;
            isPlaced = false;
            hologram.SetActive(true);
        }
    }

    /// <summary>
    /// Moves the object to hologram location
    /// </summary>
    private void MoveToPlacement()
    {
        gameObject.transform.position = hologram.transform.position;
        gameObject.transform.rotation = hologram.transform.rotation;
        bombPartRigid.isKinematic = true;
        bombPartRigid.useGravity = false;
        isPlaced = true;
    }
}
