using System.Linq;
using UnityEngine;

public class DeyNightScript : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip daySound;

	[SerializeField]
	private AudioClip nightSound;

	private Light[] dayLights;
    private Light[] nightLights;

    private bool isDay;

    [SerializeField]
    private Material daySkybox;

	[SerializeField]
    private Material nightSkybox;

	void Start()
    {
		audioSource = GetComponent<AudioSource>();
		dayLights = GameObject.FindGameObjectsWithTag("DayLight").Select(x=> x.GetComponent<Light>()).ToArray();
		nightLights = GameObject.FindGameObjectsWithTag("NightLight").Select(x => x.GetComponent<Light>()).ToArray();
        isDay = true;
        SwitchDayNight(isDay);


		

	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.N)) 
        { 
            SwitchDayNight(!isDay);
        }
    }

    private void SwitchDayNight(bool isDay)
    {
        this.isDay = isDay;

        foreach (var day in dayLights) 
        { 
            day.enabled = isDay;
        }

        foreach (var day in nightLights)
        {
            day.enabled = !isDay;
        }

        RenderSettings.skybox = isDay ? daySkybox : nightSkybox;
        RenderSettings.ambientIntensity = isDay ? 1.0f : 0.3f;

		audioSource.clip = isDay ? daySound : nightSound;
		audioSource.Play();
	}
}
