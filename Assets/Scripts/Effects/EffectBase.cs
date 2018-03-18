using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Effects;
using UnityEngine.EventSystems;
using UnityStandardAssets.Utility;

public class EffectBase : MonoBehaviour
{
    [SerializeField]
    private AudioClip _pickupSound;

    [SerializeField]
    private GameObject _pickupEffect;

    [SerializeField]
    private GameMaster _gameMaster;

    [SerializeField]
    private bool _isInstantExpire;

    private float _destroyDelay = 5f;

	void Start ()
    {
	}
	
	void Update ()
    {
	}

    public virtual void Collected(GameObject collector)
    { 
        if (collector.tag != "Player") return;

        if (_pickupSound != null)
        {
            _gameMaster.PlaySound(_pickupSound);
        }
        if (_pickupEffect != null)
        {
            Instantiate(_pickupEffect, transform.position, Quaternion.identity);
        }
        transform.position = collector.transform.position;
        transform.SetParent(collector.transform);
    }

    public virtual void Activated()
    {
        if (_isInstantExpire)
            Expired();
    }

    public virtual void Expired()
    {
        EventsListeners.Execute<IEffectEvents>(null, (x, y) => x.OnExpire());
        Destroy(gameObject, _destroyDelay);
    }
}