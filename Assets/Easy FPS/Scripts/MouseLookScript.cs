using UnityEngine;
using System.Collections;


public class MouseLookScript : MonoBehaviour
{
    [HideInInspector]
    public Transform myCamera;

    // 에임 흔들림(숨 참는 기능 이용하지 않을 때)을 담당하는 변수들
    [Header("The Variables of Shaking")]
    // 흔들림 속도 
    public float shakingSpeed = 2.0f;
    // 좌우 흔들림 폭 (각도 이용)
    float shakingX = 0.15f;
    // 위 아래 흔들림 폭 (각도 이용)
    float shakingY = 1.5f;

    // 펄린 노이즈 전용 타이머
    private float breathTimer = 0f;
    /*
	 * Hiding the cursor.
	 */

    // ESC 눌림 여부를 확인하는 변수
    public bool isPressESC = false;
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        myCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    /*
	* Locking the mouse if pressing L.
	* Triggering the headbob camera omvement if player is faster than 1 of speed
	*/
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPressESC = true;
        }

        if (PlayerManagement.Instance.missLocked)
            return;

        MouseInputMovement();
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

        if (GetComponent<PlayerMovementScript>().currentSpeed > 1)
            HeadMovement();
    }

    [Header("Z Rotation Camera")]
    [HideInInspector] public float timer;
    [HideInInspector] public int int_timer;
    [HideInInspector] public float zRotation;
    [HideInInspector] public float wantedZ;
    [HideInInspector] public float timeSpeed = 2;

    [HideInInspector] public float timerToRotateZ;
    /*
	* Switching Z rotation and applying to camera in camera Rotation().
	*/
    void HeadMovement()
    {
        timer += timeSpeed * Time.deltaTime;
        int_timer = Mathf.RoundToInt(timer);
        if (int_timer % 2 == 0)
        {
            wantedZ = -1;
        }
        else
        {
            wantedZ = 1;
        }

        zRotation = Mathf.Lerp(zRotation, wantedZ, Time.deltaTime * timerToRotateZ);
    }
    [Tooltip("Current mouse sensivity, changes in the weapon properties")]
    public float mouseSensitvity = 0;
    [HideInInspector]
    public float mouseSensitvity_notAiming = 300;
    [HideInInspector]
    public float mouseSensitvity_aiming = 50;

    /*
    * FixedUpdate()
    * If aiming set the mouse sensitvity from our variables and vice versa.
    */
    void FixedUpdate()
    {

        /*
         * Reduxing mouse sensitvity if we are aiming.
         */
        if (Input.GetAxis("Fire2") != 0)
        {
            mouseSensitvity = mouseSensitvity_aiming;
        }
        else if (GetComponent<PlayerMovementScript>().maxSpeed > 5)
        {
            mouseSensitvity = mouseSensitvity_notAiming;
        }
        else
        {
            mouseSensitvity = mouseSensitvity_notAiming;
        }

        ApplyingStuff();
    }


    private float rotationYVelocity, cameraXVelocity;
    [Tooltip("Speed that determines how much camera rotation will lag behind mouse movement.")]
    public float yRotationSpeed, xCameraSpeed;

    [HideInInspector]
    public float wantedYRotation;
    [HideInInspector]
    public float currentYRotation;

    [HideInInspector]
    public float wantedCameraXRotation;
    [HideInInspector]
    public float currentCameraXRotation;

    [Tooltip("Top camera angle.")]
    public float topAngleView = 60;
    [Tooltip("Minimum camera angle.")]
    public float bottomAngleView = -45;
    /*
     * Upon mouse movenet it increases/decreased wanted value. (not actually moving yet)
     * Clamping the camera rotation X to top and bottom angles.
     */
    void MouseInputMovement()
    {

        wantedYRotation += Input.GetAxis("Mouse X") * mouseSensitvity;

        wantedCameraXRotation -= Input.GetAxis("Mouse Y") * mouseSensitvity;

        wantedCameraXRotation = Mathf.Clamp(wantedCameraXRotation, bottomAngleView, topAngleView);

    }

    // 스태미터 정도에 따라 흔들림 여부를 결정한다.
    // 스태미너를 이용한 흔들림 여부 변수
    public bool isStamina = true;
    /*
     * Smoothing the wanted movement.
     * Calling the waeponRotation form here, we are rotating the waepon from this script.
     * Applying the camera wanted rotation to its transform.
     */
    void ApplyingStuff()
    {

        currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, yRotationSpeed);
        currentCameraXRotation = Mathf.SmoothDamp(currentCameraXRotation, wantedCameraXRotation, ref cameraXVelocity, xCameraSpeed);

        // 흔들림 제어하는 부분
        // 순수 에임 좌표에서 미세한 움직임 값을 추가해 화면을 회전시키는 offset 더하기 방식을 이용

        // 펄린 노이즈 : 컴퓨터에서 자연스러운 랜덤 효과를 내기위해 사용하는 부드러운 연속 확률 함수
        // 매 프레임마다 독립적인 무작위값을 가져오는 일반 랜덤함수와 달리 수학적으로 부드러운 곡선형태의 랜덤 값을 반환하는 펄린 노이즈
        // Mathf.PerlinNoise(x, y)는 0과 1사이 값을 반환
        float swayX = 0f;
        float swayY = 0f;

        // 흔들리는 조건
        // 1. B 키를 누르지 않았을 때
        // 1 - 1. 스태미너가 특정 범위 아래로 떨어지지 않았을 때
        // 1 - 2. 스태미너가 특정 범위 아래로 떨어졌을 때

        // 2. B 키를 눌렀을 때
        // 2 - 1. 스태미너가 특정 범위 아래로 떨어졌을 때

        // 흔들리는 조건에 해당할 때 (경우의 수를 모두 나열)
        if (!Input.GetKey(KeyCode.B) && isStamina == true || !Input.GetKey(KeyCode.B) && isStamina == false|| Input.GetKey(KeyCode.B) && isStamina == false)
        {
            // FixedUpdate 에서 실행되므로 fixedDeltaTime을 사용한다.
            breathTimer += Time.fixedDeltaTime * shakingSpeed;

            // 펄린 노이즈를 통해 자연스러운 무작위 값 추출
            // 시간에 따라 breathTimer가 복잡한 곡선을 따라 부드럽게 전진
            // 펄린 노이즈 결과 값(0.0 ~ 1.0)에 2를 곱하고 1을 빼서 범위를 -1.0 ~ 1.0으로 한정

            // x축과 y축을 따로 따로 움직이도록 분리한다.
            // => 펄린 노이즈는 단순 방향, shaking 값이 움직이는 각도 및 범위를 의미한다.
            swayX = (Mathf.PerlinNoise(breathTimer, 0f) * 2f - 1f) * shakingX;
            swayY = (Mathf.PerlinNoise(0f, breathTimer) * 2f - 1f) * shakingY;
        }

        WeaponRotation();

        transform.rotation = Quaternion.Euler(0, currentYRotation + swayX, 0);
        myCamera.localRotation = Quaternion.Euler(currentCameraXRotation + swayY, 0, zRotation);

    }

    private Vector2 velocityGunFollow;
    private float gunWeightX, gunWeightY;
    [Tooltip("Current weapon that player carries.")]
    [HideInInspector]
    public GameObject weapon;
    private GunScript gun;
    /*
     * Rotating current weapon from here.
     * Checkig if we have a weapon, if we do, if its a gun it iwll fetch the gun and rotate it accordingly,
     * same goes for the sword.
     * Incase we dont have a weapon or gun or it didnt find it, it will write into the console that it cant find a weapon.
     */
    void WeaponRotation()
    {
        if (!weapon)
        {
            weapon = GameObject.FindGameObjectWithTag("Weapon");
            if (weapon)
            {
                if (weapon.GetComponent<GunScript>())
                {
                    try
                    {
                        gun = GameObject.FindGameObjectWithTag("Weapon").GetComponent<GunScript>();
                    }
                    catch (System.Exception ex)
                    {
                        print("gun not found->" + ex.StackTrace.ToString());
                    }
                }
            }
        }

    }

    float deltaTime = 0.0f;
    [Tooltip("Shows FPS in top left corner.")]
    public bool showFps = true;
    /*
    * Shows fps if its set to true.
    */
    void OnGUI()
    {

        if (showFps)
        {
            FPSCounter();
        }

    }
    /*
    * Calculating real fps because unity status tab shows too much fps even when its not that mutch so i made my own.
    */
    void FPSCounter()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = Color.white;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }

}
