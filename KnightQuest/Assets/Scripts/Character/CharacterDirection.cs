using UnityEngine;

public enum CharacterDirection
{
    Right,
    RightUp,
    Up,
    LeftUp,
    Left,
    LeftDown,
    Down,
    RightDown
}

public static class CharacterDirectionUtils
{
    public static Vector2 ToWorldDirection(this CharacterDirection self)
    {
        const float sqrtHalf = 0.70710678118f;

        switch (self)
        {
            case CharacterDirection.Right: return new Vector2(1, 0);
            case CharacterDirection.RightUp: return new Vector2(sqrtHalf, sqrtHalf);
            case CharacterDirection.Up: return new Vector2(0, 1);
            case CharacterDirection.LeftUp: return new Vector2(-sqrtHalf, sqrtHalf);
            case CharacterDirection.Left: return new Vector2(-1, 0);
            case CharacterDirection.LeftDown: return new Vector2(-sqrtHalf, -sqrtHalf);
            case CharacterDirection.Down: return new Vector2(0, -1);
            case CharacterDirection.RightDown: return new Vector2(sqrtHalf, -sqrtHalf);
        }

        throw new System.NotSupportedException();
    }

    public static int ToInteger(this CharacterDirection self)
    {
        return (int)self;
    }

    public static CharacterDirection CharacterDirectionFromNonzero(Vector2 vector)
    {
        float angle = Mathf.Atan2(vector.y, vector.x);
        float angleFromMinOfRightDir = angle + Mathf.PI / 8;
        if (angleFromMinOfRightDir < 0)
            angleFromMinOfRightDir += 2 * Mathf.PI;

        int direction = (int)(8 * angleFromMinOfRightDir / (2 * Mathf.PI));
        if (direction > 8 || direction < 0)
            direction = 0;

        return (CharacterDirection)direction;
    }
}