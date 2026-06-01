using UnityEngine;
using System.Collections;
using Unity.Mathematics;

public class BulletScript : MonoBehaviour {

	[Tooltip("Furthest distance bullet will look for target")]
	public float maxDistance = 1000000;
	RaycastHit hit;
	[Tooltip("Prefab of wall damange hit. The object needs 'LevelPart' tag to create decal on it.")]
	public GameObject decalHitWall;
	[Tooltip("Decal will need to be sligtly infront of the wall so it doesnt cause rendeing problems so for best feel put from 0.01-0.1.")]
	public float floatInfrontOfWall;
	[Tooltip("Blood prefab particle this bullet will create upoon hitting enemy")]
	public GameObject bloodEffect;
	[Tooltip("Put Weapon layer and Player layer to ignore bullet raycast.")]
	public LayerMask ignoreLayer;

	/*
	* Uppon bullet creation with this script attatched,
	* bullet creates a raycast which searches for corresponding tags.
	* If raycast finds somethig it will create a decal of corresponding tag.
	*/

	// 오차를 가져오기 위한 스크립트 변수
	GunScript gunScript;

	// 임시 오차 각도
	public Quaternion errorRotation;
	// 최종 오차 위치
	Vector3 errorDirection;

    private void Start()
    {
		gunScript = FindObjectOfType<GunScript>();

		// 정면을 기준으로 오차각도를 계산
		errorRotation = Quaternion.Euler(gunScript.UpDownError, gunScript.LeftRightError, 0f);
		// 정면(forward)에 회전을 곱해준다.
		errorDirection = errorRotation * transform.forward;
    }


    void Update () {

		// 총기 오차를 수정하면 실시간으로 반영되어야하기 때문에 Update() 메서드 이용
        errorRotation = Quaternion.Euler(gunScript.UpDownError, gunScript.LeftRightError, 0f);
        errorDirection = errorRotation * transform.forward;

        // 보통 메서드는 결과 값을 return으로 하나만 돌려줄 수 있다.
        // 하지만, 타격 여부와 별개로 어디에, 누구에게 어디에서 같은 많은 정보가 필요하기 대문에
        // out 은 함수가 끝날 때, 이 변수에 값을 채워 return 하겠다는 약속이다.
        // 따라서, hit.point, hit.collider처럼 미리 정의한 다양한 정보가 들어있다.

        // 수정된 방향을 적용
        if (Physics.Raycast(transform.position, errorDirection,out hit, maxDistance, ~ignoreLayer)){
			if(decalHitWall){
				if(hit.transform.tag == "LevelPart"){
					Debug.Log("Pop1");
					Instantiate(decalHitWall, hit.point + hit.normal * floatInfrontOfWall, Quaternion.LookRotation(hit.normal));
					Destroy(gameObject);
				}
				if(hit.transform.tag == "Dummie"){
					Debug.Log("Pop2");
					Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
					Destroy(gameObject);
				}
			}		
			Destroy(gameObject);
		}
		Destroy(gameObject, 0.1f);
	}

}
