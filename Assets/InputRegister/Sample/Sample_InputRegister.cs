using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RegInputEvent;
public class Sample_InputRegister : MonoBehaviour
{
    [SerializeField] GameObject notif;
    [SerializeField] UnityEngine.UI.Text txtSliVal;
    bool isWaitHideNotif;
    void Start()
    {
        notif.gameObject.SetActive(false);

        //key event
        InputEvent.OnKey(KeyCode.D, KeyPressMode.Down, () =>
        {
            print("d pressed");
            var box = GameObject.FindObjectsOfType<BoxCollider>();
            foreach (var item in box)
            {
                GameObject.Destroy(item.gameObject);
            }
        });
        //left mouse event
        InputEvent.OnMouse(new Mouse()
        {
            mouseCode = MouseCode.Left,
            mousePressMode = MousePressMode.Down,
            action = OnLeftMouseDown
        });
        //right mouse event
        InputEvent.OnMouse(new Mouse()
        {
            mouseCode = MouseCode.Right,
            mousePressMode = MousePressMode.Drag,
            action = OnRightMouseDrag
        });
        //ui event
        InputEvent.OnButton("btn-test", () =>
        {
            OnButtonTestClick();
        });
        InputEvent.OnSlider("sli-test", (v) =>
        {
            txtSliVal.text = v.ToString();
        });
    }
    private void OnButtonTestClick()
    {
        if (!isWaitHideNotif)
        {
            isWaitHideNotif = true;
            notif.gameObject.SetActive(true);
            StartCoroutine(HideNotif());
        }
    }
    private IEnumerator HideNotif()
    {
        yield return new WaitForSeconds(.5f);
        isWaitHideNotif = false;
        notif.gameObject.SetActive(false);
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
