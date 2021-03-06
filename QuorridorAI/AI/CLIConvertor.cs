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
        private const string inputCommandIdentificator = "->";
        private const string outputCommandIdentificator = "<-";
        private static string GetCellCoord(Cell cell) => $"{(char) ('A' + cell.GridX)}{9 - cell.GridY}";

        private static string GetWallCoord(PlaceWallCommand command)
        {
            var allCells = new List<Cell>
                {command._cell1Pair1, command._cell1Pair2, command._cell2Pair1, command._cell2Pair2};
            var maxX = allCells.Max(x => x.GridX);
            var maxY = allCells.Max(x => x.GridY);
            return $"{(char) ('S' + maxX - 1)}{9 - maxY}";
        }

        public static bool IsFirstTurn(string message)
        {
            if (message != "white" && message != "black")
                throw new ArgumentException($"Invalid argument {message}");
            return message == "black";
        }

        public static string Convert(MovePawnCommand command) =>
            $"{(command.IsJump ? "jump" : "move")} {GetCellCoord(command._targetCell)}";

        public static string Convert(PlaceWallCommand command) =>
            $"wall {GetWallCoord(command)}{(command._isWallVertical ? 'v' : 'h')}";

        public static IMakeTurnCommand Parse(string command, Grid grid, Pawn playerPawn)
        {
            var commandParts = command.Split(' ');
            var commandType = commandParts[0];
            var inputArgs = commandParts[1].ToUpper();


            if (commandType == "jump" || commandType == "move")
            {
                var targetCell = grid.GetCellByCoordinates(inputArgs[0] - 'A', '9' - inputArgs[1]);
                return new MovePawnCommand(playerPawn, grid, grid.GetPawnCell(playerPawn), targetCell);
            }
            else if (commandType == "wall")
            {
                if (inputArgs[2] != 'V' && inputArgs[2] != 'H')
                    throw new ArgumentException($"wall is neither horizontal nor vertical. command args: {inputArgs}");

                (int x, int y) wallCoords = (inputArgs[0] - 'S', '8' - inputArgs[1]);

                var cell1 = grid.GetCellByCoordinates(wallCoords.x, wallCoords.y);
                var cell2 = grid.GetCellByCoordinates(wallCoords.x + 1, wallCoords.y);
                var cell3 = grid.GetCellByCoordinates(wallCoords.x, wallCoords.y + 1);
                var cell4 = grid.GetCellByCoordinates(wallCoords.x + 1, wallCoords.y + 1);

                var isVertical = inputArgs[2] == 'V';
                if (isVertical)
                    return new PlaceWallCommand(grid, cell1, cell2, cell3, cell4, isVertical);
                else
                    return new PlaceWallCommand(grid, cell1, cell3, cell2, cell4, isVertical);
            }
            else
                throw new ArgumentException($"command type \"{commandType}\"is not valid");
        }
    }
}