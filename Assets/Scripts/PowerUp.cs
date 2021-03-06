using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUp : MonoBehaviour
{
    [Header("PowerUp Properties")]
    [SerializeField][Range(0.0f, 8.0f)] private float _speed = 2.5f;
    [SerializeField] private int _powerUpID;

    [Header("Audio")] 
    [SerializeField] private AudioClip _powerUpCollectionSound;
    private AudioSource _audioSource;

    private Transform _powerUpDestroyPoint;
    private Player _player;
    private UIManager _uiManager;
    private SpawnManager _spawnManager;


    // ======================================================
    private void Start()
    {
        FindGameObjects();
        NullChecking();
    }
    
    private void Update()
    {
        Movement();
    }

    private void FindGameObjects()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _powerUpDestroyPoint = GameObject.Find("EnemyDestroyPosition").transform;
        _audioSource = GameObject.Find("PowerUp_AudioManager").GetComponent<AudioSource>();
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _spawnManager = GameObject.FindObjectOfType<SpawnManager>();
    }

    private void NullChecking()
    {
        if (_powerUpDestroyPoint == null)
            Debug.LogError("'_enemyDestroyPoint' is NULL! Have you named your GameObject 'EnemyDestroyPosition'?");

        if (_player == null)
            Debug.LogError("'_player' is NULL! Have you named your GameObject 'Player'?");
        
        if (_audioSource == null)
            Debug.LogError("'_audioSource' is NULL! Have you added a 'Audio Source' component?");
    }
    
    private void Movement()
    {
        transform.Translate(Vector3.down * (_speed * Time.deltaTime));

        if (transform.position.y < _powerUpDestroyPoint.transform.position.y)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            _audioSource.clip = _powerUpCollectionSound;
            _audioSource.Play();
            
            switch (_powerUpID)
            {
                case 0:
                    _player.ActivateTripleShot();
                    break;
                case 1:
                    _player.ActivateSpeedBoost();
                    break;
                case 2:
                    _player.ActivateShield();
                    break;
                case 3:
                    _player.AddAmmo(15);
                    break;
                case 4: // HP+
                    _player.AddLive();
                    _uiManager.UpdateLives(_player.GetLives());
                    _player.UpdateDamageVisualizer();
                    break;
                default:
                    Debug.LogWarning("Invalid powerup ID!");
                    break;
            }
        }
    }
}
