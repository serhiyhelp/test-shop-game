using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeState : GameState
{
    public TradeState(Merchant merchant)
    {
        Merchant = merchant;
    }

    public override bool MovementAllowed { get => false; }
    
    public Merchant Merchant { get; }
}
