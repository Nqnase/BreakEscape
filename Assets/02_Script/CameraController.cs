using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // プレイヤーの Transform
    public Vector3 offset = new Vector3(0, 10, -10); // プレイヤーからのオフセット位置
    public float smoothSpeed = 0.125f; // カメラの追従スピード
    public Vector3 lookAtOffset = new Vector3(0, 2, 0); // カメラが見る位置のオフセット

    public Transform eventTarget; // イベント中にカメラが移動するターゲット
    public float eventDuration = 2f; // イベントの持続時間

    public bool isEventActive = false; // イベント中かどうか
    public float eventTimer = 0f; // イベントの経過時間
   public Transform currentTarget; // 現在のカメラターゲット

    // イベントを起動するオブジェクト
    public GameObject triggerObject;

    void Start()
    {
        currentTarget = player; // 初期状態ではプレイヤーを追従
    }

    void Update()
    {
        currentTarget = player;
        if (isEventActive)
        {
            eventTimer += Time.deltaTime;
            //transform.position = Vector3.Lerp(transform.position, eventTarget.position, smoothSpeed);
           // transform.LookAt(eventTarget.position);

            if (eventTimer >= eventDuration)
            {
                isEventActive = false;
                currentTarget = player; // イベントが終了したらプレイヤーを再追従
            }
        }
    }

    void LateUpdate()
    {
        if (!isEventActive)
        {
            Vector3 desiredPosition = currentTarget.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
            transform.LookAt(currentTarget.position + lookAtOffset);
        }
    }

    // プレイヤーがイベントを起動するオブジェクトに接触した場合
    private void OnTriggerEnter(Collider collider)
    {
        // プレイヤーが指定された triggerObject に接触した場合
        if (triggerObject.gameObject.CompareTag("Player"))
        {
            
           
        }
    }

   
}
