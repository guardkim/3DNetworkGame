using UnityEngine;
using UnityEngine.UI;

public class UI_Canvas : Singleton<UI_Canvas>
{
    public Slider StaminaProgress;

    private void SetPlayer(Player player)
    {

    }
    public void StaminaBind(PlayerStat stat)
    {
        StaminaProgress.value = stat.Stamina / stat.MaxStamina;
        stat.OnStaminaChanged += UpdateStamina;
    }
    private void UpdateStamina(float ratio)
    {
        StaminaProgress.value = ratio;
    }

}
