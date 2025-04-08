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
        if (!enterPlayer.hasDalgona) // ���� ���� ���� Ȯ��
        {
            StopCoroutine(Talk());
            StartCoroutine(Talk("������ �ʿ���!"));
            return;
        }

        // ���� 1�� �Һ�
        enterPlayer.hasDalgona = false;

        // �÷��̾� �ް� ���� �ʱ�ȭ
        enterPlayer.starDalgona = false;
        enterPlayer.heartDalgona = false;
        enterPlayer.circleDalgona = false;

        // ������ ��縸 true ����
        switch (index)
        {
            case 0:
                enterPlayer.starDalgona = true;
                StartCoroutine(Talk("�ް������ ���������� �ٲ��!"));
                break;
            case 1:
                enterPlayer.heartDalgona = true;
                StartCoroutine(Talk("�ް������ ���������� �ٲ��!"));
                break;
            case 2:
                enterPlayer.circleDalgona = true;
                StartCoroutine(Talk("�ް������ ���������� �ٲ��!"));
                break;
        }

        // ���� ������Ʈ ����
        Vector3 ranVec = Vector3.right * Random.Range(-3, 3) + Vector3.forward * Random.Range(-3, 3);
    }

    private IEnumerator Talk(string message = "�ް��� ������!")
    {
        talkText.text = message;
        yield return new WaitForSeconds(2f);
        talkText.text = "";
    }
}
