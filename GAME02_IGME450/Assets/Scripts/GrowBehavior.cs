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

    float maxHeight = 525f;
    float width = 22.5f;
    RectTransform stalk;
    public GameObject bulb;
    public GameObject bloom;


    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        currentState = LifeState.Seed;
        challengeActive = false;

        stalk = this.GetComponent<RectTransform>();
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
        } else
        {
            return;
        }

        switch(currentState)
        {
            case LifeState.Seed:
                if (timer > 5)
                {
                    timer = 0;
                    currentState++;
                }
                else
                {
                    float baseSize = 5f;
                    float size = baseSize + (timer / 5) * (width - baseSize);
                    stalk.sizeDelta = new Vector2(size, size);
                }
                break;
            case LifeState.Sapling:
                if (timer > 20)
                {
                    timer = 0;
                    stalk.sizeDelta = new Vector2(width, maxHeight);
                    currentState++;

                    bulb.transform.localScale = new Vector3(0, 0, 1);
                    bulb.SetActive(true);
                } else
                {
                    stalk.sizeDelta = new Vector2(width, ((timer / 20) * (maxHeight - width)) + width);
                }
                break;
            case LifeState.Plant:
                if (timer > 15)
                {
                    timer = 0;
                    bulb.transform.localScale = new Vector3(1, 1, 1);
                    currentState++;
                    
                    bloom.SetActive(true);
                }
                else
                {
                    float scale = (timer / 15);
                    bulb.transform.localScale = new Vector3(scale, scale, 1);
                }
                break;
            case LifeState.Flower:
                break;
        }

        //if(timer >= 5 && !challengeInProgress && currentState != LifeState.Flower)
        //{
        //    Debug.Log(currentState);
        //    timer = 0;
        //    currentState++;
        //}
        //Debug.Log(timer);
    }

    public bool HasBloomed()
    {
        return currentState == LifeState.Flower;
    }
}
