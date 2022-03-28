using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TrainManager : MonoBehaviour
{
    /* �� ��ũ��Ʈ������ AR��ҵ��� �����ϰ� �����Ұ̴ϴ�. 
     * ��� �ռ� �ʿ��� ������ �ҷ��;߰�����?
     * �ε������Ͷ� ������ ARManager, �׸��� ��ȯ�� �𵨸��� �ʿ��ϰڳ׿�. */
//     ( 0 ) �̰ź��� ����
    public GameObject indicator;

//    ( 11 ) 3D �𵨸��� �ҷ��� ����
    public GameObject myTrain;

    ARRaycastManager arManager;
    GameObject placedObject;

    // Start is called before the first frame update
    void Start()
    {
        /* �ε������� ������Ʈ�� �۵� �������� ���̰��ϱ� ���ؼ� ��� ��Ȱ��ȭ ��ų�Կ�. */
//       ( 1 ) �ε������͸� ��Ȱ��ȭ �Ѵ�.
        indicator.SetActive(false);
//       ( 2 ) AR Raycast Manager�� �����´�.
        /* AR��� ��� �ʿ��ؿ�. */
        arManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
//        ( 3 ) �ٴ� ���� �� ǥ�� ��¿� �Լ�
        DetectGround();

        /* ����� ��ݱ��� �ε������� Ȱ��ȭ�� ���� �Լ��� ��������ϴ�.
         * ���� ������ �ε������͸� ����Ͽ� �ش� ��ġ�� 3D �𵨸��� �����ǰ� �غ��Կ�~
         * �켱 3D �𵨸��� �ҷ��� ������Ʈ ������ �߰��ҰԿ�~ */
        // ( 12 ) �ε������Ͱ� Ȱ��ȭ ���� �� ȭ���� ��ġ�ϸ� ���� �𵨸��� �����ǰ� �ϰ� �ʹ�!
        // ( 12 ) ����, �ε������Ͱ� Ȱ��ȭ ���̸鼭 ȭ�� ��ġ�� �ִ� ���¶��...
        // ( 13 )
        /* �ε������Ͱ� ���忡 �������̰���? �׸��� ��ġ�� �ѹ� �̻� �ߴٸ�? */
        if(indicator.activeInHierarchy && Input.touchCount > 0)
        {
            // ù ��° ��ġ ���¸� �����´�.
            Touch touch = Input.GetTouch(0);

            // ����, ��ġ�� ���۵� ���¶�� �ڵ����� �ε������Ϳ� ������ ���� �����Ѵ�.
            if(touch.phase == TouchPhase.Began)
            {
                /* ��ġ ���¸� ��Ÿ���� TouchPhase�� ����ϰ� Began�� �� �״�� �����ѻ��¸� ���ؿ�. */

                // ���� ������ ������Ʈ�� ���ٸ� �������� ���� �����ϰ�
                // placedObject ������ �Ҵ��Ѵ�.
                if(placedObject == null)
                {
                    placedObject = Instantiate(myTrain, indicator.transform.position, indicator.transform.rotation);
                }
                // ������ ������Ʈ�� �ִٸ� �� ������Ʈ�� ��ġ�� ȸ�� ���� �����Ѵ�.
                else
                {
                    placedObject.transform.SetPositionAndRotation(indicator.transform.position, indicator.transform.rotation);
                }
                /* ( 14 ) ��~ �� �������� ����ߴ� �Ф� */
            }
        }
    }

    void DetectGround()
    {
        /* ���ǻ� Raycast(�����ɽ�Ʈ)�� ���̷� �θ��Կ�. */

 //       ( 4 ) ��ũ���� �߾� ������ ã�´�.
        Vector2 screenSize = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        /* 2���� 1�� 0.5�ϱ� ���� ���� 0,5 �����ݴϴ�. */

//        ( 5 ) ���̿� �ε��� ������ ������ ������ ����Ʈ ������ �����.
        List<ARRaycastHit> hitInfos = new List<ARRaycastHit>();
        /* ARRaycast�� ����ϱ� �Ʒ��� ���� ����� �����ؾ��ؿ�.
         * using UnityEngine.XR.ARFoundation;
         * using UnityEngine.XR.ARSubsystems; */


//        ( 6 ) ����, ��ũ���� �߾� �������� ���̸� �߻����� �� Plane Ÿ�� ���� ����� �ִٸ�,
        /* Raycast�Լ��� ���ø�screenPoint, hitResult, trackableTypes�� ����? �ϳ��ϳ� �־�Կ� */
        if(arManager.Raycast(screenSize, hitInfos, TrackableType.Planes))
        {
//          ( 7 ) ǥ�� ������Ʈ�� Ȱ��ȭ �Ѵ�.
            indicator.SetActive(true);

 //            ( 9 ) ǥ�� ������Ʈ�� ��ġ �� ȸ�� ���� ���̰� ���� ������ ��ġ��Ų��.
            /* ǥ�� ������Ʈ�� ��ġ�ϰ� ȸ������ ���̰� ���� ������ ��ġ���Ѻ��Կ�~ 
             * ����� ���̿��� �޾ƿ� �����͸� List��� �ڷ����� ������ �ϴ°ɷ� �ߴµ���,
             * List �ڷ����� ���ڳ� ���ڵ��� �����̿���~ */
            indicator.transform.position = hitInfos[0].pose.position;
            indicator.transform.rotation = hitInfos[0].pose.rotation;

            /* �̴�� �ε������͸� ��ġ��Ű�� �ٴڿ� �������� �Ⱥ�����. �׷��� ��¦ �ø��Կ�~ */
//            ( 10 ) �ε������� ��ġ�� ���� �������� 0.1m �ø���.
            indicator.transform.position += indicator.transform.up * 0.1f;
            // ( 10 ) ������� �߿Դ�? �Ф� ���� Update �Լ��� ���� ( 11 )�� ����!

            // (����) ������Ʈ ȸ�� �ȵǼ� ����.
            // indicator.transform.Rotate(90, 0, 0);
        }
//        ( 8 ) �׷��� �ʴٸ� ǥ�� ������Ʈ�� ��Ȱ��ȭ �Ѵ�.
        else
        {
            indicator.SetActive(false);
        }
    }
}
