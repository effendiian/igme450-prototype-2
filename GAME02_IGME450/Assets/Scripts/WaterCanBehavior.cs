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



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //maybe change this to simply check phone oreientation instead of rate
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            runTimer = true;
            SolvingChallenge();
        }


#else
        

        
#endif


        rotSpeed = Input.acceleration.x;
        if (rotSpeed > 5)
        {
            runTimer = true;
            SolvingChallenge();
        }

        if (runTimer)
        {
            pourTimer += Time.deltaTime;
        }

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
        dryPotCreated.transform.SetParent(this.challengeCreator.canvas.transform, false);
        dryPotCreated.transform.SetSiblingIndex(2);

        
    }

    //function to be called when the user rotates phone to solve challenge
    private void SolvingChallenge()
    {
        //add water can tipping and pouring for a few seconds
        this.transform.Rotate(0, 0, -45, Space.Self);

        this.transform.position = new Vector3(this.transform.position.x +25, this.transform.position.y + 175, this.transform.position.z);

        waterDrop = Instantiate(water);
        waterDrop.transform.SetParent(this.challengeCreator.canvas.transform, false);
    }
    //function to end the challenge
    private void FinishChallenge()
    {
        //add fixing dirt color
        DestroyImmediate(dryPotCreated, true);
        DestroyImmediate(waterDrop, true);

        this.Complete();

        DestroyImmediate(this.gameObject, true);
    }
}
