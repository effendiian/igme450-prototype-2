using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeInput : MonoBehaviour
{
    private Vector2 start;
    private Vector2 end;

    public float minimumMagnitude = 150;

    public List<GameObject> observerObjects = new List<GameObject>();
    public List<SwipeObserver> observers = new List<SwipeObserver>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (var obj in observerObjects)
        {
            observers.Add(obj.GetComponent<SwipeObserver>());
        }
    }

    // Update is called once per frame
    //Used this for reference: https://forum.unity.com/threads/swipe-in-all-directions-touch-and-mouse.165416/?_ga=2.2870772.514999772.1582845110-1405545100.1579059357
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point
            start = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            end = Input.mousePosition;
            EndSwipe();
        }
#else
        //If they are using one finger
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                //save began touch 2d point
                start = new Vector2(t.position.x, t.position.y);
            }
            if (t.phase == TouchPhase.Ended)
            {
                //save ended touch 2d point
                end = new Vector2(t.position.x, t.position.y);
                EndSwipe();
            }
        }
#endif
    }

    private void EndSwipe()
    {
        Vector2 swipe = end - start;

        Debug.Log(swipe.magnitude);
        if (swipe.magnitude > minimumMagnitude)
        {
            foreach (var observer in observers)
            {
                observer.OnSwipe(swipe, start);
            }
        }
    }
}
