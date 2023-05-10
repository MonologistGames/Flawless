using Monologist.Patterns.State;

namespace Flawless.PlayerCharacter
{
    public class PlayerStateMachine : StateMachine
    {
        public readonly PlanetController PlanetController;
        
        public PlayerStateMachine(PlanetController planetController) : base()
        {
            PlanetController = planetController;
        }

        public override void Initialize()
        {
            base.Initialize();
            StatePool.Add("MoveState", new MoveState(this));
            StatePool.Add("JumpState", new JumpState(this));
        }
    }
}