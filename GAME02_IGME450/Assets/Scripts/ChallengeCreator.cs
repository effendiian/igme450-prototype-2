using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using NaughtyAttributes;

public class ChallengeCreator : MonoBehaviour
{
    public List<ChallengeEntry> challenges;

    /// <summary>
    /// Value that a roll must beat in order to generate a challenge on a given tick.
    /// </summary>
    [Range(0.0f, 1.0f)]
    public float rollChance = 0.8f;

    // public List<GameObject> challenges;
    // public List<int> challengeProbabilities = new List<int>();
    public GameObject canvas;

    public GrowBehavior flower;
    public GameObject flowerObject;

    private List<Type> challengeTypes = new List<Type>();

    private List<int> availableChallenges = new List<int>();
    private List<Challenge> currentChallenges = new List<Challenge>();

    float time = 0;
    float buffer = 0;

    private int activeChallenges = 0;


    /// <summary>
    /// Roll is the probability check.
    /// It internally chooses a value (0...1].
    /// </summary>
    /// <returns>Returns true/false.</returns>
    public bool Roll()
    {
        float chance = this.rollChance * 100.0f; // [0...1] * 100 eg. 0.65 * 100 = 65%.
        float roll = Random.Range(0.0f, 100.0f); // [0...1] * 100 => eg. 1%.
        return (chance == 1.0f) || ((chance != 0.0f) && (roll > (100 - chance)));
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < challenges.Count; i++)
        {
            challengeTypes.Add(challenges[i].GetChallengeType());
            availableChallenges.Add(i);
        }

        //Set up base probabilites if there are none specified
        /* if (challengeProbabilities.Count < challenges.Count)
        {
            int countToAdd = (challenges.Count - challengeProbabilities.Count);
            for (int i = 0; i < countToAdd; i++)
            {
                challengeProbabilities.Add(chance);
            }
        }*/
    }

    private void FixedUpdate()
    {
        if (!HUDController.Instance || !HUDController.Instance.IsPaused)
        {
            if (!flower.HasBloomed() && availableChallenges.Count > 0 && buffer <= 0)
            {
                bool roll = this.Roll();
                for (int i = 0; i < availableChallenges.Count; i++)
                {
                    int num = availableChallenges[i];
                    if (roll && challenges[num].Roll())
                    {
                        CreateChallenge(num);
                        break;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!HUDController.Instance || !HUDController.Instance.IsPaused)
        {
            time += Time.deltaTime;
            buffer -= Time.deltaTime;

            if (!flower.HasBloomed() && time > 7)
            {
                CreateChallenge(Random.Range(0, availableChallenges.Count));
            }
        }
    }

    private void CreateChallenge(int num)
    {
        time = 0;

        availableChallenges.Remove(num);

        GameObject newChallenge = Instantiate(challenges[num].host);
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
        // Debug.Log(activeChallenges);

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
