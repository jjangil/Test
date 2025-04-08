using System.Collections;
using UnityEngine;

public class TrashZone : MonoBehaviour
{
    public GameObject pressTrashCanvas;     // 청소하라할때 알려줄 UI 

    private bool isNearTrash = false; // 플레이어가 쓰레기통 근처에 있는지 확인
    public int trashCount = 0;  // 현재 쓰레기 개수
    public int maxTrash = 5;    // 최대 쓰레기 개수
    PlayerMove player;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerMove>();
        if (player != null)
        {
            StartCoroutine(SpawnTrash());
        }
        UpdateTrashUI();
    }

    // 일정 시간마다 쓰레기 추가
    private IEnumerator SpawnTrash()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(10, 20));

            if (trashCount < maxTrash)
            {
                trashCount++;
                UpdateTrashUI();
                Debug.Log($"쓰레기 개수: {trashCount}");

                // 쓰레기 5개 이상이면 플레이어 이동속도 감소
                if (trashCount == maxTrash)
                {
                    GameManager.Instance.ShowTrashPanel();
                    PressTrashCanvas();
                    Debug.Log("쓰레기 너무 많음! 이동 속도 감소!");
                    player.speed = 1f;  // 이동속도 감소
                }
            }
        }
    }

    // C키를 눌러 쓰레기 청소 (쓰레기통 근처일 때만 가능)
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && isNearTrash)
        {
            CleanTrash();  // C키를 누르면 즉시 청소!
        }
    }

    // 쓰레기 청소 함수 (즉시 실행)
    private void CleanTrash()
    {
        trashCount = 0; // 쓰레기 초기화
        UpdateTrashUI();
        GameManager.Instance.HideTrashPanel();
        GameManager.Instance.ShowCleanTrash();
        player.speed = 2f; // 플레이어 속도 복구
        Debug.Log("Cleaning Complete! 쓰레기 청소 완료!");
    }

    private void UpdateTrashUI()
    {
        GameManager.Instance.SetTrashCount(trashCount);  // GameManager와 싱크 맞추기
    }

    // 플레이어가 쓰레기통 근처에 들어왔을 때
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearTrash = true;
            Debug.Log("쓰레기통 근처에 있음! C키로 청소 가능!");
        }
    }

    // 플레이어가 쓰레기통에서 멀어졌을 때
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearTrash = false;
        }
    }

    public void PressTrashCanvas()
    {
        StartCoroutine(pressTrashCanvasCoroutine());
    }

    private IEnumerator pressTrashCanvasCoroutine()
    {
        pressTrashCanvas.SetActive(true);
        yield return new WaitForSeconds(2);
        pressTrashCanvas.SetActive(false);
        yield return new WaitForSeconds(1);
        pressTrashCanvas.SetActive(true);
        yield return new WaitForSeconds(2);
        pressTrashCanvas.SetActive(false);
    }




}