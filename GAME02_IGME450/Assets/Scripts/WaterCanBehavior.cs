using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCanBehavior : MonoBehaviour
{
    //variables
    public GameObject waterCan; //GameObject to hold water can object that is set in editor
    public GameObject dirt; //GameObject to be set in editor that will change the dirt to make it look more dry



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //maybe change this to simply check phone oreientation instead of rate

        //checking to see if user rotates phone
        if(Input.gyro.rotationRate.x > 5)
        {
            SolvingChallenge();
        }
    }

    //function to set up the chaallenge
    public void Setup()
    {
        //move the water can to be slightly visible on screen



        //change the dirt to make it look dry


    }

    //function to be called when the user rotates phone to solve challenge
    private void SolvingChallenge()
    {
        //add water can tipping and pouring for a few seconds


        //add fixing dirt color


        //remove watering can from screen
    }
}
