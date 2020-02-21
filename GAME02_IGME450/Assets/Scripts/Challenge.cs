using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Challenge: MonoBehaviour
{
   private ChallengeCreator challengeCreator;

   public void SetCreator(ChallengeCreator challengeCreator)
    {
        this.challengeCreator = challengeCreator;
    }

   public void Complete()
    {
        challengeCreator.CompleteChallenge(this);
    }

    public abstract void Setup();
}
