using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField]private float sunExposure;
    public bool exposed;
    public bool burningAlready;

    private void Start()
    {
        sunExposure = 0;
        burningAlready = false;
    }

    private void Update()
    {
        if (exposed && !burningAlready)
        {
            StopAllCoroutines();
            StartCoroutine(Exposed());
        }
        else if (!exposed && burningAlready)
        {
            StopAllCoroutines();
            StartCoroutine(Hidden());
        }
    }
    public IEnumerator Exposed()
    {
        burningAlready = true;
        while (sunExposure < 5)
        {
            sunExposure += 1;
            yield return new WaitForSeconds(1);
        }
        while (sunExposure == 5)
        {
            Debug.Log("DAMAGE");
            yield return new WaitForSeconds(1);
        }
    }
    public IEnumerator Hidden()
    {
        burningAlready = false;
        while (sunExposure > 0)
        {
            sunExposure -= 1;
            yield return new WaitForSeconds(1);
        }
    }
}
