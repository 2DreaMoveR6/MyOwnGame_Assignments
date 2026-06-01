using UnityEngine;
using UnityEngine.UI;

public class SlideControl : MonoBehaviour
{
    // 유니티 [Header]은 인스펙터 창에서 변수들을 그룹화하고 굵은 글씨로
    // 제목을 달아주는 속성
    // [Header("헤더명")] 형태로 사용한다.
    [Header("UI Elements")]
    public Slider xSlider;
    public Slider ySlider;

    [Header("Modifying Error Range")]
    // 오차값이 존재하는 범위에 더하거나 빼서 오차 범위를 수정하는 변수
    public float modiXError;
    public float modiYError;

    // 오차 값을 수정하기 위해 Gun 스크립트 랜덤 오차함수를 직접 건드려서 값을 수정
    GunScript gunScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gunScript = FindObjectOfType<GunScript>();

        // 슬라이더 값이 바뀔 때마다 실행할 메서드 연결
        // onValueChanged : 슬라이더 값이 바뀔때 발생하는 이벤트
        // AddListener() : 이벤트 발생시 괄호 안에 주는 일을 시키도록 등록
        // delegate{} : 원래 함수 이름이 들어가야하지만, delegate를 이용해 익명함수를 만들어 담았다.
        xSlider.onValueChanged.AddListener(delegate { UpdateXError(); });
        ySlider.onValueChanged.AddListener (delegate { UpdateYError(); });
    }

    public void UpdateYError()
    {
        // X축 기반 회전은 위아래로 움직이고
        // Y축 기반 회전은 좌우로 움직이기 때문에 
        gunScript.LeftRightError += 1;
    }

    public void UpdateXError()
    {
        gunScript.UpDownError += 1;
    }
}
