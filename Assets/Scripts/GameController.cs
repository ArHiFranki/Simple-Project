using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    [SerializeField] private Menu _menu;
    [SerializeField] private bool _isGamePause;
    [SerializeField] private bool _isGameOver;
    [SerializeField] private bool _isPreparationPhase;
    [SerializeField] private bool _isFightPhase;
    [SerializeField] private int _baseHitPointsMax;
    [SerializeField] private int _gold;

    private int _curruntBaseHitPoints;

    public bool IsGamePause => _isGamePause;
    public bool IsGameOver => _isGameOver;
    public bool IsPreparationPhase => _isPreparationPhase;
    public bool IsFightPhase => _isFightPhase;
    public int BaseHitPoints => _curruntBaseHitPoints;
    public int Gold => _gold;

    public event UnityAction GameOver;
    public event UnityAction<int> GoldChanged;
    public event UnityAction<int, int> BaseHitPointsChanged;

    private void OnEnable()
    {
        _menu.GamePause += SetGamePauseCondition;
    }

    private void OnDisable()
    {
        _menu.GamePause -= SetGamePauseCondition;
    }

    private void Start()
    {
        _isGameOver = false;
        _isPreparationPhase = true;
        _isFightPhase = false;
        _curruntBaseHitPoints = _baseHitPointsMax;
    }

    public void SetGamePauseCondition(bool condition)
    {
        _isGamePause = condition;
    }

    public void ApplyDamage(int damage)
    {
        _curruntBaseHitPoints -= damage;
        BaseHitPointsChanged?.Invoke(_curruntBaseHitPoints, _baseHitPointsMax);

        if (_curruntBaseHitPoints <= 0)
        {
            _isGameOver = true;
            GameOver?.Invoke();
        }
    }

    public void AddHitPoints(int value)
    {
        if (_curruntBaseHitPoints < _baseHitPointsMax)
        {
            _curruntBaseHitPoints += value;

            if (_curruntBaseHitPoints > _baseHitPointsMax)
            {
                _curruntBaseHitPoints = _baseHitPointsMax;
            }
        }
        BaseHitPointsChanged?.Invoke(_curruntBaseHitPoints, _baseHitPointsMax);
    }

    public void ChangeGold(int goldValue)
    {
        _gold += goldValue;
        GoldChanged?.Invoke(_gold);
    }

    public void SetPreparationPhase()
    {
        _isPreparationPhase = true;
        _isFightPhase = false;
    }

    public void SetFightPhase()
    {
        _isPreparationPhase = false;
        _isFightPhase = true;
    }
}
