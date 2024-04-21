using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ConditionalNarrationClip
{
    public AudioClip clip;
    public float delay;
    public bool isUrgent;
    public bool shouldClearQueue;
    public AudioClip[] requiredClips;
    public AudioClip[] bannedClips;
    [System.NonSerialized] public bool hasBeenTriggered;
}

public class ConditionalNarratorTrigger  : MonoBehaviour
{
    public ConditionalNarrationClip[] narrationClips;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(PlayNarrationSequence());
        }
    }

    IEnumerator PlayNarrationSequence()
    {
        for (int i = 0; i < narrationClips.Length; i++)
        {
            if (AllRequiredClipsPlayed(narrationClips[i]) && !AnyBannedClipsPlayed(narrationClips[i]) && !narrationClips[i].hasBeenTriggered)
            {
                narrationClips[i].hasBeenTriggered = true;
                yield return new WaitForSeconds(narrationClips[i].delay);
                NarrationManager.Instance.PlayNarration(narrationClips[i].clip, narrationClips[i].shouldClearQueue, narrationClips[i].isUrgent);
                yield return new WaitForSeconds(narrationClips[i].clip.length);
            }
        }
    }

    private bool AllRequiredClipsPlayed(ConditionalNarrationClip narrationClip)
    {
        foreach (var requiredClip in narrationClip.requiredClips)
        {
            if (!NarrationManager.Instance.HasPlayed(requiredClip))
                return false;
        }
        return true;
    }
    private bool AnyBannedClipsPlayed(ConditionalNarrationClip narrationClip)
    {
        foreach (var bannedClip in narrationClip.bannedClips)
        {
            if (NarrationManager.Instance.HasPlayed(bannedClip))
                return true;
        }
        return false;
    }
}