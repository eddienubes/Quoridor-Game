using System;
using System.Collections.Generic;
using System.Linq;

namespace Quorridor.Model.Commands
{
    public static class CommandsSerializer
    {
        #region Misc
        private enum CommandType : short
        {
            PlaceWall,
            MovePawn
        }

        private const int CellSize = sizeof(int) * 2;

        #endregion

        #region DeSerialization

        private static Cell DeserializeCell(byte[] bytes, int index, Grid grid)
        {
            var xCoord = BitConverter.ToInt32(bytes, index);
            var yCoord = BitConverter.ToInt32(bytes, index + sizeof(int));
            return grid.GetCellByCoordinates(xCoord, yCoord);
        }

        private static PlaceWallCommand DeserializeWallCommand(Game game, byte[] bytes, int index)
        {
            var isVertical = BitConverter.ToBoolean(bytes, index);
            index += sizeof(bool);
            var cell11 = DeserializeCell(bytes, index, game.Grid);
            index += CellSize;
            var cell21 = DeserializeCell(bytes, index, game.Grid);
            index += CellSize;
            var cell12 = DeserializeCell(bytes, index, game.Grid);
            index += CellSize;
            var cell22 = DeserializeCell(bytes, index, game.Grid);
            return new PlaceWallCommand(game.Grid, cell11, cell21, cell12, cell22, isVertical);
        }

        private static MovePawnCommand DeserializeMovePawnCommand(Game game, byte[] bytes, int index)
        {
            var pawnId = BitConverter.ToInt32(bytes, index);
            index += sizeof(bool);
            var cell1 = DeserializeCell(bytes, index, game.Grid);
            index += CellSize;
            var cell2 = DeserializeCell(bytes, index, game.Grid);

            var pawn = game.Players.First(x => x.Pawn.PlayerId == pawnId).Pawn;
            return new MovePawnCommand(pawn, game.Grid, cell1, cell2);
        }

        public static IMakeTurnCommand Deserialize(Game game, byte[] bytes)
        {
            if (bytes.Length < sizeof(short) + sizeof(int))
                throw new ArgumentException("Too small packet!");

            int index = sizeof(int);
            CommandType commandType = (CommandType)BitConverter.ToInt16(bytes, index);
            index += sizeof(short);
            return commandType switch
            {
                CommandType.MovePawn => DeserializeMovePawnCommand(game, bytes, index),
                CommandType.PlaceWall => DeserializeWallCommand(game, bytes, index),
                _ => default
            };
        }

        #endregion

        #region Serialization

        private static IEnumerable<byte> Serialize(Cell cell)
        {
            var xBytes = BitConverter.GetBytes(cell.GridX);
            var yBytes = BitConverter.GetBytes(cell.GridY);
            return xBytes.Concat(yBytes);
        }

        private static IEnumerable<byte> Serialize(PlaceWallCommand command) =>
            BitConverter.GetBytes((short)CommandType.PlaceWall)
                .Concat(BitConverter.GetBytes(command._isWallVertical))
                .Concat(Serialize(command._cell1Pair1))
                .Concat(Serialize(command._cell2Pair1))
                .Concat(Serialize(command._cell1Pair2))
                .Concat(Serialize(command._cell2Pair2))
                .ToArray();

        private static IEnumerable<byte> Serialize(MovePawnCommand command)
            => BitConverter.GetBytes((short)CommandType.MovePawn)
                .Concat(BitConverter.GetBytes(command.playerPawn.PlayerId))
                .Concat(Serialize(command._startCell))
                .Concat(Serialize(command._targetCell))
                .ToArray();

        public static byte[] Serialize(IMakeTurnCommand command)
        {
            IEnumerable<byte> bytes;
            if (command is PlaceWallCommand wallCommand)
            {
                bytes = Serialize(wallCommand);
            }
            else if (command is MovePawnCommand pawnCommand)
            {
                bytes = Serialize(pawnCommand);
            }
            else
                throw new ArgumentException($"Unhandled command {command}");

            return BitConverter.GetBytes(bytes.Count()).Concat(bytes).ToArray();
        }

        #endregion
    }
}