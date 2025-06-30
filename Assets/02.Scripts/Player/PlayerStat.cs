using System;
using Unity.VisualScripting;
using UnityEngine;
[Serializable]
public class PlayerStat
{
    public float MoveSpeed = 7f;
    public float RunSpeed = 12f;
    public float JumpForce = 2.5f;
    public float RotationSpeed = 300f;

    [Header("체력")]
    public float MaxHealth = 100;
    public float Health = 100;

    [Header("공격")]
    public float AttackSpeed = 1.6f;
    public float Damage =  20.0f;

    [Header("스태미나")]
    private float _stamina;
    public float MaxStamina = 100.0f;
    public float StaminaRecovery = 5.0f;

    public float Stamina
    {
        get => _stamina;
        set
        {
            float clamped = Mathf.Clamp(value, 0, MaxStamina);
            if (Math.Abs(_stamina - clamped) < 0.01f) return;

            _stamina = clamped;
            OnStaminaChanged?.Invoke(_stamina / MaxStamina); // 0~1 비율
        }
    }
    public event Action<float> OnStaminaChanged;
}
