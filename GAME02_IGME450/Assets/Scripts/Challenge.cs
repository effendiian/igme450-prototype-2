using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Challenge: MonoBehaviour
{

    protected ChallengeCreator challengeCreator;
    protected GameObject flower;
    protected bool active;
    protected bool wasActivated;
    protected float probability = 1; // 1 out of 1.

   public void SetCreatorAndFlower(ChallengeCreator challengeCreator, GameObject flower)
    {
        this.challengeCreator = challengeCreator;
        this.flower = flower;
    }

    public void Activate()
    {
        challengeCreator.ActivateChallenge();
        active = true;
        wasActivated = true;
    }

   public void Complete()
    {
        challengeCreator.CompleteChallenge(this, wasActivated);
    }

    public bool WasActivated()
    {
        return wasActivated;
    }

    public abstract void Setup();

    /// <summary>
    /// Set chance based on value between (0...1].
    /// </summary>
    /// <param name="chance">Value to assign</param>
    public void SetProbability(float chance = 1.0f)
    {
        this.probability = chance;
    }

    /// <summary>
    /// Probability value.
    /// </summary>

    public float Probability
    {
        get => this.probability;
        set => this.SetProbability(value);
    }
}
