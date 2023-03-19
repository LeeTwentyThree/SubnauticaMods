namespace AlterraFlux.Mono;

internal class CyclopsDockDoor : MonoBehaviour
{
    public Transform waterLevelPoint;

    private Animator animator;

    public bool Open { get; private set; }

    private void Start()
    {
        waterLevelPoint = gameObject.transform.Find("Armature/Bone008/Bone009/Bone002");
        animator = GetComponent<Animator>();
    }

    public void SetOpen(bool state)
    {
        Open = state;
        animator.SetBool("open", state);
    }
}
