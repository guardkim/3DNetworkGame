using Photon.Pun;
/// <summary>Defines the OnPhotonSerializeView method to make it easy to implement correctly for observable scripts.</summary>
/// \ingroup callbacks
public interface IPunObservable
{
    /// <summary>
    /// Called by PUN several times per second, so that your script can write and read synchronization data for the PhotonView.
    /// </summary>
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);
}
