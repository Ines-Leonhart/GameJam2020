using UnityEngine;

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

    // TODO: add countdown to death

    [SerializeField] GameObject seed;
    [SerializeField] GameObject grownPlant;
    [SerializeField] GameObject infectionElements;

    public State CurrentState { get; private set; }

    void Start()
    {
        CurrentState = State.Seed;
        seed.SetActive(true);
        grownPlant.SetActive(false);
        infectionElements.SetActive(false);
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
        }
    }

    public void Attack()
    {
        if (CurrentState == State.Seed)
        {
            // TODO: play an animation before destroying
            Destroy(this.gameObject);
        }
        else if (CurrentState == State.Grown)
        {
            CurrentState = State.Infected;
            infectionElements.SetActive(true);
        }
    }
}
