using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private RaycastHit _raycastHit;

    [SerializeField] private List<NavMeshAgent> _selectUnits = new();
    private NavMeshAgent[] _agents;
    [SerializeField]private NavMeshAgent _selectUnit;
    private Vector3 _targetPosition;

    private Vector2 _startMousePosition;
    private Vector2 _endMousePosition;

    private Rect selectionRect;

    private void Start()
    {
        _agents = FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.InstanceID);
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
                //case "Player":
                //    _selectUnit = _raycastHit.collider.gameObject.GetComponent<NavMeshAgent>();
                //    break;
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
        _selectUnits.Clear();
        foreach (var agent in _agents)
        {
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

        if (_selectUnit != null)
        {
            _targetPosition.y = _selectUnit.transform.position.y;
            _selectUnit.SetDestination(_targetPosition);
            _selectUnit = null;
        }

        if(_selectUnits.Count > 0)
        {
            foreach(var agent in _selectUnits)
            {
                _targetPosition.y = agent.transform.position.y;
                agent.SetDestination(_targetPosition);
            }
        }
    }

    private void OnGUI()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GUI.Box(selectionRect, "");
        }
    }
}