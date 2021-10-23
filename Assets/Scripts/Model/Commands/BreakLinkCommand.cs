namespace graph_sandbox.Commands
{
    using System;
    using System.Linq;

    public class BreakLinkCommand : ICommand
    {
        private Cell linker, linked;
        private int index;

        public BreakLinkCommand(Cell linker, int index)
        {
            this.linker = linker; 
            this.index = index;
            if (index < 0 || index >= linker.Neighbors.Length)
            {
                throw new IndexOutOfRangeException("index is out of neighbour array bounds");
            }
        }

        public void Execute()
        {
            linked = linker.Neighbors[index];
            linker.Neighbors[index] = null;
        }

        public void Undo()
        {
            linker.Neighbors[index] = linked;
        }
    }
}