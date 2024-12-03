using UnityEngine;

public class ClockScript : MonoBehaviour
{
    private float gameTime;
    private TMPro.TextMeshProUGUI clock;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameTime = 0.0f;
		clock = GetComponent<TMPro.TextMeshProUGUI>();
	}

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;

	}

	private void LateUpdate()
	{
        int t = (int)gameTime;

        int h = t/3600;
        int m = (t % 3600) / 60;
        int s = t % 60;

        clock.text = $"{h:00}:{m:00}:{s:D2}";
	}
}
