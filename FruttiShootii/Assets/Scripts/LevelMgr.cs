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

    private readonly HashSet<Shootable> deadShootables = new();

    private void Awake()
    {
        Instance = this;
    }

    public void VegetableDied(Shootable shootable)
    {
        if (finished)
        {
            return;
        }

        deadShootables.Add(shootable);

        deadCount++;
        if (deadCount >= requiredDeadCount)
        {
            finished = true;
            Debug.LogWarning(deadShootables.Count);
            foreach (var oneOfShootable in FindObjectsByType<Shootable>(FindObjectsSortMode.None))
            {
                if (deadShootables.Contains(oneOfShootable))
                {
                    Debug.LogWarning("YY");
                    if (oneOfShootable.TryGetComponent(out PlantTransmutator trans))
                    {
                        trans.Tansmutate();
                        trans.Tansmutate(); // dwa razy, bo ma dwa stage D:
                    }
                }
                oneOfShootable.Hit(100, Vector3.zero, Vector3.zero);
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
