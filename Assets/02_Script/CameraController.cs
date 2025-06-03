using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // �v���C���[�� Transform
    public Vector3 offset = new Vector3(0, 10, -10); // �v���C���[����̃I�t�Z�b�g�ʒu
    public float smoothSpeed = 0.125f; // �J�����̒Ǐ]�X�s�[�h
    public Vector3 lookAtOffset = new Vector3(0, 2, 0); // �J����������ʒu�̃I�t�Z�b�g

    public Transform eventTarget; // �C�x���g���ɃJ�������ړ�����^�[�Q�b�g
    public float eventDuration = 2f; // �C�x���g�̎�������

    public bool isEventActive = false; // �C�x���g�����ǂ���
    public float eventTimer = 0f; // �C�x���g�̌o�ߎ���
   public Transform currentTarget; // ���݂̃J�����^�[�Q�b�g

    // �C�x���g���N������I�u�W�F�N�g
    public GameObject triggerObject;

    void Start()
    {
        currentTarget = player; // ������Ԃł̓v���C���[��Ǐ]
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
                currentTarget = player; // �C�x���g���I��������v���C���[���ĒǏ]
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

    // �v���C���[���C�x���g���N������I�u�W�F�N�g�ɐڐG�����ꍇ
    private void OnTriggerEnter(Collider collider)
    {
        // �v���C���[���w�肳�ꂽ triggerObject �ɐڐG�����ꍇ
        if (triggerObject.gameObject.CompareTag("Player"))
        {
            
           
        }
    }

   
}
