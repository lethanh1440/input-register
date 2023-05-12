# input-register
This is module simple for event keyboard and mouse event in Unity.

Eg:
+ Drag prefab 'InputRegister' into scene
+ Use sample code below:
```csharp 
using RegInputEvent;
public class Test : MonoBehaviour
{
    void Start()
    {
        //key event
        InputEvent.OnKey(KeyCode.D, KeyPressMode.Down, () =>
        {
            print("d pressed");    
        });
        //mouse event
        InputEvent.OnMouse(new Mouse()
        {
            mouseCode = MouseCode.Right,
            mousePressMode = MousePressMode.Drag,
            action = OnRightMouseDrag
        });
    }
    
    void OnRightMouseDrag(Vector3 mousePos)
    {
        print("mouse drag " + mousePos);   
    }
}
````
