using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Material[] materials;
    public float timer = 5;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 3.5f)
        {
            gameObject.GetComponent<MeshRenderer>().material = materials[0];
        }
        if(timer <=0)
        {
            gameObject.GetComponent<MeshRenderer>().material = materials[1];
        }
        if (timer <= -10)
        {
            gameObject.GetComponent<MeshRenderer>().material = materials[0];
        }
    }

    //public void ChangeAlpha(float alphaValue)
    //{
    //    Color oldColor = mat.color;
    //    Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alphaValue);
    //    mat.SetColor("Color", newColor);
    //}
}
