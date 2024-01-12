using Nautilus.Utility;
using UnityEngine;

namespace TheRedPlague.Mono;

public class LaserReceptacleController : HandTarget, IHandTarget
{
	public override void Awake()
	{
		base.Awake();
		_animator = GetComponent<Animator>();
		_cinematicController = GetComponent<PlayerCinematicController>();
		_cinematicController.informGameObject = gameObject;
	}

	public void OnHandHover(GUIHand hand)
    {
	    if (!_unlockedTest)
	    {
		    HandReticle.main.SetText(HandReticle.TextType.Hand, "InsertPlagueHeart", true, GameInput.Button.LeftHand);
		    HandReticle.main.SetText(HandReticle.TextType.HandSubscript, string.Empty, false, GameInput.Button.None);
		    HandReticle.main.SetIcon(HandReticle.IconType.Hand);
	    }
    }

    public void OnHandClick(GUIHand hand)
    {
	    if (!_unlockedTest)
	    {
		    Pickupable pickupable = Inventory.main.container.RemoveItem(ModPrefabs.PlagueHeart.TechType);
		    if (pickupable != null)
		    {
			    _restoreQuickSlot = Inventory.main.quickSlots.activeSlot;
			    Inventory.main.ReturnHeld(true);
			    _insertedItem = pickupable.gameObject;
			    _insertedItem.transform.SetParent(Inventory.main.toolSocket);
			    _insertedItem.transform.localPosition = Vector3.zero;
			    _insertedItem.transform.localRotation = Quaternion.identity;
			    _insertedItem.transform.localScale = Vector3.one * 0.4f;
			    _insertedItem.SetActive(true);
			    Rigidbody component = _insertedItem.GetComponent<Rigidbody>();
			    if (component != null)
			    {
				    UWE.Utils.SetIsKinematicAndUpdateInterpolation(component, true);
			    }
			    _cinematicController.StartCinematicMode(Player.main);
			    Utils.PlayFMODAsset(UseSound, transform);
		    }
	    }
    }
    
    public void OpenDeck()
	{
		if (_unlockedTest)
		{
			return;
		}
		_animator.SetBool("Open", true);
		Utils.PlayFMODAsset(OpenSound, base.transform);
	}

	public void CloseDeck()
	{
		if (_animator.GetBool("Open"))
		{
			_animator.SetBool("Open", false);
			Utils.PlayFMODAsset(CloseSound, base.transform);
		}
	}
	
	public void OnPlayerCinematicModeEnd(PlayerCinematicController controller)
	{
		if (_insertedItem)
		{
			Destroy(_insertedItem);
		}
		CloseDeck();
		DisableLaser();
		if (_restoreQuickSlot != -1)
		{
			Inventory.main.quickSlots.Select(_restoreQuickSlot);
		}
	}

	private void DisableLaser()
	{
		StoryUtils.ForceFieldLaserDisabled.Trigger();
		Utils.PlayFMODAsset(AudioUtils.GetFmodAsset("DisableDomeSound"), new Vector3(-75.89f, 323.22f, -56.99f));
	}
	
	private PlayerCinematicController _cinematicController;

	private Animator _animator;

	private static readonly FMODAsset UseSound = AudioUtils.GetFmodAsset("event:/player/cube terminal_use");

	private static readonly FMODAsset OpenSound = AudioUtils.GetFmodAsset("event:/player/cube terminal_open");

	private static readonly FMODAsset CloseSound = AudioUtils.GetFmodAsset("event:/player/cube terminal_close");

	private bool _unlockedTest;
	private GameObject _insertedItem;
	private int _restoreQuickSlot = -1;
}