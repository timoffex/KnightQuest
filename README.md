# What is this

This is a game that I'm writing using Unity and C#. As usual, my goal
is not to "make a game" but rather to learn something new. In this
case, my goal is to explore OOP.

# OOP? Is that some kind of monad?

(The joke is that I obsessed over Haskell throughout most of 2019 and
2020.)

Actually, it did start with Haskell: I read the paper behind
"OOHaskell" ("Haskell's overlooked object system" by Oleg Kiselyov and
Rälf Lammel), something that I had overlooked because isn't Haskell
all about "functional programming"? It turns out Haskell is an awesome
object-oriented language. Imagine OOP, but with Haskell's
flexibility + structural typing. Except it's not the same structural
typing as in TypeScript: field and method labels in TypeScript are
just text, so you can "accidentally implement" an interface by having
a method with the same name and type signature (but a different
contract, which is why this is a problem). In OOHaskell, labels are
backed by types, and you have to import them to use them. You can have
multiple labels with the same "name" that are exported by different
modules, and they will act like different labels.

Why am I not writing an object-oriented program in Haskell?

I've used Unity since 2015, but every project I worked on ended by
eventually becoming too difficult to develop. Unity is an
object-oriented game engine, and C# is an object-oriented language. If
I couldn't write a good object-oriented program using those, why would
I be able to do it in Haskell? And if I am now able do it in Haskell,
then I should be able to finally finish a Unity game.

# What I used to do wrong

Over the last few years, my programming-to-thinking ratio plummeted. I
spent an increasingly large amount of time thinking about how to
structure my code, so much that I sometimes never even got to
programming. Case in point: in my _first_ attempt at writing this game
(half a year ago, not on GitHub), I filled up half a notebook with
design thoughts over the span of two months. I only wrote code during
the second month, and I still spent at least 5 nights a week _just
writing in my notebook_.

But "thinking less" wouldn't have saved me: there were design problems
that I simply didn't know how to solve, and I would have run into them
eventually. Well, maybe. Or maybe writing a little bit of code would
have helped me think through those problems faster.

I'm about half a month into writing this game as of now (I started
around the 4th of January, and it is currently the 18th), and I've
been _flying through it_. I don't think I've made this much progress
on any game in this amount of time, ever. I've been making
modifications on a daily basis; I have not spent a single day "just
coding a system" without any noticeable effect on the playable part of
the game. I credit this velocity to my new understanding of
object-oriented programming. I also credit it to a few new rules that
I've been following:

- **Avoid overspecification**. I love to document everything
  thoroughly and to write down the exact contract on every interface
  that I define, and boy do I love defining interfaces. When working
  in Unity, this is a problem because Unity is too flexible. It lets
  you do too much. You can `Destroy` anything; you can activate and
  deactivate GameObjects, and you can separately enable and disable
  components; you can access the `SceneManager` from anywhere because
  it's a collection of static functions (that mutate global state).
  You can `Instantiate` anything, you can change the `transform` of
  anything or add any number of any components on anything.
  
  Trying to write down how every MonoBehaviour can and can't be used
  _before I even use it_ is extremely difficult. Instead, in this
  repository I document lazily. I add documentation _after_ I've
  written the code and figured out how I use it rather than planning
  it out before writing it. If you like Haskell, I changed
  
  ```hs
  do
    design <- designCode
	writeAndDocumentCode design
	writeDependentCode
  ```
  
  into
  
  ```hs
  mdo -- Look up "MonadFix is time travel"
    -- The @design@ here comes from the third statement
    code1  <- writeAndDocumentCode design
	code2  <- writeDependentCode
	design <- describeHow code2 `uses` code1
	return ()
  ```
  
  (I also applied this concept to the idea of "coming up with rules".
  I only came up with these rules _after_ I started applying them.)

- **Don't plan**. This is related to the above, but slightly
  different. I usually try to write my code so that I never have to
  change it. I want to "solve the problem exactly once". What I end up
  doing is using my brain as a scratchpad where I write the code
  multiple times and see where its design fails. But that's stupid.
  There is a class of tools made specifically to facilitate writing
  code, and those tools are called IDEs.
  
  In small, personal projects, refactoring is less painful than
  planning, and I can't believe I'm only realizing it now.

Some more specific rules:

- Avoid C# events. Subclassing is usually more appropriate, even
  though it is less flexible. I know that's a hard sell; this argument
  wouldn't have convinced me before I was already convinced. (Note to
  self: I wrote more about this in a private document under `~/org/`)
  
- Don't be afraid of subclassing. I wouldn't design a library around
  subclassing: maybe a good rule here is "don't subclass across
  library boundaries". But sometimes subclassing is appropriate.
  Specifically, it is appropriate when you...
  
- Avoid sum types. Well, C# doesn't have sum types. What I mean is
  avoid enums, and definitely avoid emulating sum types with class
  hierarchies and `is` checks. But how do you avoid sum types? You do
  it by...
  
- Modeling using "objects" not "data". Sum types are great for
  modeling data, so why don't they exist in the most popular
  object-oriented languages? Because objects hide data. If you want to
  write this (I'll use pseudo-Dart for the example):
  
  ```dart
  class Weapon {
    Either<SwordData, BowData> get data;
  }
  
  class CombatStats {
    void takeDamageFrom(Weapon weapon) {
	  // Not real Dart
	  case (weapon.data) {
	    match Either.Left(sword):
		  _hitWithSword(sword.sharpness);
	    match Either.Right(bow):
		  _hitWithBow(bow.springiness);
	  }
	}
	
	...
  }
  ```
  
  write this instead:
  
  ```dart
  abstract class Weapon {
    void hit(CombatStats stats);
  }
  
  class Sword extends Weapon {
    final float _sharpness;
	
	@override
	void hit(CombatStats stats) => stats.hitWithSword(_sharpness);
  }
  
  class Bow extends Weapon {
    final float _springiness;
	
	@override
	void hit(CombatStats stats) => stats.hitWithBow(_springiness);
  }
  
  class CombatStats {
    void hitWithSword(float sharpness) => ...
	void hitWithBow(float springiness) => ...
  }
  ```

  This is similar to modelling `Either A B` by `forall r. (A -> r, B
  -> r) -> r` (using Haskell this time), except it's intentionally
  _less_ polymorphic (so no `forall r`).
  
  ```hs
  data CombatStats
    = CombatStats
	  { hitWithSword :: Sharpness -> IO ()
	  , hitWithBow   :: Springiness -> IO () }
  data Weapon
    = Weapon
	  { hit :: CombatStats -> IO () }
	  
  -- Weapon is isomorphic to
  --   (Sharpness -> IO (), Springiness -> IO ()) -> IO ()
  -- which is a specialization of
  --   forall r. (Sharpness -> r, Springiness -> r) -> r
  -- which is isomorphic to
  --   Either Sharpness Springiness
  ```
