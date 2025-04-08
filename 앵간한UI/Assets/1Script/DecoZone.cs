using UnityEngine;
using System.Collections;

public class DecoZone : MonoBehaviour
{
    private bool isPlayerInZone = false;  // 플레이어가 존 안에 있는지 여부
    private bool isDecorating = false;    // 장식 진행 여부
    private bool didShowMessage = false;  // 메시지 중복 출력 방지
    private string selectedDecoration = "기본 모양"; // 현재 선택된 장식 모양

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            didShowMessage = false; // 존에 다시 들어오면 메시지 리셋
            Debug.Log("장식 구역에 들어왔습니다. 장식을 시작하세요. (Q: 별, W: 하트, E: 동그라미)(R: 시작)");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            // Debug.Log("장식 구역을 벗어났습니다.");
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

                if (playerMove != null && playerMove.hasDalgona) // 달고나가 있어야 장식 가능
                {
                    // 모양 선택
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        selectedDecoration = "별 모양";
                    }
                    else if (Input.GetKeyDown(KeyCode.W))
                    {
                        selectedDecoration = "하트 모양";
                    }

                    else if (Input.GetKeyDown(KeyCode.E))
                    {
                        selectedDecoration = "동그라미 모양";
                    }

                    // 장식 시작
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        StartCoroutine(StartDecoration(playerMove));
                    }
                }
                else if (!didShowMessage)
                {
                    Debug.Log("달고나가 필요합니다!");
                    didShowMessage = true; // 한 번만 출력
                }
            }
        }
    }

    private IEnumerator StartDecoration(PlayerMove playerMove)
    {
        isDecorating = true;
        Debug.Log($"{selectedDecoration} 장식을 시작합니다...");

        yield return new WaitForSeconds(3f);

        Debug.Log($"장식이 완료되었습니다! {selectedDecoration}으로 달고나를 꾸몄습니다.");

        // 플레이어의 달고나 상태 업데이트
        playerMove.hasDalgona = false;
        if (selectedDecoration == "별 모양")
        {
            playerMove.starDalgona = true;
        }
        else if (selectedDecoration == "하트 모양")
        {
            playerMove.heartDalgona = true;
        }
        else if (selectedDecoration == "동그라미 모양")
        {
            playerMove.circleDalgona = true;
        }
        isDecorating = false;
    }
}