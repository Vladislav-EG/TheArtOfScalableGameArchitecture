using UnityEngine;
using UnityHFSM;

namespace PlayerStates
{
    public class IdleState : StateBase<string>
    {
        public IdleState() : base(needsExitTime: false) { }

        public override void OnEnter()
        {
            DebugColorLog.LogEnter<IdleState>();
        }

        public override void OnLogic()
        {
            // Idle logic
        }

        public override void OnExit()
        {
            DebugColorLog.LogExit<IdleState>();
        }
    }
    
    public class WalkState : StateBase<string>
    {
        public WalkState() : base(needsExitTime: false) { }

        public override void OnEnter()
        {
            DebugColorLog.LogEnter<WalkState>();
        }

        public override void OnLogic()
        {
            // Walk logic
        }

        public override void OnExit()
        {
            DebugColorLog.LogExit<WalkState>();
        }
    }
    
    public class JumpState : StateBase<string>
    {
        public JumpState() : base(needsExitTime: false) { }

        public override void OnEnter()
        {
            DebugColorLog.LogEnter<JumpState>();
        }

        public override void OnLogic()
        {
            // Jump logic
        }

        public override void OnExit()
        {
            DebugColorLog.LogExit<JumpState>();
        }
    }
    
    public class FallState : StateBase<string>
    {
        public FallState() : base(needsExitTime: false) { }

        public override void OnEnter()
        {
            DebugColorLog.LogEnter<FallState>();
        }

        public override void OnLogic()
        {
            // Fall logic
        }

        public override void OnExit()
        {
            DebugColorLog.LogExit<FallState>();
        }
    }
}


public static class DebugColorLog
{
    public static void LogEnter<T>() where T : class
    {
        string stateName = typeof(T).Name;
        Debug.Log($"<color=green>Enter {stateName}</color>");
    }

    public static void LogExit<T>() where T : class
    {
        string stateName = typeof(T).Name;
        Debug.Log($"<color=red>Exit {stateName}</color>");
    }
}