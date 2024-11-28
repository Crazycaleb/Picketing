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
    public GameObject[] Stations;

    public Material Green;
    public Material Yellow;
    public Material Red;

    public Material[] PaintColors;

    int boostNumber;

    private int _moduleId;
    private static int _moduleIdCounter = 1;
    private bool _moduleSolved;

    public Material PaintAnswer;

    private Dictionary<string, Material> colorDictionary;
    public string[] ColorNames;



    int buttonCounter = 0;

    private void Start()
    {
        _moduleId = _moduleIdCounter++;
        boostNumber = Rnd.Range(20, 30);
        foreach (KMSelectable Button in Buttons)
        {
            Button.OnInteract += delegate () { ButtonPress(Button); return false; };
        }
        /*foreach (KMSelectable Can in PaintCans){
            Can.OnInteract += delegate () { CanPress(Can); return false; };
        }*/


        colorDictionary = new Dictionary<string, Material>();
        for (int i = 0; i < PaintColors.Length; i++)
        {
            if (i < ColorNames.Length)
            {
                colorDictionary[ColorNames[i]] = PaintColors[i];
            }
        }

        GenerateMod();
    }

    public void AssignColor(string colorName)
    {
        Material material;
        if (colorDictionary.TryGetValue(colorName, out material)) // Explicitly declare 'material' outside 'out'
        {
            PaintAnswer = material;
        }
        else
        {
            Debug.LogWarning("Color '" + colorName + "' not found in the dictionary."); // Use string concatenation
        }
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

        if (Button == Buttons[0]){
            /* Writing Station */
            Stations[0].SetActive(true);
            Stations[1].SetActive(false);
            Stations[2].SetActive(false);
            var renderer = ButtonColors[0].GetComponent<MeshRenderer>();
            var mats = renderer.materials;
            mats[1] = Yellow;
            renderer.materials = mats;


            /* resetting the other buttons colors*/
            var renderer2 = ButtonColors[1].GetComponent<MeshRenderer>();
            var mats2 = renderer2.materials;
            mats2[1] = Red;
            renderer2.materials = mats2;

            var renderer3 = ButtonColors[2].GetComponent<MeshRenderer>();
            var mats3 = renderer3.materials;
            mats3[1] = Red;
            renderer3.materials = mats3;
        }

        else if (Button == Buttons[1])
        {
            /* Designing Station */
            Stations[0].SetActive(false);
            Stations[1].SetActive(true);
            Stations[2].SetActive(false);
            var renderer = ButtonColors[1].GetComponent<MeshRenderer>();
            var mats = renderer.materials;
            mats[1] = Yellow;
            renderer.materials = mats;


            /* resetting the other buttons colors*/
            var renderer2 = ButtonColors[0].GetComponent<MeshRenderer>();
            var mats2 = renderer2.materials;
            mats2[1] = Red;
            renderer2.materials = mats2;

            var renderer3 = ButtonColors[2].GetComponent<MeshRenderer>();
            var mats3 = renderer3.materials;
            mats3[1] = Red;
            renderer3.materials = mats3;
        }

        else if (Button == Buttons[2])
        {
            /* Painting Station */
            Stations[0].SetActive(false);
            Stations[1].SetActive(false);
            Stations[2].SetActive(true);
            var renderer = ButtonColors[2].GetComponent<MeshRenderer>();
            var mats = renderer.materials;
            mats[1] = Yellow;
            renderer.materials = mats;

            /* resetting the other buttons colors*/
            var renderer2 = ButtonColors[0].GetComponent<MeshRenderer>();
            var mats2 = renderer2.materials;
            mats2[1] = Red;
            renderer2.materials = mats2;

            var renderer3 = ButtonColors[1].GetComponent<MeshRenderer>();
            var mats3 = renderer3.materials;
            mats3[1] = Red;
            renderer3.materials = mats3;

        }

        if (Button == Buttons[3]){
            /* BOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOST Station */
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

    /*void CanPress(KMSelectable Can){
        Button.AddInteractionPunch(0.5f);
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        if (_moduleSolved)
        {
            return;
        }
        if (Stations[2] != true){
            return;
        }


    }*/

    void PaintingStation(){
        switch(BombInfo.GetBatteryCount()){
        case 0: 
            if (BombInfo.GetPortCount(Port.PS2) > BombInfo.GetOffIndicators().Count()){
                if (BombInfo.IsPortPresent(Port.RJ45)){
                     AssignColor("Beige");
                }
            else {
                AssignColor("Teal");
            }
            }        
            else if (BombInfo.GetOffIndicators().Count() >= 1){
                AssignColor("Grey");
            }
                else {
                AssignColor("Red");
            }
        break;

        case 1:
            if (BombInfo.IsPortPresent(Port.Parallel))
            {
                if (BombInfo.GetPortCount(Port.Parallel) > BombInfo.GetPortCount(Port.DVI))
                {
                    AssignColor("Yellow");
                }
                else
                {
                    AssignColor("Indigo");
                }
            }
            else if (BombInfo.GetPortCount(Port.Serial) > BombInfo.GetOnIndicators().Count())
            {
                AssignColor("Blue");
            }
            else
            {
                AssignColor("Purple");
            }
            break;

        case 2:
            if (BombInfo.GetOnIndicators().Count() >= 1)
            {
                if (BombInfo.GetBatteryCount(Battery.AA) > BombInfo.GetBatteryCount(Battery.D))
                {
                    AssignColor("Cyan");
                }
                else
                {
                    AssignColor("Black");
                }
            }
            else if (BombInfo.GetPortCount(Port.PS2) + BombInfo.GetPortCount(Port.Serial) >= 2)
            {
                AssignColor("Orange");
            }
            else
            {
                AssignColor("Lavender");
            }
            break;

        default: 
        if (BombInfo.GetPortCount(Port.StereoRCA) > BombInfo.GetPortCount(Port.RJ45)){
            if (BombInfo.GetIndicators().Count() < 3){
                AssignColor("Gold");
            }
            else{
                AssignColor("Magenta");
            }
        }
        else if (BombInfo.IsPortPresent(Port.Serial)){
                AssignColor("Brown");
        }
        else {
                AssignColor("Green");
        }
        break;
        }


    }
}