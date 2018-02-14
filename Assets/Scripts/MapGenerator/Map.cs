[System.Serializable]
public class Map {
    public Coord mapSize = new Coord(30, 30);
    public IntRange roadLength = new IntRange(3, 10);
    public IntRange roadsCount = new IntRange(3, 15);
    public CustomizeMap customizeMap;
}