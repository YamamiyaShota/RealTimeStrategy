using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private RaycastHit _raycastHit;

    [SerializeField] private List<Unit> _selectUnits = new();
    private Unit[] _agents;
    private Vector3 _targetPosition;

    private Vector2 _startMousePosition;
    private Vector2 _endMousePosition;

    private Rect selectionRect;
    private SelectedType _selectedType;
    private GameObject _enemy;

    private void Start()
    {
        _agents = FindObjectsByType<Unit>(FindObjectsSortMode.InstanceID);
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        _startMousePosition = Mouse.current.position.ReadValue();
        if (Physics.Raycast(ray, out _raycastHit))
        {
            switch (_raycastHit.collider.gameObject.tag)
            {
                case "Player":
                    if (_raycastHit.collider.gameObject.TryGetComponent<Unit>(out Unit unit))
                    {
                        if (_selectUnits.Contains(unit))
                        {
                            return;
                        }
                        _selectUnits.Add(unit);
                        _selectedType = SelectedType.Player;
                    }
                    break;
                case "Enemy":
                    _enemy = _raycastHit.collider.gameObject;
                    if (_selectedType == SelectedType.Player)
                    {
                        for (int i = 0; i < _selectUnits.Count; i++)
                        {
                            IState state = _selectUnits[i].GetComponent<IState>();
                            state.SetState(StateType.Attack);
                            _selectUnits[i].SetAttackTarget(_enemy);
                        }
                    }
                    break;
                case "Ground":
                    MoveTargetPosition(_raycastHit);
                    break;
            }
        }
    }

    public void OnRelease(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        _endMousePosition = Mouse.current.position.ReadValue();
        float x = Mathf.Min(_startMousePosition.x, _endMousePosition.x);
        float y = Mathf.Min(_startMousePosition.y, _endMousePosition.y);

        float width = Mathf.Abs(_startMousePosition.x - _endMousePosition.x);
        float height = Mathf.Abs(_startMousePosition.y - _endMousePosition.y);

        selectionRect = new Rect(x, y, width, height);

        SelectUnitsInRect(selectionRect);
    }

    private void SelectUnitsInRect(Rect selection)
    {
        foreach (var agent in _agents)
        {
            if (_selectUnits.Contains(agent))
            {
                return;
            }
            var screenPos = _camera.WorldToScreenPoint(agent.transform.position);
            if (selection.Contains(new Vector2(screenPos.x, screenPos.y)))
            {
                _selectUnits.Add(agent);
            }
        }
    }

    /// <summary>
    /// クリックした場所に選択されているユニットを移動させる
    /// </summary>
    /// <param name="raycastHit"></param>
    private void MoveTargetPosition(RaycastHit raycastHit)
    {
        var distance = Vector3.Distance(_camera.gameObject.transform.position, raycastHit.point);
        var mousePosition = new Vector3(Mouse.current.position.x.value, Mouse.current.position.y.value, distance);
        _targetPosition = _camera.ScreenToWorldPoint(mousePosition);

        if (_selectUnits.Count > 0)
        {
            var unit = _selectUnits[0];
            unit.SetMoveType(MoveType.Parent);
            unit.SetTargetPosition(_targetPosition);
            unit.TryGetComponent<IState>(out IState state);
            state.SetState(StateType.Move);
            if (_selectUnits.Count > 1)
            {
                for (int i = 1; i < _selectUnits.Count; i++)
                {
                    _selectUnits[i].SetMoveType(MoveType.Child);
                    _selectUnits[i].SetFollowUnit(unit.transform);
                    _selectUnits[i].SetTargetPosition(_targetPosition);
                    _selectUnits[i].TryGetComponent<IState>(out state);
                    state.SetState(StateType.Move);
                    unit = _selectUnits[i];
                }
            }
            _selectUnits.Clear();
        }
    }
}

public enum SelectedType
{
    Player,
    Enemy,
}