using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace AOC2024
{
    internal class Day09: IAdventSolution
    {
        class FileBlock
        {
            private int capacity;
            private string index;

            public int Capacity
            {
                get {return capacity;}
            }
            
            public FileBlock(int capacity, string index)
            {
                this.capacity = capacity;
                this.index = index;
            }

            public FileBlock Clone()
            {
                return new FileBlock(this.capacity, this.index);
            }

            public void ShrinkBy(int count)
            {
                capacity -= count;
            }

            public bool IsEmpty()
            {
                return capacity == 0;
            }

            public bool IsFree()
            {
                return index == null;
            }

            public bool HasCapacityFor(FileBlock other)
            {
                return this.capacity >= other.capacity;
            }

            public FileBlock MoveFrom(FileBlock other)
            {
                var remainingCapacity = this.capacity - other.capacity;

                this.index = other.index;
                this.capacity = other.capacity;

                if (remainingCapacity == 0)
                {
                    return null;
                }

                return new FileBlock(remainingCapacity, null);
            }

            public int MovePartFrom(FileBlock other)
            {
                this.index = other.index;

                return this.capacity;
            }

            public bool SameAs(FileBlock other)
            {
                return this.index == other.index;
            }

            public void Delete()
            {
                index = null;
            }

            public long Checksum(int m)
            {
                if (this.index == null)
                {
                    return 0;
                }
                long chekcsum = 0;
                long index = long.Parse(this.index);
                for (int i = 0; i < capacity; i++)
                {
                    chekcsum += index * m++;
                }

                return chekcsum;
            }

            public override string ToString()
            {
                if (index == null)
                {
                    return new string('.', capacity);
                }

                return index + "x" + capacity.ToString();
            }
        }


        public (long, long) Execute(string[] lines)
        {
            var disk = lines[0]
                .Select((c, index) => index % 2 == 0 ? new FileBlock(c - '0', (index / 2).ToString()) : new FileBlock(c - '0', null))
                .Where(block => !block.IsEmpty())
                .ToList();          

            
            var diskA = FragmentationA(disk);
            var diskB = FragmentationB(disk);

            long part1 = 0;
            long part2 = 0;
            int m = 0;
            foreach (var block in diskA)
            {
                part1 += block.Checksum(m);
                m += block.Capacity;
            }

            m = 0;
            foreach (var block in diskB)
            {
                part2 += block.Checksum(m);
                m += block.Capacity;
            }

            return (part1, part2);
        }

        private List<FileBlock> FragmentationA(List<FileBlock> defragmentedDisk)
        {
            var disk = defragmentedDisk.Select(block => block.Clone()).ToList();

            int freeBlockIdx;
            int fullBlockIdx;
            do {            
                freeBlockIdx = GetFeeIndex(disk);
                fullBlockIdx = GetFilledIndex(disk);

                if (disk[freeBlockIdx].HasCapacityFor(disk[fullBlockIdx]))
                {
                    var freeRemaingSpace = disk[freeBlockIdx].MoveFrom(disk[fullBlockIdx]);
                    disk.RemoveAt(fullBlockIdx);

                    if (freeRemaingSpace != null)
                    {
                        disk.Insert(freeBlockIdx + 1, freeRemaingSpace);
                    }
                    continue;
                }
               
                int movedCount = disk[freeBlockIdx].MovePartFrom(disk[fullBlockIdx]);

                disk[fullBlockIdx].ShrinkBy(movedCount);
                
            } while(freeBlockIdx < fullBlockIdx);

            return disk;
        }

        private List<FileBlock> FragmentationB(List<FileBlock> defragmentedDisk)
        {
            var disk = defragmentedDisk.Select(block => block.Clone()).ToList();

            var writtenFileBlocks = disk.Where(block => !block.IsFree()).Reverse().ToList();

            foreach (var fileBlock in writtenFileBlocks)
            {
                for (int i = 0; i < disk.Count; i++)
                {
                    if (disk[i].SameAs(fileBlock))
                    {
                        break;
                    }

                    if (disk[i].IsFree() && disk[i].HasCapacityFor(fileBlock))
                    {
                        var freeRemaingSpace = disk[i].MoveFrom(fileBlock);
                        fileBlock.Delete();

                        if (freeRemaingSpace != null)
                        {
                            disk.Insert(i + 1, freeRemaingSpace);
                        }
                        break;
                    }
                }
            }

            return disk;
        }

        private int GetFeeIndex(List<FileBlock> blocks)
        {
            for (int i = 0; i < blocks.Count(); i++)
            {
                if (blocks[i].IsFree())
                {
                    return i;
                }
            }

            throw new Exception("co sem dame");
        }

        private int GetFilledIndex(List<FileBlock> blocks)
        {
             for (int i = blocks.Count() - 1; i >= 0; i--)
            {
                if (!blocks[i].IsFree())
                {
                    return i;
                }
            }

            throw new Exception("co sem dame");
        }

    }
}
