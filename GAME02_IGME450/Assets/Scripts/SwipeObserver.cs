using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SwipeObserver
{
    void OnSwipe(Vector2 swipe, Vector2 startPosition);
}
