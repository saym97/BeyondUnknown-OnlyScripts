using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WeaponType { Machete, Pistol, Machinegun, Shotgun, Railgun }
public enum WeaponState {Idle, Firing, Reloading, Switch}
public class Shooting : MonoBehaviour {

    public Camera mCam;

    //Particle systems for bullet impact
    public GameObject stoneImpact;
    public GameObject bloodImpact;
    public GameObject metalImpact;

    //layer mask to ignore player while raycast from weapon
    private int layerMask = 1 << 8;

    //Variable related to weapons
    public List<Weapon> weaponCollection;
    public Recoil recoil;
    [HideInInspector]
    public int currentWeapon = 0;
    bool nelioMode;

    float fireRateTime;
    public WeaponState weaponState;
    WeaponStat currentWeaponStats;
    [SerializeField]
    AudioSource weaponSounds;
    [SerializeField]
    AudioClip emptyWeapon;

    public PlayerMove player;
    [SerializeField]
    private WeaponOverlay weaponOverlay;
    [HideInInspector]
    public int unLockedWeapons = 2;
    [Header("nelio mode")]
    public GameObject waterfall;
    public GameObject neliofall;

    void Start() {
        weaponState = WeaponState.Idle;
        fireRateTime = 0;
        layerMask = ~layerMask;
        SwitchWeapon(true, currentWeapon);
        nelioMode = false;
    }

