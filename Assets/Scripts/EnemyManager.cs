using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public List<Enemy> enemies;

    public Vector2 avoidFriends(Enemy self) {
        Vector2 steer = new Vector2(0f, 0f);
        int count = 0;
        float dist;
        Vector2 diff;

        for(int i = 0; i < enemies.Count; i++) {
            if (enemies[i] == null) {
                enemies.RemoveAt(i);
                continue;
            }
            if (self.target == enemies[i].target) {
                dist = Vector2.Distance(self.transform.position, enemies[i].transform.position);
                if (dist > 0 && dist < self.maxDistanceRadius) {
                    diff = self.transform.position - enemies[i].transform.position;
                    diff.Normalize();
                    diff /= dist;
                    steer += diff;
                }
            }
        }
        if (count > 0) steer /= (float)count;
        return steer;
    }
}
