using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class RhythmEventController : MonoBehaviour
{
    public RhythmTool rhythmTool;
    public RhythmEventProvider eventProvider;
    public float ScrollPerBeat;
    public float MinLineSpacing;
    public float BeatPrefabSpacingSize = 0.5f;
    public string SelectedWord = "";

    /// <summary>
    /// Transform at location furthest left side of the scene where notes will be generated from.
    /// </summary>
    public Transform NotesRangeLeft;
    
    /// <summary>
    /// Transform at location further right side of the scene where notes will be generated from.
    /// </summary>
    public Transform NotesRangeRight;
    public Transform NotesRangeTop;
    public Transform NotesRangeBottom;
    public Vector3 v_NotesRangeMiddle;

    /// <summary>
    /// List of audio clips that can be added to the scene and played by the RhythmController
    /// </summary>
    public List<AudioClip> audioClips;

    /// <summary>
    /// Index indicating the current song being played.
    /// </summary>
    private int m_currentSong;

    /// <summary>
    /// The beatPrefab will be the object we use to create the user-interactive, "beats"
    /// which expose letters/touchable items that users can interact with.
    /// </summary>
    public GameObject beatPrefab;

    /// <summary>
    /// Beat Observers is a list of objects that will process beat events
    /// via the IBeatObserver interface as a song begins, is played, and ends.
    /// </summary>
    public List<IBeatObserverSyncContainer> BeatObservers;

    /// <summary>
    /// I honestly don't get this, but am copypasta-ing all this magnitude stuff b/c of my lack of desire to think about it under a time crunch.
    /// </summary>
    private ReadOnlyCollection<float> magnitudeSmooth;

    private float m_lastPrefabX;

    public string[] TestWords;
    public int TestWordIndex;

    public TextMesh WordUp;
    public TextMesh WordDown;
    public TextMesh WordLeft;
    public TextMesh WordRight;
    public bool ReadyForKey;

    string NextTestWord() {
        TestWordIndex++;
        if (TestWordIndex >= TestWords.Length) {
            TestWordIndex = 0;
        }
        return TestWords[TestWordIndex];
    }

    string GetWord(string key) {
        string retword = "error";
        TextMesh thisone = null;
        string nextword = "_";
        while (nextword[0] == '_') nextword = NextTestWord();
        if (key == "Up") thisone = WordUp;
        if (key == "Down") thisone = WordDown;
        if (key == "Left") thisone = WordLeft;
        if (key == "Right") thisone = WordRight;
        if (retword != null) {
            retword = thisone.text;
            thisone.text = NextTestWord();
        }
        return retword;
    }

    void Start()
    {
        m_currentSong = -1;
        Application.runInBackground = true;
        eventProvider.Onset += OnOnset;
        eventProvider.Beat += OnBeat;
        eventProvider.Change += OnChange;
        eventProvider.SongLoaded += OnSongLoaded;
        eventProvider.SongEnded += OnSongEnded;
        magnitudeSmooth = rhythmTool.low.magnitudeSmooth;
        m_lastPrefabX = NotesRangeLeft.transform.position.x;
        v_NotesRangeMiddle = (NotesRangeTop.position + NotesRangeBottom.position) / 2f;
        WordUp.text = NextTestWord();
        WordDown.text = NextTestWord();
        WordLeft.text = NextTestWord();
        WordRight.text = NextTestWord();
        ReadyForKey = false;

        if (audioClips.Count <= 0)
        {
            Debug.LogWarning("no songs configured");
        }
        else
        {
            NextSong();
        }
    }

    private void OnSongLoaded()
    {
        foreach (var observer in BeatObservers)
        {
            if(observer != null)
                observer.Result.OnSongLoaded();
        }
        rhythmTool.Play();
    }

    private void OnSongEnded()
    {
        foreach (var observer in BeatObservers)
        {
            if (observer != null)
                observer.Result.OnSongEnded();
        }
        NextSong();
    }

    private void NextSong()
    {
        // Note: BeatObservers like the beat prefab objects will know to destory themselves when an OnSongEnded event occurs.
        m_currentSong++;

        if (m_currentSong >= audioClips.Count)
            m_currentSong = 0;

        rhythmTool.audioClip = audioClips[m_currentSong];
    }

    private void OnBeat(Beat beat)
    {
        // default to there being enough space
        float lowest = NotesRangeBottom.position.y + MinLineSpacing + 1f;
        bool LineExited = false;
        bool LineEntered = false;
        // destroy what's gone off the top, dim what's gone past the middle
        foreach (var observer in BeatObservers)
        {
            if (observer != null) {
                observer.Result.OnBeat(beat);
                if (observer.Result.Tag() == "Beat" && observer.Result.ObserverGameObject != null)
                { 
                    Vector3 pos = observer.Result.Position;
                    pos.y += ScrollPerBeat;
                    if (pos.y < lowest)
                        lowest = pos.y;
                    observer.Result.Position = pos;
                    // TODO: replace this, it's super inefficient.
                    if (pos.y > v_NotesRangeMiddle.y - 4f) {
                        TextMesh m_TextMesh = 
                            observer.Result.ObserverGameObject.GetComponentInChildren<TextMesh>();
                        Color tcolor = m_TextMesh.color;
                        if (pos.y < v_NotesRangeMiddle.y) {
                            tcolor.b = 0.1f;
                            LineEntered = true;
                        } else {
                            if (m_TextMesh.text[0] == '_') {
                                if (SelectedWord == "") {
                                    m_TextMesh.text = "BLEEP!";
                                    // You lose! Good day, sir!
                                } else {
                                    m_TextMesh.text = SelectedWord;
                                    SelectedWord = "";
                                }
                                LineExited = true;
                            }
                            tcolor.g = 1f;
                            tcolor.a = 0.5f;
                        }
                        m_TextMesh.color = tcolor;
                    }
                    // end TODO
                    if (LineExited) ReadyForKey = false;
                    if (LineEntered) ReadyForKey = true;
                }
            }
        }

        // Add another line of text only if we have room
        if (lowest > NotesRangeBottom.position.y + MinLineSpacing) {
            float yPos = NotesRangeBottom.position.y;
            BeatObservers.Add(CreateBeat(1, beatPrefab, 1f, 
                NotesRangeLeft.transform.position.x + BeatPrefabSpacingSize, yPos, 
                NextTestWord()));
            BeatObservers.Add(CreateBeat(2, beatPrefab, 1f, 
                NotesRangeLeft.transform.position.x + BeatPrefabSpacingSize * 2, yPos, 
                NextTestWord()));
            BeatObservers.Add(CreateBeat(3, beatPrefab, 1f, 
                NotesRangeLeft.transform.position.x + BeatPrefabSpacingSize * 3, yPos, 
                NextTestWord()));
        }
    }

    private void OnChange(int index, float change)
    {
        foreach (var observer in BeatObservers)
        {
            if (observer != null)
                observer.Result.OnChange(index, change);
        }
    }
    private void OnOnset(OnsetType type, Onset onset)
    {
        // Signal each of our observers about the onset event
        foreach (var observer in BeatObservers)
        {
            if (observer != null)
                observer.Result.OnOnset(type, onset);
        }

        // Create musical objects based on the onset ranking and type
        if (onset.rank < 4 && onset.strength < 5)
            return;

        //float xPos = m_lastPrefabX + BeatPrefabSpacingSize;
        //if (xPos > NotesRangeRight.transform.position.x)
        //    xPos = NotesRangeLeft.transform.position.x;
        //m_lastPrefabX = xPos;
        
    }

    private IBeatObserverSyncContainer CreateBeat(int onsetIndex, GameObject prefab, float opacity, float xPosition, float yPosition, string text)
    {
        GameObject beatObject = Instantiate(prefab) as GameObject;
            xPosition += Random.value;
        beatObject.transform.position = new Vector3(xPosition, yPosition, -1f);
        BeatBehavior beatObserver = beatObject.GetComponent<BeatBehavior>();
        beatObserver.Text = text;
        beatObserver.Opacity = opacity;
        IBeatObserver beatInterfaceImpl = beatObserver as IBeatObserver;
        IBeatObserverSyncContainer syncContainer = new IBeatObserverSyncContainer();
        syncContainer.Result = beatInterfaceImpl;
        syncContainer.Result.OnsetIndex = onsetIndex;
        return syncContainer;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            NextSong();

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (ReadyForKey && SelectedWord == "") {
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                SelectedWord = GetWord("Up");
            }
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                SelectedWord = GetWord("Down");
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                SelectedWord = GetWord("Left");
            }
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                SelectedWord = GetWord("Right");
            }
        }
        if (rhythmTool.songLoaded)
        {
            UpdateBeatObjects();
        }
    }

    private void UpdateBeatObjects()
    {
        // Remove any "Beat" object beat observers that have been destroyed either by
        // moving too far off screen or through player interaction.
        List<IBeatObserverSyncContainer> toRemove = new List<IBeatObserverSyncContainer>();
        foreach (var observer in BeatObservers)
        {
            // if ((observer.Result.Tag() == "Beat") 
            //     && (observer.Result.OnsetIndex < rhythmTool.currentFrame 
            //     || observer.Result.OnsetIndex > rhythmTool.currentFrame + eventProvider.offset))

            if ((observer.Result.Tag() == "Beat") &&
                observer.Result.ObserverGameObject.transform.position.y > NotesRangeTop.position.y)
            {
                Destroy(observer.Result.ObserverGameObject);
                toRemove.Add(observer);
            }
        }
        foreach (var observer in toRemove)
        { 
            BeatObservers.Remove(observer);
        }

        // Do mathy stuff with magnitude smoothing; TODO: figure this part out when there's time
        float[] cumulativeMagnitudeSmooth = new float[eventProvider.offset + 1];
        float sum = 0;
        for (int i = 0; i < cumulativeMagnitudeSmooth.Length; i++)
        {
            int index = Mathf.Min(rhythmTool.currentFrame + i, rhythmTool.totalFrames - 1);

            sum += magnitudeSmooth[index];
            cumulativeMagnitudeSmooth[i] = sum;
        }

        // Move each, "Beat" object based on the magnitude smoothing options above.
        // foreach (var observer in BeatObservers)
        // {
        //     if (observer.Result.Tag() == "Beat" && observer.Result.ObserverGameObject != null)
        //     { 
        //         Vector3 pos = observer.Result.Position;
        //         // pos.y = (cumulativeMagnitudeSmooth[observer.Result.OnsetIndex - rhythmTool.currentFrame] * .03f) - 9f;
        //         // pos.y -= (magnitudeSmooth[rhythmTool.currentFrame] * .03f * rhythmTool.interpolation) - 9f;
        //         pos.y += ScrollConstant * sum;
        //         observer.Result.Position = pos;
        //     }
        // }
    }
};
