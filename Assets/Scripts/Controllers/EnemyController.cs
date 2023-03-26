
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public MapGrid _MapGrid;
    private Enemy[] _listEnemy;
    public Player[] _listplayers;
    private PathFinder _finder;
    private float speed = 5f;
    private float bulletspeed = 8f;
    public GameObject _test;

    private void Awake()
    {
        _listEnemy = FindObjectsOfType<Enemy>();
        _listplayers = FindObjectsOfType<Player>();
        _finder = new PathFinder(_MapGrid);

    }

    private void Start()
    {
        foreach (var player in _listplayers)
        {
            _MapGrid.datacell[player.transform.position].walkable = true;
        }
    }

     // Update is called once per frame
     private void Update()
     {
         if (Input.GetMouseButtonDown(0))
         {
             StartCoroutine(StartMove(_listEnemy));


         }
     }
    private IEnumerator StartMove(Enemy[] enemies)
    {
        foreach (var enemy in enemies)
        {
            var target = SetTargetPlayer(_listplayers, enemy.transform.position);
            /*if (target.transform.position.x <= enem.transform.position.x)                    это поворот врага лицом
            {
                enem._Renderer.flipX = true;
            }
            else
            {
                enem._Renderer.flipX = false;
            }*/
            var path =_finder.GetPath(enemy.transform.position,target.transform.position);
            yield return (StartCoroutine(Move(enemy, path,target)));

        }
    }
    private IEnumerator Move(Enemy start,List<Vector2> path, Player player)
    {

           
        Vector2 pos = start.transform.position;
        for (int i = path.Count-1; i >=path.Count-5; i--) //tut meniau
        {
            if(i<0)
                break;
            if (pos == path[1])
                break;
            while ((Vector2)start.transform.position != new Vector2(path[i].x, path[i].y))
            {
                var step =  speed * Time.deltaTime;
                start.transform.position=Vector2.MoveTowards(start.transform.position, path[i],step);
                yield return null;
            }

        }
        EnemyAttack(player,start);


    }

    private Player SetTargetPlayer( Player[] players, Vector2 enemy) // vrode nahodit
    {
        var distance = Vector2.Distance(enemy, players[0].transform.position);
        var tagret = players[0];
        foreach (var pl in players)
        {
            var dis = Vector2.Distance(enemy, pl.transform.position);
            if (dis < distance)
            {
                distance = dis;
                tagret = pl;
            }
        }
        return tagret;
    }
    
    private void EnemyAttack(Player player,Enemy enemy)
    {
      var bullet =  Instantiate(_test, enemy.transform.position, Quaternion.identity);
      StartCoroutine(BulletMove(player, bullet));

    }
    private IEnumerator BulletMove(Player target, GameObject bullet)
    {
        while (bullet.transform.position != target.transform.position)
        {
                var step =  bulletspeed * Time.deltaTime;
                bullet.transform.position=Vector2.MoveTowards(bullet.transform.position, target.transform.position,step);
                yield return null;
        }
    }
}
