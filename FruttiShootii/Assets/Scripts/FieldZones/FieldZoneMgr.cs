using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldZoneMgr : MonoBehaviour
{
    // Ile sekund podlewania jest wymaganych do zmiany poziomu
    [SerializeField]
    private float RequiredWateringTime = 10.0f;

    public static FieldZoneMgr Instance { get; private set; }

    public int PlayerZoneTouchesCount { get; private set; }

    private float wateringTime = 0;
    private bool watering = false;

    private bool finished = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (finished)
        {
            return;
        }

        if (watering && PlayerZoneTouchesCount > 0)
        {
            wateringTime += Time.deltaTime;
            if (wateringTime >= RequiredWateringTime)
            {
                SceneTransitionManager.singleton.GoToSceneAsync(2);
                finished = true;
            }
        }
    }

    public void PlayerEnteredZone()
    {
        PlayerZoneTouchesCount++;
    }

    public void PlayerLeftZone()
    {
        PlayerZoneTouchesCount--;
    }

    public void WateringBegin()
    {
        watering = true;
    }

    public void WateringEnd()
    {
        watering = false;
    }
}
