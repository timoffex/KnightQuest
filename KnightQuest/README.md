# Coding notes

## Rules

### Store data in a `Data` component

Every `MonoBehaviour` should either be `sealed` or should have a corresponding sealed 
`MonoBehaviour` with the `Data` suffix. The `Data` component must be required
(`RequireComponent`) by the main component and should contain all of the main component's
serializable data.

This pattern makes it possible to covariantly change component types when creating prefab variants.
See the section on prefab variants and component inheritance.

### Unity callbacks should be `virtual`

If a `MonoBehaviour` is not `sealed`, all of its Unity callback methods should be `virtual`.
(Don't try to do the thing where you have a nonvirtual `Awake` that calls a virtual `AwakeImpl`;
first of all, classes more than 1 level down will still have to remember to call `base.AwakeImpl`,
and second of all, subclasses are responsible for their interactions with the parent class anyway).

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

## MonoBehaviour taxonomy

### Pure data

Pure data components are `sealed`, have all public fields, and have no methods. These components
should be used by exactly one other component and should have that component's name with the `Data`
suffix.

These components exist to support component inheritance hierarchies. Unity's serialization doesn't
support inheritance because it treats components as data (or vice versa, I'm not implying
an intentional design here) even though its runtime treats seems to treat them like objects.

### Objects

Object components are objects in the "object-oriented programming" sense. They are meant to be
accessed by other scripts via `GetComponent` or `GetComponentInParent`. They can have public
methods, but data is private.

Objects have no serializable data and instead have an associated `Data` component. I don't think 
there is ever a reason to make one of these `sealed`, but if you had a reason, then you wouldn't
need a separate `Data` component (since the only purpose of these is to make class inheritance
work well with prefab inheritance).

#### "Noun" objects

"Noun" object components correspond to prefab hierarchies. There is at most one noun component on
any `GameObject`, although there could be other object components. `Character` and `Weapon` are
noun components. Note how it doesn't make sense to have both on the same `GameObject`.

#### "Adjective" objects

"Adjective" object components add functionality to a `GameObject` but don't correspond to their own
prefab hierarchies. For example, `Attackable` is an adjective component. It doesn't make sense
to have an `Attackable` prefab, but you can add an `Attackable` component on a `Character` object.

I'm also tempted to call these "mixin objects" or "mixins".

When thinking about what should be a prefab, keep in mind that there is a fundamental difference
between nouns and adjectives: adjectives can be composed. Prefabs should be _simple nouns_ (no
adjectives) and prefab variants should have an _is a_ relationship with their parents. "An
attackable character is a character" is wrong because "attackable character" has an adjective.
You'll run into trouble down the line because you can't combine an "attackable character" with a 
"interactable character" to get an "attackable interactable character". This logic also applies to
classes.

Watch out for nouns that serve as modifiers to other nouns. Don't model "player character" as a
subclass of "character": it's better modeled as any "character" with a "player" mixin. This allows
you to create "player knight" and "player goblin" without any extra work. (You might wonder if
"knight" isn't the same as a "knight character". What's the difference? Consider the fact that
"player character" and "playable character" sound interchangeable, whereas you wouldn't replace
"knight character" by "knightly character". Also think about how while a "goblin knight" is both
a knight and a goblin, it's not exactly "a knight plus a goblin".)