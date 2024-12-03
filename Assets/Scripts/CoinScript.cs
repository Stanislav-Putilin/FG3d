using System.Linq;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private float minOffset   = 40f;   // мін. відстань від країв "світу"
    private float minDistance = 50f;  // мін. відстань від попереднього положення
    private Animator animator;
    private Collider[] colliders;

    void Start()
    {
        animator = GetComponent<Animator>();
        colliders = GetComponents<Collider>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Character")
        {
            ReplaceCoin();

			//if (colliders[0].bounds.Intersects(other.bounds))
   //         {
   //             animator.SetBool("IsCollected", true);
   //         }
   //         else
   //         {
   //             Debug.Log("Closer");
   //         }
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
        //animator.SetBool("IsCollected", false);
    }
}
/* Д.З. Створити анімацію (кліп) пульсації монети
 * Реалізувати переходи між усіма станами аніматора 
 * (не між кожною парою доцільні переходи).
 * * Впровадити переходи при наближенні персонажа.
 */
