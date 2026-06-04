using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    // 스태미너 구현에 사용할 슬라이더 변수
    public Slider staminaSlider;

    // 스태미너 초기 값
    public float maxStamina = 100f;
    public float currentStamina = 100f;
    // 숨을 참을 경우 초당 소모되는 스태미너
    public float consumeStamina = 20f;
    // 숨을 참지 않을 경우 초당 회복되는 스태미너
    public float recoverStamina = 10f;

    // 스태미너 부족해짐에 따라 심해져가는 흔들림을 구현하기 위해 기존 코드 활용
    MouseLookScript mouseLookScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mouseLookScript = FindObjectOfType<MouseLookScript>();

        // 슬라이더 초기화
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = currentStamina;
    }

    // 슬라이드 타이머 변수
    float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        // 불발 이벤트에는 아무 동작도 못하도록 막는 부분
        // 메서드이므로 return 이용
        if (PlayerManagement.Instance.missLocked)
            return;

        // 1초마다 슬라이드를 업데이트
        // 그 아래로 적용하면, 너무 빨리 닳아없어진다.
        if (timer > 1.0f)
        {
            HandleBreath();
            UpdateSlider();

            timer = 0f;
        }

        // 스태미너 정도에 따라 속도 조절
        // 2단계로 구분 (20% 이하일 때와 10% 이햐일 때로)
        if (currentStamina <= 20.0f)
        {
            // 10% 이하일 때
            if (currentStamina <= 10.0f)
            {
                mouseLookScript.isStamina = false;
                mouseLookScript.shakingSpeed = 6.0f;
            }
            // 10% 초과지만, 20% 이하일 때
            else
            {
                mouseLookScript.isStamina = false;
                mouseLookScript.shakingSpeed = 4.0f;
            }
        }
        else
        {
            mouseLookScript.isStamina = true;
            mouseLookScript.shakingSpeed = 2.0f;
        }

        timer += Time.deltaTime;
    }

    // 숨을 멈추거나 멈추지 않았을 때, 스태미너 조절
    private void HandleBreath()
    {
        if (Input.GetKey(KeyCode.B))
            currentStamina -= consumeStamina;
        else
            currentStamina += recoverStamina;

        // 스태미너의 최소와 최대값을 제한하는 부분
        // Mathf.Clamp(float value, float min, float max) 
        // value는 제한하려는 값, min은 허용할 최솟값, max는 허용할 최댓값
        // 값이 최솟값보다 작으면 최솟값을 최댓값보다 크면 최댓값을 반환한다.
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }

    // 슬라이더 UI 업데이트 
    void UpdateSlider()
    {
        staminaSlider.value = currentStamina;
    }

}
