using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using NaughtyAttributes;
using System;

[System.Serializable]
public struct ChallengeEntry
{
    [Required]
    public GameObject host; // Controller gameObject.

    private Challenge challenge;

    [Range(0.0f, 1.0f)]
    public float probability;

    /// <summary>
    /// Create a ChallengeEntry object.
    /// </summary>
    /// <param name="target">Target containing the challenge.</param>
    public ChallengeEntry(GameObject target)
    {
        this.challenge = (target) ? target.GetComponent<Challenge>() : null;
        this.host = target ?? null;
        this.probability = (this.challenge) ? this.challenge.Probability : 0.0f;
    }

    /// <summary>
    /// Roll is the probability check.
    /// It internally chooses a value (0...1].
    /// </summary>
    /// <returns>Returns true/false.</returns>
    public bool Roll()
    {
        if (this.challenge)
        {
            this.challenge.SetProbability(this.probability);
            float chance = this.Probability * 100; // [0...1] * 100 eg. 0.65 * 100 = 65%.
            float roll = Random.Range(0.0f, 100.0f); // [0...1] * 100 => eg. 1%.
            Debug.Log($"Challenge Roll: {roll}% >?= {(100 - chance)}%");
            return (chance == 1.0f) || ((chance != 0.0f) && (roll > (100 - chance)));
        }
        return false;
    }

    /// <summary>
    /// 1 means always spawn this challenge if selected. 0 means never.
    /// </summary>
    public float Probability => this.challenge ? this.challenge.Probability : 0;

    /// <summary>
    /// Return challenge type.
    /// </summary>
    /// <returns></returns>
    public Type GetChallengeType()
    {
        if(this.host && this.challenge)
        {
            return this.challenge.GetType();
        }
        else
        {
            return null;
        }
    }

}

