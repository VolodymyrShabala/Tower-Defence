using UnityEngine;

public class MapGenerator : MonoBehaviour {
    [SerializeField] private Transform groundTile;
    [SerializeField] private Transform roadTile;
    [SerializeField] private Map[] maps = new Map[1];
    [SerializeField] private int currentMapIndex;
    private Map currentMap;
    private bool[,] roadMap;
    private Transform[,] myMap;
    private Renderer[,] mapRenderer;

    private Road[] roads;
    private Vector3 scaleGround = new Vector3(.9f, .9f, .9f);
    private Vector3 scaleRoad = new Vector3(.9f, .9f, .9f);

    public delegate void MapCreated(float y);
    public event MapCreated mapCreated;

    public void CreateMap() {
        currentMap = maps[currentMapIndex];
        myMap = new Transform[currentMap.mapSize.x, currentMap.mapSize.y];

        //Object to hold the map for easier map regeneration
        string holderName = "MapHolder";
        if (transform.Find(holderName)) {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }
        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.transform.parent = transform;

        CreateRoad();
        for (int x = 0; x < currentMap.mapSize.x; x++) {
            for (int y = 0; y < currentMap.mapSize.y; y++) {
                Transform t;
                if (roadMap[x, y]) {
                    t = Instantiate(roadTile, new Vector3(x, 0, y), Quaternion.identity, mapHolder);
                    t.localScale = scaleRoad;
                } else {
                    t = Instantiate(groundTile, new Vector3(x, 0, y), Quaternion.identity, mapHolder);
                    t.localScale = scaleGround;
                }
                myMap[x, y] = t;
            }
        }

        mapRenderer = new Renderer[currentMap.mapSize.x, currentMap.mapSize.y];
        for (int x = 0; x < currentMap.mapSize.x; x++) {
            for (int y = 0; y < currentMap.mapSize.y; y++) {
                mapRenderer[x, y] = myMap[x, y].GetComponent<Renderer>();
            }
        }

        if (mapCreated != null) {
            mapCreated(currentMap.mapSize.y);
        }
    }

    private void CreateRoad() {
        roads = new Road[currentMap.roadsCount.Random];
        roadMap = new bool[currentMap.mapSize.x, currentMap.mapSize.y];

        //Roads can't be longer then a plane itself
        if (currentMap.roadLength.min >= currentMap.mapSize.x || currentMap.roadLength.min >= currentMap.mapSize.y ||
            currentMap.roadLength.max >= currentMap.mapSize.x || currentMap.roadLength.max >= currentMap.mapSize.y) {
            return;
        }

        int max = 0;
        //Creates roads
        for (int i = 0; i < roads.Length; i++) {
            max++;
            if (max >= 1000) {
                break;
            }
            Road road = new Road();
            if (i == 0) {
                road.startCoord = new Coord(Random.Range(0, currentMap.mapSize.x), Random.Range(0, currentMap.mapSize.y));
                road.first = true;
                road.SetUpRoad(road, currentMap.roadLength);
                roads[0] = road;
            } else {
                road = new Road();
                road.SetUpRoad(roads[i - 1], currentMap.roadLength);
            }
            if (!InMapBounds(road)) {
                i--;
                continue;
            }
            if (currentMap.customizeMap.roadsDoNotCross && !RoadsDoesNotCross(road)) {
                i--;
                continue;
            }
            roads[i] = road;

            for (int j = 0; j < road.coords.Length; j++) {
                roadMap[road.coords[j].x, road.coords[j].y] = true;
            }
        }
    }

    public void ChangeTileSize(float i, bool changeRoadSize) {
        for (int x = 0; x < currentMap.mapSize.x; x++) {
            for (int y = 0; y < currentMap.mapSize.y; y++) {
                if (!changeRoadSize && roadMap[x, y]) {
                    continue;
                }

                if (changeRoadSize && roadMap[x, y]) {
                    scaleRoad = Vector3.one * (1 - i);
                } else {
                    scaleGround = Vector3.one * (1 - i);
                }

                myMap[x, y].localScale = Vector3.one * (1 - i);
            }
        }
    }

    //Need to check that it is working
    public void GeneratePathNodes() {
        Transform pathRevealer = FindObjectOfType<PathRevealer>().transform;
        if (pathRevealer == null) {
            return;
        }

        Transform[] t = new Transform[pathRevealer.childCount];
        for (int i = 0; i < t.Length; i++) {
            t[i] = pathRevealer.GetChild(i);
        }
        foreach (Transform child in t) {
            DestroyImmediate(child.gameObject);
        }

        foreach (Road r in roads) {
            Instantiate(new GameObject("Path"), new Vector3(r.EndPositionX, 1, r.EndPositionY),
                        Quaternion.identity, pathRevealer);
        }
    }

    public void ChangeColorMap() {
        for (int x = 0; x < currentMap.mapSize.x; x++) {
            for (int y = 0; y < currentMap.mapSize.y; y++) {
                mapRenderer[x, y].material.color = roadMap[x, y] ? currentMap.customizeMap.roadColor :
                                       currentMap.customizeMap.planeColor;
            }
        }
    }

    //Need to check that it is working and maybe optimize a bit
    public void BlendColors() {
        for (int x = 0; x < currentMap.mapSize.x; x++) {
            for (int y = 0; y < currentMap.mapSize.y; y++) {
                if (!roadMap[x, y]) {
                    Material m = new Material(mapRenderer[x, y].material);
                    float colorPercent = (float)y / currentMap.mapSize.y;
                    m.color = Color.Lerp(currentMap.customizeMap.foreGroundColor,
                                         currentMap.customizeMap.backGroundColor, colorPercent);
                    mapRenderer[x, y].material = m;

                }
            }
        }
    }

    public void SetStartTileColor() {
        myMap[roads[0].startCoord.x, roads[0].startCoord.y].GetComponent<Renderer>().material.color =
            currentMap.customizeMap.spawnTileColor;
    }

    public void SetFinishTileColor() {
        myMap[roads[roads.Length - 1].EndPositionX, roads[roads.Length - 1].EndPositionY].GetComponent<Renderer>().
                                                                                          material.color = currentMap.customizeMap.finishTileColor;
    }

    private bool RoadsDoesNotCross(Road road) {
        for (int i = 1; i < road.coords.Length; i++) {
            if (roadMap[road.coords[i].x, road.coords[i].y]) {
                return false;
            }
        }
        return true;
    }

    private bool InMapBounds(Road road) {
        for (int i = 0; i < road.coords.Length; i++) {
            if (road.coords[i].x < 0 || road.coords[i].x >= currentMap.mapSize.x || road.coords[i].y < 0 ||
                road.coords[i].y >= currentMap.mapSize.y) {
                return false;
            }
        }
        return true;
    }

    public Map Map {
        get {
            return currentMap;
        }
    }
}

[System.Serializable] public struct CustomizeMap {
    [Range(0, 1)] public float outlLinePercent;
    public bool changeRoadSize;
    public bool roadsDoNotCross;

    public Color spawnTileColor;
    public Color finishTileColor;

    public Color planeColor;
    public Color roadColor;

    public Color foreGroundColor;
    public Color backGroundColor;
}

[System.Serializable] public struct Coord {
    public int x, y;

    public Coord(int _x, int _y) {
        x = _x;
        y = _y;
    }
}

[System.Serializable] public struct IntRange {
    public int min, max;

    public IntRange(int _min, int _max) {
        min = _min;
        max = _max;
    }

    public int Random {
        get {
            return UnityEngine.Random.Range(min, max + 1);
        }
    }
}