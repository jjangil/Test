using UnityEngine;
using System.Collections;

public class CookingZone : MonoBehaviour
{
    private bool isPlayerInZone = false; // 플레이어가 존 안에 있는지 여부
    private bool isCooking = false;      // 요리 진행 여부

    private DishZone dishZone;

    void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        dishZone = FindFirstObjectByType<DishZone>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 플레이어가 존에 들어오면
        {
            isPlayerInZone = true;
            Debug.Log("요리 구역에 들어왔습니다. E를 눌러 요리를 시작하세요.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // 플레이어가 존에서 나가면
        {
            isPlayerInZone = false;
            // Debug.Log("요리 구역을 벗어났습니다.");
        }
    }

    private void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E) && !isCooking)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerMove playerMove = player.GetComponent<PlayerMove>();
                if (playerMove != null && playerMove.hasSugar) // 설탕이 있을 때만 요리 가능
                {
                    StartCoroutine(StartCooking(playerMove));
                }
                else
                {
                    Debug.Log("설탕이 필요합니다!");
                }
            }
        }
    }

    private IEnumerator StartCooking(PlayerMove playerMove)
    {
        playerMove.isControllable = false;
        isCooking = true;
        Debug.Log("요리를 시작합니다...");
        GameManager.Instance.ShowCookingPanel();
        yield return new WaitForSeconds(3);

        playerMove.hasDalgona = true;
        playerMove.hasSugar = false; // 요리 후 설탕 소모
        Debug.Log("요리가 완성되었습니다! 달고나를 얻었습니다.");
        GameManager.Instance.ShowCookPanel();

        isCooking = false;
        playerMove.isControllable = true;

        // ✅ 달고나 1개 = 더러운 접시 1개 증가
        if (dishZone != null)
        {
            dishZone.AddDirtyDish();
            Debug.Log("🍽️ 더러운 접시가 추가되었습니다!");
        }
        else
        {
            Debug.LogWarning("⚠️ DishZone을 찾을 수 없습니다!");
        }
    }
}