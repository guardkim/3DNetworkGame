using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : PlayerAbility, IPunObservable
{
    public Slider HealthSlider;
    private bool _isDead = false;
    private float _reviveTimer = 5.0f;
    private float _receivedValue = 1f;

    private void Start()
    {
        Refresh();
    }
    private void Refresh()
    {
        HealthSlider.value = _owner.Stat.Health / _owner.Stat.MaxHealth;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_owner.Stat.Health /  _owner.Stat.MaxHealth);
        }
        else if (stream.IsReading)
        {
            float value = (float)stream.ReceiveNext();
            _receivedValue = value;
        }
    }
    private void Update()
    {
        Refresh();
        if (_isDead)
        {
            _reviveTimer -= Time.deltaTime;
        }
        if (_reviveTimer <= 0)
        {
            _reviveTimer = 5.0f;
            _photonView.RPC(nameof(Revive), RpcTarget.All);
        }
    }

    [PunRPC]
    public void Die()
    {
        if (_photonView.IsMine == false) return;
        _photonView.RPC(nameof(DieAnimation), RpcTarget.All);
        _isDead = true;
    }
    [PunRPC]
    private void DieAnimation()
    {
        _animator.SetTrigger("Die");
        _animator.SetBool("IsDead", true);
    }
    [PunRPC]
    public void Revive()
    {
        _isDead = false;
        _animator.SetBool("IsDead", false);
        _owner.Stat.Init();
        transform.position = SpawnPoints.Instance.GetRandomSpawnPoint();
        Refresh();
    }

}
