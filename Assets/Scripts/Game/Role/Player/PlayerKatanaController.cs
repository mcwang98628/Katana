using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKatanaController : PlayerController
{
    public bool IsFevering => _playerFever != null && _playerFever.Fevering;
    [SerializeField]
    private Player_Fever _playerFever;

    public override void Init()
    {
        base.Init();
        if (_playerFever != null)
            _playerFever.Init(this);
    }

    public void AddFeverPower(float value)
    {
        _playerFever.AddPower(value);
    }
}
