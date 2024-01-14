using Nautilus.Utility;
using Story;
using UnityEngine;

namespace TheRedPlague.Mono;

public class LaserReceptacleController : HandTarget, IHandTarget
{
	private PlayerCinematicController _cinematicController;

	private Animator _animator;

	private static readonly FMODAsset UseSound = AudioUtils.GetFmodAsset("event:/player/cube terminal_use");

	private static readonly FMODAsset OpenSound = AudioUtils.GetFmodAsset("event:/player/cube terminal_open");

	private static readonly FMODAsset CloseSound = AudioUtils.GetFmodAsset("event:/player/cube terminal_close");

	private bool _unusable;
	private GameObject _insertedItem;
	private int _restoreQuickSlot = -1;

	private Mode _lastUsedMode;
	
	public override void Awake()
	{
		base.Awake();
		_animator = GetComponent<Animator>();
		_cinematicController = GetComponent<PlayerCinematicController>();
		_cinematicController.informGameObject = gameObject;
	}

	public void OnHandHover(GUIHand hand)
	{
		var mode = GetCurrentMode();
	    if (!_unusable)
	    {
		    if (mode == Mode.PlagueHeart)
		    {
			    HandReticle.main.SetText(HandReticle.TextType.Hand, "InsertPlagueHeart", true, GameInput.Button.LeftHand);
		    }
		    else if (mode == Mode.EnzymeContainer)
		    {
			    HandReticle.main.SetText(HandReticle.TextType.Hand, "InsertEnzymeContainer", true, GameInput.Button.LeftHand);
		    }
		    HandReticle.main.SetText(HandReticle.TextType.HandSubscript, string.Empty, false, GameInput.Button.None);
		    HandReticle.main.SetIcon(HandReticle.IconType.Hand);
	    }
    }

    public void OnHandClick(GUIHand hand)
    {
	    var mode = GetCurrentMode();

	    if (_unusable || mode == Mode.Unusable) return;

	    var isPlagueHeart = mode == Mode.PlagueHeart;
	    var pickupable = Inventory.main.container.RemoveItem(isPlagueHeart ? ModPrefabs.PlagueHeart.TechType : ModPrefabs.EnzymeContainer.TechType);
	    
	    if (pickupable == null) return;
	    
	    _restoreQuickSlot = Inventory.main.quickSlots.activeSlot;
	    Inventory.main.ReturnHeld(true);
	    _insertedItem = pickupable.gameObject;
	    Destroy(_insertedItem.GetComponent<PlagueHeartBehavior>());
	    _insertedItem.transform.SetParent(Inventory.main.toolSocket);
	    _insertedItem.transform.localPosition = Vector3.zero;
	    _insertedItem.transform.localRotation = Quaternion.identity;
	    _insertedItem.transform.localScale = Vector3.one * (isPlagueHeart ? 0.4f : 1.3f);
	    _insertedItem.SetActive(true);
	    Rigidbody component = _insertedItem.GetComponent<Rigidbody>();
	    if (component != null)
	    {
		    UWE.Utils.SetIsKinematicAndUpdateInterpolation(component, true);
	    }
	    _cinematicController.StartCinematicMode(Player.main);
	    Utils.PlayFMODAsset(UseSound, transform);
	    _lastUsedMode = mode;
    }
    
    public void OpenDeck()
	{
		if (_unusable)
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
		if (_lastUsedMode == Mode.PlagueHeart)
		{
			StoryUtils.DisableInfectionLaser();
		}
		else if (_lastUsedMode == Mode.EnzymeContainer)
		{
			StoryUtils.StartInfectionRain();
		}
		if (_restoreQuickSlot != -1)
		{
			Inventory.main.quickSlots.Select(_restoreQuickSlot);
		}
	}
	
	private static Mode GetCurrentMode()
	{
		if (StoryGoalManager.main.IsGoalComplete(StoryUtils.EnzymeRainEnabled.key))
		{
			return Mode.Unusable;
		}
		if (StoryGoalManager.main.IsGoalComplete(StoryUtils.ForceFieldLaserDisabled.key))
		{
			return Mode.EnzymeContainer;
		}
		if (StoryGoalManager.main.IsGoalComplete(StoryUtils.PlagueHeartGoal.key))
		{
			return Mode.PlagueHeart;
		}

		return Mode.Unusable;
	}
	
	
	private enum Mode
	{
		PlagueHeart,
		EnzymeContainer,
		Unusable
	}
}