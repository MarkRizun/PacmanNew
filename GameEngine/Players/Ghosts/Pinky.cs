namespace Pacman.GameEngine
{
    public class Pinky : Ghost
    {
        #region Initialization

        public Pinky(Player pacman, Grid grid, int x, int y, float size)
            : base(pacman, grid, x, y, size)
        {
            Name = "Pinky";
            Distance = 3;
        }

        public override void InitializePatrolPath()
        {
            PatrolPath = Algorithm.CalculatePath(StartCell, Level.Map[8, 6], Level.Map);
            PatrolPath.AddRange(Algorithm.CalculatePath(Level.Map[8, 6], Level.Map[4, 6], Level.Map));
            PatrolPath.AddRange(Algorithm.CalculatePath(Level.Map[4, 6], StartCell, Level.Map));
        }

        #endregion

        #region Behaviour

        public override Cell CalculateTargetCell()
        {
            Cell cell = new Cell();
            cell.Content = Content.Wall;

            while (cell.IsWall())
            {
                switch (Pacman.Direction)
                {
                    case Direction.Up:
                        {
                            if (IsUpAvailable())
                            {
                                cell = GetUpCell();
                            }
                            break;
                        }
                    case Direction.Down:
                        {
                            if (IsDownAvailable())
                            {
                                cell = GetDownCell();
                            }
                            break;
                        }
                    case Direction.Left:
                        {
                            if (IsLeftAvailable())
                            {
                                cell = GetLeftCell();
                            }
                            break;
                        }
                    case Direction.Right:
                        {
                            if (IsRightAvailable())
                            {
                                cell = GetRightCell();
                            }
                            break;
                        }
                    default: return Pacman.CurrentCell();
                }

                if (Distance <= 1)
                {
                    cell = Pacman.CurrentCell();
                    break;
                }

                Distance--;
            }

            return cell;
        }

        #endregion
    }
}
