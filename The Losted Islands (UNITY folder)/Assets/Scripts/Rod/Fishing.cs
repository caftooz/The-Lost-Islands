using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkHorizon;
using UnityEditor.SceneManagement;

public class Fishing : MonoBehaviour
{
    [SerializeField] GameObject _waterGO;
    [SerializeField] GameObject _fishPrefab;

    public GameObject _fishingPanel;
    public GameObject _blue;
    public GameObject _red;

    public float _bluePositionX;
    public Vector3 _redPosition;

    public bool _isMinigameNow = false;
    int _move = 1;
    [SerializeField]  int _moveSpeed = 1;

    Transform _maxPosition;
    Transform _minPosition;
    
    InventoryManager inventoryManager;
    QuickslotInventory QuickslotPanel;

    private void Start()
    {
        inventoryManager = GameObject.FindObjectOfType<InventoryManager>();
        QuickslotPanel = GameObject.FindObjectOfType<QuickslotInventory>();
        _waterGO = GameObject.FindWithTag("water");

        _red = _fishingPanel.transform.GetChild(0).gameObject;
        _blue = _fishingPanel.transform.GetChild(1).gameObject;
        _maxPosition = _fishingPanel.transform.GetChild(2);
        _minPosition = _fishingPanel.transform.GetChild(3);

        _bluePositionX = _blue.transform.position.x;
        _redPosition = _red.transform.position;

    }
    private void Update()
    {
        if (_isMinigameNow) CheckMinigameFishingWin(); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == _waterGO) StartCoroutine(StartFishing());
    }

    IEnumerator StartFishing()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        yield return new WaitForSeconds(Random.Range(5,10));
        StartMinigameFishing();
    }

    private void StartMinigameFishing()
    {
        _isMinigameNow = true;
        _fishingPanel.SetActive(true);
        _red.transform.position = new Vector3(Random.Range(_minPosition.position.x+20, _maxPosition.position.x-20), _red.transform.position.y, _red.transform.position.z);    
        
    }
    private void CheckMinigameFishingWin()
    {
        if (_blue.transform.position.x > _maxPosition.position.x-10) _move = -1;
        if (_blue.transform.position.x < _minPosition.position.x+10) _move = 1;

        _blue.transform.position += new Vector3(_move * (_maxPosition.position.x - _minPosition.position.x) * _moveSpeed/1000, 0, 0);

        if( Input.GetMouseButtonDown(0))
        {
            if ( _blue.transform.position.x < _red.transform.position.x + 30 && _blue.transform.position.x > _red.transform.position.x - 30)
            {
                _blue.transform.position = new Vector3(_bluePositionX, _blue.transform.position.y, _blue.transform.position.z);
                _red.transform.position = _redPosition;
                _fishingPanel.SetActive(false);
                inventoryManager.AddItem(_fishPrefab.GetComponent<Item>().item, Random.Range(1, 3));
                _isMinigameNow = false;
                QuickslotPanel.StopFishing();
            }
            else
            {
                _blue.transform.position = new Vector3(_bluePositionX, _blue.transform.position.y, _blue.transform.position.z);
                _red.transform.position = _redPosition;
                _fishingPanel.SetActive(false);
                _isMinigameNow = false;
                QuickslotPanel.StopFishing();

            }
        }

    }
}
