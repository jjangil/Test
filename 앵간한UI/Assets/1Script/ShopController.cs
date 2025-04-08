using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public RectTransform uiGroup;
    PlayerMove enterPlayer;
    public GameObject[] itemObj;
    public Text talkText;

    public void Enter(PlayerMove player)
    {
        enterPlayer = player;
        uiGroup.anchoredPosition = Vector3.zero;
    }

    public void Exit()
    {
        uiGroup.anchoredPosition = Vector3.down * 1000;
    }

    public void Buy(int index)
    {
        if (!enterPlayer.hasDalgona) // 설탕 보유 여부 확인
        {
            StopCoroutine(Talk());
            StartCoroutine(Talk("설탕이 필요해!"));
            return;
        }

        // 설탕 1개 소비
        enterPlayer.hasDalgona = false;

        // 플레이어 달고나 상태 초기화
        enterPlayer.starDalgona = false;
        enterPlayer.heartDalgona = false;
        enterPlayer.circleDalgona = false;

        // 선택한 모양만 true 설정
        switch (index)
        {
            case 0:
                enterPlayer.starDalgona = true;
                StartCoroutine(Talk("달고나모양을 성공적으로 바꿨어!"));
                break;
            case 1:
                enterPlayer.heartDalgona = true;
                StartCoroutine(Talk("달고나모양을 성공적으로 바꿨어!"));
                break;
            case 2:
                enterPlayer.circleDalgona = true;
                StartCoroutine(Talk("달고나모양을 성공적으로 바꿨어!"));
                break;
        }

        // 실제 오브젝트 생성
        Vector3 ranVec = Vector3.right * Random.Range(-3, 3) + Vector3.forward * Random.Range(-3, 3);
    }

    private IEnumerator Talk(string message = "달고나가 부족해!")
    {
        talkText.text = message;
        yield return new WaitForSeconds(2f);
        talkText.text = "";
    }
}
