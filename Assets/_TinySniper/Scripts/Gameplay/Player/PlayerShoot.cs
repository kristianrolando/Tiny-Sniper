using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Aldo.PubSub;

public class PlayerShoot : MonoBehaviour
{

    [Header("Component")]
    [SerializeField] Camera playerCam;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPref;
    [SerializeField] RecoilSystem _recoil;

    [Header("Bullet and Relaod")]
    [SerializeField] int maxBullet = 5;
    [SerializeField] float reloadTime = 3f;

    int currentBullet;
    bool isReloading = false;

    public Animator gunAnim;

    private void Start()
    {
        currentBullet = maxBullet;
        GameplayScene.Instance.UpdateBulletText(maxBullet, currentBullet);
    }

    /// <summary>
    /// this function handle shot logic
    /// </summary>
    public void Shoot()
    {
        // check bullet
        if (currentBullet <= 0)
        {
            if (!isReloading)
                StartCoroutine(Reload());
            return;
        }

        Vector3 _targetPos;
        Transform _targetTransform = null;

        // shoot ray in center of screen
        Ray ray = playerCam.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        _targetPos = GetPosTarget(ray);
        _targetTransform = GetTransformTarget(ray);

        //if (Physics.Raycast(ray, out RaycastHit hit))
        //{
        //    _targetPos = hit.point;
        //    _targetTransform = hit.collider.GetComponent<Transform>();
        //    if(!hit.collider.GetComponent<Enemy>())
        //    {
        //        PublishSubscribe.Instance.Publish<MessageShootMiss>(new MessageShootMiss());
        //        GameplayScene.Instance._arlertNotifObj.SetActive(true);
        //    }
        //}

        if(_targetTransform != null)
        {
            if (_targetTransform.tag == "Enemy")
            {
                // check if headshot
                if (_targetTransform.tag == "EnemyHead")
                {
                    // headshoot
                }
            }
        }
        

        // spawn bullet
        GameObject _bullet = Instantiate(bulletPref, firePoint.position, Quaternion.identity);
        _bullet.GetComponent<Bullet>().Initial(_targetPos, 100, _targetTransform);

        currentBullet -= 1;

        _recoil.RecoilFire();
        GameplayScene.Instance.UpdateBulletText(maxBullet, currentBullet);
        
        if(AudioManager.Instance != null)
            AudioManager.Instance.PlaySfx("shoot");
    }
    private Vector3 GetPosTarget(Ray ray)
    {
        Vector3 _targetPos;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            _targetPos = hit.point;
        }
        else
            _targetPos = ray.GetPoint(200f);
        return _targetPos;
    }
    private Transform GetTransformTarget(Ray ray)
    {
        Transform _targetTransform = null;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            _targetTransform = hit.collider.GetComponent<Transform>();
            if (!hit.collider.GetComponent<EnemyController>())
            {
                PublishSubscribe.Instance.Publish<MessageShootMiss>(new MessageShootMiss());
            }
        }

        return _targetTransform;
    }

    private IEnumerator Reload()
    {
        gunAnim.SetTrigger("reload");
        AudioManager.Instance.PlaySfx("reload");
        isReloading = true;
        GameplayScene.Instance._reloadNotifObj.SetActive(true);
        // reloading
        yield return new WaitForSeconds(reloadTime);
        // reload done
        currentBullet = maxBullet;
        isReloading = false;
        GameplayScene.Instance._reloadNotifObj.SetActive(false);
        GameplayScene.Instance.UpdateBulletText(maxBullet, currentBullet);
    }
    public void ReloadButton()
    {
        StartCoroutine(Reload());
    }
}
