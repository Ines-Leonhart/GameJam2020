using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class Plant : MonoBehaviour
{
    public enum State
    {
        Seed,
        Grown,
        Infected,
        Dead
        // TODO: add dry and other states maybe
    }

    Game Game
    {
        get
        {
            return Singleton.Get<Game>();
        }
    }

    // TODO: add countdown to death

    [SerializeField] GameObject seed;
    [SerializeField] GameObject grownPlant;
    [SerializeField] GameObject infectionElements;
    [SerializeField] float infectionTime;
    [SerializeField] GameObject cellPrefab;
    [SerializeField] List<Renderer> Renderers;

    private Vector3 cellSize;
    public State CurrentState { get; private set; }
    private float timeLeft;
    private bool startTween = false;
    private List<Color> materialDefaultColors;
    private List<Tween> tweens = new List<Tween>();

    void Start()
    {
        cellSize = cellPrefab.GetComponentInChildren<Renderer>().bounds.size;
        CurrentState = State.Seed;
        seed.SetActive(true);
        grownPlant.SetActive(false);
        infectionElements.SetActive(false);
        timeLeft = infectionTime;
        var rotation = transform.rotation;
        rotation.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        transform.rotation = rotation;
        materialDefaultColors = new List<Color>();
    }

    public void Water()
    {
        if (CurrentState == State.Seed)
        {
            CurrentState = State.Grown;

            seed.SetActive(false);
            grownPlant.SetActive(true);
        }
    }

    public void Heal()
    {
        if (CurrentState == State.Infected)
        {
            CurrentState = State.Grown;

            infectionElements.SetActive(false);

            timeLeft = infectionTime;
            foreach (Tween tween in tweens)
            {
                tween.Kill();
            }
            for (int i = 0; i < Renderers.Count; i++)
            {
                Renderers[i].material.color = materialDefaultColors[i];
            }

            GetComponentInChildren<Animator>().SetBool("isInfected", false);
        }
    }

    public void Attack()
    {
        Game.plantAttack.Play();
        if (CurrentState == State.Seed)
        {
            // TODO: play an animation before destroying
            Destroy(this.gameObject);
        }
        else if (CurrentState == State.Grown)
        {
            CurrentState = State.Infected;
            infectionElements.SetActive(true);

            foreach (Renderer renderer in Renderers)
            {
                materialDefaultColors.Add(renderer.material.color);
                tweens.Add(renderer.material.DOColor(Color.red, infectionTime));
            }

            GetComponentInChildren<Animator>().SetBool("isInfected", true);
        }
        else if (CurrentState == State.Infected)
        {
            Destroy(this.gameObject);
        }
    }

    private GameObject checkCell(Vector3 position)
    {
        Debug.DrawRay(position, Vector3.down, Color.green, 10);
        var iterable = Physics.RaycastAll(position, Vector3.down);
        for (int i = 0; i < iterable.Length; i++)
        {
            if (iterable[i].transform.tag == "cell")
            {
                return iterable[i].transform.gameObject;
            }
        }
        return null;
    }

    public void InfectOthers()
    {
        GameObject rayUp = checkCell(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z) + new Vector3(0, 0, -cellSize.z));
        GameObject rayDown = checkCell(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z) + new Vector3(0, 0, cellSize.z));
        GameObject rayLeft = checkCell(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z) + new Vector3(-cellSize.x, 0, 0));
        GameObject rayRight = checkCell(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z) + new Vector3(cellSize.x, 0, 0));

        Debug.Log(rayUp + " " + rayDown + " " + rayLeft + " " + rayRight);

        if (rayUp != null && rayUp.GetComponent<cellScript>().Plant != null)
        {
            var plant = rayUp.GetComponent<cellScript>().Plant;
            plant.Attack();
        }
        if (rayDown != null && rayDown.GetComponent<cellScript>().Plant != null)
        {
            var plant = rayDown.GetComponent<cellScript>().Plant;
            plant.Attack();
        }
        if (rayLeft != null && rayLeft.GetComponent<cellScript>().Plant != null)
        {
            var plant = rayLeft.GetComponent<cellScript>().Plant;
            plant.Attack();
        }
        if (rayRight != null && rayRight.GetComponent<cellScript>().Plant != null)
        {
            var plant = rayRight.GetComponent<cellScript>().Plant;
            plant.Attack();
        }
    }

    private void Update()
    {
        if(CurrentState == State.Infected && Game.CurrentState == Game.State.Play)
        { 
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                InfectOthers();
                Attack();
            }
        }
    }
}
