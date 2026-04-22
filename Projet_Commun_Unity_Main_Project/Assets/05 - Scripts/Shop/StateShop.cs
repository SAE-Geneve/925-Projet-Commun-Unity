using System;
using UnityEngine;
using UnityEngine.Serialization;

public class StateShop :Shop
{
    private enum State
    {
        Speed,
        Dive,
        Strength
    }
    
    [FormerlySerializedAs("_state")]
    [Header("State Parameters")] 
    [SerializeField] private State state;

    private PlayerBonus _playerBonus;
    
    protected override bool Buy(PlayerController playerController)
    {
        _playerBonus.Speed = 0;
        _playerBonus.Dive = 0;
        _playerBonus.Strength = 0;
        _playerBonus.SpeedBuy = 0;
        _playerBonus.DiveBuy = 0;
        _playerBonus.StrengthBuy = 0;
        
        switch (state)
        {
            case State.Speed:
                if (playerController.PlayerBonus.SpeedBuy >= PlayerBonus.MaxBuyForAllShop)
                {
                    return false;
                }
                _playerBonus.Speed = 1;
                _playerBonus.SpeedBuy = 1;
                break;
            case State.Dive:
                if (playerController.PlayerBonus.DiveBuy >= PlayerBonus.MaxBuyForAllShop)
                {
                    return false;
                }
                _playerBonus.Dive = 1;
                _playerBonus.DiveBuy = 1;
                break;
            case State.Strength:
                if (playerController.PlayerBonus.StrengthBuy >= PlayerBonus.MaxBuyForAllShop)
                {
                    return false;
                }
                _playerBonus.Strength = 1;
                _playerBonus.StrengthBuy = 1;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        playerController.PlayerBonus += _playerBonus;
        return true;
    }
}
