# How game data is persisted

Persistence includes saving and loading, but it is also important for going between different
regions in the game, such as inside a building and outside it.

I have an object-oriented implementation of persistence, since my goal for this entire project is to
explore OOP. (And wow, I'm very happy with it. Things that seemed so hard to do in the past, ideas
that gave me unending analysis paralysis, are solved easily and quickly when I stick to principles
of OOP).

Before looking at the code, it is useful to understand some realizations that motivated my design.
I have tried several approaches to persistence in the past but could never get a satisfying
implementation because of the seeming clash between wanting to define serialization on a
per-component basis but not wanting to dump all Unity GameObject data into a savefile.

One naive approach to saving a game in Unity is to just serialize all loaded objects into a file.
Global game data like "quest progress" can be defined in serializable fields on a MonoBehaviour.
Besides the amount of unnecessary data that you end up saving, you also lose the ability to update
your game. Once a save file is made, the only changes that will be picked up when that save is
loaded is changes to assets and other indirect data. You can adjust your texture assets, but you
can't reposition static objects or add new components to objects in the game (even if they were
instantiated from a prefab, since that information is lost at runtime).

Going toward a different extreme, you might try to define all of your game data in a centralized
place. You have a serializable "GameData" object (different from the one in this repository!) which
stores the player's quest progress, the player's current scene, the stats of all characters, etc,
all as plain serializable C# classes. Since this contains _all_ of your game data, it also needs to
store character positions, projectile velocities (so that you make all the arrows disappear by
reloading the game), AI decision-making states, and more. Even if you figure out how to organize all
of this and deal with the fact that you have two places for each piece of data (one on a component
and one in GameData), the biggest problem with this approach is that you lose the most useful part
of Unity: its editor. Yes, you can write your own editor scripts to edit GameData, but that sucks.
You lose the ability to just drag an enemy prefab into a scene and have it work; you need to
manually enter values in some "initial GameData" object or write your own editor that mimics
Unity's or write a complicated system that parses your scenes to construct an initial GameData and
then strips dynamic data from those scenes.

> After writing this, I realized that I actually have an old game project on GitHub! What a
> throwback! Take a look at the monstrosity at the bottom of [this file](https://github.com/timoffex/Birdkeeper/blob/master/Assets/Scripts/SavingAndLoading/Game.cs)
> which roughly implements that latter "all data in one place" approach. To my credit, I do think
> it worked. Maybe the worst part about that project is that I used tabs instead of spaces.
>
> I also see an attempt at reflection-based saving and loading (the ObjectGraph stuff). It looks
> terrifying. I don't think I finished that.

I have learned my lesson: **don't fight Unity**. If you're trying to have a "plain C# model of the
game" and use Unity as "the user interface and the physics engine", you're probably going down the
wrong path. (Maybe someone has had success doing it this way, but it's been a nightmare for me!)

# How it works in this repository

Initial game data is defined in Unity normally. You can drag an enemy prefab into a scene, and that
becomes that enemy's initial position and settings.

Every GameObject with data to save has a `PersistablePrefab` component. When a scene is loaded for
the first time, the `PersistablePrefab`s in it act as its initial data. In subsequent loads, all
`PersistablePrefab`s in the scene are destroyed and replaced by the scene's saved data. This is
slightly inefficient, but it can be improved by splitting each scene into a "static" and "dynamic"
part and only loading the "dynamic" part on the first load. No need to spend effort on this yet
though; my game is way too small for performance problems.

`PersistablePrefab` stores a string that identifies the prefab that the object can be recreated
from when the game is reloaded. The mapping between prefab IDs and prefabs is stored in a singleton
ScriptableObject with the `PersistablePrefabCollection` script. It has to be edited manually right
now: writing editor utilities for it this early would be a waste of time. I try to make all prefab
IDs semantically meaningful so that they're easier to debug (as opposed to using randomly generated
strings).

Every `PersistablePrefab` must either be a root object or attached to another `PersistablePrefab`.
This allows me to save object hierarchies without having to give "scene IDs" to objects (if a
saveable object is parented to a static object, I need a way of identifying that static object
to properly parent the saveable object when reloading). Reparenting a `PersistablePrefab` at runtime
is not supported, but could be.

Every component whose presence should be saved extends `PersistableComponent` instead of
`MonoBehaviour` and must be attached to an object with a `PersistablePrefab` component. Persistable
components can be added and removed at runtime, with the limitation that persistable components
_on prefabs_ can't be removed at runtime because I don't persist the absence of a component (this
limitation is not fundamental). `PersistableComponent` defines two virtual functions, `Save` and
`Load`, which subclasses can override to serialize and deserialize their data. Note that this keeps
component data encapsulated: only the component knows how to save itself and read itself from a
byte stream.

Every `PersistableComponent` subclass must be associated to a unique string in the static
`PersistableComponents` class. While I could identify components by their qualified names,
associating them to strings helps me avoid reflection and allows me to rename components without
breaking save files. In theory, it is possible to define the component ID in an annotation on the
component class and generate `PersistableComponents` automatically.

Finally, `GameData` keeps track of the currently loaded `PersistablePrefab` objects and manages
dynamic scene data. It handles serializing all `PersistablePrefab` data when unloading a scene and
instantiating `PersistablePrefab`s when loading a scene with save data. Right now, it holds all
data for all scenes that the player has visited in memory; this limitation can be lifted without
changing `GameData`'s interface. `GameData` uses the `ObjectData` class to represent serialized
`PersistablePrefab` instances; note that `ObjectData` treats the serialized `PersistablePrefab`
data as a blob. This is what makes this approach different from defining a central structure that
completely encodes all game data (and transitively depends on the structure of every component in
the game).
