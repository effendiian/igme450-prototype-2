using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeCreator : MonoBehaviour
{
    public List<GameObject> challenges;
    public GameObject canvas;
    public int chance = 80;

    public GrowBehavior flower;
    private List<Challenge> currentChallenges = new List<Challenge>();

    float time = 0;
    float buffer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
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
        if (currentChallenges.Count > 0 || buffer > 0)
        {
            return;
        }

        time = 0;

        int choice = Random.Range(0, challenges.Count);
        GameObject newChallenge = Instantiate(challenges[choice]);
        newChallenge.transform.SetParent(canvas.transform, false);

        Challenge script = newChallenge.GetComponent<Challenge>();
        script.SetCreator(this);
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
        this.currentChallenges.Remove(challenge);
        buffer = 1;
    }
}
