
using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class CameraController : MonoBehaviour , IDragHandler,IBeginDragHandler,IEndDragHandler
{
    public enum StateCamera
    {
        playerMove = 1,
        enemyMove = 2,
        editormoveTest =3,
    }
    
    public float Sens;
    public StateCamera CurrentStateCamera = StateCamera.editormoveTest;
    private Transform _camera;
    // В конце перекинуть этот скрипт на КамераКонтроллер и убрать ЭдиторСтейт


    private void Awake()
    {
        _camera = FindObjectOfType<Camera>().transform;
    }

    private void Update()
    {
        switch (CurrentStateCamera)
        {
            case StateCamera.editormoveTest:
                break;
            case StateCamera.playerMove:
                CameraMove();
                break;
            case StateCamera.enemyMove:
                break;
        }
       
    }

    private void CameraMove()
    {
        if (Input.touchCount <= 0) return;
        var touch = Input.GetTouch(0);
        switch (touch.phase)
        {
            case TouchPhase.Began:
                break;
            case TouchPhase.Moved:
                _camera.transform.position += (Vector3)touch.deltaPosition.normalized * -Sens;
                break;
            case TouchPhase.Stationary:
                break;
            case TouchPhase.Ended:
                break;
            case TouchPhase.Canceled:
                break;

        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        _camera.transform.position += (Vector3) eventData.delta * -Sens;

    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
}
