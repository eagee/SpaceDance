using UnityEngine;
using System.Collections;

public class EventsExample : MonoBehaviour
{
    public TestDanceScript dancer;

    public TestDanceScript dancer2;

    public TransformModify bar;

    public RhythmTool rhythmTool;

    public RhythmEventProvider eventProvider;

    public AudioClip audioClip;

    private int m_skipBeats;

    void Start()
    {
        eventProvider.SongLoaded += OnSongLoaded;
        eventProvider.Beat += OnBeat;
        eventProvider.SubBeat += OnSubBeat;
        eventProvider.Change += OnChange;
        m_skipBeats = 0;
        rhythmTool.audioClip = audioClip;
    }

    private void OnSongLoaded()
    {
        rhythmTool.Play();
    }

    private void OnBeat(Beat beat)
    {
        m_skipBeats++;
        if(m_skipBeats > 1)
        {
            m_skipBeats = 0;
            dancer.UpdateDance(); 
            dancer2.UpdateDance();
        }

        bar.UpdateBeat();
    }

    private void OnChange(int index, float change)
    {
        dancer.ChangeDance();
        dancer2.ChangeDance();
        bar.UpdateBehavior();
    }

    private void OnSubBeat(Beat beat, int count)
    {
        //if (count == 0 || count == 2)
        //bar.UpdateBeat();
        //bar.BeatMagnitude = rhythmTool.low.magnitudeAvg[0];
    }
}
