using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using UnityEngine;
public class Sun : MonoBehaviour
{
    [SerializeField] private PlayerDamage player;
    [SerializeField] private Collider playerCollider;
    [SerializeField] private float lightAngle;
    [SerializeField] private GameObject sunlight;
    [SerializeField] private LayerMask terrainLayer;
    private bool night;

    private float timer;
    private void Start()
    {
        StartCoroutine(DayNightCycle());
    }

    private void Update()
    {
        Vector3 sunAngle = sunlight.transform.forward;
        Ray ray = new Ray(player.transform.position, -sunAngle);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayer)) 
        {
            if (hit.collider || night)
            {
                player.exposed = false;
                Debug.DrawRay(player.transform.position, -sunAngle * 100, Color.green);
            }
        }
        else if (!night)
        {
            player.exposed = true;
            Debug.DrawRay(player.transform.position, -sunAngle * 100, Color.red);
        }
    }

    IEnumerator DayNightCycle()
    {
        while (true)
        {
            night = false;
            sunlight.SetActive(true);
            while (timer < 15)
            {
                timer += Time.deltaTime;
                var t = timer / 15;
                lightAngle = Mathf.LerpAngle(45.0f, 135.0f, t);
                yield return null;
                sunlight.transform.rotation = Quaternion.Euler(lightAngle, 0f, 0f);
            }
            night = true;
            sunlight.SetActive(false);
            yield return new WaitForSeconds(15f);
            timer = 0;
        }
    }
}
