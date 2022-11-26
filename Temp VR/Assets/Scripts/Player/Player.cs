using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    //Used to check if player is hiding in closet
    public bool isHidden;

    //Add Damage Effect
    public bool electricDamage = true;

    //Cameras
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera hiddenCamera;
    [SerializeField] private GameObject trackingSpace;

    //Post Processing
    [SerializeField] private Volume volume;
    private Vignette vignette;
    private ColorAdjustments colorAdjustments;
    private float deafaultIntensity;
    private float deafaultSmoothness;
    private float increaseIntensity = 0.1f;

    //Tag Strings
    private string hiddenTag = "Hidden";
    private string electricTag = "ElectricTrigger";

    private void Start()
    {
        //Gets the Post Processing Effects
        volume.profile.TryGet<Vignette>(out vignette);
        volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);

        //Enable and Disable cameras
        mainCamera.enabled = true;
        hiddenCamera.enabled = false;

        isHidden = false;

        //Used to reset Vignette Intensity and Smoothness
        deafaultIntensity = vignette.intensity.value;
        deafaultSmoothness = vignette.smoothness.value;
    }

    private void Update()
    {
        //Player equips glasses so see hidden object
        if(Input.GetKeyDown(KeyCode.G))
        {
            //Activates the Color Adjustment effect
            if (colorAdjustments.active == false)
            {
                colorAdjustments.active = true;
            }
            else
            {
                colorAdjustments.active = false;
            }
        }

        if (colorAdjustments.active == true)
        {
            //Change Hue Shift
            if(Input.GetMouseButton(0))
            {
                colorAdjustments.hueShift.value++;
            }

            if(Input.GetMouseButton(1))
            {
                colorAdjustments.hueShift.value--;
            }

            //Reveals hidden objects when the hue shift reaches a certain threshold
            if((colorAdjustments.hueShift.value > 50  && colorAdjustments.hueShift.value < 60) || (colorAdjustments.hueShift.value < -50 && colorAdjustments.hueShift.value > -60))
            {
                //Endables hidden camera
                mainCamera.enabled = false;
                hiddenCamera.enabled = true;
                Debug.Log("Can See Hidden Objects");
            }
            else
            {
                mainCamera.enabled = true;
                hiddenCamera.enabled = false;
            }

            Debug.Log("Hue Shift Value: " + colorAdjustments.hueShift.value);
        }

    }

    //Player takes damage
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(electricTag))
        {
            electricDamage = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //If player crouches by the window they are hidden
        if (other.CompareTag(hiddenTag))
        {
            //How far the need to crouch
            if (trackingSpace.transform.localPosition.y < -0.5)
            {
                isHidden = true;
                Debug.Log("Player Is Hidden");
                
                //Activate vignette effect
                VignetteControl(Color.black, deafaultIntensity, deafaultSmoothness);
            }
            else
            {
                vignette.active = false;
            }
        }

        //Plays vignette effect if player is in electic trigger
        if (other.CompareTag(electricTag))
        {
            //Increase the vignette intensity overtime
            increaseIntensity += 0.01f;
            if(electricDamage == true)
            {
                VignetteControl(Color.cyan, increaseIntensity, 0.5f);
                Debug.Log("Player take damage");
            }
            else
            {
                Debug.Log("Player take no damage");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Disables vigentte when player is no longer in hidden trigger
        if(other.CompareTag(hiddenTag))
        {
            vignette.active = false;
            isHidden = false;
        }

        //Reset variables when player is no longer in electrical trigger
        if(other.CompareTag(electricTag))
        {
            increaseIntensity = 0.1f;
            electricDamage = false;

            //Remove vignette effect over time
            StartCoroutine(RemoveVignette());
        }
    }

    //Activate vignette effect with variables
    private void VignetteControl(Color color, float intensity, float smoothness)
    {
        vignette.active = true;
        vignette.color.Override(color);
        vignette.intensity.Override(intensity);
        vignette.smoothness.Override(smoothness);
    }

    //Removes the vignette effect overtime
    private IEnumerator RemoveVignette()
    {
        do 
        {
            yield return new WaitForSeconds(0.1f);
            vignette.intensity.value -= 0.2f;
        } 
        while (vignette.intensity.value >= 0.2);

        vignette.active = false;
    }
}
