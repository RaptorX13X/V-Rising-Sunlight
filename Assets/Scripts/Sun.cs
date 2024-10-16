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
    //[SerializeField] private DOTweenPath path;
    [SerializeField] private float lightAngle;
    [SerializeField] private GameObject sunlight;
    [SerializeField] private LayerMask terrainLayer;
    private bool night;

    private float timer;
    private void Start()
    {
        //path.DOPlay();
        //night = false;
        StartCoroutine(DayNightCycle());
    }

    private void Update()
    {
        Vector3 sunAngle = sunlight.transform.forward;
        Ray ray = new Ray(player.transform.position, -sunAngle);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayer)) // to też nie działa, coś jest nei tak
        {
            if (hit.collider || night)
            {
                player.exposed = false;
                Debug.DrawRay(player.transform.position, -sunAngle * 100, Color.green);
                //Debug.Log(hit.collider);
                //Debug.Log(night);
            }
            // else if (hit.collider == null && !night) 
            // {
            //     player.exposed = true;
            //     Debug.DrawRay(player.transform.position, -sunAngle * 100, Color.red);
            // } 
        }
        else if (!night)
        {
            player.exposed = true;
            Debug.DrawRay(player.transform.position, -sunAngle * 100, Color.red);
        }
    }
    /*private void Update()
        {
            Vector3 playerDirection = -transform.position + (player.transform.position + new Vector3(0, 1, 0));
            Ray ray = new Ray(transform.position, playerDirection * 100);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider == playerCollider && !night)
                {
                    player.exposed = true;
                    Debug.DrawRay(transform.position, playerDirection * 100, Color.red);
                }
                else if (hit.collider != playerCollider || night)
                {
                    player.exposed = false;
                    Debug.DrawRay(transform.position, playerDirection * 100, Color.green);
                }
            }
        }*/

    IEnumerator DayNightCycle()
    {
        while (true)
        {
            night = false;
            sunlight.SetActive(true);
            while (timer < 15)
            {
                timer += Time.deltaTime;
               // Debug.Log(timer);
                var t = timer / 15;
                lightAngle = Mathf.LerpAngle(45.0f, 135.0f, t); //nie
                // lightAngle += 6; też robi syf
                yield return null;
                sunlight.transform.rotation = Quaternion.Euler(lightAngle, 0f, 0f); //niepoprawne cyferki, skacze wszędzie ale nie tam gdzie trzeba
                //sunlight.transform.Rotate(Vector3.right, 6); //nie jest gładko ALE DZIAŁA
                //lightAngle = sunlight.transform.rotation.eulerAngles.x;
                //Debug.Log(lightAngle);
                //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            }
            night = true;
            sunlight.SetActive(false);
            yield return new WaitForSeconds(15f);
            timer = 0;
            //sunlight.transform.Rotate(Vector3.right, -90); 
        }
    }
    
    /*public void Restart()
    {
        StartCoroutine(Finished());
    }
    IEnumerator Finished()
    {
        path.DORestart();
        path.DOPause();
        timer = 0;
        night = true;
        while (timer < 15)
        {
            timer += 1;
            Debug.Log(timer);
            yield return new WaitForSeconds(1f);
        }
        night = false;
        path.DOPlay();
    }*/
}
