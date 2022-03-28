using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TrainManager : MonoBehaviour
{
    /* 이 스크립트에서는 AR요소들을 관리하고 제어할겁니다. 
     * 제어에 앞서 필요한 재료들을 불러와야겠지요?
     * 인디케이터랑 제어할 ARManager, 그리고 소환할 모델링도 필요하겠네요. */
//     ( 0 ) 이거부터 시작
    public GameObject indicator;

//    ( 11 ) 3D 모델링을 불러올 변수
    public GameObject myTrain;

    ARRaycastManager arManager;
    GameObject placedObject;

    // Start is called before the first frame update
    void Start()
    {
        /* 인디케이터 오브젝트는 작동 순간에만 보이게하기 위해서 상시 비활성화 시킬게요. */
//       ( 1 ) 인디케이터를 비활성화 한다.
        indicator.SetActive(false);
//       ( 2 ) AR Raycast Manager를 가져온다.
        /* AR요소 제어에 필요해요. */
        arManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
//        ( 3 ) 바닥 감지 및 표식 출력용 함수
        DetectGround();

        /* 저희는 방금까지 인디케이터 활성화를 위한 함수를 만들었습니다.
         * 이제 생성한 인디케이터를 사용하여 해당 위치에 3D 모델링을 생성되게 해볼게요~
         * 우선 3D 모델링을 불러올 오브젝트 변수를 추가할게요~ */
        // ( 12 ) 인디케이터가 활성화 중일 때 화면을 터치하면 기차 모델링이 생성되게 하고 싶다!
        // ( 12 ) 만일, 인디케이터가 활성화 중이면서 화면 터치가 있는 상태라면...
        // ( 13 )
        /* 인디케이터가 극장에 출현중이겠죠? 그리고 터치를 한번 이상 했다면? */
        if(indicator.activeInHierarchy && Input.touchCount > 0)
        {
            // 첫 번째 터치 상태를 가져온다.
            Touch touch = Input.GetTouch(0);

            // 만일, 터치가 시작된 상태라면 자동차를 인디케이터와 동일한 곳에 생성한다.
            if(touch.phase == TouchPhase.Began)
            {
                /* 터치 상태를 나타내는 TouchPhase를 사용하고 Began은 말 그대로 시작한상태를 말해요. */

                // 만일 생성된 오브젝트가 없다면 프리팹을 씬에 생성하고
                // placedObject 변수에 할당한다.
                if(placedObject == null)
                {
                    placedObject = Instantiate(myTrain, indicator.transform.position, indicator.transform.rotation);
                }
                // 생성된 오브젝트가 있다면 그 오브젝트의 위치와 회전 값을 변경한다.
                else
                {
                    placedObject.transform.SetPositionAndRotation(indicator.transform.position, indicator.transform.rotation);
                }
                /* ( 14 ) 와~ 다 끝나간다 고생했다 ㅠㅠ */
            }
        }
    }

    void DetectGround()
    {
        /* 편의상 Raycast(레이케스트)를 레이로 부를게요. */

 //       ( 4 ) 스크린의 중앙 지점을 찾는다.
        Vector2 screenSize = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        /* 2분의 1이 0.5니까 가로 세로 0,5 곱해줍니다. */

//        ( 5 ) 레이에 부딪힌 대상들의 정보를 저장할 리스트 변수를 만든다.
        List<ARRaycastHit> hitInfos = new List<ARRaycastHit>();
        /* ARRaycast를 사용하기 아래와 같은 모듈을 선언해야해요.
         * using UnityEngine.XR.ARFoundation;
         * using UnityEngine.XR.ARSubsystems; */


//        ( 6 ) 만일, 스크린의 중앙 지점에서 레이를 발사했을 때 Plane 타입 추적 대상이 있다면,
        /* Raycast함수를 보시면screenPoint, hitResult, trackableTypes가 있죠? 하나하나 넣어볼게요 */
        if(arManager.Raycast(screenSize, hitInfos, TrackableType.Planes))
        {
//          ( 7 ) 표식 오브젝트를 활성화 한다.
            indicator.SetActive(true);

 //            ( 9 ) 표식 오브젝트의 위치 및 회전 값을 레이가 닿은 지점에 일치시킨다.
            /* 표식 오브젝트의 위치하고 회전값을 레이가 닿은 지점에 일치시켜볼게요~ 
             * 저희는 레이에서 받아온 데이터를 List라는 자료형에 저장을 하는걸로 했는데요,
             * List 자료형은 숫자나 문자들의 집합이예요~ */
            indicator.transform.position = hitInfos[0].pose.position;
            indicator.transform.rotation = hitInfos[0].pose.rotation;

            /* 이대로 인디케이터를 위치시키면 바닥에 겹쳐져서 안보여요. 그래서 살짝 올릴게요~ */
//            ( 10 ) 인디케이터 위치를 위쪽 방향으로 0.1m 올린다.
            indicator.transform.position += indicator.transform.up * 0.1f;
            // ( 10 ) 여기까지 잘왔니? ㅠㅠ 위에 Update 함수로 가서 ( 11 )을 보자!

            // (번외) 오브젝트 회전 안되서 넣음.
            // indicator.transform.Rotate(90, 0, 0);
        }
//        ( 8 ) 그렇지 않다면 표식 오브젝트를 비활성화 한다.
        else
        {
            indicator.SetActive(false);
        }
    }
}
