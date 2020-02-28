using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Challenge: MonoBehaviour
{
   protected ChallengeCreator challengeCreator;
   protected bool active;

   public void SetCreator(ChallengeCreator challengeCreator)
    {
        this.challengeCreator = challengeCreator;
    }

    public void Activate()
    {
        challengeCreator.ActivateChallenge();
        active = true;
    }

   public void Complete()
    {
        challengeCreator.CompleteChallenge(this);
    }

    public abstract void Setup();
}
