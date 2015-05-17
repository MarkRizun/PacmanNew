using System;
using System.Collections.Generic;
using System.Linq;

namespace Pacman.GameEngine
{
    public class Clyde : Ghost
    {
        #region Initialization

        public Clyde(Player pacman, Grid grid, int x, int y, float size)
            : base(pacman, grid, x, y, size)
        {
            Name = "Clyde";
        }

        public override void InitializePatrolPath()
        {
            PatrolPath = Algorithm.CalculatePath(StartCell, Level.Map[8, 27], Level.Map);
            PatrolPath.AddRange(Algorithm.CalculatePath(Level.Map[8, 27], Level.Map[12, 30], Level.Map));
            PatrolPath.AddRange(Algorithm.CalculatePath(Level.Map[12, 30], StartCell, Level.Map));
        }

        #endregion

        #region Behaviour

        public override void UpdateChasePath()
        {
            int xDistance, yDistance, distance;

            xDistance = Math.Abs(GetX() - Pacman.GetX());
            yDistance = Math.Abs(GetY() - Pacman.GetY());
            distance = xDistance + yDistance;

            if (distance > 8)
            {
                base.UpdateChasePath();
            }
            else
            {
                UseStupidPath();
            }
        }

        public void UseStupidPath()
        {
            List<Cell> bestPath = Algorithm.CalculatePath(CurrentCell(), Level.GetRandomFreeCell(), Level.Map);
            PathIterator = 0;

            SelectChasePath(bestPath);

            TargetCell = ChasePath.Last();
        }

        #endregion
    }
}
