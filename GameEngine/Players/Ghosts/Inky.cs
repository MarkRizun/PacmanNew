namespace Pacman.GameEngine
{
    public class Inky : Ghost
    {
        #region Fields

        private int _xShift;
        private int _yShift;

        private int _xDistance;
        private int _yDistance;

        private Blinky _blinky;

        #endregion

        #region Properties

        public Blinky Blinky
        {
            set
            {
                _blinky = value;
            }
        }

        #endregion

        #region Initialization

        public Inky(Player pacman, Grid grid, int x, int y, float size)
            : base(pacman, grid, x, y, size)
        {
            Name = "Inky";
            Distance = 2;
        }

        public override void InitializePatrolPath()
        {
            PatrolPath = Algorithm.CalculatePath(StartCell, Level.Map[21, 27], Level.Map);
            PatrolPath.AddRange(Algorithm.CalculatePath(Level.Map[21, 27], Level.Map[23, 30], Level.Map));
            PatrolPath.AddRange(Algorithm.CalculatePath(Level.Map[23, 30], StartCell, Level.Map));
        }

        #endregion

        #region Behaviour

        public override Cell CalculateTargetCell()
        {
            Cell offsetCell = CalculateOffsetCell();
            Cell cell;

            _xDistance = Pacman.GetX() - _blinky.GetX();
            _yDistance = Pacman.GetY() - _blinky.GetY();

            TargetInBounds();

            cell = Level.Map[Pacman.GetX() + _xDistance, Pacman.GetY() + _yDistance];

            return TargetEmpty(cell);
        }

        private Cell CalculateOffsetCell()
        {
            Cell offsetCell = new Cell();
            offsetCell.Content = Content.Wall;

            while (offsetCell.IsWall())
            {
                switch (Pacman.Direction)
                {
                    case Direction.Up: offsetCell = GetUpCell();
                        break;
                    case Direction.Down: offsetCell = GetDownCell();
                        break;
                    case Direction.Left:
                        {
                            if (IsLeftAvailable())
                            {
                                offsetCell = GetLeftCell();
                            }
                            break;
                        }
                    case Direction.Right:
                        {
                            if (IsRightAvailable())
                            {
                                offsetCell = GetRightCell();
                            }
                            break;
                        }
                    default: offsetCell = Pacman.CurrentCell();
                        break;
                }

                if (Distance == 0)
                {
                    offsetCell = Pacman.CurrentCell();
                    break;
                }

                Distance--;
            }

            return offsetCell;
        }

        private void TargetInBounds()
        {
            LeftBoundShift();

            RightBoundShift();

            DownBoundShift();

            UpBoundShift();
        }

        private void LeftBoundShift()
        {
            while (Pacman.GetX() + _xDistance < 0)
            {
                _xDistance++;
            }
        }

        private void RightBoundShift()
        {
            while (Pacman.GetX() + _xDistance > Level.Width - 1)
            {
                _xDistance--;
            }
        }

        private void DownBoundShift()
        {
            while (Pacman.GetY() + _yDistance < 0)
            {
                _yDistance++;
            }
        }

        private void UpBoundShift()
        {
            while (Pacman.GetY() + _yDistance > Level.Height - 1)
            {
                _yDistance--;
            }
        }

        private Cell TargetEmpty(Cell cell)
        {
            int upperBound;
            int count = 1;

            _xShift = 0;
            _yShift = 0;

            while (cell.IsWall())
            {
                upperBound = CalculateUpperBound(count);

                for (; _yShift <= upperBound; _yShift++)
                {
                    if (IsShiftedInBounds(cell))
                    {
                        ShiftCell(ref cell);
                        if (!cell.IsWall())
                        {
                            return cell;
                        }
                    }
                }

                count++;
            }

            return cell;
        }

        private bool IsShiftedInBounds(Cell cell)
        {
            return cell.GetX() + _xShift > 0 &&
                   cell.GetX() + _xShift < Level.Width &&
                   cell.GetY() + _yShift > 0 &&
                   cell.GetY() + _yShift < Level.Height;
        }

        private int CalculateUpperBound(int count)
        {
            int bound;

            if (count % 2 == 0)
            {
                _xShift = count / 2;
                _yShift = -_xShift;
                bound = _xShift;
            }
            else
            {
                _xShift = -count / 2;
                bound = -_xShift;
                _yShift = _xShift;
            }

            return bound;
        }

        private void ShiftCell(ref Cell cell)
        {
            cell = Level.Map[cell.GetX() + _xShift, cell.GetY() + _yShift];
        }

        #endregion
    }
}
