using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Provides interface for user with RyhtmEventController objects that may change behavior at runtime.
/// </summary>
public interface IBeatObserver {
    /// <summary>
    ///  Returns tag associated with gameobject attached to IBeatObserver implementation
    /// </summary>
    /// <returns></returns>
    string Tag();
    /// <summary>
    ///  Triggered when a song is first loaded by RythmEventController
    /// </summary>
    void OnSongLoaded();
    /// <summary>
    ///  Triggered when a song being played by RythmEventController ends
    /// </summary>
    void OnSongEnded();
    /// <summary>
    /// Triggered for each beat during song playback by RythmEventController
    /// </summary>
    /// <param name="beat"></param>
    void OnBeat(Beat beat);
    /// <summary>
    ///  Triggered for each Onset (e.g. note being played) during song playback by RythmEventController
    /// </summary>
    /// <param name="type">See OnsetType, this can be Low Medium High, or All</param>
    /// <param name="onset">See Onset, provides rank and strength of the onset</param>
    void OnOnset(OnsetType type, Onset onset);
    /// <summary>
    /// Triggered when a change in intensity happens during song playback by RhythmEventController
    /// </summary>
    /// <param name="index"></param>
    /// <param name="change"></param>
    void OnChange(int index, float change);

    /// <summary>
    /// Triggered when the player misses a key interaction when processing the beat.
    /// </summary>
    void OnMissedBeat();
    /// <summary>
    /// Exposes the position for this object, allowing it to be modified by the 
    /// rhythm controller if necessary.
    /// </summary>
    Vector3 Position { get; set; }

    /// <summary>
    ///  Exposes the game object associated with the beat observer
    /// </summary>
    GameObject ObserverGameObject { get; }

    /// <summary>
    /// Returns an index specifying the onset index associated with the BeatObserver
    /// Note: This will probably only be used on Beat objects with the tag, "Beat" that
    /// are used for the actual game mechanic.
    /// </summary>
    int OnsetIndex { get; set; }
}

/// <summary>
/// Provides a container implementation that can be used with the Unity Editor to provide a list of
/// polymorphic IBeatObserver implementations. E.g. Expose this as a public member and
/// IBeatObserer implementations can be dropped onto it.
/// </summary>
[System.Serializable]
public class IBeatObserverSyncContainer : IUnifiedContainer<IBeatObserver>
{
}
