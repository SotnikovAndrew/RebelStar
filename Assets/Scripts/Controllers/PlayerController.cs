using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   public enum State
        {
            None = 1,
            Select = 2,
            Combat = 3,
        }

        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _selectedMask;
        [SerializeField] private MapGrid _mapGrid;
        public State _currentState = State.None;
        private Player _selectedPlayer;
        private PathFinder _finder;
        private MoveZone _moveZone;
        private float speed = 2f;
        public bool _isMoving;
        public event Action<Player> OnSelected;
        public event Action OffSelected;
        public event Action MovePointChanged;
        public event Action Disable;

        private void Awake()
        {
            _finder = new PathFinder(_mapGrid);
            _moveZone = new MoveZone(_mapGrid, _finder);
        }
        

        private void Update()
        {
            #if UNITY_ANDROID
            
            
            #endif

            if (_isMoving)
                return;
            if (Input.GetMouseButtonDown(0) && _currentState == State.Select)
            {
                if (CanMove())
                {
                    MoveTo(_selectedPlayer.transform.position,TakePoint());
                }
               

            }
            if (Input.GetMouseButtonDown(0) && _currentState == State.None)
            {
                SelectPlayer();
            }

            if (Input.GetMouseButtonDown(1) && _currentState == State.Select)
            {
                EndSelectPlayer();
                ClearMoveZone();
            }


        }

        private void SelectPlayer()
        {
            Vector2 mouseInput = _camera.ScreenToWorldPoint(Input.mousePosition);
            var collider = Physics2D.OverlapPoint(mouseInput, _selectedMask);
            if (collider == null) return;
            _selectedPlayer = collider.gameObject.GetComponent<Player>();
            _currentState = State.Select;
            SetMoveZone(); 
        }

        private void EndSelectPlayer()
        {
            _mapGrid.datacell[_selectedPlayer.transform.position].walkable = false;
            _selectedPlayer = null;
            _currentState = State.None;
        }

        private void SetMoveZone()
        {
            _mapGrid.datacell[_selectedPlayer.transform.position].walkable = true;
            _moveZone.CalculateMoveGrid(_selectedPlayer);
        }

        private void ClearMoveZone()
        {
            _moveZone.ClearHighLight();
        }

        private bool CanMove()
        {
            return _moveZone.ConteinPath(TakePoint());
        }

        private Vector2 TakePoint()
        {
            Vector2 mouseInput = _camera.ScreenToWorldPoint(Input.mousePosition);
            return new Vector2(Mathf.Round(mouseInput.x), Mathf.Round(mouseInput.y));
        }
        
        private void MoveTo(Vector2 start, Vector2 finish)
        {
            var path = _finder.GetPath(_selectedPlayer.transform.position, TakePoint());
            
            StartCoroutine(Move(path));

        }

        private IEnumerator Move( List<Vector2> path)
        {
            _isMoving = true;
            for (int i = path.Count - 1; i >= 0; i--)
            {

                while ((Vector2)_selectedPlayer.transform.position != new Vector2(path[i].x, path[i].y))
                {
                    var step = speed * Time.deltaTime;
                    _selectedPlayer.transform.position =
                        Vector2.MoveTowards(_selectedPlayer.transform.position, path[i], step);
                    yield return null;
                }

                _selectedPlayer._movePoint--;
                MovePointChanged?.Invoke();

            }
            _isMoving = false;
            if (_selectedPlayer._movePoint!=0)
            {
                _moveZone.UpdateMoveGrid();
            }
            else
            {
                _moveZone.ClearHighLight();
            }
        }

        private void EndTurne()
        {
            _selectedPlayer = null;
        }

        private void GetCombat()
        {
           // _combatController.gameObject.SetActive(true);

        }

        private void OnDestroy()
        {
           // _uiController.EndTurnePress -= EndTurne;
           // _uiController.CombatPress -= GetCombat;
        }

        private void OnDisable()
        {
            Disable?.Invoke();
        }
}
