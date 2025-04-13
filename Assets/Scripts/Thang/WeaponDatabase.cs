using System.Collections.Generic;
using UnityEngine;

public class WeaponDatabase : MonoBehaviour
{
    public static WeaponDatabase Instance;

    public List<WeaponBase> allWeapons;

    private void Awake()
    {
        Instance = this;
    }

    public WeaponBase GetWeaponByName(string name)
    {
        return allWeapons.Find(w => w.name == name);
    }
}
