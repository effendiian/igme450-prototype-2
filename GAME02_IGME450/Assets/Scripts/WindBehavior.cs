using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBehavior : MonoBehaviour, SwipeObserver
{
    private float timeShown = 0;
    private float timeToShow = 0.5f;

    public float force = 20f;
    private float maxMagSwipe = 800f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeShown += Time.deltaTime;

        if (timeShown > timeToShow)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void OnSwipe(Vector2 swipe, Vector2 startPosition)
    {
        float rotation = Mathf.Atan2(swipe.y, swipe.x);
        RectTransform rect = this.gameObject.GetComponent<RectTransform>();
        rect.rotation = Quaternion.Euler(0, 0, rotation * Mathf.Rad2Deg);

        this.gameObject.transform.position = startPosition;
        float scale = 0.5f + (Mathf.Min(maxMagSwipe, swipe.magnitude) / maxMagSwipe) * 0.5f;
        this.gameObject.transform.localScale = new Vector2(scale, scale);

        this.gameObject.SetActive(true);
        timeShown = 0;

        GameObject[] ladyBugs = GameObject.FindGameObjectsWithTag("Ladybug");
        foreach (var bug in ladyBugs)
        {
            bug.GetComponent<LadybugChallenge>().OnSwipe(swipe);
        }
    }

}
