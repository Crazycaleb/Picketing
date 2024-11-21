using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Rnd = UnityEngine.Random;
using KModkit;

public class PicketingScript : MonoBehaviour
{
    public KMBombModule Module;
    public KMBombInfo BombInfo;
    public KMAudio Audio;
    public GameObject[] SignShapes;
    public KMSelectable[] Buttons;

    public GameObject[] ButtonColors;

    public Material Green;

    public Material[] PaintColors;

    int boostNumber;

    private int _moduleId;
    private static int _moduleIdCounter = 1;
    private bool _moduleSolved;


    
    int buttonCounter = 0;

    private void Start()
    {
        _moduleId = _moduleIdCounter++;
        boostNumber = Rnd.Range(20, 30);
        foreach (KMSelectable Button in Buttons)
        {
            Button.OnInteract += delegate () { ButtonPress(Button); return false; };
        }
        GenerateMod();
    }

    void GenerateMod(){
        Debug.LogFormat("[Picketing #{0}] The Boost Press Number is {1}", _moduleId, boostNumber);
        //PaintingStation();
    }

    void ButtonPress(KMSelectable Button){
        Button.AddInteractionPunch(0.5f);
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        if (_moduleSolved)
        {
            return;
        }

        if (Button == Buttons[3]){
            buttonCounter++;
            Debug.LogFormat("Pressed button {0} times.", buttonCounter);
            if (buttonCounter >= boostNumber){
                var renderer = ButtonColors[3].GetComponent<MeshRenderer>();
                var mats = renderer.materials;
                mats[1] = Green;
                renderer.materials = mats;
                Debug.Log("We is there");
            }
            else {
                Debug.LogFormat("Not there yet");
            }
        }
    }

    /*void PaintingStation(){
        switch(BombInfo.GetBatteryCount())

        case 0: if (BombInfo.GetPortCount(Port.PS2) > BombInfo.GetUnlitIndicators()){
            if (BombInfo.IsPortPresent(Port.RJ45)){
                return string tan;
            }
            else {
                return string teal;
            }
        }        
        else if (BombInfo.IsIndicatorOff()){
            return string gray;
        }
            else {
            return string red;
            }
        break;

        case 1:
            if (BombInfo.IsPortPresent(Port.Parallel))
            {
                if (BombInfo.GetPortCount(Port.Parallel) > BombInfo.GetPortCount(Port.DVI))
                {
                    return string yellow;
                }
                else
                {
                    return string indigo;
                }
            }
            else if (BombInfo.GetPortCount(Port.Serial) > BombInfo.GetLitIndicators())
            {
                return string blue;
            }
            else
            {
                return string purple;
            }
            break;

        case 2:
            if (BombInfo.IsIndicatorOn())
            {
                if (BombInfo.GetBatteryCount(KMBombInfoExtensions.KnownBatteryType.AA) > BombInfo.GetBatteryCount(KMBombInfoExtensions.KnownBatteryType.D))
                {
                    return string cyan;
                }
                else
                {
                    return string black;
                }
            }
            else if (BombInfo.GetPortCount(Port.PS2) + BombInfo.GetPortCount(Port.Serial) >= 2)
            {
                return string orange;
            }
            else
            {
                return string lavender;
            }
            break;

        default: if (BombInfo.GetPortCount(Port.RCA) > BombInfo.GetPortCount(Port.RJ45)){
            if (BombInfo.GetIndicators() < 3){
                return string gold;
            }
            else (){
                return string magenta;
            }
        }
        else if (BombInfo.IsPortPresent(Port.Serial)){
            return string brown;
        }
        else {
            return string green;
        }
        break;
    }*/
}