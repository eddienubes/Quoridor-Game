using System;
using System.Collections.Generic;
using System.Linq;
using Quorridor.Model;
using Quorridor.Model.Commands;

namespace Quorridor.AI
{
    /// <summary>
    /// Конвертирует <see cref="Codice.Client.Common.ICommand"/> в аргументы командной строки и наоборот
    /// </summary>
    public static class CLIConvertor
    {
        private const string inputCommandIdentificator = "<-";
        private const string outputCommandIdentificator = "->";
        private static string GetCellCoord(Cell cell) => $"{'A' + cell.GridX}{cell.GridY + 1}";

        private static string GetWallCoord(PlaceWallCommand command)
        {
            var allCells = new List<Cell>
                { command._cell1Pair1, command._cell1Pair2, command._cell2Pair1, command._cell2Pair2 };
            var maxX = allCells.Max(x => x.GridX);
            var maxY = allCells.Max(x => x.GridY);
            return $"{'S' + maxX - 1}{maxY}";
        }

        public static string Convert(MovePawnCommand command) =>
            $"{outputCommandIdentificator} {(command.IsJump ? "jump" : "move")} {GetCellCoord(command._targetCell)}";

        public static string Convert(PlaceWallCommand command) =>
            $"{outputCommandIdentificator} wall {GetWallCoord(command)}{(command._isWallVertical ? 'v' : 'h')}";

        public static ICommand Parse(string command, Grid grid, Pawn playerPawn)
        {
            var commandParts = command.Split(' ');
            var inputOutput = commandParts[0];
            var commandType = commandParts[1];
            var inputArgs = commandParts[2].ToUpper();

            if (inputOutput != inputCommandIdentificator)
                throw new ArgumentException(
                    $"command input arf \"{inputOutput}\"is not valid, must be  \"{inputCommandIdentificator}\"");

            if (commandType == "jump" || commandType == "move")
            {
                var targetCell = new Cell(inputArgs[0] - 'A', inputArgs[1] - '1');
                return new MovePawnCommand(playerPawn, grid, grid.GetPawnCell(playerPawn), targetCell);
            }
            else if (commandType == "wall")
            {
                if (inputArgs[2] != 'V' && inputArgs[2] != 'H')
                    throw new ArgumentException($"wall is neither horizontal nor vertical. command args: {inputArgs}");

                (int x, int y) wallCoords = (inputArgs[0] - 'S', inputArgs[1] - '1');

                var cell1 = new Cell(wallCoords.x, wallCoords.y);
                var cell2 = new Cell(wallCoords.x + 1, wallCoords.y);
                var cell3 = new Cell(wallCoords.x, wallCoords.y + 1);
                var cell4 = new Cell(wallCoords.x + 1, wallCoords.y + 1);

                var isVertical = inputArgs[2] == 'V';
                return new PlaceWallCommand(grid, cell1, cell2, cell3, cell4, isVertical);
            }
            else
                throw new ArgumentException($"command type \"{commandType}\"is not valid");
        }
    }
}