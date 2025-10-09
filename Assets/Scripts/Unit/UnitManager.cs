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
                    if (_raycastHit.collider.gameObject.TryGetComponent<Unit>(out var unit))
                    {
                        if (_selectUnits.Contains(unit))
                        {
                            return;
                        }
                        _selectUnits.Add(unit);
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

        FIndSelectionsUnit(selectionRect);
    }

    private void FIndSelectionsUnit(Rect selection)
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
            var character = _selectUnits[0];
            character.SetTargetPosition(_targetPosition);
            for (int i = 1; i < _selectUnits.Count; i++)
            {
                _selectUnits[i].SetCharacter(character.gameObject);
                _selectUnits[i].SetTargetPosition(_targetPosition);
                character = _selectUnits[i];
            }
            _selectUnits.Clear();
        }
    }
}