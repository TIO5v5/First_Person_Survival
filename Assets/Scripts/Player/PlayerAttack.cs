using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private WeaponManager weaponManager;

    public float fireRate = 15f;
    private float nextTimeToFire;
    public float damage = 20f;

    public const string Axe = "AXE_TAG";
    public const string Crosshair = "CROSSHAIR";
    public const string ZoomIn = "ZoomIn";
    public const string ZoomOut = "ZoomOut";
    public const string Enemy = "Enemy";

    private Animator zoomCameraAnim;
    private Camera mainCam;
    private GameObject crosshair;
    private bool isAiming;

    [SerializeField]
    private GameObject arrowPrefab, spearPrefab;

    [SerializeField]
    private Transform arrowBowStartPosition;

    private bool isBowSpearActive = true;


    void Awake()
    {
        weaponManager = GetComponent<WeaponManager>();
        zoomCameraAnim = transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>();
        crosshair = GameObject.FindWithTag(Crosshair);
        mainCam = Camera.main;
    }

    void Update()
    {
        WeaponShoot();
        ZoomInAndOut();
    }

    void WeaponShoot()
    {
        if (weaponManager.GetCurrentSelectedWeapon().fireType == WeaponFireType.MULTIPLE)
        {
            if (Input.GetMouseButton(0) && Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
                BulletFired();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (weaponManager.GetCurrentSelectedWeapon().tag == Axe)
                {
                    weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
                }
                if (weaponManager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.BULLET)
                {
                    weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
                    BulletFired();
                }
                else
                {
                    if (isAiming && isBowSpearActive)
                    {
                        weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
                        isBowSpearActive = false;

                        if (weaponManager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.ARROW)
                        {
                            //кинути стрілу
                            ThrowArrowOrSpear(true);

                        }
                        else if (weaponManager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.SPEAR)
                        {
                            // кинути спис
                            ThrowArrowOrSpear(false);
                        }
                    }
                }
            }
        }
    }

    public void ActivateBowSpear()
    {
        isBowSpearActive = true;
    }

    private void ZoomInAndOut()
    {
        if (weaponManager.GetCurrentSelectedWeapon().weaponAim == WeaponAim.AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                zoomCameraAnim.Play(ZoomIn);
                crosshair.SetActive(false);
            }

            if (Input.GetMouseButtonUp(1))
            {
                zoomCameraAnim.Play(ZoomOut);
                crosshair.SetActive(true);
            }
        }

        if (weaponManager.GetCurrentSelectedWeapon().weaponAim == WeaponAim.SELF_AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                weaponManager.GetCurrentSelectedWeapon().Aim(true);
                isAiming = true;
            }

            if (Input.GetMouseButtonUp(1))
            {
                weaponManager.GetCurrentSelectedWeapon().Aim(false);
                isAiming = false;
            }
        }
    }

    private void ThrowArrowOrSpear(bool isArrow)
    {
        if (isArrow)
        {
            // Create Arrow
            GameObject arrow = Instantiate(arrowPrefab);
            arrow.transform.position = arrowBowStartPosition.position;
            arrow.GetComponent<ArrowBowScript>().Launch(mainCam);
        }
        else
        {
            // Create Spear
            GameObject spear = Instantiate(spearPrefab);
            spear.transform.position = arrowBowStartPosition.position;
            spear.GetComponent<ArrowBowScript>().Launch(mainCam);
        }

        //GameObject something = Instantiate(isArrow ? arrowPrefab : spearPrefab);
        //something.transform.position = arrowBowStartPosition.position;
        //something.GetComponent<ArrowBowScript>().Launch(mainCam); 
    }

    void BulletFired()
    {
        RaycastHit hit;
        // якщо ми влучили в об'єкт
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit))
        {
            // далі будемо перевіряти в кого ми влучили
            // зараз просто виводимо інформацію про об'єкт в який влучили в консоль
            if (hit.transform.tag == Enemy)
            {
                // наносимо шкоду
                hit.transform.GetComponent<HealthScript>().ApplyDamage(damage);
            }
        }
    }

}
