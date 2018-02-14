using UnityEngine;

public class Road {
    public enum Direction { North, East, South, West }
    public Direction dir;
    public int length;

    public Coord startCoord;
    public Coord[] coords;

    public bool first;

    public void SetUpRoad(Road road, IntRange roadLength) {
        dir = (Direction)Random.Range(0, 4);
        length = roadLength.Random;

        if (!first) {
            startCoord = new Coord(road.EndPositionX, road.EndPositionY);
            if (dir == road.dir) {
                dir = (Direction)((int)(dir + 1) % 4);
            }
            length++;
        }

        coords = new Coord[length];
        for (int i = 0; i < length; i++) {
            switch (dir) {
                case Direction.North:
                    coords[i] = new Coord(startCoord.x, startCoord.y + i);
                    break;
                case Direction.South:
                    coords[i] = new Coord(startCoord.x, startCoord.y - i);
                    break;
                case Direction.East:
                    coords[i] = new Coord(startCoord.x + i, startCoord.y);
                    break;
                case Direction.West:
                    coords[i] = new Coord(startCoord.x - i, startCoord.y);
                    break;
            }
        }
    }

    public int EndPositionX {
        get {
            switch (dir) {
                case Direction.North:
                case Direction.South:
                    return startCoord.x;
                case Direction.East:
                    return startCoord.x + length - 1;
            }
            return startCoord.x - length + 1;
        }
    }

    public int EndPositionY {
        get {
            switch (dir) {
                case Direction.North:
                    return startCoord.y + length - 1;
                case Direction.East:
                case Direction.West:
                    return startCoord.y;
            }
            return startCoord.y - length + 1;
        }
    }
}