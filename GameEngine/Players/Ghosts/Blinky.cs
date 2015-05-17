using System.Linq;

namespace Pacman.GameEngine
{
    public class Blinky : Ghost
    {
        #region Initialization

        public Blinky(Player pacman, Grid grid, int x, int y, float size)
            : base(pacman, grid, x, y, size)
        {
            Name = "Blinky";
        }

        public override void InitializePatrolPath()
        {
            PatrolPath = Algorithm.CalculatePath(StartCell, Level.Map[24, 6], Level.Map);
            PatrolPath.AddRange(Algorithm.CalculatePath(Level.Map[24, 6], Level.Map[28, 6], Level.Map));
            PatrolPath.AddRange(Algorithm.CalculatePath(Level.Map[28, 6], StartCell, Level.Map));
        }

        #endregion

        #region Behaviour

        public override void UpdateChasePath()
        {
            base.UpdateChasePath();

            CheckTunnel();
        }

        private void CheckTunnel()
        {
            if (Pacman.IsPassedRightTunnel)
            {
                ChaseRightTunnel();
            }
            else
                if (Pacman.IsPassedLeftTunnel)
                {
                    ChaseLeftTunnel();
                }
                else
                {
                    TargetCell = ChasePath.Last();
                }
        }

        private void ChaseLeftTunnel()
        {
            ChasePath = Algorithm.CalculatePath(CurrentCell(), Level.Map[0, 15], Level.Map);
            TargetCell = Level.Map[32, 15];
            Pacman.IsPassedLeftTunnel = false;
        }

        private void ChaseRightTunnel()
        {
            ChasePath = Algorithm.CalculatePath(CurrentCell(), Level.Map[33, 15], Level.Map);
            TargetCell = Level.Map[0, 15];
            Pacman.IsPassedRightTunnel = false;
        }

        #endregion
    }
}
