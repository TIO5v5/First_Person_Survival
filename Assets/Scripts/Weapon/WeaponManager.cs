using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private WeaponHandler[] weapons;

    private int keyValue;

    private int currentWeaponIndex;

    void Start()
    {
				//currentWeaponIndex = Random.Range(0, 5);
				//більш кращий варіант, тепер залежність від кількості зброї
        currentWeaponIndex = Random.Range(0, weapons.Length - 1);
        weapons[currentWeaponIndex].gameObject.SetActive(true);
    }
  
    void Update()
    {
        if(Input.anyKeyDown)
        {
						// виконує перетворення з String в int
            int.TryParse(Input.inputString, out keyValue);

						// коригуємо вибір зброї
						//keyValue = keyValue - 1;
						//keyValue -= 1;
            keyValue--;

						// перевірка на коректність введення даних.
            if(keyValue >= 0 && keyValue <= (weapons.Length - 1))
            {
                TurnOnSelectWeapon(keyValue);
            }
        }
    }

    private void TurnOnSelectWeapon(int weaponIndex)
    {
       if (currentWeaponIndex == weaponIndex)
       {
            return;
       }

        weapons[currentWeaponIndex].gameObject.SetActive(false);
        weapons[weaponIndex].gameObject.SetActive(true);
        currentWeaponIndex = weaponIndex;
    }

    public WeaponHandler GetCurrentSelectedWeapon()
    {
        return weapons[currentWeaponIndex];
    }
}
