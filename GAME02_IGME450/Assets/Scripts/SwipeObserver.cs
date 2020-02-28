using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SwipeObserver: MonoBehaviour
{
    public abstract void OnSwipe(Vector2 swipe);
}
