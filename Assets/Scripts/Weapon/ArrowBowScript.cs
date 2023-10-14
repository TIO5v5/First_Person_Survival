using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBowScript : MonoBehaviour
{
    private Rigidbody myBody;
    public float speed = 30f;
    public float deactivateTime = 3f;
    public float damage = 15f;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody>();
    }

    void DiactivateGameObject()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }

    void Start()
    {
        Invoke(nameof(DiactivateGameObject), deactivateTime);
    }

    public void Launch(Camera mainCamera)
    {
        myBody.velocity = mainCamera.transform.forward * speed;
        transform.LookAt(transform.position + myBody.velocity);
    }

    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Enemy")
        {
            target.GetComponent<HealthScript>().ApplyDamage(damage);
            gameObject.SetActive(false);
        }
    }
}
