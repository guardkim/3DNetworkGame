using TMPro;
using UnityEngine;
public class PlayerNickName : PlayerAbility
{
    public TextMeshProUGUI NickNameTextUI;

    private void Start()
    {
        Debug.Log($"{_photonView.Owner.NickName} _ {_photonView.Owner.ActorNumber}");

        NickNameTextUI.text = $"{_photonView.Owner.NickName} _ {_photonView.Owner.ActorNumber}";

        if (_photonView.IsMine)
        {
            NickNameTextUI.color = Color.green;
        }
        else
        {
            NickNameTextUI.color = Color.red;
        }
    }
}
