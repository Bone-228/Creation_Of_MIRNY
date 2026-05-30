using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public event Action AmmoChanged;
    public event Action<float> ReloadProgressChanged;
    [Header("GunData for weapon")]
    public GunData gunData;
    [Header("Base weapon characteristics")]
    public int Id { get; set; }
    [SerializeField]
    protected float fireRate = 0.1f;
    protected float fireTimer = 0f;

    [SerializeField]
    public float damage = 10f;

    public Func<string, bool> ShootInputMethod;

    public virtual int Ammo { get; }
    public virtual int MaxAmmo { get; }
    public virtual bool IsReloading { get; }
    protected void RaiseAmmoChanged()
    {
        AmmoChanged?.Invoke();
    }

    protected void RaiseReloadProgressChanged(float progress)
    {
        Debug.Log($"Progress: {progress}");
        ReloadProgressChanged?.Invoke(progress);
    }

    public abstract void Reload();

    protected virtual void Update()
    {
        fireTimer -= Time.deltaTime;

    }

    public virtual bool CanAttack()
    {
        if (fireTimer <= 0f)
            return true;
        return false;
    }

    public virtual void Attack()
    {
        if (!CanAttack())
            return;
    }
}
