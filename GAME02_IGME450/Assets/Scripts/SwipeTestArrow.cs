using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeTestArrow : MonoBehaviour, SwipeObserver
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSwipe(Vector2 swipe, Vector2 startPosition)
    {
        float rotation = Mathf.Atan2(swipe.y, swipe.x);
        Debug.Log(rotation);

        RectTransform rect = this.gameObject.GetComponent<RectTransform>();
        rect.rotation = Quaternion.Euler(0, 0, rotation * Mathf.Rad2Deg);

        float scale = 0.25f + (3.75f * (swipe.magnitude / 700f));
        scale = Mathf.Min(scale, 4);
        rect.localScale = new Vector2(scale, 1);
    }
}
