using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ShipMod.Ship
{
    public class ShipHUD : MonoBehaviour
    {
        private SeaVoyager ship;

        // UI images
        private Image fwdImg;
        private Image reverseImg;
        private Image leftImg;
        private Image rightImg;
        private Image stopImg;
        private Image bottomCamImg;
        private Image frontCamImg;
        private Image mapImg;
        private Image speed1Img;
        private Image speed2Img;
        private Image speed3Img;

        // sprites that the UI images can use
        private Sprite directionSpriteInactive;
        private Sprite directionSpriteActive;
        private Sprite switchedOn;
        private Sprite switchedOff;
        private Sprite spriteCamInactive;
        private Sprite spriteCamActive;
        private Sprite spriteSonarInactive;
        private Sprite spriteSonarActive;
        private Sprite spriteMapActive;
        private Sprite spriteMapInactive;
        private Sprite speed1Active;
        private Sprite speed1Inactive;
        private Sprite speed2Active;
        private Sprite speed2Inactive;
        private Sprite speed3Active;
        private Sprite speed3Inactive;

        // map
        private Transform mapTab;
        private Transform camTab;
        private RectTransform mapTransform;
        private RectTransform mapShipMarker;
        private ShipUITooltip zoomButtonTooltip;
        private MapZoom mapZoom;

        // cameras
        private RenderTexture renderTextureBottom;
        private RenderTexture renderTextureFront;
        private Camera cameraBottom;
        private Camera cameraFront;
        private RawImage camRawImage;
        private ShipCamera activeCamera;

        // audio
        private AudioClip buttonPressSound;
        private AudioSource buttonSource;
        private float poweringDownVoiceLineDelay = 2f;
        private float timeVehicleStopped;
        private bool stopVoiceLineQueued;
        private float poweringUpVoiceLineMinOfflineTime = 4f;

        void Awake()
        {
            ship = GetComponentInParent<SeaVoyager>();
            SetupHUD();
            SetActiveCamera(ShipCamera.Front);
        }

        void SetupHUD()
        {
            fwdImg = Helpers.FindChild(gameObject, "ForwardButton").GetComponent<Image>();
            reverseImg = Helpers.FindChild(gameObject, "ReverseButton").GetComponent<Image>();
            leftImg = Helpers.FindChild(gameObject, "LeftButton").GetComponent<Image>();
            rightImg = Helpers.FindChild(gameObject, "RightButton").GetComponent<Image>();
            stopImg = Helpers.FindChild(gameObject, "StopButton").GetComponent<Image>();
            bottomCamImg = Helpers.FindChild(gameObject, "BottomCameraButton").GetComponent<Image>();
            frontCamImg = Helpers.FindChild(gameObject, "FrontCameraButton").GetComponent<Image>();
            mapImg = Helpers.FindChild(gameObject, "MapButton").GetComponent<Image>();
            speed1Img = Helpers.FindChild(gameObject, "SpeedButton1").GetComponent<Image>();
            speed2Img = Helpers.FindChild(gameObject, "SpeedButton2").GetComponent<Image>();
            speed3Img = Helpers.FindChild(gameObject, "SpeedButton3").GetComponent<Image>();

            mapTab = Helpers.FindChild(gameObject, "MapTab").transform;
            camTab = Helpers.FindChild(gameObject, "CameraTab").transform;
            mapTransform = Helpers.FindChild(gameObject, "MapBG").GetComponent<RectTransform>();
            mapShipMarker = Helpers.FindChild(gameObject, "MapCenter").GetComponent<RectTransform>();

            fwdImg.GetComponent<Button>().onClick.AddListener(OnForward);
            reverseImg.GetComponent<Button>().onClick.AddListener(OnReverse);
            leftImg.GetComponent<Button>().onClick.AddListener(OnLeft);
            rightImg.GetComponent<Button>().onClick.AddListener(OnRight);
            stopImg.GetComponent<Button>().onClick.AddListener(OnStop);
            bottomCamImg.GetComponent<Button>().onClick.AddListener(SetTabBottomCam);
            frontCamImg.GetComponent<Button>().onClick.AddListener(SetTabFrontCam);
            mapImg.GetComponent<Button>().onClick.AddListener(SetTabMap);
            speed1Img.GetComponent<Button>().onClick.AddListener(OnSpeedSetting1);
            speed2Img.GetComponent<Button>().onClick.AddListener(OnSpeedSetting2);
            speed3Img.GetComponent<Button>().onClick.AddListener(OnSpeedSetting3);
            var zoomButton = Helpers.FindChild(gameObject, "MapZoomButton");
            zoomButton.GetComponent<Button>().onClick.AddListener(OnToggleMapZoom);

            fwdImg.gameObject.AddComponent<ShipUITooltip>().Init("Forward");
            reverseImg.gameObject.AddComponent<ShipUITooltip>().Init("Reverse");
            leftImg.gameObject.AddComponent<ShipUITooltip>().Init("Turn left");
            rightImg.gameObject.AddComponent<ShipUITooltip>().Init("Turn right");
            stopImg.gameObject.AddComponent<ShipUITooltip>().Init("Disable engine");
            bottomCamImg.gameObject.AddComponent<ShipUITooltip>().Init("View sonar map");
            frontCamImg.gameObject.AddComponent<ShipUITooltip>().Init("View forward-facing camera");
            mapImg.gameObject.AddComponent<ShipUITooltip>().Init("View region map");
            speed1Img.gameObject.AddComponent<ShipUITooltip>().Init("Slow speed");
            speed2Img.gameObject.AddComponent<ShipUITooltip>().Init("Standard speed");
            speed3Img.gameObject.AddComponent<ShipUITooltip>().Init("Emergency speed");
            mapShipMarker.gameObject.AddComponent<ShipUITooltip>().Init("Sea Voyager", false);
            zoomButtonTooltip = zoomButton.AddComponent<ShipUITooltip>();
            zoomButtonTooltip.Init("Zoom in");

            directionSpriteInactive = QPatch.bundle.LoadAsset<Sprite>("ArrowOff");
            directionSpriteActive = QPatch.bundle.LoadAsset<Sprite>("ArrowOn");

            switchedOn = QPatch.bundle.LoadAsset<Sprite>("ShipOn");
            switchedOff = QPatch.bundle.LoadAsset<Sprite>("ShipOff");

            spriteCamActive = QPatch.bundle.LoadAsset<Sprite>("CameraOn");
            spriteCamInactive = QPatch.bundle.LoadAsset<Sprite>("CameraOff");

            spriteSonarActive = QPatch.bundle.LoadAsset<Sprite>("SonarOn");
            spriteSonarInactive = QPatch.bundle.LoadAsset<Sprite>("SonarOff");

            spriteMapActive = QPatch.bundle.LoadAsset<Sprite>("MapOn");
            spriteMapInactive = QPatch.bundle.LoadAsset<Sprite>("MapOff");

            speed1Active = QPatch.bundle.LoadAsset<Sprite>("SpeedSetting1");
            speed1Inactive = QPatch.bundle.LoadAsset<Sprite>("SpeedSetting1Inactive");
            speed2Active = QPatch.bundle.LoadAsset<Sprite>("SpeedSetting2");
            speed2Inactive = QPatch.bundle.LoadAsset<Sprite>("SpeedSetting2Inactive");
            speed3Active = QPatch.bundle.LoadAsset<Sprite>("SpeedSetting3");
            speed3Inactive = QPatch.bundle.LoadAsset<Sprite>("SpeedSetting3Inactive");

            camRawImage = Helpers.FindChild(gameObject, "CameraView").GetComponent<RawImage>();
            renderTextureBottom = new RenderTexture(256, 128, 24);
            renderTextureFront = new RenderTexture(512, 256, 16);

            cameraBottom = Helpers.FindChild(ship.gameObject, "BottomCamera").AddComponent<Camera>();
            cameraBottom.fieldOfView = 70f;
            cameraBottom.targetTexture = renderTextureBottom;
            cameraBottom.gameObject.AddComponent<SonarCam>();

            cameraFront = Helpers.FindChild(ship.gameObject, "FrontCamera").AddComponent<Camera>();
            cameraFront.fieldOfView = 80f;
            cameraFront.targetTexture = renderTextureFront;

            // water fog fix
            cameraFront.gameObject.SetActive(false);

            var waterVolumeOnCamera = cameraFront.gameObject.AddComponent<WaterscapeVolumeOnCamera>();
            waterVolumeOnCamera.settings = WaterBiomeManager.main.gameObject.GetComponent<WaterscapeVolume>();

            // This literally KILLS your PC if you uncomment it. DONT!!!!
            //var waterSurfaceOnCamera = cameraFront.gameObject.AddComponent<WaterSurfaceOnCamera>();
            //waterSurfaceOnCamera.waterSurface = WaterSurface.Get();

            cameraFront.gameObject.SetActive(true);

            buttonSource = gameObject.AddComponent<AudioSource>();
            buttonSource.volume = QPatch.config.NormalizedAudioVolume;
            buttonPressSound = QPatch.bundle.LoadAsset<AudioClip>("ButtonPress");
        }

        void Start()
        {
            UpdateButtonImages();
        }

        void SetActiveCamera(ShipCamera cam)
        {
            activeCamera = cam;
            switch (cam)
            {
                default:
                    return;
                case ShipCamera.Front:
                    camRawImage.texture = renderTextureFront;
                    return;
                case ShipCamera.Bottom:
                    camRawImage.texture = renderTextureBottom;
                    return;
            }
        }

        void PlayClickSound()
        {
            buttonSource.PlayOneShot(buttonPressSound);
        }

        public void UpdateButtonImages()
        {
            fwdImg.sprite = directionSpriteInactive;
            reverseImg.sprite = directionSpriteInactive;
            leftImg.sprite = directionSpriteInactive;
            rightImg.sprite = directionSpriteInactive;
            stopImg.sprite = switchedOff;
            if (ship.currentState == ShipState.Idle)
            {
                stopImg.sprite = switchedOn;
            }
            if (ship.currentState == ShipState.Moving || ship.currentState == ShipState.MovingAndRotating)
            {
                if (ship.moveDirection == ShipMoveDirection.Forward) fwdImg.sprite = directionSpriteActive;
                else if (ship.moveDirection == ShipMoveDirection.Reverse) reverseImg.sprite = directionSpriteActive;
            }
            if (ship.currentState == ShipState.Rotating || ship.currentState == ShipState.MovingAndRotating)
            {
                if (ship.rotateDirection == ShipRotateDirection.Left) leftImg.sprite = directionSpriteActive;
                else if (ship.rotateDirection == ShipRotateDirection.Right) rightImg.sprite = directionSpriteActive;
            }
            var speed = ship.speedSetting;
            speed1Img.sprite = speed == ShipSpeedSetting.Slow ? speed1Active : speed1Inactive;
            speed2Img.sprite = speed == ShipSpeedSetting.Medium ? speed2Active : speed2Inactive;
            speed3Img.sprite = speed == ShipSpeedSetting.Fast ? speed3Active : speed3Inactive;
            zoomButtonTooltip.displayText = mapZoom == MapZoom.ZoomedIn ? "Zoom out" : "Zoom in";
        }

        #region Voices
        void TryPlayEnginePoweringUp()
        {
            if (Time.time < timeVehicleStopped + poweringUpVoiceLineMinOfflineTime)
            {
                return;
            }
            if (ship.voice.PlayVoiceLine(ShipVoice.VoiceLine.EnginePoweringUp, true))
            {
                MainCameraControl.main.ShakeCamera(3f, 5f, MainCameraControl.ShakeMode.BuildUp, 0.5f);
                ship.shipMove.StopMovementForSeconds(4f);
            }
        }
        void TryPlayEnginePoweringDown()
        {
            if (ship.voice.PlayVoiceLine(ShipVoice.VoiceLine.EnginePoweringDown))
            {
                MainCameraControl.main.ShakeCamera(1f, 2f, MainCameraControl.ShakeMode.Sqrt, 0.5f);
                stopVoiceLineQueued = false;
            }
        }

        #endregion
        #region Movement buttons
        void OnForward()
        {
            var lastState = ship.currentState;
            if (ship.moveDirection == ShipMoveDirection.Forward)
            {
                ship.moveDirection = ShipMoveDirection.Idle;
                if (ship.currentState == ShipState.MovingAndRotating) ship.currentState = ShipState.Rotating;
                else if (ship.currentState == ShipState.Moving)
                {
                    SetStateIdle();
                }
            }
            else if (ship.currentState == ShipState.Rotating || ship.currentState == ShipState.MovingAndRotating)
            {
                ship.currentState = ShipState.MovingAndRotating;
                ship.moveDirection = ShipMoveDirection.Forward;
            }
            else
            {
                ship.currentState = ShipState.Moving;
                ship.moveDirection = ShipMoveDirection.Forward;
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
            var lastState = ship.currentState;
            if (ship.moveDirection == ShipMoveDirection.Reverse)
            {
                ship.moveDirection = ShipMoveDirection.Idle;
                if (ship.currentState == ShipState.MovingAndRotating) ship.currentState = ShipState.Rotating;
                else if (ship.currentState == ShipState.Moving)
                {
                    SetStateIdle();
                }
            }
            else if (ship.currentState == ShipState.Rotating || ship.currentState == ShipState.MovingAndRotating)
            {
                ship.currentState = ShipState.MovingAndRotating;
                ship.moveDirection = ShipMoveDirection.Reverse;
            }
            else
            {
                ship.currentState = ShipState.Moving;
                ship.moveDirection = ShipMoveDirection.Reverse;
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
            if (ship.rotateDirection == ShipRotateDirection.Left)
            {
                ship.rotateDirection = ShipRotateDirection.Idle;
                if (ship.currentState == ShipState.MovingAndRotating) ship.currentState = ShipState.Moving;
                else if (ship.currentState == ShipState.Rotating)
                {
                    SetStateIdle();
                }
            }
            else if (ship.currentState == ShipState.Moving || ship.currentState == ShipState.MovingAndRotating)
            {
                ship.currentState = ShipState.MovingAndRotating;
                ship.rotateDirection = ShipRotateDirection.Left;
            }
            else
            {
                ship.currentState = ShipState.Rotating;
                ship.rotateDirection = ShipRotateDirection.Left;
            }
            OnRotationChanged();
            PlayClickSound();
            UpdateButtonImages();
        }
        void OnRight()
        {
            if (ship.rotateDirection == ShipRotateDirection.Right)
            {
                ship.rotateDirection = ShipRotateDirection.Idle;
                if (ship.currentState == ShipState.MovingAndRotating) ship.currentState = ShipState.Moving;
                else if (ship.currentState == ShipState.Rotating)
                {
                    SetStateIdle();
                }
            }
            else if (ship.currentState == ShipState.Moving || ship.currentState == ShipState.MovingAndRotating)
            {
                ship.currentState = ShipState.MovingAndRotating;
                ship.rotateDirection = ShipRotateDirection.Right;
            }
            else
            {
                ship.currentState = ShipState.Rotating;
                ship.rotateDirection = ShipRotateDirection.Right;
            }
            OnRotationChanged();
            PlayClickSound();
            UpdateButtonImages();
        }
        void SetStateIdle()
        {
            ship.currentState = ShipState.Idle;
            timeVehicleStopped = Time.time;
            stopVoiceLineQueued = true;
        }
        void OnRotationChanged()
        {
            if (ship.currentState == ShipState.Rotating || ship.currentState == ShipState.MovingAndRotating)
            {
                ship.rb.AddTorque(Vector3.up * -ship.rb.angularVelocity.y * 50f * ship.rb.mass);
            }
        }
        public void OnStop()
        {
            if (ship.currentState != ShipState.Idle)
            {
                SetStateIdle();
            }
            ship.currentState = ShipState.Idle;
            ship.moveDirection = ShipMoveDirection.Idle;
            ship.rotateDirection = ShipRotateDirection.Idle;
            UpdateButtonImages();
            PlayClickSound();
        }
        void OnSpeedSetting1()
        {
            ship.speedSetting = ShipSpeedSetting.Slow;
            UpdateButtonImages();
            PlayClickSound();
            ship.voice.PlayVoiceLine(ShipVoice.VoiceLine.AheadSlow);
        }
        void OnSpeedSetting2()
        {
            ship.speedSetting = ShipSpeedSetting.Medium;
            UpdateButtonImages();
            PlayClickSound();
            ship.voice.PlayVoiceLine(ShipVoice.VoiceLine.AheadStandard);
        }
        void OnSpeedSetting3()
        {
            ship.speedSetting = ShipSpeedSetting.Fast;
            UpdateButtonImages();
            PlayClickSound();
            ship.voice.PlayVoiceLine(ShipVoice.VoiceLine.AheadFlank);
        }
        #endregion

        #region Map
        void SetTabMap()
        {
            bottomCamImg.sprite = spriteSonarInactive;
            frontCamImg.sprite = spriteCamInactive;
            mapImg.sprite = spriteMapActive; 
            mapTab.gameObject.SetActive(true);
            camTab.gameObject.SetActive(false);
            PlayClickSound();
            ship.voice.PlayVoiceLine(ShipVoice.VoiceLine.RegionMap);
        }
        void OnToggleMapZoom()
        {
            if(mapZoom == MapZoom.Full)
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
            mapZoom = newZoom;
            if (mapZoom == MapZoom.Full)
            {
                mapTransform.localScale = new Vector2(0.3f, 0.3f);
            }
            else
            {
                mapTransform.localScale = new Vector2(1f, 1f);
            }
        }
        #endregion

        #region Camera
        void SetTabBottomCam()
        {
            SwitchToCameraMode();
            bottomCamImg.sprite = spriteSonarActive;
            SetActiveCamera(ShipCamera.Bottom);
            ship.voice.PlayVoiceLine(ShipVoice.VoiceLine.SonarMap);
        }
        void SetTabFrontCam()
        {
            SwitchToCameraMode();
            frontCamImg.sprite = spriteCamActive;
            SetActiveCamera(ShipCamera.Front);
        }
        void SwitchToCameraMode()
        {
            bottomCamImg.sprite = spriteSonarInactive;
            frontCamImg.sprite = spriteCamInactive;
            mapImg.sprite = spriteMapInactive;
            mapTab.gameObject.SetActive(false);
            camTab.gameObject.SetActive(true);
            PlayClickSound();
        }
        #endregion

        void Update()
        {
            if (ship.LOD.current == LODState.Full) //Only run if you are close to or inside of the ship.
            {
                cameraBottom.enabled = activeCamera == ShipCamera.Bottom && ship.IsOccupiedByPlayer;
                cameraFront.enabled = activeCamera == ShipCamera.Front && ship.IsOccupiedByPlayer;
                if (mapZoom == MapZoom.Full)
                {
                    mapTransform.localPosition = Vector2.zero;
                    mapShipMarker.localPosition = new Vector2(ship.transform.position.x / 26.3f, ship.transform.position.z / 26.3f);
                }
                else
                {
                    mapTransform.localPosition = new Vector2(-ship.transform.position.x / 7.8f, -ship.transform.position.z / 7.8f);
                    mapShipMarker.localPosition = Vector2.zero;
                }
                mapShipMarker.localEulerAngles = new Vector3(0f, 0f, -ship.transform.eulerAngles.y);

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
                cameraBottom.enabled = false;
                cameraBottom.enabled = false;
            }
            if (stopVoiceLineQueued && Time.time > timeVehicleStopped + poweringDownVoiceLineDelay && ship.Idle)
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
