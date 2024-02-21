using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src
{
    public class RoomGenerator
    {
        private int roomWidth, roomHeight;
        private MetaTile[,] roomLayout;
        private Queue<(int x, int y, Connectivity directionFrom)> expansionQueue = new();
        private Random rng = new Random();

        public RoomGenerator(int roomWidth, int roomHeight)
        {
            this.roomWidth = roomWidth;
            this.roomHeight = roomHeight;
            this.roomLayout = new MetaTile[roomWidth, roomHeight];
        }

        public Schematic GenerateSchematic(List<MetaTile> metaTiles = null)
        {
            bool usePredefinedMetatiles = metaTiles != null && metaTiles.Any();
            int metatilePlaced;
            int tries = 0;
            do
            {
                metatilePlaced = 0;
                tries++;
                expansionQueue.Clear();
                roomLayout = new MetaTile[roomWidth, roomHeight];
                Debug.WriteLine($"try #{tries}");
                // Initialize with a starting MetaTile that opens in at least one direction
                int startX = rng.Next(roomWidth);
                int startY = 0;
                Debug.WriteLine("starting at " + startX + ", " + startY);
                if (usePredefinedMetatiles)
                {
                    // Select a random metatile from the provided list
                    roomLayout[startX, startY] = metaTiles[rng.Next(metaTiles.Count)].DeepCopy(); // Use DeepCopy if necessary to avoid modifying the original
                }
                else
                {
                    Connectivity startDirection = (Connectivity)rng.Next(1, 16); // Exclude None, include All
                    roomLayout[startX, startY] = MetaTile.GenerateMetaTile(startDirection);
                }

                // Add open directions to the queue for the starting tile
                AddToQueue(startX, startY, roomLayout[startX, startY].Connectivity);

                while (expansionQueue.Count > 0)
                {
                    var (x, y, directionFrom) = expansionQueue.Dequeue();
                    Debug.WriteLine("continuing at " + x + ", " + y);

                    TryExpand(x, y, directionFrom, metaTiles);
                    metatilePlaced++;
                }
            } while (metatilePlaced < Math.Min(roomWidth, roomHeight) && tries < 5);

            // Fill the rest with closed tiles
            FillRemainingWithClosedTiles();

            return new Schematic(roomLayout);
        }

        private void AddToQueue(int x, int y, Connectivity direction)
        {
            // This method now adds all possible expansion directions to the queue based on the current tile's connectivity
            foreach (var directionFlag in ConnectivityExtensions.GetIndividualFlags(direction))
            {
                var (nextX, nextY) = GetNextCoordinates(x, y, directionFlag);
                if (IsInBounds(nextX, nextY) && roomLayout[nextX, nextY] == null)
                {
                    expansionQueue.Enqueue((nextX, nextY, GetOppositeDirection(directionFlag)));
                }
            }
        }
        private static Connectivity GetOppositeDirection(Connectivity direction)
        {
            switch (direction)
            {
                case Connectivity.Left: return Connectivity.Right;
                case Connectivity.Right: return Connectivity.Left;
                case Connectivity.Top: return Connectivity.Bottom;
                case Connectivity.Bottom: return Connectivity.Top;
                default: return Connectivity.None;
            }
        }
        private void TryExpand(int x, int y, Connectivity directionFrom, List<MetaTile> metaTiles = null)
        {
            bool usePredefinedMetatiles = metaTiles != null && metaTiles.Any();

            if (!IsInBounds(x, y) || roomLayout[x, y] != null) return;

            if (usePredefinedMetatiles)
            {
                // Select a random metatile from the list, ensuring it can connect in the required direction
                MetaTile selectedTile = null;
                var compatibleTiles = metaTiles.Where(mt => ConnectivityExtensions.GetIndividualFlags(mt.Connectivity).Contains(directionFrom)).ToList();
                if (compatibleTiles.Count != 0)
                {
                    selectedTile = compatibleTiles[rng.Next(compatibleTiles.Count)].DeepCopy(); // Use DeepCopy to avoid modifying the original
                }
                roomLayout[x, y] = selectedTile ?? MetaTile.GenerateMetaTile(Connectivity.None); // Fallback to a closed tile if no compatible tile found
            }
            else
            {
                // Original logic for generating a new metatile based on connectivity
                Connectivity newTileConnectivity = directionFrom | GetRandomConnectivityExcluding(directionFrom);
                roomLayout[x, y] = MetaTile.GenerateMetaTile(newTileConnectivity);
            }

            // Proceed with adding to the queue using the newly placed tile's connectivity
            if (roomLayout[x, y] != null)
            {
                AddToQueue(x, y, roomLayout[x, y].Connectivity);
            }
        }


        private static (int, int) GetNextCoordinates(int x, int y, Connectivity direction)
        {
            switch (direction)
            {
                case Connectivity.Left: return (x - 1, y);
                case Connectivity.Right: return (x + 1, y);
                case Connectivity.Top: return (x, y - 1);
                case Connectivity.Bottom: return (x, y + 1);
                default: return (x, y); // Should never happen
            }
        }

        private bool IsInBounds(int x, int y) => x >= 0 && x < roomWidth && y >= 0 && y < roomHeight;

        private Connectivity GetRandomConnectivityExcluding(Connectivity exclude)
        {
            // Generate a random connectivity that does not include 'exclude'. This ensures variety in connectivity.
            Connectivity result;
            do
            {
                result = (Connectivity)rng.Next(1, 16);
            } while (result == exclude || !ConnectivityExtensions.GetIndividualFlags(result).Contains(exclude));
            return result;
        }

        private void FillRemainingWithClosedTiles()
        {
            for (int x = 0; x < roomWidth; x++)
            {
                for (int y = 0; y < roomHeight; y++)
                {
                    if (roomLayout[x, y] == null)
                    {
                        roomLayout[x, y] = MetaTile.GenerateMetaTile(Connectivity.None); // Closed tile
                    }
                }
            }
        }
    }
    public class Schematic
    {
        MetaTile[,] tiles;
        public Schematic(int roomWidth, int roomHeight)
        {
            tiles = new MetaTile[roomWidth, roomHeight];

        }
        public Schematic(MetaTile[,] tiles) => this.tiles = tiles;

        public MetaTile this[int x, int y]
        {
            get => tiles[x, y];
            set => tiles[x, y] = value;
        }
        public int Width => tiles.GetLength(0);
        public int Height => tiles.GetLength(1);
        //public Bitmap GenerateBitmap()
        //{
        //    int roomWidth = tiles.GetLength(0);
        //    int roomHeight = tiles.GetLength(1);
        //    Bitmap roomBitmap = new(roomWidth * MetaTile.META_TILE_SIZE, roomHeight * MetaTile.META_TILE_SIZE);
        //    for (int x = 0; x < roomWidth; x++)
        //    {
        //        for (int y = 0; y < roomHeight; y++)
        //        {
        //            MetaTile tile = tiles[x, y];
        //            if (tile != null)
        //            {
        //                for (int i = 0; i < MetaTile.META_TILE_SIZE; i++)
        //                {
        //                    for (int j = 0; j < MetaTile.META_TILE_SIZE; j++)
        //                    {
        //                        bool isCheckerBlack = (x + y) % 2 == 0; // Black for even sums, green for odd sums
        //                        Color color = isCheckerBlack ? Color.Black : Color.Black;

        //                        // Assuming the metaTile size matches the Cells array size for simplicity
        //                        // Check if the cell is a platform for color, otherwise use the checkerboard logic
        //                        color = tile[i, j] == TileType.Platform ? color : Color.White;
        //                        roomBitmap.SetPixel(x * MetaTile.META_TILE_SIZE + i, y * MetaTile.META_TILE_SIZE + j, color);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                // Fill the missing metaTile area with white
        //                for (int i = 0; i < MetaTile.META_TILE_SIZE; i++)
        //                {
        //                    for (int j = 0; j < MetaTile.META_TILE_SIZE; j++)
        //                    {
        //                        roomBitmap.SetPixel(x * MetaTile.META_TILE_SIZE + i, y * MetaTile.META_TILE_SIZE + j, Color.White);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return roomBitmap;
        //}
    }
    public class MetaTile
    {
        public const int META_TILE_SIZE = 10;
        public Connectivity Connectivity { get; set; }
        public TileType[] Tiles { get; set; }
        public MetaTile(Connectivity connectivity) : this()
        {
            Connectivity = connectivity;
            InitializeWalls();
        }
        public MetaTile()
        {
            Tiles = new TileType[META_TILE_SIZE * META_TILE_SIZE]; // Initialize as a 1D array
            Connectivity = Connectivity.None;
            ResetTiles();
        }

        // Update the indexer to work with a 1D array
        public TileType this[int x, int y]
        {
            get => Tiles[y * META_TILE_SIZE + x];
            set => Tiles[y * META_TILE_SIZE + x] = value;
        }

        // Adjust DeepCopy method for 1D array
        public MetaTile DeepCopy()
        {
            MetaTile copy = new();
            Array.Copy(this.Tiles, copy.Tiles, this.Tiles.Length); // Use Array.Copy for efficiency

            copy.Connectivity = this.Connectivity;

            return copy;
        }

        // Adjust ResetTiles method for 1D array
        public void ResetTiles()
        {
            for (int i = 0; i < Tiles.Length; i++)
            {
                Tiles[i] = TileType.Air; // Reset each tile to Air
            }

            // Reset all openings to true
            Connectivity = Connectivity.All;
        }
        private void InitializeWalls()
        {
            // Calculate start and end points for the opening in the middle of each wall
            int openingStart = META_TILE_SIZE / 5; // Start at 25% to create a 50% opening
            int openingEnd = META_TILE_SIZE - openingStart; // End at 75%
            if (Connectivity == Connectivity.None)
            {
                for (int x = 0; x < META_TILE_SIZE; x++)
                {
                    for (int y = 0; y < META_TILE_SIZE; y++)
                    {
                        this[x, y] = TileType.Platform;
                    }
                }
            }
            else
            {
                for (int x = 0; x < META_TILE_SIZE; x++)
                {
                    for (int y = 0; y < META_TILE_SIZE; y++)
                    {
                        // Determine if we're on an edge
                        bool isOnEdge = x == 0 || x == META_TILE_SIZE - 1 || y == 0 || y == META_TILE_SIZE - 1;

                        // Check for connectivity and if the current position falls within the opening range
                        bool shouldHaveOpening = (
                            (x == 0 && Connectivity.HasFlag(Connectivity.Left) && (y >= openingStart && y < openingEnd)) ||
                            (x == META_TILE_SIZE - 1 && Connectivity.HasFlag(Connectivity.Right) && (y >= openingStart && y < openingEnd)) ||
                            (y == 0 && Connectivity.HasFlag(Connectivity.Top) && (x >= openingStart && x < openingEnd)) ||
                            (y == META_TILE_SIZE - 1 && Connectivity.HasFlag(Connectivity.Bottom) && (x >= openingStart && x < openingEnd))
                        );

                        // If we're on an edge but not within the opening range, it's a wall
                        if (isOnEdge && !shouldHaveOpening)
                        {
                            this[x, y] = TileType.Platform;
                        }
                        else
                        {
                            this[x, y] = TileType.Air; // Everything else is air, including the hole
                        }
                    }
                }
            }
        }
        public static MetaTile GenerateMetaTile(Connectivity connectivity)
        {
            return new MetaTile(connectivity);
        }
    }
    [Flags]
    public enum Connectivity
    {
        None = 0,
        Left = 1,
        Right = 2,
        Top = 4,
        Bottom = 8,
        All = Left | Right | Top | Bottom
    }
    public enum TileType
    {
        Air,
        Platform,
        Portal
    }
    public static class ConnectivityExtensions
    {
        public static IEnumerable<Connectivity> GetIndividualFlags(Connectivity connectivity)
        {
            foreach (Connectivity flag in Enum.GetValues(typeof(Connectivity)))
            {
                if (flag == Connectivity.None)
                {
                    continue; // Skip the 'None' value
                }

                if (connectivity.HasFlag(flag))
                {
                    yield return flag;
                }
            }
        }
    }
}
