using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyChallenge : Challenge
{
    FlyState state = FlyState.Entering;

    public float enteringSpeed = 200f;
    public float leavingSpeed = 400f;

    private int startPosition = 100;
    private int direction = 1;
    
    public int currentAngle;
    public Vector3 currentDestination;
    public float timeTraveled;
    

    public override void Setup()
    {
        float startX, startY;

        //Come from the sides
        if (Random.Range(0, 100) <= 70)
        {
            startY = Random.Range(0, Screen.height + 100);
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
    }

    // Start is called before the first frame update
    void Start()
    {
        //Setup();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case FlyState.Entering:
                Move(true, enteringSpeed);

                if (Mathf.Abs(transform.position.x - (Screen.width / 2)) < 25 && Mathf.Abs(transform.position.y - (Screen.height / 2)) < 25)
                {
                    state = FlyState.Sitting;
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

    private void Move(bool towardsFlower, float speed)
    {
        int change = towardsFlower ? 1 : -1;

        Vector3 vector = new Vector3(Screen.width / 2, Screen.height / 2, 1) - this.transform.position * change;
        if (!towardsFlower && direction < 0)
        {
            vector.x *= -1;
        }

        vector.Normalize();
        vector *= Time.deltaTime * speed;

        timeTraveled += Time.deltaTime;
        if (timeTraveled > 0.5)
        {
            currentAngle = Random.Range(-70, 70);
            timeTraveled = 0;
        }
        vector = Quaternion.Euler(0, 0, currentAngle) * vector;
        this.transform.position += vector;
    }

    private bool IsOffScreen()
    {
        return this.gameObject.transform.position.x < -100 || this.gameObject.transform.position.x > Screen.width + 100 ||
            this.gameObject.transform.position.y < -100 || this.gameObject.transform.position.y > Screen.width + 100;
    }


    public void Click()
    {
        state = FlyState.Leaving;
        this.Complete();
    }

    private void OnMouseDown()
    {
        state = FlyState.Leaving;
        this.Complete();
    }
}

public enum FlyState
{
    Entering,
    Sitting,
    Leaving
}