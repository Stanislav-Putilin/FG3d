using UnityEngine;
using UnityEngine.UI;

public class BurstIndicatorScript : MonoBehaviour
{
    private Image image;
    private CharacterScript characterScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		image = GetComponent<Image>();
		characterScript = GameObject.Find("Character").GetComponent<CharacterScript>();

	}

    // Update is called once per frame
    void Update()
    {
        //image.fillAmount = characterScript.burstLevel;
    }
}