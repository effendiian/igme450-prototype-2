using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChallengeCreator : MonoBehaviour
{
    public List<GameObject> challenges;
    public List<int> challengeProbabilities = new List<int>();
    public GameObject canvas;
    public int chance = 80;

    public GrowBehavior flower;
    public GameObject flowerObject;

    private List<Type> challengeTypes = new List<Type>();

    private List<int> availableChallenges = new List<int>();
    private List<Challenge> currentChallenges = new List<Challenge>();

    float time = 0;
    float buffer = 0;

    private int activeChallenges = 0;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < challenges.Count; i++)
        {
            challengeTypes.Add(challenges[i].GetComponent<Challenge>().GetType());
            availableChallenges.Add(i);
        }

        //Set up base probabilites if there are none specified
        if (challengeProbabilities.Count < challenges.Count)
        {
            int countToAdd = (challenges.Count - challengeProbabilities.Count);
            for (int i = 0; i < countToAdd; i++)
            {
                challengeProbabilities.Add(chance);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!flower.HasBloomed() && availableChallenges.Count > 0 && buffer <= 0)
        {
            for (int i = 0; i < availableChallenges.Count; i++)
            {
                int num = availableChallenges[i];
                if (Random.Range(0, challengeProbabilities[num]) == 0)
                {
                    CreateChallenge(num);
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        buffer -= Time.deltaTime;
        
        if (!flower.HasBloomed() && time > 10)
        {
            CreateChallenge(Random.Range(0, availableChallenges.Count));
        } 
    }

    private void CreateChallenge(int num)
    {
        time = 0;

        availableChallenges.Remove(num);

        GameObject newChallenge = Instantiate(challenges[num]);
        newChallenge.transform.SetParent(canvas.transform, false);

        Challenge script = newChallenge.GetComponent<Challenge>();
        script.SetCreatorAndFlower(this, flowerObject);
        script.Setup();
        currentChallenges.Add(script);

        newChallenge.SetActive(true);
    }

    public void ActivateChallenge()
    {
        activeChallenges += 1;
        Debug.Log(activeChallenges);

        if (flower)
            flower.challengeActive = true;
    }

    public void CompleteChallenge(Challenge challenge, bool wasActive)
    {
        //TODO: Remove if check when flower is set up
        if (wasActive)
        {
            activeChallenges -= 1;
            Debug.Log(activeChallenges);
        }

        if (activeChallenges == 0 && flower)
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
