using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PredictionManager : Singleton<PredictionManager>
{
    // predict trajectory of object
    public GameObject obstacles;
    public int maxIterations;
    
    Scene currentScene;
    Scene predictionScene;
    
    PhysicsScene2D currentPhysicsScene;
    PhysicsScene2D predictionPhysicsScene;
    
    List<GameObject> dummyObstacles = new List<GameObject>();
    private Vector3[] posOfPredictionPoints;
    GameObject dummy;

    public GameObject cam;
    private void Start()
    {
        Physics2D.simulationMode = SimulationMode2D.Script;

        currentScene = SceneManager.GetActiveScene();
        currentPhysicsScene = currentScene.GetPhysicsScene2D();

        CreateSceneParameters parameters = new CreateSceneParameters(LocalPhysicsMode.Physics2D);
        predictionScene = SceneManager.CreateScene("PredictionScene", parameters);
        predictionPhysicsScene = predictionScene.GetPhysicsScene2D();

        posOfPredictionPoints = new Vector3[maxIterations];
        
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void FixedUpdate()
    {
        if (currentPhysicsScene.IsValid())
        {
            currentPhysicsScene.Simulate(Time.fixedDeltaTime);
        }
    }

    public void CopyAllObstacles()
    {
        foreach (Transform t in obstacles.transform)
        {
            if (t.gameObject.GetComponent<BoxCollider2D>() != null)
            {
                GameObject fakeT = Instantiate(t.gameObject);
                fakeT.transform.position = t.position;
                fakeT.transform.rotation = t.rotation;
                SpriteRenderer fakeSR = t.GetComponent<SpriteRenderer>();
                if (fakeSR)
                {
                    fakeSR.enabled = false;
                }
                SceneManager.MoveGameObjectToScene(fakeT, predictionScene);
                dummyObstacles.Add(fakeT);
            }
        }
    }
    
    private void DestroyAllObstacles()
    {
        foreach (GameObject g in dummyObstacles)
        {
            Destroy(g);
        }
        dummyObstacles.Clear();
    }

    public Vector3[] Predict(GameObject subject, Vector3 currentPosition, Vector3 force)
    {
        Common.Log(cam.transform.position.y);
        for (int i = 0; i < obstacles.transform.childCount; i++)
        {
            obstacles.transform.GetChild(i).position = new Vector3(obstacles.transform.GetChild(i).position.x, cam.transform.position.y, obstacles.transform.GetChild(i).position.z);
        }
        if (currentPhysicsScene.IsValid() && predictionPhysicsScene.IsValid())
        {
            if (dummy == null)
            {
                dummy = Instantiate(subject, currentPosition, Quaternion.identity);
                SceneManager.MoveGameObjectToScene(dummy, predictionScene);
            }

            dummy.transform.position = currentPosition;
            dummy.GetComponent<Rigidbody2D>().velocity = force;

            for (int i = 0; i < maxIterations; i++)
            {
                predictionPhysicsScene.Simulate(Time.fixedDeltaTime);
                posOfPredictionPoints[i] = dummy.transform.position;
            }
            Destroy(dummy);
        }
        
        return posOfPredictionPoints;
    }
    

    private void OnDestroy()
    {
        DestroyAllObstacles();
    }
}
