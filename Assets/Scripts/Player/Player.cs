using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int _movePoint = 3;
    public Transform _bulletPoint;
    public int _health =5;
    public bool canShoot;
    public int ammo;
    public TypeSoldier type;
   // private Dictionary<TypeSoldier, GameObject> bulletPrefab;
    public List<GameObject> bulletPrefab;
    private Vector2 _spawnPoint;
    private float _bulletSpeed;
    public enum TypeSoldier
    {
        Pistol = 1,
        Rifle = 2,
        Rocket = 3,
        Autogun = 4,
    }


    private void Awake()
    {
        switch (type)
        {
            case TypeSoldier.Rifle:
                canShoot = true;
                _movePoint = 3;
                ammo = 20;
                _bulletSpeed = 5;
                break;
            case TypeSoldier.Pistol:
                canShoot = true;
                _movePoint = 4;
                ammo = 15;
                _bulletSpeed = 5;
                break;
            case TypeSoldier.Rocket:
                canShoot = false;
                _movePoint = 2;
                ammo = 5;
                _bulletSpeed = 5;
                break;
            case TypeSoldier.Autogun:
                canShoot = true;
                _movePoint = 3;
                ammo = 30;
                _bulletSpeed = 5;
                break;
            default:
                print("Videri igroka Valera");
                break;
        }

        
        
        
        _spawnPoint = gameObject.transform.Find("BulletPoint").position;
    }
   

    public void Shoot()
    {
       var bullet = CreateBullet();
       StartCoroutine(MoveBullet(bullet, Vector2.zero));
       canShoot = false;
    }

    private GameObject CreateBullet()
    {
      return  Instantiate(bulletPrefab[0], _spawnPoint, Quaternion.identity);
    }

    // private void SetTypeBullet()
    // {
    //     switch (type)
    //     {
    //         TypeSoldier.Rifle:
    //     }
    // }
    
    private IEnumerator MoveBullet(GameObject bullet, Vector2 target)
    {
        while ((Vector2)bullet.transform.position != target)
        {
                var step = _bulletSpeed * Time.deltaTime;
                bullet.transform.position =  Vector2.MoveTowards(bullet.transform.position, target, step);
                yield return null;
        }

    }
}


