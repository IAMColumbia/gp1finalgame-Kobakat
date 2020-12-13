using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This interface is probably better off as an abstract parent class that actually implements
/// these functions because the logic is identical. MovementState seems like the proper name for this parent class
/// but it already belongs to an actual state
///
/// TLDR;
/// this exists because i am lazy and picky about my names
/// </summary>

public interface IMoveState
{
    void UpdateRectAndCheckForCollisions(UnityMover m);
    void UpdatePosition(UnityMover m);
}
