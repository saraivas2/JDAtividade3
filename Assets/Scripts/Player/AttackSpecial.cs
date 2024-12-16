using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpecial : MonoBehaviour
{
    private GameObject AttackFire;
    private float vel = 7f;
    // Start is called before the first frame update
    void Start()
    {
        AttackFire = Resources.Load("Fogo") as GameObject;  
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0,-vel*Time.deltaTime,0));
    }
}
