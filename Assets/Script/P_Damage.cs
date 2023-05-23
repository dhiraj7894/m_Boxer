using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class P_Damage : MonoBehaviourPunCallbacks, IDamageable
{

    [SerializeField] private Collider _fistLeftCollider;
    [SerializeField] private Collider _fistRightCollider;
    [SerializeField] private Collider _legCollider;
    [SerializeField] private Image HealthSlider;
    [SerializeField] private GameObject HUI;



    public bool IsHit;
    public float MaxHealth;
    public float HitReset;
    public float DamageAmount = 5;

    private float currentHealth;

    PlayerManager playerManager;

    private void Start()
    {
        currentHealth = MaxHealth;
        playerManager = PhotonView.Find((int)P_Character.Instance.PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    public void Damage(float damage)
    {
        P_Character.Instance.PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
        //Health-= damage;
    }
    private void Update()
    {
        HealthSliderManage();
        //HealthSlider.transform.forward = -cam.transform.forward;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Body"))
        {
            Debug.LogError(collision);
            IDamageable damageable = collision.gameObject.GetComponentInParent<IDamageable>();

            if (damageable != null)
            {
                if (!IsHit)
                {
                    damageable.Damage(DamageAmount);
                    P_Character.Instance.Animator.SetTrigger("hit");
                    IsHit = true;
                    LeanTween.delayedCall(HitReset, () => { IsHit = false; });
                }

            }
        }
        
    }
    

    public void LeftFist()
    {
        if (_fistLeftCollider.enabled)
        {
            _fistLeftCollider.enabled = false;
        }
        else
        {
            _fistLeftCollider.enabled = true;
        }
    }

    public void RightFist()
    {
        if (_fistRightCollider.enabled)
        {
            _fistRightCollider.enabled = false;
        }
        else
        {
            _fistRightCollider.enabled = true;
        }
    }

    public void Leg()
    {
        if (_legCollider.enabled)
        {
            _legCollider.enabled = false;
        }
        else
        {
            _legCollider.enabled = true;
        }
    }


    void HealthSliderManage()
    {
        /*float healthDecresingSpeed = 30;
        if(currentHealth < HealthSlider.value)
        {
            HealthSlider.value -= healthDecresingSpeed * Time.deltaTime;
            if(HealthSlider.value <= currentHealth)
            {
                HealthSlider.value = currentHealth;
            }
        }*/
        
    }


    public void LeavRoom()
    {

        StartCoroutine(LeaveRoom());


/*        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        
        */
    }

    IEnumerator LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
            yield return null;
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        PhotonNetwork.LoadLevel(0);
    }

    public override void OnLeftRoom()
    {
        //PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        //PhotonNetwork.LoadLevel(0);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!P_Character.Instance.PV.IsMine)
            return;
        HealthSlider.fillAmount = currentHealth / MaxHealth;
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        playerManager.Die();
    }
}
