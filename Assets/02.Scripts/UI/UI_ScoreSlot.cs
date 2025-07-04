using TMPro;
using UnityEngine;

public class UI_ScoreSlot : MonoBehaviour
{
    public TextMeshProUGUI RankText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI NicknameText;

    public void Set(string rank, string nickname, int score)
    {
        RankText.text = rank;
        NicknameText.text = nickname;
        ScoreText.text = score.ToString("N0");
    }

}
