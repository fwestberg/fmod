using System;
using System.Runtime.InteropServices;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
    
// Variables that are modified in the FMOD callback need to be part of a seperate class.
// This class needs to be 'blittable' otherwise it can't be pinned in memory.
[StructLayout(LayoutKind.Sequential)]
public class TimelineInfo
{
    public int currentMusicBar = 0;
    public int currentMusicBeat = 0;
    public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
}