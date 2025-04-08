using UnityEngine;

public class SugarZone : MonoBehaviour
{
    private bool isPlayerInZone = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            Debug.Log("설탕 구역에 들어왔습니다. E를 눌러 설탕을 가져가세요.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            // Debug.Log("설탕 구역을 벗어났습니다.");
        }
    }

    private void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerMove playerMove = player.GetComponent<PlayerMove>();
                if (playerMove != null)
                {
                    playerMove.hasSugar = true;
                    Debug.Log("설탕을 획득했습니다!");
                    GameManager.Instance.ShowSugarPanel();
                }
            }
        }
    }
}
