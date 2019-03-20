//--------------------------------------------------------------------
//
// This is a Unity behaviour script that demonstrates how to use
// timeline markers in your game code. 
//
// Timeline markers can be implicit - such as beats and bars. Or they 
// can be explicity placed by sound designers, in which case they have 
// a sound designer specified name attached to them.
//
// Timeline markers can be useful for syncing game events to sound
// events.
//
// The script starts a piece of music and then displays on the screen
// the current bar and beat – as well as the last marker encountered.
//
// This script is (≈100%) based on ScriptUsageTimeline.cs from FMOD.
// It has been slightly modified by Flemming Westberg for the course
// Game Audio on Sonic College
//
// Modifications include: 
//      - Dependence on singleton base class (can be omitted)
//      - Public variable for setting the event path (with [EventRef])
//      - Inclusion of beat markers
//      - Event functionality: other classes can subscribe to TimelineChanged
//
//--------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicSystem : Singleton<MusicSystem>
{
    // Here we specify the path for our music event in FMOD
    [EventRef]
    public string eventPath;
    public EventInstance musicInstance;

    EVENT_CALLBACK beatCallback;

    TimelineInfo timelineInfo;
    GCHandle timelineHandle;

    // Other classes must subscribe to the TimelineChanged event
    public delegate void TimelineAction(object sender, TimelineEventArgs args);
    public static event TimelineAction TimelineChanged;

    // Check this in the inspector to show a small GUI with bar, beat and marker information on the screen
    public bool showGUI;

    void Start()
    {
        timelineInfo = new TimelineInfo();

        // Explicitly create the delegate object and assign it to a member so it doesn't get freed
        // by the garbage collected while it's being used
        beatCallback = new EVENT_CALLBACK(BeatEventCallback);

        musicInstance = RuntimeManager.CreateInstance(eventPath);

        // Pin the class that will store the data modified during the callback
        timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
        // Pass the object through the userdata of the instance
        musicInstance.setUserData(GCHandle.ToIntPtr(timelineHandle));

        musicInstance.setCallback(beatCallback, EVENT_CALLBACK_TYPE.TIMELINE_BEAT | EVENT_CALLBACK_TYPE.TIMELINE_MARKER);

        musicInstance.start();
    }

    void OnDestroy()
    {
        musicInstance.setUserData(IntPtr.Zero);
        musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        musicInstance.release();
        timelineHandle.Free();
    }

    void OnGUI()
    {
        if(showGUI)
            GUILayout.Box(String.Format("Bar = {0}, Beat = {1}, Last Marker = {2}", timelineInfo.currentMusicBar, timelineInfo.currentMusicBeat, (string)timelineInfo.lastMarker));
    }

    [AOT.MonoPInvokeCallback(typeof(EVENT_CALLBACK))]
    static FMOD.RESULT BeatEventCallback(EVENT_CALLBACK_TYPE type, EventInstance instance, IntPtr parameterPtr)
    {
        // Retrieve the user data
        IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError("Timeline Callback error: " + result);
        }
        else if (timelineInfoPtr != IntPtr.Zero)
        {
            // Get the object to store beat and marker details
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            switch (type)
            {
                case EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                    {
                        var parameter = (TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(TIMELINE_BEAT_PROPERTIES));
                        timelineInfo.currentMusicBar = parameter.bar;
                        timelineInfo.currentMusicBeat = parameter.beat;
                    }
                    break;
                case EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                    {
                        var parameter = (TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(TIMELINE_MARKER_PROPERTIES));
                        timelineInfo.lastMarker = parameter.name;
                    }
                    break;
            }

            // Raise the TimelineChanged event (if it is not null)
            TimelineEventArgs args = new TimelineEventArgs();
            args.Info = timelineInfo;
            TimelineChanged?.Invoke(MusicSystem.Instance, args);
        }
        return FMOD.RESULT.OK;
    }
}