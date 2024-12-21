using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class AttackBarra : MonoBehaviour
{
    public GameObject barra;
    public GameObject Basebarra;
    public GameObject SimbleAttack;
    public player playerScript;

    // Update is called once per frame
    void Update()
    {
        float power = playerScript.GetPowerPlayer();
        float barraForca = barra.transform.localScale.x;

        if (power>= 20)
        {
            barra.SetActive(false);
            Basebarra.SetActive(false);
            SimbleAttack.SetActive(true);
        }
        else
        {
            barra.SetActive(true);
            Basebarra.SetActive(true);
            SimbleAttack.SetActive(false);
        }

        power = (1.0f * power) / 20;
        if (power < 0)
        {
            power = 0;
        }
        barra.transform.localScale = new Vector3(power, barra.transform.localScale.y, barra.transform.localScale.z);
    }
}
