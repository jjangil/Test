using UnityEngine;

public class SugarZone : MonoBehaviour
{
    private bool isPlayerInZone = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            Debug.Log("���� ������ ���Խ��ϴ�. E�� ���� ������ ����������.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            // Debug.Log("���� ������ ������ϴ�.");
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
                    Debug.Log("������ ȹ���߽��ϴ�!");
                    GameManager.Instance.ShowSugarPanel();
                }
            }
        }
    }
}
