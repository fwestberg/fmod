using UnityEngine;
using FMOD.Studio;
using FMODUnity;

// This is an example of a script subscribing to the TimelineChanged event from the MusicSystem class
// It works roughly as a Unity event:
// Each time there's new timeline information in the EventInstance instantiated in MusicSystem,
// the event TimelineChanged is raised – resulting in the method DoSomething (in this script) is called
public class MusicSubscriber : MonoBehaviour
{
    // When this object is enabled, subscribe to the event, TimelineChanged, from MusicSystem
    void OnEnable()
    {
        MusicSystem.TimelineChanged += DoSomething;
    }

    // When this object is disabled, unsubscribe from the event
    // It is extremely important to include this step to prevent memory leaks and errors
    void OnDisable()
    {
        MusicSystem.TimelineChanged -= DoSomething;
    }

    // In this method, you perform whatever action you want
    void DoSomething(object sender, TimelineEventArgs args){
        Debug.Log(this + " is doing something");
        // Debug.Log("Current bar is: " + args.Info.currentMusicBar);
        // Debug.Log("Current beat is: " + args.Info.currentMusicBeat);
        // Debug.Log("Last marker is: " + args.Info.lastMarker);
        // MusicSystem.Instance.musicInstance.setParameterValue("someParameterName", 1.0f);
    }
}