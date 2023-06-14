using Monologist.Patterns.State;

namespace Flawless.PlayerCharacter
{
    public class PlayerStateMachine : StateMachine
    {
        public readonly PlayerController PlayerController;
        
        public PlayerStateMachine(PlayerController playerController) : base()
        {
            PlayerController = playerController;
        }

        public override void Initialize()
        {
            base.Initialize();
            StatePool.Add("Move", new MoveState(this));
            StatePool.Add("Controlled", new ControlledState(this));
        }
        
        public override void Enable()
        {
            CurrentState.OnEnter();
        }
        
        public override void Disable()
        {
            CurrentState.OnExit();
        }
    }
}