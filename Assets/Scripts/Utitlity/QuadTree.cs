using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    
public struct DistanceAndPoint
{
    public Vector2 Point;
    public float Distance;

    public DistanceAndPoint(float distance, Vector2 point)
    {
        Distance = distance;
        Point = point;
    }
}

public class QuadTree
{
    enum Area
    {
        LT,
        RT,
        LB,
        RB
    }

    private readonly QuadTree[] children;
    public readonly Vector2 Value;

    public QuadTree(Vector2 value)
    {
        Value = value;
        children = new QuadTree[4];
    }

    public void Insert(Vector2 point)
    {
        if (Value.Equals(point)) return;

        var area = Detect(point);
        var areaIndex = (int) area;
        if (children[areaIndex] == null)
            children[areaIndex] = new QuadTree(point);
        else
            children[areaIndex].Insert(point);
    }

    public DistanceAndPoint NearestFrom(Vector2 point, DistanceAndPoint nearest)
    {
        var result = nearest;
        // 自分との距離
        var dist = (Value - point).sqrMagnitude;

        // 現在の最小距離の範囲にある領域が探索候補
        if (dist < result.Distance)
        {
            result = new DistanceAndPoint(dist, Value);
        }

        // 存在する可能性のある Area を探索
        var areaFlags = new bool[] {false, false, false, false};
        var index = (int) Detect(new Vector2(point.x + result.Distance, point.y));
        areaFlags[index] = true;
        index = (int) Detect(new Vector2(point.x - result.Distance, point.y));
        areaFlags[index] = true;
        index = (int) Detect(new Vector2(point.x, point.y + result.Distance));
        areaFlags[index] = true;
        index = (int) Detect(new Vector2(point.x, point.y - result.Distance));
        areaFlags[index] = true;

        for (var i = 0; i < 4; i++)
        {
            if (!areaFlags[i]) continue;
            var tree = children[i];

            if (tree == null) continue;

            var childResult = tree.NearestFrom(point, result);
            var d = childResult.Distance;

            if (d >= result.Distance) continue;

            result = childResult;
        }

        return result;
    }

    private Area Detect(Vector2 point)
    {
        if (point.x < Value.x)
            return point.y < Value.y ? Area.LB : Area.LT;
        else
            return point.y < Value.y ? Area.RB : Area.RT;
    }
}