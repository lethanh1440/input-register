using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RegInputEvent;
public class Sample_InputRegister : MonoBehaviour
{
    void Start()
    {
        //key event
        InputEvent.OnInput(KeyCode.D, KeyPressMode.Down, () =>
        {
            print("d pressed");
            var box = GameObject.FindObjectsOfType<BoxCollider>();
            foreach (var item in box)
            {
                GameObject.Destroy(item.gameObject);
            }
        });
        //left mouse event
        InputEvent.mouses.Add(new Mouse()
        {
            mouseCode = MouseCode.Left,
            mousePressMode = MousePressMode.Down,
            action = OnLeftMouseDown
        });
        //right mouse event
        InputEvent.mouses.Add(new Mouse()
        {
            mouseCode = MouseCode.Right,
            mousePressMode = MousePressMode.Drag,
            action = OnRightMouseDrag
        });
    }
    private void OnLeftMouseDown(Vector3 mousePos)
    {
        //print("left mouse down " + mousePos);
        var ray = Camera.main.ScreenPointToRay(mousePos);
        var point = ray.GetPoint(25);
        var ins = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ins.transform.eulerAngles = new Vector3(Random.Range(0, 90), Random.Range(0, 90));
        ins.transform.position = point;

    }
    private void OnRightMouseDrag(Vector3 mousePos)
    {
        //print("left mouse drag " + mousePos);
        var ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject.Destroy(hit.collider.gameObject);
        }
    }
}
