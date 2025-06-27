using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AddressableAssets;
public abstract class PlayerAbility : MonoBehaviour
{
    protected Player _owner { get; private set; }
    protected PhotonView _photonView;
    protected Animator _animator;

    protected virtual bool CanRegenStamina() => true;

    protected virtual void Awake()
    {
        _owner = GetComponent<Player>();
        _photonView = GetComponent<PhotonView>();
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        UI_Canvas.Instance.StaminaBind(_owner.Stat);
    }
    private void Update()
    {
        HandleStaminaRegen();
    }
    private void HandleStaminaRegen()
    {
        if (CanRegenStamina())
        {
            _owner.Stat.Stamina += _owner.Stat.StaminaRecovery * Time.deltaTime;
        }
    }
    protected bool TryUseStamina(float amount)
    {
        if (_owner.Stat.Stamina >= amount)
        {
            _owner.Stat.Stamina -= amount;
            return true;
        }
        return false;
    }
}
