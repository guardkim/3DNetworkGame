using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player Player;
    public UI_Canvas Canvas;
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        //Canvas.StaminaBind(Player.Stat);
    }
}

