using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public Vector3 sway;
    public Vector3 output;
    public float damp= 15;
    public float originalZ;
    public Vector3 originalRot; 
    // Start is called before the first frame update
    void Start()
    {
        
        originalZ = transform.localRotation.eulerAngles.z;
        sway = Vector3.forward * originalZ;
        originalRot = Vector3.forward * originalZ;
        Debug.Log(originalZ);
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("MouseX") * 30 * Time.deltaTime;
        if (Mathf.Abs(mouseX) > 0) {
            
            sway += Vector3.forward * mouseX;
        }
        sway = Vector3.Lerp(sway, Vector3.forward * originalZ,damp * Time.deltaTime);
        output = Vector3.Lerp(output, sway, damp * Time.deltaTime);
        if (sway != originalRot) {
            //Debug.Log("true ");
            transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, output.z));

        }
        
        //mouseX = Mathf.Lerp(transform.rotation.y, transform.rotation.y + mouseX, 5 * Time.fixedDeltaTime);
        //transform.Rotate(Vector3.up * mouseX);
    }
}
