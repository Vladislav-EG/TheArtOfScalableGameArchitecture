using System.Security.Cryptography;
using UnityEngine;
using UnityHFSM;

namespace PlayerStates
{
    public class IdleState : ActionStateWithFixed
    {
        public IdleState() : base(needsExitTime: false) { }

        public override void OnEnter()
        {
            DebugColorLog.LogEnter<IdleState>();
        }

        public override void OnLogic()
        {
            // Idle logic
            // UnityEngine.Debug.Log("Update");
        }
        
        public override void OnFixedLogic()
        {
            // UnityEngine.Debug.Log("FixedUpdate");
        }
        
        public override void OnExit()
        {
            DebugColorLog.LogExit<IdleState>();
        }
    }
    
    public class WalkState : ActionStateWithFixed
    {
        private readonly Rigidbody2D _rigidbody2D;
        
        public WalkState(Rigidbody2D rigidbody2D) : base(needsExitTime: false)
        {
            _rigidbody2D = rigidbody2D;
        }

        public override void OnEnter()
        {
            DebugColorLog.LogEnter<WalkState>();
        }

        public override void OnLogic()
        {
            var moveVelocity = Input.GetAxis("Horizontal");

            _rigidbody2D.linearVelocity = new Vector2(moveVelocity * 5f, 0f);
        }

        public override void OnExit()
        {
            DebugColorLog.LogExit<WalkState>();
        }
        
        public override void OnFixedLogic()
        {
        }
    }
    
    public class JumpState : ActionStateWithFixed
    {
        private readonly Rigidbody2D _rigidbody2D;
        private readonly Transform _transform;

        public JumpState(Rigidbody2D rigidbody2D, Transform transform) : base(needsExitTime: false)
        {
            _rigidbody2D = rigidbody2D;
            _transform = transform;
        }

        public override void OnEnter()
        {
            DebugColorLog.LogEnter<JumpState>();
            _rigidbody2D.AddForce(_transform.up * 8f, ForceMode2D.Impulse);

        }

        public override void OnLogic()
        {
            
        }

        public override void OnExit()
        {
            DebugColorLog.LogExit<JumpState>();
        }
        
        public override void OnFixedLogic()
        {
        }
    }
    
    public class DashState : ActionStateWithFixed
    {
        private readonly Rigidbody2D _rigidbody2D;
        private readonly Transform _transform;

        public DashState(Rigidbody2D rigidbody2D, Transform transform) : base(needsExitTime: false)
        {
            _rigidbody2D = rigidbody2D;
            _transform = transform;
        }

        public override void OnEnter()
        {
            DebugColorLog.LogEnter<DashState>();
            _rigidbody2D.AddForce(_transform.right * 5f, ForceMode2D.Impulse);

        }

        public override void OnLogic()
        {
            
        }

        public override void OnExit()
        {
            DebugColorLog.LogExit<DashState>();
        }
        
        public override void OnFixedLogic()
        {
        }
    }
    
    public class FallState : ActionStateWithFixed
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
        
        public override void OnFixedLogic()
        {
        }
    }
    
    public class CrouchState : ActionStateWithFixed
    {
        public CrouchState() : base(needsExitTime: false) { }

        public override void OnEnter()
        {
            DebugColorLog.LogEnter<CrouchState>();
        }

        public override void OnLogic()
        {
            // Fall logic
        }

        public override void OnExit()
        {
            DebugColorLog.LogExit<CrouchState>();
        }
        
        public override void OnFixedLogic()
        {
        }
    }
    
    public class IdleCrouchState : ActionStateWithFixed
    {
        public IdleCrouchState() : base(needsExitTime: false) { }

        public override void OnEnter()
        {
            DebugColorLog.LogEnter<IdleCrouchState>();
        }

        public override void OnLogic()
        {
            // Fall logic
        }

        public override void OnExit()
        {
            DebugColorLog.LogExit<IdleCrouchState>();
        }
        
        public override void OnFixedLogic()
        {
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