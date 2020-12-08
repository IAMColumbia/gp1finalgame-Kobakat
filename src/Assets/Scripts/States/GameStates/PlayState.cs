using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayState : GameState
{
    Player player;
    public PlayState(Player Player)
    {
        this.player = Player;
    }
}
