using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public enum SelectionLogic
    {
        Sequential,
        Random,
        WeaveDefault,
        RandomWeave
    }

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] clips;
    [SerializeField] float timeBetweenClips = 10;

    [SerializeField] SelectionLogic selectionType;

    private float timeLeft;
    private int current = 0;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = clips[0].length + timeBetweenClips;
        audioSource.PlayOneShot(clips[0]);
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0)
        {
            switch (selectionType)
            {
                case SelectionLogic.Random:
                    PickRandom();
                    break;
                case SelectionLogic.WeaveDefault:
                    PickWeavedDefault();
                    break;
                case SelectionLogic.RandomWeave:
                    PickRandomWeave();
                    break;
                default:
                    PickSequential();
                    break;
            }
        }
    }

    /// <summary>
    /// Cycles through each clip in order
    /// </summary>
    private void PickSequential()
    {
        current = (current + 1) % clips.Length;

        timeLeft = clips[current].length + timeBetweenClips;
        audioSource.PlayOneShot(clips[current]);
    }

    /// <summary>
    /// Selects a random clip to play
    /// </summary>
    private void PickRandom()
    {
        current = (int)Random.Range(0, clips.Length - 1);

        timeLeft = clips[current].length + timeBetweenClips;
        audioSource.PlayOneShot(clips[current]);
    }

    /// <summary>
    /// cycles through each clip, playing the default BGM once between each other clip
    /// </summary>
    private void PickWeavedDefault()
    {
        current = (current + 1) % (clips.Length * 2);

        if (current % 2 == 0)
        {
            timeLeft = clips[0].length + timeBetweenClips;
            audioSource.PlayOneShot(clips[0]);
        }
        else
        {
            int index = (int)Mathf.Ceil(current / 2.0f);
            timeLeft = clips[index].length + timeBetweenClips;
            audioSource.PlayOneShot(clips[index]);
        }
    }

    /// <summary>
    /// Alternates between selecting the default BGM and using a random other clip
    /// </summary>
    private void PickRandomWeave()
    {
        if (current > 0)
        {
            current = 0;
            timeLeft = clips[0].length + timeBetweenClips;
            audioSource.PlayOneShot(clips[0]);
        }
        else
        {
            current = 1;
            int index = Random.Range(1, clips.Length - 1);
            timeLeft = clips[index].length + timeBetweenClips;
            audioSource.PlayOneShot(clips[index]);
        }
    }
}
