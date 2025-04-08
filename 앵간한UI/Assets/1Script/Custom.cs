using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Custom : MonoBehaviour
{
    public Transform targetPosition; // 손님이 도착해야 할 목표 위치
    public Transform targetEndPosition;  // 손님이 떠날 때 이동할 위치
    public float moveSpeed = 5f;     // 손님의 이동 속도
    // public float waitTime = 5f;      // 도착 후 기다리는 시간
    private bool isWaiting = false;  // 현재 기다리는 상태인지 여부
    private bool hasReceivedDalgona = false; // 달고나를 받은 상태인지 여부

    public string desiredShape;     // 손님이 원하는 달고나 모양

    // 손님이 떠날 때 호출되는 델리게이트(이벤트 핸들러)
    public delegate void CustomerLeaveAction();
    public event CustomerLeaveAction OnCustomerLeave;

    private PlayerMove player; // PlayerMove 스크립트 참조 (점수 추가에 필요)

    public Text customMal;          // 커스텀 말(동그라미, 하트, 별)
    public Image customShape;       // 커스텀 말풍선(이건 고정)

    private void Start()
    {
        // 태그가 "Player"인 오브젝트에서 PlayerMove 컴포넌트 가져오기
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();

        // 손님이 도달할 위치를 "CustomerTarget" 오브젝트에서 찾기
        GameObject targetObject = GameObject.Find("CustomerTarget");
        GameObject endObject = GameObject.Find("CustomerEndTarget");

        // 말풍선 UI 컴포넌트 찾기
        Transform canvas = transform.Find("GameObject/Canvas");
        if (canvas != null)
        {
            customMal = canvas.Find("Custom Text")?.GetComponent<Text>();
            customShape = canvas.Find("Custom Image")?.GetComponent<Image>();
        }

        // 위치 정보가 있다면 targetPosition에 할당
        if (targetObject != null) targetPosition = targetObject.transform;
        if (endObject != null) targetEndPosition = endObject.transform;

        // 달고나 모양 중 하나를 랜덤으로 선택
        string[] possibleShapes = { "별 모양", "하트 모양", "동그라미 모양" };
        desiredShape = possibleShapes[Random.Range(0, possibleShapes.Length)];
        // 말풍선 관련 컴포넌트 찾기
        customMal = GetComponentInChildren<Text>();
        customShape = GetComponentInChildren<Image>();

        // 말풍선 활성화 및 텍스트 설정
        if (customMal != null)
        {
            customMal.text = desiredShape;
            customMal.gameObject.SetActive(true);
        }
        if (customShape != null)
        {
            customShape.gameObject.SetActive(true);
        }

        // GameManager에 손님이 원하는 모양 등록
        GameManager.Instance.AddCustomer(desiredShape);
    }

    private void Update()
    {
        // 기다리는 중이 아니고 달고나도 받지 않았다면 이동
        if (!isWaiting && !hasReceivedDalgona)
        {
            MoveToTarget();
        }
    }

    // 손님이 목표 위치로 이동하는 함수
    private void MoveToTarget()
    {
        // 현재 위치에서 목표 위치로 일정 속도로 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

        // 이동 방향 계산 및 회전 처리
        Vector3 direction = targetPosition.position - transform.position;
        if (direction != Vector3.zero)
        {
            // 회전 방향 설정
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * moveSpeed);
        }

        // 목표 위치에 도달하면 대기 코루틴 시작
        if (Vector3.Distance(transform.position, targetPosition.position) < 0.1f)
        {
            isWaiting = true;
            StartCoroutine(WaitAtPosition());
        }
    }

    // 도착한 후 대기하는 코루틴
    private IEnumerator WaitAtPosition()
    {
        isWaiting = true; // 대기 상태로 설정
        yield return new WaitForSeconds(10); // 지정된 시간 동안 대기

        if (!hasReceivedDalgona) // 아직 달고나를 못 받았다면
        {
            StartCoroutine(LeaveCoroutine()); // 그냥 떠남
            GameManager.Instance.RemoveFirstCustomer();     // 지혼자 떠났을떄도 
        }
    }

    // 플레이어가 달고나를 주었을 때 호출되는 함수
    public void ReceiveDalgona(string dalgonaShape)
    {
        if (hasReceivedDalgona) return; // 이미 받았다면 무시

        hasReceivedDalgona = true; // 받은 것으로 표시

        if (dalgonaShape == desiredShape)
        {
            // 모양이 맞으면 점수 추가 및 메시지 출력
            Debug.Log($"손님이 '{dalgonaShape}' 달고나를 받아 기뻐하며 떠납니다!");
            GameManager.Instance.RemoveFirstCustomer();
            player.point += 1000;
            GameManager.Instance.PointOPanel();
            GameManager.Instance.AddScore(1000);
            GameManager.Instance.AddClearCustomer();
        }
        else
        {
            // 틀린 모양을 주었을 때
            GameManager.Instance.PointXPanel();
            Debug.Log("딴거를 주면 어떡해ㅠㅠ");
        }

        StartCoroutine(LeaveCoroutine()); // 어떤 경우든 손님은 떠남
    }

    // 손님이 떠날 때 호출되는 함수
    private IEnumerator LeaveCoroutine()
    {
        while (Vector3.Distance(transform.position, targetEndPosition.position) > 0.3f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetEndPosition.position, moveSpeed * Time.deltaTime);

            Vector3 direction = targetEndPosition.position - transform.position;
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * moveSpeed);
            }
            yield return null;
        }
        OnCustomerLeave?.Invoke();  // 게속나오게하는거 
        // 목적지 도착 후 손님 제거
        Destroy(gameObject);
    }
}
