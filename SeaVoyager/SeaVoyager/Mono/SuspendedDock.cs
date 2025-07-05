using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeaVoyager.Mono
{
    public class SuspendedDock : MonoBehaviour, IManagedUpdateBehaviour
    {
        public SeaVoyager ship;
        public LineRenderer cableRenderer;
        public Transform cableConnectionPoint;
        public Transform armTransform;
        public SkinnedMeshRenderer dynamicCableModel;
        public Sprite spriteButtonActive;
        public Sprite spriteButtonInactive;
        public Button retractButton;
        public Button extendButton;
        public Image retractButtonImage;
        public Image extendButtonImage;
        public AudioSource moveSound;
        public CableTrigger cableTrigger;
        public ShipUITooltip toggleButtonTooltip;
        public ShipUITooltip releaseVehicleButtonTooltip;
        public ShipUITooltip extendCableButtonTooltip;
        public ShipUITooltip retractCableButtonTooltip;

        public Vehicle dockedVehicle;

        private float _buttonNextPressTime;

        /// <summary>
        /// The appearance-wise y position of the cable hook.
        /// </summary>
        private float _cableLocalY;

        /// <summary>
        /// Whether the arm is hanging over the water or not.
        /// </summary>
        private bool _dockExtended;

        /// <summary>
        /// The code-wide y position of the cable hook.
        /// </summary>
        private float _cableTargetLocation;

        private CableState _cableState;
        private const float MaxCableDepth = -200f;
        private const float MaxDivingBellDepth = -400f;
        private const float CableSpeed = 10f;
        private const float CableSpeedInAir = 5f;

        /// <summary>
        /// Whether the cable is holding an object or not.
        /// </summary>
        private bool Occupied => dockedVehicle != null;

        /// <summary>
        /// How long the cable is (absolute).
        /// </summary>
        private float CableLength => Mathf.Abs(_cableTargetLocation);

        /// <summary>
        /// How long the cable can be (absolute).
        /// </summary>
        private float MaxCableLength => Mathf.Abs(PlayerInDivingBell ? MaxDivingBellDepth : MaxCableDepth);

        /// <summary>
        /// How short the cable can be.
        /// </summary>
        private float MinCableLength => 2f;

        private bool CableInWater => CableTargetWorldPosition.y < 0f;

        /// <summary>
        /// Where the cable ends code-wise.
        /// </summary>
        private Vector3 CableTargetWorldPosition => new(cableConnectionPoint.position.x,
            cableConnectionPoint.position.y + _cableTargetLocation, cableConnectionPoint.position.z);

        /// <summary>
        /// Where the cable ends appearance-wise.
        /// </summary>
        private Vector3 CableEndWorldPosition => new(cableConnectionPoint.position.x,
            cableConnectionPoint.position.y + _cableLocalY, cableConnectionPoint.position.z);

        /// <summary>
        /// Whether the player is in the prawn suit while it is attached to the ship or not. I call it a "diving bell" because that's essentially what it is in this case.
        /// </summary>
        private bool PlayerInDivingBell
        {
            get
            {
                var playerVehicle = Player.main.GetVehicle();
                return playerVehicle != null && playerVehicle == dockedVehicle;
            }
        }

        /// <summary>
        /// Whether the current vehicle is a seamoth or not.
        /// </summary>
        private bool SeamothCurrentlyDocked
        {
            get
            {
                if (dockedVehicle == null) return false;
                return dockedVehicle is SeaMoth;
            }
        }

        private float CurrentCableSpeed => CableInWater ? CableSpeed : CableSpeedInAir;

        private void OnEnable()
        {
            BehaviourUpdateUtils.Register(this);
        }

        private void OnDisable()
        {
            BehaviourUpdateUtils.Deregister(this);
        }

        public void Initialize()
        {
            cableRenderer = gameObject.GetComponentInChildren<LineRenderer>();
            dynamicCableModel = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
            cableConnectionPoint = gameObject.SearchChild("CableTop").transform;
            armTransform = gameObject.SearchChild("DockArm").transform;
            moveSound = gameObject.SearchComponent<AudioSource>("ArmMoveSound");
            // moveSound.volume = Plugin.config.NormalizedAudioVolume;
            cableTrigger = gameObject.SearchChild("CableTrigger").AddComponent<CableTrigger>();
            cableTrigger.dock = this;

            var toggleButton = gameObject.SearchComponent<Button>("DockToggleButton");
            toggleButton.onClick.AddListener(OnToggleDockButton);
            toggleButtonTooltip = toggleButton.gameObject.AddComponent<ShipUITooltip>();
            toggleButtonTooltip.Init("Move arm");

            var releaseVehicleButton = gameObject.SearchComponent<Button>("ReleaseVehicleButton");
            releaseVehicleButton.onClick.AddListener(OnReleaseButton);
            releaseVehicleButtonTooltip = releaseVehicleButton.gameObject.AddComponent<ShipUITooltip>();
            releaseVehicleButtonTooltip.Init("Release vehicle");

            retractButton = gameObject.SearchComponent<Button>("CableRaiseButton");
            retractButton.onClick.AddListener(OnRetractButton);
            retractButtonImage = retractButton.GetComponent<Image>();
            retractCableButtonTooltip = retractButton.gameObject.AddComponent<ShipUITooltip>();
            retractCableButtonTooltip.Init("Retract cable");

            extendButton = gameObject.SearchComponent<Button>("CableDropButton");
            extendButton.onClick.AddListener(OnExtendButton);
            extendButtonImage = extendButton.GetComponent<Image>();
            extendCableButtonTooltip = extendButton.gameObject.AddComponent<ShipUITooltip>();
            extendCableButtonTooltip.Init("Extend cable");

            spriteButtonActive = Plugin.assetBundle.LoadAsset<Sprite>("ArrowOn");
            spriteButtonInactive = Plugin.assetBundle.LoadAsset<Sprite>("ArrowOff");
        }

        public void ManagedUpdate()
        {
            UpdateTooltips();
            AttemptToPlayVehicleDockVoice();
            switch (_cableState)
            {
                case CableState.Retracting:
                    if (_cableTargetLocation < -MinCableLength)
                    {
                        _cableTargetLocation += Time.deltaTime * CurrentCableSpeed;
                    }
                    else
                    {
                        SetButtonState(CableButtonsDisplay.Stopped);
                    }

                    break;
                case CableState.Extending:
                    if (Occupied)
                    {
                        if (Physics.Raycast(CableTargetWorldPosition + (Vector3.down * 2f), Vector3.down, 1.5f, ~0,
                                QueryTriggerInteraction.Ignore))
                        {
                            _cableState = CableState.Stopped;
                            SetButtonState(CableButtonsDisplay.Stopped);
                            break;
                        }
                    }
                    else
                    {
                        if (Physics.Raycast(CableTargetWorldPosition, Vector3.down, 3.5f, ~0,
                                QueryTriggerInteraction.Ignore))
                        {
                            int depth = Mathf.Abs(Mathf.RoundToInt(CableTargetWorldPosition.y));
                            if (CableTargetWorldPosition.y < 0f)
                                ErrorMessage.AddMessage(String.Format("Cable hit seafloor at depth {0} of meters.",
                                    depth));
                            _cableState = CableState.Stopped;
                            SetButtonState(CableButtonsDisplay.Stopped);
                            break;
                        }
                    }

                    if (_cableTargetLocation > -MaxCableLength)
                    {
                        _cableTargetLocation -= Time.deltaTime * CurrentCableSpeed;
                    }

                    break;
                case CableState.Default:
                    _cableTargetLocation = -MinCableLength;
                    break;
            }

            if (_cableState == CableState.Extending && CableLength >= MaxCableLength)
            {
                SetButtonState(CableButtonsDisplay.Stopped);
                _cableState = CableState.Stopped;
                if (PlayerInDivingBell)
                {
                    ErrorMessage.AddMessage(String.Format("Cable reached maximum diving bell length ({0} meters).",
                        MaxCableLength));
                }
                else
                {
                    ErrorMessage.AddMessage(String.Format("Cable reached maximum recommended length ({0} meters).",
                        MaxCableLength));
                }
            }

            if (_cableState == CableState.Retracting && CableLength <= 2f)
            {
                SetButtonState(CableButtonsDisplay.Stopped);
            }

            Quaternion armTargetRotation;
            if (_dockExtended)
            {
                armTargetRotation = Quaternion.identity; //The arm faces out over the deck.
            }
            else
            {
                armTargetRotation =
                    Quaternion.Euler(0f, 0f,
                        90f); //The arm is in "resting position". Technically this is pointless but it's fun for it to be interactable.
            }

            _cableLocalY =
                Mathf.MoveTowards(cableRenderer.GetPosition(1).y, _cableTargetLocation,
                    Time.deltaTime * 20f); //Move towards default position
            armTransform.localRotation =
                Quaternion.RotateTowards(armTransform.localRotation, armTargetRotation,
                    Time.deltaTime *
                    30f); //Rotate the arm to the correct position, smoothly. I believe this makes it move at the rate of 30 degrees per second.
            cableRenderer.SetPosition(1,
                new Vector3(0f, _cableLocalY, 0f)); //Apply the cableLocalY to the physical cable.
            float tightnessScale = Mathf.Clamp((_cableLocalY / -50f * 100f) - 4f, 0f, 100f);
            dynamicCableModel.SetBlendShapeWeight(0, tightnessScale);
            cableTrigger.transform.position = CableEndWorldPosition;
            if (dockedVehicle)
            {
                dockedVehicle.transform.position = cableTrigger.transform.position +
                                                   (Vector3.down * (SeamothCurrentlyDocked ? 1f : 2f));
                if (dockedVehicle is Exosuit)
                {
                    dockedVehicle.useRigidbody.isKinematic = true;
                }
                else
                {
                    dockedVehicle.useRigidbody.velocity = Vector3.zero;
                }
            }

            var vehicle = Player.main.GetVehicle();
            if (vehicle != null)
            {
                if (dockedVehicle == vehicle)
                {
                    ProcessInput();
                }
            }
        }

        void AttemptToPlayVehicleDockVoice()
        {
            if (dockedVehicle == null)
            {
                return;
            }

            if (_cableState != CableState.Retracting)
            {
                return;
            }

            if (!CableInWater && CableLength > MinCableLength)
            {
                if (ship.voice.PlayVoiceLine(ShipVoice.VoiceLine.VehicleDock))
                {
                    _buttonNextPressTime = Time.time + 2f;
                }
            }
        }

        private void UpdateTooltips()
        {
            // release vehicles button
            if (dockedVehicle == null)
            {
                releaseVehicleButtonTooltip.displayText = "No vehicle docked";
                releaseVehicleButtonTooltip.clickable = false;
            }
            else if (!_dockExtended)
            {
                releaseVehicleButtonTooltip.displayText = "Cannot release vehicles onto the dock.";
                releaseVehicleButtonTooltip.clickable = false;
            }
            else
            {
                releaseVehicleButtonTooltip.displayText = $"Release {dockedVehicle.GetName()}";
                releaseVehicleButtonTooltip.clickable = true;
            }

            releaseVehicleButtonTooltip.showTooltip = Time.time > _buttonNextPressTime;
            // toggle cable extension button
            if (CableExtendedBeyondDeck)
            {
                toggleButtonTooltip.displayText = "Cable not fully retracted";
                toggleButtonTooltip.clickable = false;
            }
            else if (dockedVehicle == null || SeamothCurrentlyDocked)
            {
                toggleButtonTooltip.displayText = _dockExtended ? "Return docking arm" : "Extend docking arm";
                toggleButtonTooltip.clickable = true;
            }
            else if (dockedVehicle != null)
            {
                toggleButtonTooltip.displayText = $"Cannot move arm with {dockedVehicle.GetName()} attached.";
                toggleButtonTooltip.clickable = false;
            }

            toggleButtonTooltip.showTooltip = Time.time > _buttonNextPressTime;
            
            // cable buttons
            if (_cableState == CableState.Extending)
            {
                extendCableButtonTooltip.displayText = "Stop cable";
            }
            else
            {
                extendCableButtonTooltip.displayText = "Extend cable";
            }

            extendCableButtonTooltip.showTooltip = CableLength < MaxCableLength && _dockExtended;
            if (_cableState == CableState.Retracting)
            {
                retractCableButtonTooltip.displayText = "Stop cable";
            }
            else
            {
                retractCableButtonTooltip.displayText = "Retract cable";
            }

            retractCableButtonTooltip.showTooltip = CableLength > MinCableLength;
        }

        void ProcessInput()
        {
            if (GameInput.GetButtonDown(GameInput.Button.MoveUp))
            {
                OnRetractButton();
            }

            if (GameInput.GetButtonDown(GameInput.Button.MoveDown))
            {
                OnExtendButton();
            }

            if (GameInput.GetButtonDown(GameInput.Button.Deconstruct))
            {
                DetatchVehicle();
            }
        }

        void OnToggleDockButton()
        {
            if (Time.time > _buttonNextPressTime)
            {
                if (SetDockExtended(!_dockExtended))
                {
                    _buttonNextPressTime = Time.time + 3f;
                }
            }
        }

        void OnReleaseButton()
        {
            if (Time.time > _buttonNextPressTime)
            {
                if (Occupied && _dockExtended)
                {
                    _buttonNextPressTime = Time.time + 1f;
                    DetatchVehicle();
                }
            }
        }

        void IgnorePhysicsWithVehicle(Vehicle vehicle, bool state)
        {
            if (ship == null)
            {
                return;
            }

            foreach (Collider col in ship.gameObject.GetComponentsInChildren<Collider>())
            {
                if (!col.isTrigger)
                {
                    foreach (var col2 in vehicle.GetComponentsInChildren<Collider>())
                    {
                        if (col2.gameObject.GetComponent<Player>() == null) Physics.IgnoreCollision(col, col2, state);
                    }
                }
            }
        }

        public bool SetDockExtended(bool newState)
        {
            if (newState == _dockExtended || CableExtendedBeyondDeck)
            {
                return false;
            }

            if (Occupied && _dockExtended && !SeamothCurrentlyDocked)
            {
                return false;
            }

            if (ship != null && !ship.HasPower)
            {
                return false;
            }

            _dockExtended = newState;
            moveSound.Play();
            return true;
        }

        private bool CableExtendedBeyondDeck => CableLength > 3f;

        public bool GetCanDock()
        {
            return !Occupied && _dockExtended;
        }

        public void SetButtonState(CableButtonsDisplay state)
        {
            switch (state)
            {
                default:
                    retractButtonImage.sprite = spriteButtonInactive;
                    extendButtonImage.sprite = spriteButtonInactive;
                    return;
                case CableButtonsDisplay.Extending:
                    retractButtonImage.sprite = spriteButtonInactive;
                    extendButtonImage.sprite = spriteButtonActive;
                    return;
                case CableButtonsDisplay.Retracting:
                    retractButtonImage.sprite = spriteButtonActive;
                    extendButtonImage.sprite = spriteButtonInactive;
                    return;
            }
        }

        public void AttachVehicle(Vehicle vehicle)
        {
            dockedVehicle = vehicle;
            ErrorMessage.AddMessage(string.Format("{0} attached.", new object[] { dockedVehicle.GetName() }));
            if (dockedVehicle is Exosuit) dockedVehicle.useRigidbody.isKinematic = true;
            dockedVehicle.gameObject.EnsureComponent<HeldByCable>().dock = this;
            _cableState = CableState.Stopped;
            SetButtonState(CableButtonsDisplay.Stopped);
            IgnorePhysicsWithVehicle(vehicle, true);
            if (Player.main.GetVehicle() == vehicle)
            {
                PrintExoCustomControls();
            }

            if (ship != null)
            {
                ship.voice.PlayVoiceLine(ShipVoice.VoiceLine.VehicleAttached);
            }
        }

        public static void PrintExoCustomControls()
        {
            ErrorMessage.AddMessage(LanguageCache.GetButtonFormat("SuspendedDockReturnToSurface", GameInput.Button.MoveUp));
            ErrorMessage.AddMessage(LanguageCache.GetButtonFormat("SuspendedDockDescend", GameInput.Button.MoveDown));
            ErrorMessage.AddMessage(LanguageCache.GetButtonFormat("SuspendedDockDetachCable", GameInput.Button.Deconstruct));
        }

        public void DetatchVehicle()
        {
            if (dockedVehicle != null)
            {
                if (dockedVehicle is Exosuit) dockedVehicle.useRigidbody.isKinematic = false;
                dockedVehicle.gameObject.EnsureComponent<HeldByCable>().dock = null;
                _cableState = CableState.Stopped;
                SetButtonState(CableButtonsDisplay.Stopped);
                IgnorePhysicsWithVehicle(dockedVehicle, false);
                dockedVehicle = null;
                if (ship != null)
                {
                    ship.voice.PlayVoiceLine(ShipVoice.VoiceLine.VehicleReleased);
                }
            }
        }

        private void OnRetractButton()
        {
            if (CableLength > 1f)
            {
                if (_cableState == CableState.Retracting)
                {
                    _cableState = CableState.Stopped;
                    SetButtonState(CableButtonsDisplay.Stopped);
                }
                else
                {
                    _cableState = CableState.Retracting;
                    SetButtonState(CableButtonsDisplay.Retracting);
                }
            }
        }

        private void OnExtendButton()
        {
            if (_dockExtended && CableLength < MaxCableLength)
            {
                if (_cableState == CableState.Extending)
                {
                    _cableState = CableState.Stopped;
                    SetButtonState(CableButtonsDisplay.Stopped);
                }
                else
                {
                    _cableState = CableState.Extending;
                    SetButtonState(CableButtonsDisplay.Extending);
                }
            }
        }

        public string GetProfileTag()
        {
            return "SeaVoyager:SuspendedDock";
        }

        public int managedUpdateIndex { get; set; }
    }

    public enum CableState
    {
        Default,
        Extending,
        Retracting,
        Stopped
    }

    public enum CableButtonsDisplay
    {
        Stopped,
        Extending,
        Retracting
    }
}