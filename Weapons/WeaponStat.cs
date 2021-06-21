using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName ="Weapon")]


public class WeaponStat : ScriptableObject
{
    [Header("Waepon Specifications")]
    public string weaponName;
    public WeaponType weaponType;
    public float range;
    public int damageAmount;
    public int bulletsInMagazine;
    public int magSize;
    public int currentAmmo;
    public int maxAmmo;
    public bool automatic;
    [Range(0, 2)]
    public float fireRate;
    public float reloadTime;
    public AudioClip shotSound;
    public AudioClip reloadSound;
    [Space(10)]
    [Header("UI")]
    public Sprite weaponIcon;
    public Sprite crossHair;
    [Space(10)]
    [Header("Recoil attributes")]
    [Range(0, 50)]
    public float kick_back_position;
    [Range(0, 50)]
    public float kick_back_rotation;
    [Range(1,10)]
    public int rotation_damping;
    [Range(1, 10)]
    public int position_damping;
    [Range(1, 10)]
    public float camera_damp_multiplier;
}
