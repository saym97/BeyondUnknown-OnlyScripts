using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]


public class Weapon
{
    public WeaponStat weaponStat;
    public GameObject weaponModel;
    public ParticleSystem muzzleFlash;
    public Animator anim;
    public bool unlocked;
}
