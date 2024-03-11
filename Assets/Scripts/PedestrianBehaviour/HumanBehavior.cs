using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolyPerfect.City
{
    [RequireComponent(typeof(PathFinding)),RequireComponent(typeof(Rigidbody))]
    public class HumanBehavior : MonoBehaviour
    {
        [HideInInspector]
        private List<Path> trajectory = new List<Path>();
        private PathFinding pathFinding;
        private Animator animator;
        private float maxspeed = 5.0f;
        private bool randomDestination = true;
        private float speed;
        private int activepoint = 0;
        private int activePath = 0;
        private float rotationSpeed = 2.5f;
        bool isMoving = false;
        private Vector3 targetPoint;
        private Vector3 destination;
        private Vector3 start;
        private static readonly int Speed = Animator.StringToHash("speed");
        private static readonly int Walking = Animator.StringToHash("Walking");

        private void Awake()
        {
            pathFinding = GetComponent<PathFinding>();
            animator = GetComponent<Animator>();
        }
        void Start()
        {
            
            maxspeed = Random.Range(1f, 1.5f);
            start = transform.position;
            if (randomDestination)
            {
                //Selects random tile which is at least 60m away and less then 300m
                SetRandomDestination();
            }
            trajectory = pathFinding.GetPath(start, destination, PathType.Sidewalk);
            if (trajectory == null) trajectory = CheckTrajectory();
        

            if (trajectory != null)
            {
                isMoving = true;
                GetClosestPoint();
                targetPoint = trajectory[0].pathPositions[activepoint].transform.position;
                start = transform.position;
                animator.SetBool(Walking, true);
            }
            else
            {
                Debug.Log("path not found");
            }

        }

        // recursive function to ensure a trajectory gets created (because sometimes pathFinding.GetPath() will return a null)
        private List<Path> CheckTrajectory()
        {
            if (randomDestination)
            {
                SetRandomDestination();
            }

            trajectory = pathFinding.GetPath(start, destination, PathType.Sidewalk);
            if (trajectory != null) return trajectory;
          
            return CheckTrajectory();
        }

        void FixedUpdate()
        {
            if (isMoving)
            {
                if (Vector3.Distance(targetPoint , transform.position) < 0.1f)
                {
                    MoveToNextPoint();
                }
                Vector3 direction = targetPoint - transform.position;
                Quaternion rotation = Quaternion.LookRotation(targetPoint - transform.position);

                speed = Mathf.Lerp(speed, maxspeed, Time.deltaTime);
                if (speed > maxspeed)
                {
                    speed = Mathf.Lerp(speed, maxspeed, 10 * Time.deltaTime);
                }

                var transform1 = transform;
                Vector3 newPosition = transform1.position + (direction.normalized * (speed * Time.deltaTime));
                transform1.position = newPosition;

                if (direction != Vector3.zero)
                {
                    direction.y = 0;
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
                }
            }
            else
            {
                speed = 0;
            }
            animator.SetFloat(Speed,speed * 0.8f);
        }
        public void MoveToNextPoint()
        {
            if (activePath == trajectory.Count - 1)
            {
                if (activepoint == trajectory[activePath].pathPositions.Count - 1)
                {
                    isMoving = false;
                    if (randomDestination)
                    {
                        //Selects random tile which is at least 90m away 
                        SetRandomDestination();
                    }
                    else
                    {
                        destination = start;
                        start = transform.position;
                    }
                    trajectory = pathFinding.GetPath(start,destination,PathType.Sidewalk);
                    if (trajectory != null)
                    {
                        activePath = 0;
                        activepoint = 0;
                        GetClosestPoint();
                        speed = 0;
                        isMoving = true;
                    }
                    else
                    {
                        Debug.Log(name + ": Path not found");
                    }
                }
                else
                {
                    activepoint++;
                }
            }
            else
            {
                if (activepoint == trajectory[activePath].pathPositions.Count - 1)
                {
                    activePath++;
                   /* if (trajectory[activePath].speed < maxspeed)
                    {
                        maxspeed = trajectory[activePath].speed;
                    }
                    else
                    {
                        currentMaxSpeed = maxspeed;
                    }*/

                    activepoint = 1;
                }
                else
                {
                    activepoint++;
                }
            }
            if(trajectory != null)
                targetPoint = trajectory[activePath].pathPositions[activepoint].transform.position + (trajectory[activePath].pathPositions[activepoint].transform.right * Random.Range(-0.8f,0.8f));
        }
        private void SetRandomDestination()
        {
            start = transform.position;
            destination = start;
            while (Vector3.Distance(start, destination) < 60 || Vector3.Distance(start, destination) > 300)
            {
                Tile t = Tile.tiles[UnityEngine.Random.Range(0, Tile.tiles.Count - 1)];
                if (t != null && (t.tileType == Tile.TileType.Road || t.tileType == Tile.TileType.OnlyPathwalk))
                {
                    if (t.verticalType == Tile.VerticalType.Bridge)
                    {
                        destination = t.transform.position + (Vector3.up * 12);
                    }
                    else
                    {
                        destination = t.transform.position;
                    }
                }
            }
        }
        private void GetClosestPoint()
        {
            float minDistance = Mathf.Infinity;
            for (int i = 0; i < trajectory[activePath].pathPositions.Count; i++)
            {
                float distance = Vector3.Distance(trajectory[activePath].pathPositions[i].position, transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    activepoint = i;
                }
            }
        }
        
    }
}
