﻿using UnityEngine;

public class Motor : MonoBehaviour
{
    public int MotorID;
    public int Offset;
    private ArduinoComms commsScript;

    public Vector3 StartPos;

    public float MinRot = -180;
    public float MaxRot = 180;

    public bool IsOn = true;
    public bool invert;

    public enum Axis
    {
        x,
        y,
        z
    }
    public Axis thisAxis;
    public Vector3 quatAxis;

    private char GetAxisResult;

    private void Awake()
    {
        commsScript = ArduinoComms.instance;

        StartPos = transform.localPosition;

        switch (thisAxis)
        {
            case Axis.x:
                quatAxis = new Vector3(1, 0, 0);
                break;
            case Axis.y:
                quatAxis = new Vector3(0, 1, 0);
                break;
            case Axis.z:
                quatAxis = new Vector3(0, 0, 1);
                break;
        }
    }

    /// <summary>
    /// Returns a char from the Motor class
    /// <para>
    /// The motor returns it's assign cartesian axis as a char to avoid sharing Enum configuration.
    /// </para>
    /// </summary>
    public char GetAxis()
    {
        switch (thisAxis)
        {
            case Axis.x:
                GetAxisResult = 'x';
                break;
            case Axis.y:
                GetAxisResult = 'y';
                break;
            case Axis.z:
                GetAxisResult = 'z';
                break;
            default:
                Debug.LogError("Cannot find Axis type.");
                GetAxisResult = 'w';
                break;
        }

        return GetAxisResult;
    }

    public void Reposition(float newAngle)
    {
        if (!IsOn)
        {
            return;
        }

        newAngle = Mathf.Clamp(newAngle, MinRot, MaxRot);

        if (invert)
        {
            commsScript.Messages[MotorID] = Mathf.RoundToInt(Mathf.Abs(MaxRot - newAngle)) + Offset;
        }

        else
        {
            commsScript.Messages[MotorID] = Mathf.RoundToInt(newAngle) + Offset;
        }        

        switch (thisAxis)
        {
            case Axis.x:
                transform.localEulerAngles = new Vector3(newAngle, 0, 0);
                break;
            case Axis.y:
                transform.localEulerAngles = new Vector3(0, newAngle, 0);
                break;
            case Axis.z:
                transform.localEulerAngles = new Vector3(0, 0, newAngle);
                break;
        }
    }
}