using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : PlayerAbility
{
    public Slider HealthSlider;

    private void Start()
    {
        Refresh();
    }
    public void Refresh()
    {
        HealthSlider.value = _owner.Stat.Health / _owner.Stat.MaxHealth;
    }
}
