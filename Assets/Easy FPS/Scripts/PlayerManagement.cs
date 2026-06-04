using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    public static PlayerManagement Instance;
    // 불발 이벤트가 일어났을 때, 다른 동작을 막기위해 확인하는 조건
    public bool missLocked = false;
    // 불발 이벤트시 활성화하는 텍스트
    public GameObject warningText;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (missLocked)
            warningText.SetActive(true);
        else
            warningText.SetActive(false);
    }
}
