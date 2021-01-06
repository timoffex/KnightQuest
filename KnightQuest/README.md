# Coding notes

## Rules

### Either `sealed` or with a `Data` component

Every `MonoBehaviour` should either be `sealed` or should have a corresponding sealed 
`MonoBehaviour` with the `Data` suffix. The `Data` component must be required
(`RequireComponent`) by the main component and should contain all of the main component's
serializable data.

This pattern makes it possible to covariantly change component types when creating prefab variants.
See the section on prefab variants and component inheritance.

## Prefab variants and component inheritance

A child prefab can't override a parent prefab's component with a subclass. In essence, this is
because the serialization system treats components as data, not as objects. That's unfortunate,
because the runtime treats components as objects (they're _supposed_ to have methods and to
encapsulate their data).

There is a simple solution: separate the serializable data from the methods. Given a component
like

```csharp
class MyComponent : MonoBehaviour
{
    public float myData;
}
```

turn it into the following

```csharp
sealed class MyComponentData : MonoBehavior
{
    public float myData;
}

[RequireComponent(typeof(MyComponentData))]
class MyComponent : MonoBehaviour
{
    MyComponentData data;
    public float MyData => data.myData;

    protected virtual void Awake()
    {
        data = GetComponent<MyComponentData>();
    }
}
```

Here's a derived component:

```csharp
sealed class MyDerivedComponentData : MonoBehaviour
{
    public float myDerivedData;
}

[RequireComponent(typeof(MyDerivedComponentData))]
class MyDerivedComponent : MonoBehaviour
{
    MyDerivedComponentData data;
    public float MyDerivedData => data.myData;

    protected virtual void Awake()
    {
        base.Awake();
        data = GetComponent<MyDerivedComponentData>();
    }
}
```

Now, when you're creating a prefab variant and want it to have a `MyDerivedComponent` instead of a
`MyComponent`, just delete `MyComponent` and add `MyDerivedComponent`. This will keep 
`MyComponentData` and add a `MyDerivedComponentData` component, so your prefab variant will still
inherit all of the data of its base component.