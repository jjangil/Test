using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    float hAxis;
    float vAxis;
    public int point;

    public bool hasSugar = false;  // 설탕 보유 여부
    public bool hasDalgona = false; // 달고나 보유 여부

    public bool starDalgona = false;        // 별모양 달고나
    public bool heartDalgona = false;        // 하트모양 달고나
    public bool circleDalgona = false;        // 동그라미 모양 달고나 

    public bool isControllable = true; // 이동/입력 가능 여부

    public string currentFood = ""; // 플레이어가 들고 있는 음식
    private GameObject nearCustomer = null; // 가까운 손님

    public SkillController skillController;

    Vector3 moveVec;
    Rigidbody rigid;

    GameObject nearObject;  // 상호작용 가능한 오브젝트
    void Awake()
    {
        skillController = FindFirstObjectByType<SkillController>();
        rigid = GetComponent<Rigidbody>();
        rigid.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY; // Y축 이동 금지
    }

    void FixedUpdate()
    {
        if (isControllable)
        {
            rigid.linearVelocity = moveVec * speed;
        }
        else
        {
            rigid.linearVelocity = Vector3.zero; // 움직임 차단 중일 때 멈추기
        }
    }


    void Update()
    {
        if (!isControllable) return; // 움직임과 입력 차단

        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        transform.position += moveVec * speed * Time.deltaTime; // 이동 

        if (moveVec != Vector3.zero)
        {
            transform.LookAt(transform.position + moveVec); // 회전
        }

        // G키를 눌렀을때 스킬발동
        if (Input.GetKeyDown(KeyCode.G))
        {
            skillController.Skill();
        }

        if (Input.GetKeyDown(KeyCode.F)) // F 키로 음식 전달
        {
            GiveFoodToCustomer();
        }

        if (Input.GetKeyDown(KeyCode.H)) // F 키로 음식 전달
        {
            speed = 10f;
        }

        // 👉 E 키로 상점 상호작용
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (nearObject != null && nearObject.CompareTag("Shop"))
            {
                ShopController shop = nearObject.GetComponent<ShopController>();
                if (shop != null)
                {
                    shop.Enter(this);
                }
            }
        }

    }

    public void SetNearCustomer(GameObject customer)
    {
        nearCustomer = customer;
    }

    private void GiveFoodToCustomer()
    {
        if (nearCustomer == null)
        {
            Debug.Log("근처에 손님이 없습니다!");
            return;
        }

        Custom custom = nearCustomer.GetComponent<Custom>();
        if (custom != null)
        {
            string dalgonaShape = ""; // 플레이어가 들고 있는 달고나 모양 저장용

            if (starDalgona) dalgonaShape = "별 모양";
            else if (heartDalgona) dalgonaShape = "하트 모양";
            else if (circleDalgona) dalgonaShape = "동그라미 모양";

            if (!string.IsNullOrEmpty(dalgonaShape))
            {
                custom.ReceiveDalgona(dalgonaShape);
                // Debug.Log($"손님에게 {dalgonaShape} 달고나를 건네주었습니다!");

                // 달고나 전달 후 초기화
                hasDalgona = false;
                starDalgona = false;
                heartDalgona = false;
                circleDalgona = false;
            }
            else
            {
                Debug.Log("플레이어가 들고 있는 달고나가 없습니다!");
            }
        }
        else
        {
            Debug.LogError("손님에게 Custom 스크립트가 없습니다!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Customer")) // 손님 태그 확인
        {
            nearCustomer = other.gameObject;
            Debug.Log("손님이 근처에 있습니다! F를 눌러 음식을 주세요!");
        }

        if (other.tag == "Shop")
        {
            nearObject = other.gameObject;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Shop")
        {
            nearObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Customer")) // 손님이 범위를 벗어남
        {
            nearCustomer = null;
            Debug.Log("손님이 멀어졌습니다.");
        }
        else if (other.CompareTag("Shop"))
        {
            ShopController shop = nearObject.GetComponent<ShopController>();
            shop.Exit();
            nearObject = null;
        }
    }
}