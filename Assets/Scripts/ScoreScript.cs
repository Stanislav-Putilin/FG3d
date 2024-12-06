using UnityEngine;

public class ScoreScript : MonoBehaviour
{
	private TMPro.TextMeshProUGUI score;
	void Start()
    {
		GameState.AddListener(nameof(GameState.coin), OnCoinChanged);
		score = GetComponent<TMPro.TextMeshProUGUI>();
		score.text = GameState.coin.ToString();
	}	

	private void OnCoinChanged(string ignored)
	{
		score.text = GameState.coin.ToString();
	}
	private void OnDestroy()
	{
		GameState.RemoveListener(nameof(GameState.coin), OnCoinChanged);
	}
}