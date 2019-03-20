using System;
using System.Runtime.InteropServices;
using UnityEngine;
    
// Wrapper class for event arguments

// Variables that are modified in the FMOD callback need to be part of a seperate class.
// This class needs to be 'blittable' otherwise it can't be pinned in memory.
[StructLayout(LayoutKind.Sequential)]
public class TimelineEventArgs : EventArgs 
{
    public TimelineInfo Info { get; set; }
}