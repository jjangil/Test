using UnityEngine;

public class SkillController : MonoBehaviour
{
    private float coolTime = 20f;               // 쿨타임 (초)
    private float lastUsedTime = -20f;          // 마지막 사용 시간

    private TrashZone trash;
    private DishZone dish;
    private PlayerMove player;

    private bool isCoolingDown = false;        // 쿨타임 여부 체크

    void Start()
    {
        // Trash 및 DishZone 매니저를 찾아 저장
        trash = FindFirstObjectByType<TrashZone>();
        dish = FindFirstObjectByType<DishZone>();
        player = FindFirstObjectByType<PlayerMove>();
    }

    void Update()
    {
        // 스킬이 사용된 후 쿨타임이 남아있다면 UI 업데이트
        if (isCoolingDown)
        {
            float remain = Mathf.Max(0, coolTime - (Time.time - lastUsedTime));
            GameManager.Instance.SetSkillCooldown(remain);

            // 쿨타임이 끝났다면 다시 사용 가능하도록 설정
            if (remain <= 0)
            {
                isCoolingDown = false;
            }
        }
    }

    public void Skill()
    {
        // 쓰레기와 더러운 접시가 모두 없으면 스킬 사용 불가
        if (trash.trashCount == 0 && dish.dirtyDish == 0)
        {
            Debug.Log("처리할 방해 요소 없음! 스킬을 사용할 수 없습니다.");
            return;
        }

        if (!isCoolingDown && trash.trashCount >= 1 || dish.dirtyDish >= 1)
        {
            ActiveSkill();            // 스킬 실행
            lastUsedTime = Time.time; // 시간 기록
            isCoolingDown = true;     // 쿨타임 시작

            GameManager.Instance.SetSkillCooldown(coolTime); // UI에 쿨타임 반영
            GameManager.Instance.ShowSkillPanel();
        }
        else
        {
            float remain = Mathf.Max(0, coolTime - (Time.time - lastUsedTime));
            Debug.Log($"[스킬] 쿨타임 중입니다. 남은 시간: {remain:F2}초");
        }
    }

    private void ActiveSkill()
    {
        // 1. 쓰레기 초기화 (trashCount = 0)
        if (trash != null)
        {
            trash.trashCount = 0;
            Debug.Log("🧹 스킬 발동! 쓰레기 전부 제거됨");
            player.speed = 2f;
            GameManager.Instance.HideDishPanel();  // ✅ UI 닫기
            // GameManager를 통해 UI 업데이트
            GameManager.Instance.SetTrashCount(0);
        }

        // 2. 설거지 자동 완료 처리
        if (dish != null && dish.dirtyDish > 0)
        {
            dish.dirtyDish = 0;
            Debug.Log("설거지 자동 완료!");
            GameManager.Instance.HideTrashPanel();  // UI 닫기
            // GameManager를 통해 UI 업데이트
            GameManager.Instance.SetDishCount(0);
        }
    }
}