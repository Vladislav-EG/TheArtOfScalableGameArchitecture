using UnityEngine;
using UnityHFSM;

namespace PlayerStates
{
    public class IdleState : StateBase<string>
    {
        public IdleState() : base(needsExitTime: false) { }

        public override void OnEnter()
        {
            Debug.Log("Enter IdleState");
        }
        
        public override void OnLogic()
        {
            // Idle logic
        }
        
        public override void OnExit()
        {
            Debug.Log("Exit IdleState");
        }
    }
    
    public class WalkState : StateBase<string>
    {
        public WalkState() : base(needsExitTime: false) { }

        public override void OnEnter()
        {
            Debug.Log("Enter WalkState");
        }
        
        public override void OnLogic()
        {
            // Idle logic
        }
        
        public override void OnExit()
        {
            Debug.Log("Exit WalkState");
        }
    }
    
    public class JumpState : StateBase<string>
    {
        public JumpState() : base(needsExitTime: false) { }

        public override void OnEnter()
        {
            Debug.Log("Enter JumpState");
        }
        
        public override void OnLogic()
        {
            // Idle logic
        }
        
        public override void OnExit()
        {
            Debug.Log("Exit JumpState");
        }
    }
    
    public class JumpWalk : StateBase<string>
    {
        public JumpWalk() : base(needsExitTime: false) { }

        public override void OnEnter()
        {
            Debug.Log("Enter JumpWalk");
        }
        
        public override void OnLogic()
        {
            // Idle logic
        }
        
        public override void OnExit()
        {
            Debug.Log("Exit JumpWalk");
        }
    }
}