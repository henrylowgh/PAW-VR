using System;
using UnityEngine;

///<summary>
/// Card input
///</summary>
public class TouchCard : InputBase
{
    [Header("Card ID")]
    public int cardId;
    [Header("Card effect particle")]
    public GameObject victoryParticle;
    [Header("Card mesh")]
    public GameObject mesh;
    public bool canFlipOver;
    private Action checkCardFaceUp;
    private Action<TouchCard> onAddCard;
    private Animator animator;
    private BoxCollider boxCollider;


    void Start()
    {
        canFlipOver = true;
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();

        //hide particle
        if (victoryParticle)
            victoryParticle.SetActive(false);
    }

    public override void OnMouseUp()
    {

        if (IsFaceUp)
        {
            return; // Do nothing if the card is already face-up
        }

        var controller = FindObjectOfType<MemoryControllerBase>();
        if (controller.CanFlipAnotherCard() && controller.IsBufferTimeElapsed())
        {
            SetIsClicked(true);
            SetAnimation(faceUp: true);

            onAddCard?.Invoke(this);
            checkCardFaceUp?.Invoke();
        }
    }

    public void SetEvents(Action<TouchCard> onAddCard, Action checkCardFaceUp)
    {
        this.onAddCard = onAddCard;
        this.checkCardFaceUp = checkCardFaceUp;
    }

    public void SetAnimation(bool faceUp)
    {
        animator.SetBool("CanFaceUp", faceUp);
        IsFaceUp = faceUp;
    }

    public void ResetCollider()
    {
        boxCollider.size = new Vector3(0, 0, 0);
    }

    public void EnableVictoryParticle(bool enabled)
    {
        victoryParticle.SetActive(enabled);
    }

    public bool IsFaceUp { get; private set; }

}