using UnityEngine;
using System.Collections;

public class DishZone : MonoBehaviour
{
    public GameObject pressCanvas;      // 설거지를 

    public bool isNearDishZone = false; // 플레이어가 설거지존에 있는지 확인하는 변수
    public int dirtyDish = 0;
    public int maxDish = 5;
    PlayerMove player;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerMove>();
        // 점수 감소 코루틴 시작 (항상 실행되도록)
        if (player != null)
        {
            StartCoroutine(ReducePointsOverTime());
        }
        UpdateDishUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearDishZone = true;
            Debug.Log("설거지 가능! C키를 눌러 설거지하세요.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearDishZone = false;
            Debug.Log("설거지존에서 벗어났습니다!");
        }
    }

    private void Update()
    {
        if (isNearDishZone && Input.GetKeyDown(KeyCode.C) && player != null)
        {
            WashDishes();
        }
    }

    private void WashDishes()
    {
        dirtyDish = 0;
        UpdateDishUI();  // UI 갱신
        GameManager.Instance.HideDishPanel();  // UI 닫기
        GameManager.Instance.ShowCleanDish(); 
        Debug.Log("설거지 완료! 설거지거리가 초기화됩니다.");
    }

    public void AddDirtyDish()
    {
        if (dirtyDish < maxDish)
        {
            dirtyDish++;
            UpdateDishUI();
            Debug.Log($"더러운 접시 개수: {dirtyDish}");

            if (dirtyDish == maxDish)
            {
                GameManager.Instance.ShowDishPanel();  // UI 띄우기
                PressCanvas();
                Debug.Log("설거지를 하지 않으면 점수가 감소합니다!");
            }
        }
    }

    private IEnumerator ReducePointsOverTime()
    {
        while (true)  // 항상 실행되도록 변경
        {
            if (dirtyDish >= maxDish && player != null && player.point > 0)
            {
                player.point -= 50; // 초당 50점 감소
                GameManager.Instance.AddScore(-50);  // ✅ UI 즉시 업데이트

                Debug.Log($"점수 감소! 현재 점수: {player.point}");
            }
            yield return new WaitForSeconds(1f);  // 1초마다 실행
        }
    }

    private void UpdateDishUI()
    {
        GameManager.Instance.SetDishCount(dirtyDish);  // GameManager와 싱크 맞추기
    }

    // --------
    public void PressCanvas()
    {
        StartCoroutine(pressCanvasCoroutine());
    }

    private IEnumerator pressCanvasCoroutine()
    {
        pressCanvas.SetActive(true);
        yield return new WaitForSeconds(2);
        pressCanvas.SetActive(false);
        yield return new WaitForSeconds(1);
        pressCanvas.SetActive(true);
        yield return new WaitForSeconds(2);
        pressCanvas.SetActive(false);
    }

}