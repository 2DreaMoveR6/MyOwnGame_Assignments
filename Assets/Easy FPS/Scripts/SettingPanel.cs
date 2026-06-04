using UnityEngine;


public class SettingPanel : MonoBehaviour
{
    public GameObject settingPanel;
    private bool isPanelOpen = false;

    private MouseLookScript mouseLookScript;

    private void Start()
    {
        ToggleSettings();
        mouseLookScript = FindObjectOfType<MouseLookScript>();
    }

    private void Update()
    {
        // 설정창이 닫혀있는데,
        // ESC도 누르지 않았다면 항상 마우스를 중앙으로 고정시키는 부분
        // 설정창을 이용하려면 반드시 ESC 이용해야한다.
        if (settingPanel.activeSelf == false && mouseLookScript.isPressESC == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // ESC 눌리면 마우스 고정 해제
        if (mouseLookScript.isPressESC == true)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            mouseLookScript.isPressESC = false;
        }
    }

    // 패널의 상태를 반전시키는 메서드
    public void ToggleSettings()
    {
        // 현재 패널 상태를 Not 시키고, 
        // 해당 bool 값으로 패널의 활성화 여부 결정
        isPanelOpen = !isPanelOpen;
        settingPanel.SetActive(isPanelOpen);

        if(isPanelOpen)
        {
            // 설정창이 열려있다면?
            // 마우스 중앙 고정 해제
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // 게임 일시 정지
            //Time.timeScale = 0f;
        }
        else
        {
            // 설정창이 닫혀있다면?
            // 마우스 중앙 고정
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // 게임 다시 시작
            //Time.timeScale = 1f;
        }

        // timescale 값이 1이면 기본 값 (정상적으로 흐르는 상태)
        // 0 이면 완전히 흐름이 멈추는 일시정지 상태
        // ex) Update 메서드는 하드웨어 사양따라 호출되므로 계속 실행되지만
        // time.deltatime을 곱해 이동을 구현한 부분이 있다면 멈춘 것처럼 보인다.
        // 변위가 0이 되기 때문
    }
}
