using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeaponOverlay : MonoBehaviour
{
    [System.Serializable]
    public struct WeaponOverlayPositions {
        public RectTransform rect;
        public int pos;
    }
    private int currentWeaponOnDisplay;
    public Vector2[] points;
    public WeaponOverlayPositions[] weapons;
    public CanvasGroup overlay;
    public Image[] weaponImages;

    [ContextMenu("MoveOverlay")]
    public void MoveOverlay(int currentWeapon) {
        foreach(Image image in weaponImages) {
            image.enabled = false;
            LeanTween.cancel(overlay.gameObject);
        }
        weaponImages[currentWeapon].enabled = true;
        LeanTween.alphaCanvas(overlay, 1.0f, 0.5f).setFrom(0f);
        LeanTween.alphaCanvas(overlay, 0f, 0.5f).setFrom(1.0f).setDelay(1f);


       /* overlay.alpha = 1.0f;
        if (iteration == +1) {
            LeanTween.moveX(weapons[currentWeaponOnDisplay].rect, -236f, 0.5f);
            if (currentWeaponOnDisplay + iteration > (unlockedweapons - 1)) {
                currentWeaponOnDisplay = 0;
            }
            else {
                currentWeaponOnDisplay++;
            }
            weapons[currentWeaponOnDisplay].rect.anchoredPosition = points[1];
            LeanTween.moveX(weapons[currentWeaponOnDisplay].rect, 0f, 0.5f);
        }
        else {
            LeanTween.moveX(weapons[currentWeaponOnDisplay].rect, 236f, 0.5f);
            if (currentWeaponOnDisplay + iteration < 0) {
                currentWeaponOnDisplay = unlockedweapons - 1;
            }
            else {
                currentWeaponOnDisplay--;
            }
            weapons[currentWeaponOnDisplay].rect.anchoredPosition = points[4];
            LeanTween.moveX(weapons[currentWeaponOnDisplay].rect, 0, 0.5f);
        }
        LeanTween.alphaCanvas(overlay, 0, 1.5f);
        /*for (int i = 0; i < weapons.Length; i++) {

            if (weapons[i].pos + iteration > (weapons.Length - 1)) weapons[i].pos = 0;
            else if (weapons[i].pos + iteration < 0) weapons[i].pos = (weapons.Length - 1);
            else weapons[i].pos += iteration; 
            if (weapons[i].pos == (weapons.Length - 3) || weapons[i].pos == (weapons.Length - 2)) {
                weapons[i].rect.position = points[weapons[i].pos];
            }
            else {
                LeanTween.move(weapons[i].rect, points[weapons[i].pos], 0.5f).setEaseInQuint();
            }
        }*/
    }
}
