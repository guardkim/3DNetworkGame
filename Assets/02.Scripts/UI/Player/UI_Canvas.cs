using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_Canvas : Singleton<UI_Canvas>
{
    public Slider StaminaProgress;
    public Slider HealthProgress;

    private PlayerStat _stat;
    private void SetPlayer(Player player)
    {

    }
    public void StaminaBind(PlayerStat stat)
    {
        _stat = stat;
        StaminaProgress.value = stat.Stamina / stat.MaxStamina;
        stat.OnStaminaChanged += UpdateStamina;
    }
    private void UpdateStamina(float ratio)
    {
        StaminaProgress.value = ratio;
    }
    private void Update()
    {
        if (_stat != null)
        {
            HealthProgress.value = _stat.Health / _stat.MaxHealth;
        }

    }

}
