using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCanBehavior : Challenge
{
    //variables
    public GameObject dryDirt;  //GameObject to hold the dry dirt sprite
    private float rotSpeed; //Variable to hold the rotation of the device
    private GameObject dryPotCreated;   //Variable to hold the pot of dry soil created
    private float pourTimer;  //Variable to hold the timer for how long the can should pour
    private bool runTimer;  //Variable to know if the timer should be running
    public GameObject water;    //Variable to hold water object
    private GameObject waterDrop;   //variable to hold the created water
    public float rotationControl;   //variable to hold the accleration needed to trigger the can
    private bool rotationComplete;  //varibable to hold if the phone has been rotated once yet



    // Start is called before the first frame update
    void Start()
    {
        rotationComplete = false;
    }

    // Update is called once per frame
    void Update()
    {

        //maybe change this to simply check phone oreientation instead of rate
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.RightArrow) && !rotationComplete)
        {
            rotationComplete = true;
            runTimer = true;
            SolvingChallenge();
        }
#else
        //might have to adjust rotation control number to make it more or less sensitive 

        rotSpeed = Input.acceleration.x;
        //checking to see if the user tilts the phone
        if (rotSpeed > rotationControl && !rotationComplete)
        {
            rotationComplete = true;
            runTimer = true;
            SolvingChallenge();
        }

        //if code above is backwards use this 
        /*
         *  if (rotSpeed < -rotationControl)
        {
            runTimer = true;
            SolvingChallenge();
        }
         * 
         * if code above is wrong axis try checking the z axis
         * 
         * 
         * 
         */


#endif



        //if the bool is true, incrmenting the timer
        if (runTimer)
        {
            pourTimer += Time.deltaTime;
        }

        //if the timer reaches 4, finshing the challenge
        if(pourTimer >= 4)
        {
            FinishChallenge();
            runTimer = false;
            pourTimer = 0;
        }
    }

    //function to set up the chaallenge
    public override void Setup()   
    {

        this.Activate();

        //change the dirt to make it look dry
        dryPotCreated = Instantiate(dryDirt);
        dryPotCreated.transform.SetParent(this.challengeCreator.canvas.transform.GetChild(1), false);
        dryPotCreated.transform.SetSiblingIndex(0);

        
    }

    //function to be called when the user rotates phone to solve challenge
    private void SolvingChallenge()
    {
        //add water can tipping and pouring for a few seconds
        this.transform.Rotate(0, 0, -45, Space.Self);

        this.transform.position = new Vector3(this.transform.position.x +25, this.transform.position.y + 175, this.transform.position.z);

        DestroyImmediate(waterDrop, true);
        waterDrop = Instantiate(water);
        waterDrop.transform.SetParent(this.challengeCreator.canvas.transform, false);
        waterDrop.transform.SetSiblingIndex(2);
    }
    //function to end the challenge
    private void FinishChallenge()
    {
        this.Complete();

        //add fixing dirt color
        DestroyImmediate(dryPotCreated, true);
        DestroyImmediate(waterDrop, true);

        Destroy(this.gameObject);
    }
}
