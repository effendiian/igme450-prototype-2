using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowBehavior : MonoBehaviour
{
    //variables
    float timer;    //float to keep track of how much time has passed
    enum LifeState  //enum to hold the life cycle of the plant
    {
        Seed,
        Sapling,
        Plant,
        Flower
    }
    LifeState currentState; //LifeState to hold where the plant currently is in the life cycle
    public bool challengeActive;    //bool to hold whether there is an active challenge or not


    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        currentState = LifeState.Seed;
        challengeActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        FlowerGrow(challengeActive);
    }


    public void FlowerGrow(bool challengeInProgress)
    {
        if(!challengeInProgress)
        {
            timer += Time.deltaTime;
        }
        if(timer >= 5 && !challengeInProgress && currentState != LifeState.Flower)
        {
            Debug.Log(currentState);
            timer = 0;
            currentState++;
        }
        Debug.Log(timer);
    }
}
