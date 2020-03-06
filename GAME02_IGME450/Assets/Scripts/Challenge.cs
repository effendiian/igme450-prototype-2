using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Challenge: MonoBehaviour
{

   protected ChallengeCreator challengeCreator;
   protected GameObject flower;
   protected bool active;
   protected bool wasActivated;

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
}
