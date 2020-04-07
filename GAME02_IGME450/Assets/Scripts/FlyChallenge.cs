using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyChallenge : Challenge
{
    FlyState state = FlyState.Entering;

    public float enteringSpeed = 300f;
    public float leavingSpeed = 1000f;

    private int startPosition = 100;
    private int direction = 1;
    
    private int currentAngle;
    private Vector3 currentDestination;
    private float timeTraveled;

    RectTransform flowerRect;
    private Vector3 finalDestination;
    

    public override void Setup()
    {

        //Get flower rect transform
        flowerRect = flower.GetComponent<RectTransform>();
        float minY = flower.transform.position.y;

        float startX, startY;

        //Come from the sides
        if (Random.Range(0, 100) <= 80)
        {
            startY = Random.Range(minY, Screen.height + 100);
            if (Random.Range(0, 100) > 50) //Left
            {
                startX = -100;
            } else //Right
            {
                startX = Screen.width + 100;
            }
        } else //Come from the top
        {
            startX = Random.Range(-100, Screen.width + 100);
            startY = Screen.height + 100;
        }

        if (startX > Screen.width / 2)
        {
            direction = -1;
            Vector3 newScale = this.transform.localScale;
            newScale.x *= -1;
            this.transform.localScale = newScale;
        }

        this.gameObject.transform.position = new Vector3(startX, startY, 0);
        
        finalDestination = new Vector3(flowerRect.transform.position.x, startY + Random.Range(-30, 30), 1);

        //If we're aiming above the flower, lower
        if (finalDestination.y > flowerRect.sizeDelta.y + minY || finalDestination.y < minY)
        {
            finalDestination.y = Random.Range((flowerRect.sizeDelta.y * 4) / 5, flowerRect.sizeDelta.y) + minY;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Setup();
    }

    // Update is called once per frame
    void Update()
    {
        if (!HUDController.Instance.IsPaused)
        {
            switch (state)
            {
                case FlyState.Entering:
                    Move(true, enteringSpeed);

                    if (Mathf.Abs(transform.position.x - finalDestination.x) < 25 &&
                        (transform.position.y < flowerRect.sizeDelta.y + flowerRect.transform.position.y && transform.position.y > flowerRect.transform.position.y))
                    {
                        state = FlyState.Sitting;
                        Activate();
                    }
                    break;
                case FlyState.Leaving:
                    Move(false, leavingSpeed);

                    if (IsOffScreen())
                    {
                        Destroy(this.gameObject);
                    }
                    break;
            }
        }
    }

    private void Move(bool towardsFlower, float speed)
    {
        int change = towardsFlower ? 1 : -1;

        Vector3 vector = finalDestination - this.transform.position * change;
        if (!towardsFlower && direction < 0)
        {
            vector.x *= -1;
        }

        vector.Normalize();
        vector *= Time.deltaTime * speed;

        timeTraveled += Time.deltaTime;
        if (timeTraveled > 0.5)
        {
            currentAngle = Random.Range(-50, 50);
            timeTraveled = 0;
        }
        vector = Quaternion.Euler(0, 0, currentAngle) * vector;
        this.transform.position += vector;
    }

    private bool IsOffScreen()
    {
        return this.gameObject.transform.position.x < -100 || this.gameObject.transform.position.x > Screen.width + 100 ||
            this.gameObject.transform.position.y < -100 || this.gameObject.transform.position.y > Screen.height + 100;
    }

    private bool IsOnScreen()
    {
        return this.gameObject.transform.position.x > -30 && this.gameObject.transform.position.x < Screen.width - 10 &&
            this.gameObject.transform.position.y > 10 && this.gameObject.transform.position.y < Screen.width - 30;
    }


    public void Click()
    {
        this.Complete();
        state = FlyState.Leaving;
    }

    private void OnMouseDown()
    {
        Click();
    }
}

public enum FlyState
{
    Entering,
    Sitting,
    Leaving
}