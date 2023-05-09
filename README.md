# input-register
This is module simple for event keyboard and mouse event in Unity.
```csharp 
//key event
InputEvent.OnInput(KeyCode.D, KeyPressMode.Down, () =>
{
    print("d pressed");    
});
//mouse event
InputEvent.mouses.Add(new Mouse()
{
    mouseCode = MouseCode.Left,
    mousePressMode = MousePressMode.Down,
    action = OnLeftMouseDown
});
````