    // Update is called once per frame
    void Update() {
        fireRateTime += Time.deltaTime;
       // if (GameManager.instance.gameState == GameState.PAUSED) return;
        if (weaponState == WeaponState.Reloading || weaponState == WeaponState.Switch) return;
        //Automatic
        if (currentWeaponStats.automatic) {
            if (Input.GetKey(KeyCode.Mouse0) && fireRateTime > currentWeaponStats.fireRate && (weaponState == WeaponState.Idle || weaponState == WeaponState.Firing)) {
                Shoot(weaponCollection[currentWeapon]);
                fireRateTime = 0;
                player.shootingStance(true);
                weaponState = WeaponState.Firing;
            } else {
                if (Input.GetKeyUp(KeyCode.Mouse0)) {
                    if (player.currentState == state.shooting) {
                        player.shootingStance(false);
                    }
                    weaponState = WeaponState.Idle;
                }
            }
        //Semi automatic
        } else {
            if (Input.GetKeyDown(KeyCode.Mouse0) && fireRateTime > currentWeaponStats.fireRate && weaponState == WeaponState.Idle) {
                Shoot(weaponCollection[currentWeapon]);
                fireRateTime = 0;
                player.shootingStance(true);
                weaponState = WeaponState.Firing;
            } else {
                if (Input.GetKeyUp(KeyCode.Mouse0)) {
                    if(player.currentState == state.shooting) {
                        player.shootingStance(false);
                    }
                    weaponState = WeaponState.Idle;
                }
            }
        }
        

        if (Input.GetAxis("MouseScrollWheel") != 0 && weaponState != WeaponState.Switch) StartCoroutine(Switch(Input.GetAxis("MouseScrollWheel")));

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            StartCoroutine(SwitchToWeapon(0));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            StartCoroutine(SwitchToWeapon(1));
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            StartCoroutine(SwitchToWeapon(2));
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            StartCoroutine(SwitchToWeapon(3));
        }

        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            StartCoroutine(SwitchToWeapon(4));
        }

        if ((Input.GetButtonDown("Reload") || currentWeaponStats.bulletsInMagazine == 0) && currentWeaponStats.currentAmmo > 0 && weaponState == WeaponState.Idle
                                            && currentWeaponStats.bulletsInMagazine != currentWeaponStats.magSize) {
            StartCoroutine(Reload(weaponCollection[currentWeapon]));
        }

        //Nelio mode
        if (Input.GetKeyDown(KeyCode.F5)) {
            nelioMode = !nelioMode;
            waterfall.SetActive(!nelioMode);
            neliofall.SetActive(nelioMode);
            
        }

    }

    //Shoot weapon
    void Shoot(Weapon weapon) {
        if (weapon.weaponStat.weaponType != WeaponType.Machete) {
            if (weapon.weaponStat.bulletsInMagazine == 0) {
                weaponSounds.PlayOneShot(emptyWeapon);
                return;
            }
            weaponSounds.PlayOneShot(currentWeaponStats.shotSound);
            recoil.Fire();
            if(!nelioMode) {
                weapon.weaponStat.bulletsInMagazine--;
            }
            BeyondUnknownEventSystem.singleton.RunshootDelegate(weapon);
            weapon.muzzleFlash.Play();
            if (weapon.weaponStat.weaponType != WeaponType.Shotgun) {
                WeaponSingleShot();
            } else {
                weapon.anim.SetTrigger("fire");
                WeaponMultiShot();      
            }
        } else {
            weaponSounds.PlayOneShot(currentWeaponStats.shotSound);
            MacheteStab(weapon);
        }
    }

    //Switching weapon with mouse wheel
    IEnumerator Switch(float mousewheel) {
        if(weaponState == WeaponState.Reloading) {
            weaponSounds.Stop();
        }
        weaponState = WeaponState.Switch;
        weaponCollection[currentWeapon].anim.SetTrigger("switchOut");

        yield return new WaitForSeconds(0.5f);
        weaponCollection[currentWeapon].anim.ResetTrigger("switchOut");
        //Disable current weapon
        SwitchWeapon(false, currentWeapon);
        if (mousewheel > 0f) {
            currentWeapon++;

            currentWeapon = (currentWeapon >= weaponCollection.Count || !weaponCollection[currentWeapon].unlocked) ? 0 : currentWeapon;
            weaponOverlay.MoveOverlay(currentWeapon);
        }
        else if (mousewheel < 0f) {
            
            currentWeapon--;
            if(currentWeapon < 0) {
                currentWeapon = weaponCollection.Count - 1;
                while (!weaponCollection[currentWeapon].unlocked) currentWeapon--;
            }
            weaponOverlay.MoveOverlay(currentWeapon);
        }
        SwitchWeapon(true, currentWeapon);
        weaponCollection[currentWeapon].anim.SetTrigger("switchIn");
        yield return new WaitForSeconds(0.5f);
        //Enable next weapon in line
        weaponState = WeaponState.Idle;
    }

    IEnumerator SwitchToWeapon(int weapon) {
        if (weaponCollection[weapon].unlocked) {
            Debug.Log(weapon);
            if (weaponState == WeaponState.Reloading) {
                weaponSounds.Stop();
            }
            weaponState = WeaponState.Switch;
            weaponCollection[currentWeapon].anim.SetTrigger("switchOut");

            yield return new WaitForSeconds(0.5f);
            weaponCollection[currentWeapon].anim.ResetTrigger("switchOut");
            SwitchWeapon(false, currentWeapon);
            currentWeapon = weapon;
            //WEAPON OVERLAY GOES HERE
            weaponOverlay.MoveOverlay(currentWeapon);
            SwitchWeapon(true, currentWeapon);
            weaponCollection[currentWeapon].anim.SetTrigger("switchIn");
            yield return new WaitForSeconds(0.5f);
            weaponState = WeaponState.Idle;
        }
    }

    //Enable or disable a particular weapon and change recoil and bullet values 
    void SwitchWeapon(bool active, int weaponindex) {
        weaponCollection[weaponindex].weaponModel.SetActive(active);
        if (active) {
            currentWeaponStats = weaponCollection[weaponindex].weaponStat;
            recoil.Recoil1 = currentWeaponStats.kick_back_rotation;
            recoil.Recoil3 = currentWeaponStats.kick_back_position;
            recoil.PositionDampTime = currentWeaponStats.position_damping;
            recoil.RotationDampTime = currentWeaponStats.rotation_damping;
            recoil.CameraDampMultiplier = currentWeaponStats.camera_damp_multiplier;
            //Telling HUD to update stats
            BeyondUnknownEventSystem.singleton.RunSwitchDelegate(weaponCollection[weaponindex]);
        }
    }

    //Reload weapon
    IEnumerator Reload(Weapon weapon) {
        weaponState = WeaponState.Reloading;
        weapon.anim.SetTrigger("reload");
        weaponSounds.PlayOneShot(currentWeaponStats.reloadSound);
        yield return new WaitForSeconds(currentWeaponStats.reloadTime);

        if(weaponState == WeaponState.Reloading) {
            int bulletsReloaded = (currentWeaponStats.currentAmmo < currentWeaponStats.magSize) ? currentWeaponStats.currentAmmo
                                                                      : (currentWeaponStats.magSize - currentWeaponStats.bulletsInMagazine);
            currentWeaponStats.currentAmmo -= bulletsReloaded;
            currentWeaponStats.bulletsInMagazine += bulletsReloaded;
            BeyondUnknownEventSystem.singleton.RunReloadDelegate(weapon);
            weaponState = WeaponState.Idle;
            
        } 
    }

    //Shooting logic for single shot weapons
    void WeaponSingleShot() {
        RaycastHit hit;
        if (Physics.Raycast(mCam.transform.position, mCam.transform.forward, out hit, 300f, layerMask)) {
            HandleImpactParticle(hit);
            if (hit.collider.gameObject.CompareTag("Enemy")) {
                float damageModifer = CalculateDamageModifier(hit);
                int damageAmount = Mathf.RoundToInt(currentWeaponStats.damageAmount * damageModifer);
                hit.collider.gameObject.GetComponent<Animal>()?._EnemyGetsDamage(damageAmount);
                hit.collider.gameObject.GetComponent<BearAI>()?._EnemyGetsDamage(damageAmount);
            }
        }
    }

    //Shooting logic for multiple projectile weapon
    void WeaponMultiShot() {
        for (int i = 7; i > 0; i--) {
            RaycastHit hit;
            Vector3 t_bloom = mCam.transform.forward * 1000f;
            t_bloom += Random.Range(-40, 40) * mCam.transform.up;
            t_bloom += Random.Range(-40, 40) * mCam.transform.right;
            t_bloom.Normalize();

            if (Physics.Raycast(mCam.transform.position, t_bloom, out hit, 300f, layerMask)) {
                HandleImpactParticle(hit);
                if (hit.collider.gameObject.CompareTag("Enemy")) {
                    float damageModifer = CalculateDamageModifier(hit);
                    int damageAmount = Mathf.RoundToInt(currentWeaponStats.damageAmount * damageModifer);
                    hit.collider.gameObject.GetComponent<Animal>()?._EnemyGetsDamage(damageAmount);
                    hit.collider.gameObject.GetComponent<BearAI>()?._EnemyGetsDamage(damageAmount);
                }
            }
        }
    }

    //Stab animation activator for machete
    void MacheteStab(Weapon machete) {
        //Animator anim = machete.weaponModel.GetComponent<Animator>();
        machete.anim.SetTrigger("Swipe");
        RaycastHit hit;
        if (Physics.Raycast(mCam.transform.position, mCam.transform.forward, out hit, currentWeaponStats.range, layerMask)) {
            if (hit.collider.gameObject.CompareTag("Enemy")) {
                hit.collider.gameObject.GetComponent<Animal>()?._EnemyGetsDamage(currentWeaponStats.damageAmount);
            }
            HandleImpactParticle(hit);
        }
    }

    //Logic for the selecting particle system for every surface
    void HandleImpactParticle(RaycastHit hit) {
        if (hit.collider.gameObject.tag == "Enemy") {
            SpawnImpact(hit, bloodImpact);
        }
        else if (hit.collider.gameObject.tag == "Ground") {
            SpawnImpact(hit, stoneImpact);
        }
        else if (hit.collider.gameObject.tag == "Metal") {
            SpawnImpact(hit, metalImpact);
        }
    }

    //Activate the particle effect corresponding to the surface 
    void SpawnImpact(RaycastHit hit, GameObject prefeb) {
        GameObject obj = Instantiate(prefeb, hit.point, Quaternion.LookRotation(hit.normal));
        obj.transform.SetParent(hit.collider.transform);
    }

    float CalculateDamageModifier(RaycastHit hit) {
        float dist = Vector3.Distance(mCam.transform.position, hit.point);
        float maxRange = currentWeaponStats.range;
        if(player.currentState == state.crouching) {
            maxRange += maxRange * 0.25f;
        }
        float targetRange = dist / maxRange;
        float damageModifier;
        if (targetRange <= 1f) {
            damageModifier = 1f;
        } else if (targetRange <= 1.5f) {
            damageModifier = 0.75f;
        } else if (targetRange <= 2f) {
            damageModifier = 0.50f;
        } else if (targetRange <= 2.5f) {
            damageModifier = 0.25f;
        } else {
            damageModifier = 0.1f;
        }
        return damageModifier;
    }
}
