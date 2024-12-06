using System.Linq;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private float minOffset   = 40f;   // мін. відстань від країв "світу"
    private float minDistance = 50f;  // мін. відстань від попереднього положення
    private Animator animator;
    private Collider[] colliders;
    private AudioSource catchSound;

    private bool isExitTrigger = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        colliders = GetComponents<Collider>();
		catchSound = GetComponent<AudioSource>();

	}

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Character")
        {
            if (colliders[0].bounds.Intersects(other.bounds))
            {
				isExitTrigger = false;
				animator.SetInteger("AnimState", 2);				
				catchSound.Play();
			}
            else
            {
				isExitTrigger = true;
				animator.SetInteger("AnimState", 1);				
            }
        }
    }

	private void OnTriggerExit(Collider other)
	{
		if (other.name == "Character")
		{
            if(isExitTrigger)
            {
				animator.SetInteger("AnimState", 0);
			}			
		}
	}

	public void ReplaceCoin()
    {
        Vector3 newPosition;
        do
        {
            newPosition = this.transform.position + new Vector3(
                Random.Range(-minDistance, minDistance),
                this.transform.position.y,
                Random.Range(-minDistance, minDistance)
            );
        } while (
            Vector3.Distance(newPosition, this.transform.position) < minDistance
            || newPosition.x < minOffset
            || newPosition.z < minOffset
            || newPosition.x > 1000 - minOffset
            || newPosition.z > 1000 - minOffset
        );
        float terrainHeight = Terrain.activeTerrain.SampleHeight(newPosition);
        newPosition.y = terrainHeight + Random.Range(2f, 20f);
        this.transform.position = newPosition;
        GameState.coin++;
		animator.SetInteger("AnimState", 0);
	}
}

/* Д.З. Створити анімацію (кліп) пульсації монети
 * Реалізувати переходи між усіма станами аніматора 
 * (не між кожною парою доцільні переходи).
 * * Впровадити переходи при наближенні персонажа.
 */
