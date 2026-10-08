using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class GameHud : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _finishScoreText;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private Button _restartButton;
    [SerializeField] private GameObject _restartWindow;

    private EntityManager _entityManager;
    private EntityQuery _scoreQuery;
    private EntityQuery _timerQuery;
    private EntityQuery _gameStateQuery;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _scoreQuery = _entityManager.CreateEntityQuery(ComponentType.ReadOnly<Score>());
        _timerQuery = _entityManager.CreateEntityQuery(ComponentType.ReadOnly<GameTimer>());
        _gameStateQuery = _entityManager.CreateEntityQuery(ComponentType.ReadWrite<GameState>());

        _restartButton.onClick.AddListener(RestartGame);
    }

    // Update is called once per frame
    void Update()
    {
        if(_scoreQuery.CalculateEntityCount() != 1 || _timerQuery.CalculateEntityCount() != 1) return;
        
        _scoreText.text = $"Score: {_scoreQuery.GetSingleton<Score>().Value}";
        _timerText.text = $"Timer: {_timerQuery.GetSingleton<GameTimer>().Value:F0}";

        _scoreQuery.CompleteDependency();
        _timerQuery.CompleteDependency();

        if (_gameStateQuery.GetSingleton<GameState>().IsOver)
        {
            _restartWindow.SetActive(true);
            _finishScoreText.text = $"Score: {_scoreQuery.GetSingleton<Score>().Value}";
            return;
        }
        else
        {
            _restartWindow.SetActive(false);
        }
    }

    private void RestartGame()
    {
        if(_gameStateQuery.CalculateEntityCount() != 1) return;

        _gameStateQuery.GetSingletonRW<GameState>().ValueRW.RestartRequest = true;

        _gameStateQuery.CompleteDependency();
    }
}
