using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMgr : MonoBehaviour
{
    // Ile wymaganych martwych warzyw do zmiany w ludzi
    [SerializeField]
    private int requiredDeadCount = 5;

    public static LevelMgr Instance {  get; private set; }

    private int deadCount = 0;
    private bool finished = false;

    private void Awake()
    {
        Instance = this;
    }

    public void VegetableDied()
    {
        if (finished)
        {
            return;
        }

        deadCount++;
        if (deadCount >= requiredDeadCount)
        {
            finished = true;
            foreach (var plant in FindObjectsByType<PlantTransmutator>(FindObjectsSortMode.None))
            {
                if (plant.TryGetComponent(out Shootable shootable))
                {
                    shootable.Hit(100, Vector3.zero, Vector3.zero);
                }
                plant.Tansmutate();
                plant.Tansmutate(); // dwa razy, bo ma dwa stage D:
            }
            StartCoroutine(NextScene());
        }
    }

    private IEnumerator NextScene()
    {
        yield return new WaitForSeconds(15);
        SceneTransitionManager.singleton.GoToSceneAsync(3);
    }
}
