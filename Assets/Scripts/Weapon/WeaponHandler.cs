using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponAim
{
    NONE,
    SELF_AIM,
    AIM
}

public enum WeaponFireType
{
    SINGLE,
    MULTIPLE
}

public enum WeaponBulletType
{
    BULLET,
    ARROW,
    SPEAR,
    NONE
}

public class WeaponHandler : MonoBehaviour
{
    private Animator anim;

    public WeaponAim weaponAim;
    public WeaponFireType fireType;
    public WeaponBulletType bulletType;

    [SerializeField]
    private GameObject mazzleFlash;

    [SerializeField]
    private AudioSource shootSound, reloadSound;

    public GameObject attackPoint;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void ShootAnimation()
    {
        anim.SetTrigger("Shoot");
    }

    public void Aim(bool canAim)
    {
        anim.SetBool("Aim", canAim);
    }

    void PlayShootSound()
    {
        shootSound.Play();
    }

    void PlayReloadSound()
    {
        reloadSound.Play();
    }

	void TurnOnMazzleFlash()
    {
        mazzleFlash.SetActive(true);
    }

    void TurnOffMazzleFlash()
    {
        mazzleFlash.SetActive(false);
    }

    void TurnOnAttackPoint()
    {
        attackPoint.SetActive(true);
    }

    void TurnOffAttackPoint()
    {
        if(attackPoint.activeInHierarchy)
        {
            attackPoint.SetActive(false);
        }
    }

    void ActivateBowSpear()
    {
        GetComponentInParent<PlayerAttack>().ActivateBowSpear();
    }
}
