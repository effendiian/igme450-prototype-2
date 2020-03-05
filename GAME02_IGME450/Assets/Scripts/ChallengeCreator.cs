using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChallengeCreator : MonoBehaviour
{
    public List<GameObject> challenges;
    public GameObject canvas;
    public int chance = 80;

    public GrowBehavior flower;
    public GameObject flowerObject;

    private List<Type> challengeTypes = new List<Type>();

    private List<int> availableChallenges = new List<int>();
    private List<Challenge> currentChallenges = new List<Challenge>();

    float time = 0;
    float buffer = 0;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < challenges.Count; i++)
        {
            challengeTypes.Add(challenges[i].GetComponent<Challenge>().GetType());
            availableChallenges.Add(i);
        }
    }

    private void FixedUpdate()
    {
        if (Random.Range(0, chance) == 0)
        {
            CreateChallenge();
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        buffer -= Time.deltaTime;
        
        if (time > 10)
        {
            CreateChallenge();
        } 
    }

    private void CreateChallenge()
    {
        //For now don't allow multiple challenges at once
        if (availableChallenges.Count == 0 || buffer > 0)
        {
            return;
        }

        time = 0;

        int rand = Random.Range(0, availableChallenges.Count);
        int choice = availableChallenges[rand];
        availableChallenges.Remove(choice);

        GameObject newChallenge = Instantiate(challenges[choice]);
        newChallenge.transform.SetParent(canvas.transform, false);

        Challenge script = newChallenge.GetComponent<Challenge>();
        script.SetCreatorAndFlower(this, flowerObject);
        script.Setup();
        currentChallenges.Add(script);

        newChallenge.SetActive(true);
    }

    public void ActivateChallenge()
    {
        if (flower)
            flower.challengeActive = true;
    }

    public void CompleteChallenge(Challenge challenge)
    {
        //TODO: Remove if check when flower is set up
        if (flower)
            flower.challengeActive = false;

        for (int i = 0; i < challengeTypes.Count; i++)
        {
            if (challengeTypes[i] == challenge.GetType())
            {
                availableChallenges.Add(i);
                break;
            }
        }

        this.currentChallenges.Remove(challenge);
        buffer = 1;
    }
}
