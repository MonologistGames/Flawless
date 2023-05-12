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
            StatePool.Add("Move", new MoveState(this));
            StatePool.Add("Controlled", new ControlledState(this));
        }
    }
}