using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [System.NonSerialized] public List<int> availableSpots;

    #region Last Coin Position System For Spawning Meteors On Coins
    private Vector3 lastCoinPosition;
    [System.NonSerialized] public bool coinsSpawning;
    public void setLastCoinPosition(Vector3 positionFromCoin)
    {
        lastCoinPosition = positionFromCoin;
    }
    public Vector3 getLastCoinPosition()
    {
        return lastCoinPosition;
    }
    #endregion

    #region Define Spawner Availablity System
    private enum spawnersEnum { meteor, coin };
    [SerializeField] private List<GameObject> spawners;
    //checks on Void Start
    private void disableSpawner(spawnersEnum projectile)
    {
        spawners[(int)projectile].SetActive(false);
    }
    #endregion

    #region Coin Variables
    [Header("Coins")]
    [SerializeField] private bool spawnCoinsAtStart = true;
    [SerializeField] private float minCoinDuration = 3f;
    [SerializeField] private float maxCoinDuration = 4f;
    [SerializeField] private float minCoinWaitingTime = 3f;
    [SerializeField] private float maxCoinWaitingTime = 4f;
    private TrapSpawnerScript coinTrapSpawner;
    #endregion

    #region Meteor Variables
    [Header("Meteors")]
    [SerializeField] private bool spawnMeteorAtStart = true;
    private TrapSpawnerScript meteorTrapSpawner;
    #endregion

    #region Unity CallBack Functions
    private void Awake()
    {
        coinTrapSpawner = Utils.findComponent<TrapSpawnerScript>("Coin Spawner");
        meteorTrapSpawner = Utils.findComponent<TrapSpawnerScript>("Meteor Trap Spawner");
    }

    private void Start()
    {
        if (spawnCoinsAtStart) spawnCoin();
        else disableSpawner(spawnersEnum.coin);
        if (spawnMeteorAtStart) spawnMeteor();
        else disableSpawner(spawnersEnum.meteor);

        availableSpots = new List<int>();
        StartCoroutine(resetAvailableSpots(0));
    }
    #endregion

    #region Coin Methods
    private void spawnCoin()
    {
        coinTrapSpawner.enabled = true;
        coinsSpawning = true;
        StartCoroutine(endSpawncoins());
    }
    private IEnumerator endSpawncoins()
    {
        yield return new WaitForSeconds(Random.Range(minCoinDuration, maxCoinDuration));
        coinsSpawning = false;
        coinTrapSpawner.enabled = false;
        StartCoroutine(waitToSpawnNextCoin());
    }
    private IEnumerator waitToSpawnNextCoin()
    {
        yield return new WaitForSeconds(Random.Range(minCoinWaitingTime, maxCoinWaitingTime));
        spawnCoin();
    }
    #endregion

    #region Meteor Methods
    private void spawnMeteor()
    {
        meteorTrapSpawner.enabled = true;
    }
    #endregion

    #region Available Spawning Spots System Methods
    public float getRandomSpot()
    {
        int num = availableSpots[Random.Range(0, availableSpots.Count)];
        return Random.Range(num, num + 1f);
    }
    public void removeAvailableSpots(int from, int to, float delay)
    {
        if (availableSpots.Count == 10)
        {
            bool adding = true;
            List<int> temporaryList = new List<int>();
            foreach (int spot in availableSpots)
            {
                if (spot == from) adding = false;
                if (adding)
                {
                    temporaryList.Add(spot);
                }
                if (spot == to) adding = true;
            }
            availableSpots = temporaryList;
        }
        StartCoroutine(resetAvailableSpots(delay));
    }
    private IEnumerator resetAvailableSpots(float delay)
    {
        yield return new WaitForSeconds(delay);
        availableSpots.Clear();
        for (int i = -5; i < 5; i++) availableSpots.Add(i);
    }
    #endregion
}
