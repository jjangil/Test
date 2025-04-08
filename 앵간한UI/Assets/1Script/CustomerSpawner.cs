using UnityEngine;
using System.Collections;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject[] customerPrefabs; // 손님 프리팹
    public Transform spawnPoint; // 손님이 생성될 위
    public float spawnInterval = 1f; // 손님 생성 간격
    public int maxCustomers = 7; // 최대 손님 수
    private int currentCustomerCount = 0; // 현재 손님 수

    private void Start()
    {
        StartCoroutine(SpawnCustomers());
    }

    private IEnumerator SpawnCustomers()
    {
        while (true)
        {
            if (currentCustomerCount < maxCustomers)
            {
                // 랜덤 프리팹 선택
                int randIndex = Random.Range(0, customerPrefabs.Length);
                GameObject selectedPrefab = customerPrefabs[randIndex];

                GameObject newCustomer = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);
                currentCustomerCount++;
                newCustomer.GetComponent<Custom>().OnCustomerLeave += DecreaseCustomerCount;
                yield return new WaitForSeconds(0.5f);
            }
            yield return null;
        }
    }


    // 손님이 떠날 때 호출하여 손님 수를 감소시킴
    public void DecreaseCustomerCount()
    {
        currentCustomerCount--;
    }
}
