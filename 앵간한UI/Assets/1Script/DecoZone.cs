using UnityEngine;
using System.Collections;

public class DecoZone : MonoBehaviour
{
    private bool isPlayerInZone = false;  // �÷��̾ �� �ȿ� �ִ��� ����
    private bool isDecorating = false;    // ��� ���� ����
    private bool didShowMessage = false;  // �޽��� �ߺ� ��� ����
    private string selectedDecoration = "�⺻ ���"; // ���� ���õ� ��� ���

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            didShowMessage = false; // ���� �ٽ� ������ �޽��� ����
            Debug.Log("��� ������ ���Խ��ϴ�. ����� �����ϼ���. (Q: ��, W: ��Ʈ, E: ���׶��)(R: ����)");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            // Debug.Log("��� ������ ������ϴ�.");
        }
    }

    private void Update()
    {
        if (isPlayerInZone && !isDecorating)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerMove playerMove = player.GetComponent<PlayerMove>();

                if (playerMove != null && playerMove.hasDalgona) // �ް��� �־�� ��� ����
                {
                    // ��� ����
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        selectedDecoration = "�� ���";
                    }
                    else if (Input.GetKeyDown(KeyCode.W))
                    {
                        selectedDecoration = "��Ʈ ���";
                    }

                    else if (Input.GetKeyDown(KeyCode.E))
                    {
                        selectedDecoration = "���׶�� ���";
                    }

                    // ��� ����
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        StartCoroutine(StartDecoration(playerMove));
                    }
                }
                else if (!didShowMessage)
                {
                    Debug.Log("�ް��� �ʿ��մϴ�!");
                    didShowMessage = true; // �� ���� ���
                }
            }
        }
    }

    private IEnumerator StartDecoration(PlayerMove playerMove)
    {
        isDecorating = true;
        Debug.Log($"{selectedDecoration} ����� �����մϴ�...");

        yield return new WaitForSeconds(3f);

        Debug.Log($"����� �Ϸ�Ǿ����ϴ�! {selectedDecoration}���� �ް��� �ٸ���ϴ�.");

        // �÷��̾��� �ް� ���� ������Ʈ
        playerMove.hasDalgona = false;
        if (selectedDecoration == "�� ���")
        {
            playerMove.starDalgona = true;
        }
        else if (selectedDecoration == "��Ʈ ���")
        {
            playerMove.heartDalgona = true;
        }
        else if (selectedDecoration == "���׶�� ���")
        {
            playerMove.circleDalgona = true;
        }
        isDecorating = false;
    }
}