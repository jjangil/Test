using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 싱글톤 패턴: 게임 내에서 하나의 GameManager만 존재하도록 함
    public static GameManager Instance;

    [Header("UI Elements")] // UI 요소들
    public Text dishText;            // 접시 개수 표시
    public Text trashText;           // 쓰레기 개수 표시
    public Text scoreText;           // 점수 표시
    public Text timeText;            // 남은 시간 표시
    public Text customText;         // 클리어한 손님 몇명인지 

    public Text FirstCustomerTypeText; // 첫 번째 손님 유형
    public Text SecondCustomerTypeText; // 두 번째 손님 유형
    public Text skillCooldownText;   // 스킬 쿨다운 표시
    public GameObject skillPanel;
    public GameObject sugarPanel;
    public GameObject cookPanel;
    public GameObject cookingPanel;
    public GameObject dishPanel;        // 설거지해라 
    public GameObject trashPanel;

    public GameObject cleanDish;        // 접시 청소했을때
    public GameObject cleanTrash;       // 쓰레기 청소했을때 

    public GameObject pointOPanel;       // 성공시키고 포인트를 얻었을떄 
    public GameObject pointXPanel;       // 성공시키고 포인트를 얻었을떄 

    // 플레이어가 재료를 얻었을시, 보여줄 UI 
    public Image getSugar;
    public Image getStar;
    public Image getHeart;
    public Image getCircle;
    public Image getDalgona;

    private List<string> customerQueue = new List<string>(); // 손님 목록 (대기열)

    [Header("Game Data")] // 게임 내 데이터 관리
    private int dishCount = 0;  // 더러운 접시 개수
    private int trashCount = 0; // 쓰레기 개수
    private int score = 0;      // 점수
    private int custom = 0;     // 클리어한 손님수 
    private float gameTime = 120f; // 남은 게임 시간 (초 단위)
    private float skillCooldown = 0f; // 스킬 쿨다운 시간

    private PlayerMove player;

    void LateUpdate()
    {
        // 설탕
        getSugar.color = new Color(1, 1, 1, player.hasSugar ? 1 : 0);
        // 별
        getStar.color = new Color(1, 1, 1, player.starDalgona ? 1 : 0);
        // 하트
        getHeart.color = new Color(1, 1, 1, player.heartDalgona ? 1 : 0);
        // 원형
        getCircle.color = new Color(1, 1, 1, player.circleDalgona ? 1 : 0);
        // 달고나
        getDalgona.color = new Color(1, 1, 1, player.hasDalgona ? 1 : 0);
    }


    private void Awake()
    {
        // 싱글톤 인스턴스 설정 (이미 있으면 삭제)
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();
        UpdateUI(); // 초기 UI 업데이트
        StartCoroutine(GameTimer()); // 게임 타이머 시작
    }

    // 코루틴: 매초 시간이 줄어드는 기능
    private IEnumerator GameTimer()
    {
        while (gameTime > 0)
        {
            yield return new WaitForSeconds(1f);
            gameTime--;
            UpdateUI();
        }

        // 시간 초과되면 게임 정지
        GameOver(); // 게임오버 처리 함수 호출
    }

    // 게임오버
    private void GameOver()
    {
        Time.timeScale = 0f; // 게임 정지
        Debug.Log("게임 오버!"); // 로그 출력

        // 필요하다면 게임 오버 UI도 표시
        // 예: gameOverPanel.SetActive(true);
    }

    // 손님을 추가하는 함수
    public void AddCustomer(string type)
    {
        customerQueue.Add(type); // 손님 목록에 추가
        UpdateCustomerUI(); // UI 갱신
    }

    // 첫 번째 손님 제거
    public void RemoveFirstCustomer()
    {
        if (customerQueue.Count > 0)
        {
            customerQueue.RemoveAt(0); // 첫 번째 손님 삭제
        }
        UpdateCustomerUI(); // UI 갱신
    }

    // 손님 정보를 UI에 반영
    private void UpdateCustomerUI()
    {
        // 손님이 있으면 표시하고, 없으면 빈 문자열로 설정
        FirstCustomerTypeText.text = customerQueue.Count > 0 ? customerQueue[0] : "";
        SecondCustomerTypeText.text = customerQueue.Count > 1 ? customerQueue[1] : "";
    }

    // UI 갱신 함수 (점수, 남은 시간, 스킬 상태, 접시/쓰레기 개수)
    public void UpdateUI()
    {
        dishText.text = $"Dish {dishCount} / 3";
        trashText.text = $"Trash {trashCount} / 5";
        scoreText.text = $"{score} 점";
        customText.text = $"{custom}";
        timeText.text = $"{(int)(gameTime / 60):00} : {(int)(gameTime % 60):00}";
        skillCooldownText.text = skillCooldown > 0 ? $"Skill: {skillCooldown:F1}s" : "Skill Ready!";
    }

    // 쓰레기 개수 설정 함수
    public void SetTrashCount(int count)
    {
        trashCount = count;
        UpdateUI();
    }

    // 설거지 개수 설정 함수
    public void SetDishCount(int count)
    {
        dishCount = count;
        UpdateUI();
    }

    // 클리어한 손님수 
    public void AddClearCustomer()
    {
        custom++;
        UpdateUI();
    }

    // 점수 추가/감소 함수
    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    // 스킬 쿨다운 설정 함수
    public void SetSkillCooldown(float cooldown)
    {
        skillCooldown = cooldown;
        UpdateUI();
    }

    // SkillPanel
    public void ShowSkillPanel()
    {
        StartCoroutine(ShowSkillPanelCoroutine());
    }

    private IEnumerator ShowSkillPanelCoroutine()
    {
        skillPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        skillPanel.SetActive(false);
    }

    // Sugar 
    public void ShowSugarPanel()
    {
        StartCoroutine(ShowSugarPanelFadeCoroutine());
    }

    private IEnumerator ShowSugarPanelFadeCoroutine()
    {
        CanvasGroup canvasGroup = sugarPanel.GetComponent<CanvasGroup>();
        sugarPanel.SetActive(true);
        canvasGroup.alpha = 1f; // 처음엔 완전 보이게

        yield return new WaitForSeconds(0.5f); // 0.5초 동안 보여지기만 함

        float duration = 0.5f; // 페이드 아웃 지속 시간
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = 1f - (elapsed / duration); // 점점 희미하게
            yield return null;
        }

        sugarPanel.SetActive(false);
    }

    // 요리 UI
    public void ShowCookPanel()
    {
        StartCoroutine(ShowCookPanelCoroutine());
    }

    private IEnumerator ShowCookPanelCoroutine()
    {
        cookPanel.SetActive(true);
        yield return new WaitForSeconds(1);
        cookPanel.SetActive(false);
    }

    // 요리중 Ui
    public void ShowCookingPanel()
    {
        StartCoroutine(ShowCookingPanelFadeCoroutine());
    }

    private IEnumerator ShowCookingPanelFadeCoroutine()
    {
        CanvasGroup canvasGroup = cookingPanel.GetComponent<CanvasGroup>();
        cookingPanel.SetActive(true);
        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(2f); // 충분히 보여준 후

        float duration = 1f; // 천천히 사라지게
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = 1f - (elapsed / duration);
            yield return null;
        }

        cookingPanel.SetActive(false);
    }

    // 설거지 요청 UI
    public void ShowDishPanel()
    {
        dishPanel.SetActive(true);  // 설거지 필요하다는 표시
    }

    public void HideDishPanel()
    {
        dishPanel.SetActive(false); // 설거지 끝나면 닫기
    }

    // 쓰레기
    public void ShowTrashPanel()
    {
        trashPanel.SetActive(true);  // 설거지 필요하다는 표시
    }

    public void HideTrashPanel()
    {
        trashPanel.SetActive(false); // 설거지 끝나면 닫기
    }

    // 설거지 완료 UI
    public void ShowCleanDish()
    {
        StartCoroutine(ShowCleanDishCoroutine());
    }

    private IEnumerator ShowCleanDishCoroutine()
    {
        cleanDish.SetActive(true);
        yield return new WaitForSeconds(1);
        cleanDish.SetActive(false);
    }


    // 쓰레기 완료 UI
    public void ShowCleanTrash()
    {
        StartCoroutine(ShowCleanTrashCoroutine());
    }

    private IEnumerator ShowCleanTrashCoroutine()
    {
        cleanTrash.SetActive(true);
        yield return new WaitForSeconds(1);
        cleanTrash.SetActive(false);
    }

    // 포인트 O UI
    public void PointOPanel()
    {
        StartCoroutine(PointOCoroutine());
    }

    private IEnumerator PointOCoroutine()
    {
        pointOPanel.SetActive(true);
        yield return new WaitForSeconds(1);
        pointOPanel.SetActive(false);
    }

    // 포인트 X UI
    public void PointXPanel()
    {
        StartCoroutine(PointXCoroutine());
    }

    private IEnumerator PointXCoroutine()
    {
        pointXPanel.SetActive(true);
        yield return new WaitForSeconds(1);
        pointXPanel.SetActive(false);
    }

}