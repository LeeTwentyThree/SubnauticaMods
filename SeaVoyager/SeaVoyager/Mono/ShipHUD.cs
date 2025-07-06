using Nautilus.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace SeaVoyager.Mono
{
    public class ShipHUD : MonoBehaviour
    {
        private SeaVoyager _ship;

        // UI images
        private Image _fwdImg;
        private Image _reverseImg;
        private Image _leftImg;
        private Image _rightImg;
        private Image _stopImg;
        private Image _bottomCamImg;
        private Image _frontCamImg;
        private Image _mapImg;
        private Image _speed1Img;
        private Image _speed2Img;
        private Image _speed3Img;

        // sprites that the UI images can use
        private Sprite _directionSpriteInactive;
        private Sprite _directionSpriteActive;
        private Sprite _switchedOn;
        private Sprite _switchedOff;
        private Sprite _spriteCamInactive;
        private Sprite _spriteCamActive;
        private Sprite _spriteSonarInactive;
        private Sprite _spriteSonarActive;
        private Sprite _spriteMapActive;
        private Sprite _spriteMapInactive;
        private Sprite _speed1Active;
        private Sprite _speed1Inactive;
        private Sprite _speed2Active;
        private Sprite _speed2Inactive;
        private Sprite _speed3Active;
        private Sprite _speed3Inactive;

        // map
        private Transform _mapTab;
        private Transform _camTab;
        private RectTransform _mapTransform;
        private RectTransform _mapShipMarker;
        private ShipUITooltip _zoomButtonTooltip;
        private MapZoom _mapZoom;

        // cameras
        private RenderTexture _renderTextureBottom;
        private RenderTexture _renderTextureFront;
        private Camera _cameraBottom;
        private Camera _cameraFront;
        private RawImage _camRawImage;
        private ShipCamera _activeCamera;

        // audio
        private static readonly FMODAsset ButtonPressSound = AudioUtils.GetFmodAsset("SvButtonPress");
        private float _poweringDownVoiceLineDelay = 2f;
        private float _timeVehicleStopped;
        private bool _stopVoiceLineQueued;
        private float _poweringUpVoiceLineMinOfflineTime = 4f;

        private void Awake()
        {
            _ship = GetComponentInParent<SeaVoyager>();
            SetupHUD();
            SetActiveCamera(ShipCamera.Front);
        }

        private void SetupHUD()
        {
            _fwdImg = Helpers.FindChild(gameObject, "ForwardButton").GetComponent<Image>();
            _reverseImg = Helpers.FindChild(gameObject, "ReverseButton").GetComponent<Image>();
            _leftImg = Helpers.FindChild(gameObject, "LeftButton").GetComponent<Image>();
            _rightImg = Helpers.FindChild(gameObject, "RightButton").GetComponent<Image>();
            _stopImg = Helpers.FindChild(gameObject, "StopButton").GetComponent<Image>();
            _bottomCamImg = Helpers.FindChild(gameObject, "BottomCameraButton").GetComponent<Image>();
            _frontCamImg = Helpers.FindChild(gameObject, "FrontCameraButton").GetComponent<Image>();
            _mapImg = Helpers.FindChild(gameObject, "MapButton").GetComponent<Image>();
            _speed1Img = Helpers.FindChild(gameObject, "SpeedButton1").GetComponent<Image>();
            _speed2Img = Helpers.FindChild(gameObject, "SpeedButton2").GetComponent<Image>();
            _speed3Img = Helpers.FindChild(gameObject, "SpeedButton3").GetComponent<Image>();

            _mapTab = Helpers.FindChild(gameObject, "MapTab").transform;
            _camTab = Helpers.FindChild(gameObject, "CameraTab").transform;
            _mapTransform = Helpers.FindChild(gameObject, "MapBG").GetComponent<RectTransform>();
            _mapShipMarker = Helpers.FindChild(gameObject, "MapCenter").GetComponent<RectTransform>();

            _fwdImg.GetComponent<Button>().onClick.AddListener(OnForward);
            _reverseImg.GetComponent<Button>().onClick.AddListener(OnReverse);
            _leftImg.GetComponent<Button>().onClick.AddListener(OnLeft);
            _rightImg.GetComponent<Button>().onClick.AddListener(OnRight);
            _stopImg.GetComponent<Button>().onClick.AddListener(OnStop);
            _bottomCamImg.GetComponent<Button>().onClick.AddListener(SetTabBottomCam);
            _frontCamImg.GetComponent<Button>().onClick.AddListener(SetTabFrontCam);
            _mapImg.GetComponent<Button>().onClick.AddListener(SetTabMap);
            _speed1Img.GetComponent<Button>().onClick.AddListener(OnSpeedSetting1);
            _speed2Img.GetComponent<Button>().onClick.AddListener(OnSpeedSetting2);
            _speed3Img.GetComponent<Button>().onClick.AddListener(OnSpeedSetting3);
            var zoomButton = Helpers.FindChild(gameObject, "MapZoomButton");
            zoomButton.GetComponent<Button>().onClick.AddListener(OnToggleMapZoom);

            _fwdImg.gameObject.AddComponent<ShipUITooltip>().Init("Forward");
            _reverseImg.gameObject.AddComponent<ShipUITooltip>().Init("Reverse");
            _leftImg.gameObject.AddComponent<ShipUITooltip>().Init("Turn left");
            _rightImg.gameObject.AddComponent<ShipUITooltip>().Init("Turn right");
            _stopImg.gameObject.AddComponent<ShipUITooltip>().Init("Disable engine");
            _bottomCamImg.gameObject.AddComponent<ShipUITooltip>().Init("View sonar map");
            _frontCamImg.gameObject.AddComponent<ShipUITooltip>().Init("View forward-facing camera");
            _mapImg.gameObject.AddComponent<ShipUITooltip>().Init("View region map");
            _speed1Img.gameObject.AddComponent<ShipUITooltip>().Init("Slow speed");
            _speed2Img.gameObject.AddComponent<ShipUITooltip>().Init("Standard speed");
            _speed3Img.gameObject.AddComponent<ShipUITooltip>().Init("Emergency speed");
            _mapShipMarker.gameObject.AddComponent<ShipUITooltip>().Init("Sea Voyager", false);
            _zoomButtonTooltip = zoomButton.AddComponent<ShipUITooltip>();
            _zoomButtonTooltip.Init("Zoom in");

            _directionSpriteInactive = Plugin.assetBundle.LoadAsset<Sprite>("ArrowOff");
            _directionSpriteActive = Plugin.assetBundle.LoadAsset<Sprite>("ArrowOn");

            _switchedOn = Plugin.assetBundle.LoadAsset<Sprite>("ShipOn");
            _switchedOff = Plugin.assetBundle.LoadAsset<Sprite>("ShipOff");

            _spriteCamActive = Plugin.assetBundle.LoadAsset<Sprite>("CameraOn");
            _spriteCamInactive = Plugin.assetBundle.LoadAsset<Sprite>("CameraOff");

            _spriteSonarActive = Plugin.assetBundle.LoadAsset<Sprite>("SonarOn");
            _spriteSonarInactive = Plugin.assetBundle.LoadAsset<Sprite>("SonarOff");

            _spriteMapActive = Plugin.assetBundle.LoadAsset<Sprite>("MapOn");
            _spriteMapInactive = Plugin.assetBundle.LoadAsset<Sprite>("MapOff");

            _speed1Active = Plugin.assetBundle.LoadAsset<Sprite>("SpeedSetting1");
            _speed1Inactive = Plugin.assetBundle.LoadAsset<Sprite>("SpeedSetting1Inactive");
            _speed2Active = Plugin.assetBundle.LoadAsset<Sprite>("SpeedSetting2");
            _speed2Inactive = Plugin.assetBundle.LoadAsset<Sprite>("SpeedSetting2Inactive");
            _speed3Active = Plugin.assetBundle.LoadAsset<Sprite>("SpeedSetting3");
            _speed3Inactive = Plugin.assetBundle.LoadAsset<Sprite>("SpeedSetting3Inactive");

            _camRawImage = Helpers.FindChild(gameObject, "CameraView").GetComponent<RawImage>();
            _renderTextureBottom = new RenderTexture(256, 128, 24);
            _renderTextureFront = new RenderTexture(512, 256, 16);

            _cameraBottom = Helpers.FindChild(_ship.gameObject, "BottomCamera").AddComponent<Camera>();
            _cameraBottom.fieldOfView = 70f;
            _cameraBottom.targetTexture = _renderTextureBottom;
            _cameraBottom.gameObject.AddComponent<SonarCam>();

            _cameraFront = Helpers.FindChild(_ship.gameObject, "FrontCamera").AddComponent<Camera>();
            _cameraFront.fieldOfView = 80f;
            _cameraFront.targetTexture = _renderTextureFront;

            // water fog fix
            _cameraFront.gameObject.SetActive(false);

            var waterVolumeOnCamera = _cameraFront.gameObject.AddComponent<WaterscapeVolumeOnCamera>();
            waterVolumeOnCamera.settings = WaterBiomeManager.main.gameObject.GetComponent<WaterscapeVolume>();

            // This literally KILLS your PC if you uncomment it. DONT!!!!
            //var waterSurfaceOnCamera = cameraFront.gameObject.AddComponent<WaterSurfaceOnCamera>();
            //waterSurfaceOnCamera.waterSurface = WaterSurface.Get();

            _cameraFront.gameObject.SetActive(true);
        }

        void Start()
        {
            UpdateButtonImages();
        }

        void SetActiveCamera(ShipCamera cam)
        {
            _activeCamera = cam;
            switch (cam)
            {
                default:
                    return;
                case ShipCamera.Front:
                    _camRawImage.texture = _renderTextureFront;
                    return;
                case ShipCamera.Bottom:
                    _camRawImage.texture = _renderTextureBottom;
                    return;
            }
        }

        private void PlayClickSound()
        {
            Utils.PlayFMODAsset(ButtonPressSound, Player.main.transform.position);
        }

        public void UpdateButtonImages()
        {
            _fwdImg.sprite = _directionSpriteInactive;
            _reverseImg.sprite = _directionSpriteInactive;
            _leftImg.sprite = _directionSpriteInactive;
            _rightImg.sprite = _directionSpriteInactive;
            _stopImg.sprite = _switchedOff;
            if (_ship.currentState == ShipState.Idle)
            {
                _stopImg.sprite = _switchedOn;
            }
            if (_ship.currentState == ShipState.Moving || _ship.currentState == ShipState.MovingAndRotating)
            {
                if (_ship.moveDirection == ShipMoveDirection.Forward) _fwdImg.sprite = _directionSpriteActive;
                else if (_ship.moveDirection == ShipMoveDirection.Reverse) _reverseImg.sprite = _directionSpriteActive;
            }
            if (_ship.currentState == ShipState.Rotating || _ship.currentState == ShipState.MovingAndRotating)
            {
                if (_ship.rotateDirection == ShipRotateDirection.Left) _leftImg.sprite = _directionSpriteActive;
                else if (_ship.rotateDirection == ShipRotateDirection.Right) _rightImg.sprite = _directionSpriteActive;
            }
            var speed = _ship.speedSetting;
            _speed1Img.sprite = speed == ShipSpeedSetting.Slow ? _speed1Active : _speed1Inactive;
            _speed2Img.sprite = speed == ShipSpeedSetting.Medium ? _speed2Active : _speed2Inactive;
            _speed3Img.sprite = speed == ShipSpeedSetting.Fast ? _speed3Active : _speed3Inactive;
            _zoomButtonTooltip.displayText = _mapZoom == MapZoom.ZoomedIn ? "Zoom out" : "Zoom in";
        }

        #region Voices
        void TryPlayEnginePoweringUp()
        {
            if (Time.time < _timeVehicleStopped + _poweringUpVoiceLineMinOfflineTime)
            {
                return;
            }
            if (_ship.voice.PlayVoiceLine(ShipVoice.VoiceLine.EnginePoweringUp, true))
            {
                MainCameraControl.main.ShakeCamera(3f, 5f, MainCameraControl.ShakeMode.BuildUp, 0.5f);
                _ship.shipMotor.StopMovementForSeconds(4f);
            }
        }
        void TryPlayEnginePoweringDown()
        {
            if (_ship.voice.PlayVoiceLine(ShipVoice.VoiceLine.EnginePoweringDown))
            {
                MainCameraControl.main.ShakeCamera(1f, 2f, MainCameraControl.ShakeMode.Sqrt, 0.5f);
                _stopVoiceLineQueued = false;
            }
        }

        #endregion
        #region Movement buttons
        void OnForward()
        {
            var lastState = _ship.currentState;
            if (_ship.moveDirection == ShipMoveDirection.Forward)
            {
                _ship.moveDirection = ShipMoveDirection.Idle;
                if (_ship.currentState == ShipState.MovingAndRotating) _ship.currentState = ShipState.Rotating;
                else if (_ship.currentState == ShipState.Moving)
                {
                    SetStateIdle();
                }
            }
            else if (_ship.currentState == ShipState.Rotating || _ship.currentState == ShipState.MovingAndRotating)
            {
                _ship.currentState = ShipState.MovingAndRotating;
                _ship.moveDirection = ShipMoveDirection.Forward;
            }
            else
            {
                _ship.currentState = ShipState.Moving;
                _ship.moveDirection = ShipMoveDirection.Forward;
                if (lastState == ShipState.Idle)
                {
                    TryPlayEnginePoweringUp();
                }
            }
            PlayClickSound();
            UpdateButtonImages();
        }
        void OnReverse()
        {
            var lastState = _ship.currentState;
            if (_ship.moveDirection == ShipMoveDirection.Reverse)
            {
                _ship.moveDirection = ShipMoveDirection.Idle;
                if (_ship.currentState == ShipState.MovingAndRotating) _ship.currentState = ShipState.Rotating;
                else if (_ship.currentState == ShipState.Moving)
                {
                    SetStateIdle();
                }
            }
            else if (_ship.currentState == ShipState.Rotating || _ship.currentState == ShipState.MovingAndRotating)
            {
                _ship.currentState = ShipState.MovingAndRotating;
                _ship.moveDirection = ShipMoveDirection.Reverse;
            }
            else
            {
                _ship.currentState = ShipState.Moving;
                _ship.moveDirection = ShipMoveDirection.Reverse;
                if (lastState == ShipState.Idle)
                {
                    TryPlayEnginePoweringUp();
                }
            }
            PlayClickSound();
            UpdateButtonImages();
        }
        void OnLeft()
        {
            if (_ship.rotateDirection == ShipRotateDirection.Left)
            {
                _ship.rotateDirection = ShipRotateDirection.Idle;
                if (_ship.currentState == ShipState.MovingAndRotating) _ship.currentState = ShipState.Moving;
                else if (_ship.currentState == ShipState.Rotating)
                {
                    SetStateIdle();
                }
            }
            else if (_ship.currentState == ShipState.Moving || _ship.currentState == ShipState.MovingAndRotating)
            {
                _ship.currentState = ShipState.MovingAndRotating;
                _ship.rotateDirection = ShipRotateDirection.Left;
            }
            else
            {
                _ship.currentState = ShipState.Rotating;
                _ship.rotateDirection = ShipRotateDirection.Left;
            }
            OnRotationChanged();
            PlayClickSound();
            UpdateButtonImages();
        }
        void OnRight()
        {
            if (_ship.rotateDirection == ShipRotateDirection.Right)
            {
                _ship.rotateDirection = ShipRotateDirection.Idle;
                if (_ship.currentState == ShipState.MovingAndRotating) _ship.currentState = ShipState.Moving;
                else if (_ship.currentState == ShipState.Rotating)
                {
                    SetStateIdle();
                }
            }
            else if (_ship.currentState == ShipState.Moving || _ship.currentState == ShipState.MovingAndRotating)
            {
                _ship.currentState = ShipState.MovingAndRotating;
                _ship.rotateDirection = ShipRotateDirection.Right;
            }
            else
            {
                _ship.currentState = ShipState.Rotating;
                _ship.rotateDirection = ShipRotateDirection.Right;
            }
            OnRotationChanged();
            PlayClickSound();
            UpdateButtonImages();
        }
        void SetStateIdle()
        {
            _ship.currentState = ShipState.Idle;
            _timeVehicleStopped = Time.time;
            _stopVoiceLineQueued = true;
        }
        void OnRotationChanged()
        {
            if (_ship.currentState == ShipState.Rotating || _ship.currentState == ShipState.MovingAndRotating)
            {
                _ship.rb.AddTorque(Vector3.up * -_ship.rb.angularVelocity.y * 50f * _ship.rb.mass);
            }
        }
        public void OnStop()
        {
            if (_ship.currentState != ShipState.Idle)
            {
                SetStateIdle();
            }
            _ship.currentState = ShipState.Idle;
            _ship.moveDirection = ShipMoveDirection.Idle;
            _ship.rotateDirection = ShipRotateDirection.Idle;
            UpdateButtonImages();
            PlayClickSound();
        }
        void OnSpeedSetting1()
        {
            _ship.speedSetting = ShipSpeedSetting.Slow;
            UpdateButtonImages();
            PlayClickSound();
            _ship.voice.PlayVoiceLine(ShipVoice.VoiceLine.AheadSlow);
        }
        void OnSpeedSetting2()
        {
            _ship.speedSetting = ShipSpeedSetting.Medium;
            UpdateButtonImages();
            PlayClickSound();
            _ship.voice.PlayVoiceLine(ShipVoice.VoiceLine.AheadStandard);
        }
        void OnSpeedSetting3()
        {
            _ship.speedSetting = ShipSpeedSetting.Fast;
            UpdateButtonImages();
            PlayClickSound();
            _ship.voice.PlayVoiceLine(ShipVoice.VoiceLine.AheadFlank);
        }
        #endregion

        #region Map
        void SetTabMap()
        {
            _bottomCamImg.sprite = _spriteSonarInactive;
            _frontCamImg.sprite = _spriteCamInactive;
            _mapImg.sprite = _spriteMapActive; 
            _mapTab.gameObject.SetActive(true);
            _camTab.gameObject.SetActive(false);
            PlayClickSound();
            _ship.voice.PlayVoiceLine(ShipVoice.VoiceLine.RegionMap);
        }
        void OnToggleMapZoom()
        {
            if(_mapZoom == MapZoom.Full)
            {
                SetMapZoom(MapZoom.ZoomedIn);
            }
            else
            {
                SetMapZoom(MapZoom.Full);
            }
            PlayClickSound();
            UpdateButtonImages();
        }
        void SetMapZoom(MapZoom newZoom)
        {
            _mapZoom = newZoom;
            if (_mapZoom == MapZoom.Full)
            {
                _mapTransform.localScale = new Vector2(0.3f, 0.3f);
            }
            else
            {
                _mapTransform.localScale = new Vector2(1f, 1f);
            }
        }
        #endregion

        #region Camera
        void SetTabBottomCam()
        {
            SwitchToCameraMode();
            _bottomCamImg.sprite = _spriteSonarActive;
            SetActiveCamera(ShipCamera.Bottom);
            _ship.voice.PlayVoiceLine(ShipVoice.VoiceLine.SonarMap);
        }
        void SetTabFrontCam()
        {
            SwitchToCameraMode();
            _frontCamImg.sprite = _spriteCamActive;
            SetActiveCamera(ShipCamera.Front);
        }
        void SwitchToCameraMode()
        {
            _bottomCamImg.sprite = _spriteSonarInactive;
            _frontCamImg.sprite = _spriteCamInactive;
            _mapImg.sprite = _spriteMapInactive;
            _mapTab.gameObject.SetActive(false);
            _camTab.gameObject.SetActive(true);
            PlayClickSound();
        }
        #endregion

        void Update()
        {
            if (_ship.LOD.current == LODState.Full) //Only run if you are close to or inside of the ship.
            {
                _cameraBottom.enabled = _activeCamera == ShipCamera.Bottom && _ship.IsOccupiedByPlayer;
                _cameraFront.enabled = _activeCamera == ShipCamera.Front && _ship.IsOccupiedByPlayer;
                if (_mapZoom == MapZoom.Full)
                {
                    _mapTransform.localPosition = Vector2.zero;
                    _mapShipMarker.localPosition = new Vector2(_ship.transform.position.x / 26.3f, _ship.transform.position.z / 26.3f);
                }
                else
                {
                    _mapTransform.localPosition = new Vector2(-_ship.transform.position.x / 7.8f, -_ship.transform.position.z / 7.8f);
                    _mapShipMarker.localPosition = Vector2.zero;
                }
                _mapShipMarker.localEulerAngles = new Vector3(0f, 0f, -_ship.transform.eulerAngles.y);

                /*if (Input.GetKeyDown(KeyCode.UpArrow)) These controls cause too many accidents...
                {
                    OnForward();
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    OnLeft();
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    OnRight();
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    OnReverse();
                }*/
            }
            else
            {
                _cameraBottom.enabled = false;
                _cameraBottom.enabled = false;
            }
            if (_stopVoiceLineQueued && Time.time > _timeVehicleStopped + _poweringDownVoiceLineDelay && _ship.Idle)
            {
                TryPlayEnginePoweringDown();
            }
        }

        public enum MapZoom
        {
            Full,
            ZoomedIn
        }
        public enum ShipCamera
        {
            Bottom,
            Front
        }
    }
}
