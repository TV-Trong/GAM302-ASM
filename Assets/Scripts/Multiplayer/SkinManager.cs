using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    [SerializeField] private List<BodyPart> skins = new List<BodyPart>();

    [SerializeField] private SpriteRenderer headSR;
    [SerializeField] private SpriteRenderer bodySR;
    [SerializeField] private SpriteRenderer backpackSR;
    [SerializeField] private SpriteRenderer rArmSR;
    [SerializeField] private SpriteRenderer larmSR;

    private int skinIndex = 0;

    private void Awake()
    {
        headSR.sprite = skins[skinIndex].head;
        bodySR.sprite = skins[skinIndex].body;
        backpackSR.sprite = skins[skinIndex].backpack;
        rArmSR.sprite = skins[skinIndex].rArm;
        larmSR.sprite = skins[skinIndex].lArm;
        PlayerPrefs.SetInt("SkinIndex", skinIndex);
    }

    public void NextSkin()
    {
        skinIndex++;
        if (skinIndex == skins.Count)
            skinIndex = 0;

        headSR.sprite = skins[skinIndex].head;
        bodySR.sprite = skins[skinIndex].body;
        backpackSR.sprite = skins[skinIndex].backpack;
        rArmSR.sprite = skins[skinIndex].rArm;
        larmSR.sprite = skins[skinIndex].lArm;
        PlayerPrefs.SetInt("SkinIndex", skinIndex);
    }

    public void PrevSkin()
    {
        skinIndex--;
        if (skinIndex < 0)
            skinIndex = skins.Count - 1;

        headSR.sprite = skins[skinIndex].head;
        bodySR.sprite = skins[skinIndex].body;
        backpackSR.sprite = skins[skinIndex].backpack;
        rArmSR.sprite = skins[skinIndex].rArm;
        larmSR.sprite = skins[skinIndex].lArm;
        PlayerPrefs.SetInt("SkinIndex", skinIndex);
    }
}

[Serializable]
public class BodyPart
{
    public Sprite head;
    public Sprite body;
    public Sprite backpack;
    public Sprite rArm;
    public Sprite lArm;
}
