using UnityEditor.Build.Content;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 슬라이더 값이 바뀔 때마다 실행할 메서드 연결
        // onValueChanged : 슬라이더 값이 바뀔때 발생하는 이벤트
        // AddListener() : 이벤트 발생시 괄호 안에 주는 일을 시키도록 등록
        // delegate{} : 원래 함수 이름이 들어가야하지만, delegate를 이용해 익명함수를 만들어 담았다.
    }
}
