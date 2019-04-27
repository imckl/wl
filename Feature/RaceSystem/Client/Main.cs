using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Newtonsoft.Json;

using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.NaturalMotion;
using CitizenFX.Core.UI;
using Client.Menus;
using MenuAPI;

namespace Client
{
    public class Main : BaseScript
    {
        private int camHandle = -1;
        private static MainMenu mainMenu;

        private int placementHandle;
        //private int errPlacementHandle;
        private int ringHandle;

        private int creatorIndex = -1;
        private List<Vector3> creatorCheckpointPos;
        /// <summary>
        /// creatorCheckpointPos:JsonFormat
        /// </summary>
        private string creatorCheckpointPosJson
        {
            get
            {
                return JsonConvert.SerializeObject(creatorCheckpointPos);
            }
        }
        private List<int> creatorBlips;
        private List<int> creatorCheckpoints;

        private bool isReceived;
        private string response;

        private CameraProcessor camProcessor;
        private double timerLocal = 0.0;

        public Main()
        {
            // Add task to every tick
            Tick += OnTick;;
            
            // Initialize instance variables
            mainMenu = new MainMenu();
            creatorCheckpointPos = new List<Vector3>();
            creatorBlips = new List<int>();
            creatorCheckpoints = new List<int>();
            camProcessor = new CameraProcessor();
            
            // Register command and events
            API.RegisterCommand("race", new Action<int, List<object>, string>(RaceCommand), false);
            EventHandlers["rs:Received"] += new Action<string>(Received);
            EventHandlers["rs:SetRaces"] += new Action<List<Dictionary<string, string>>>(SetRaces);
            EventHandlers["rs:SetRacesJson"] += new Action<string>(SetRacesJson);
        }

        private void SetRaces(List<Dictionary<string, string>> races)
        {
            mainMenu.SetRaces(races);
        }

        /// <summary>
        /// Set races as json format
        /// </summary>
        /// <param name="racesAsJson"></param>
        private void SetRacesJson(string racesAsJson)
        {
            mainMenu.SetRacesJson(racesAsJson);
        }

        private void Received(string resp)
        {
            response = resp;
            isReceived = true;
        }

        private void RaceCommand(int source, List<object> args, string raw)
        {
            // Open the menu
            mainMenu.GetMenu().OpenMenu();
        }

        private void StartFreeCam()
        {
            API.ClearFocus();
            
            Vector3 playerCoord = Game.PlayerPed.Position;
            SetPlayerEnterCam(Game.Player.Handle, true);
            
            camHandle = API.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", playerCoord.X, playerCoord.Y, playerCoord.Z, 0.0f, 0.0f, 0.0f,
                API.GetGameplayCamFov(), false, 2);
            
            API.SetCamActive(camHandle, true);
            API.RenderScriptCams(true, false, 0, true, false);
            API.SetCamAffectsAiming(camHandle, false);
        }

        private async Task<int> CreateLocalObject(string name, Vector3 pos)
        {
            int hash = API.GetHashKey(name);
            API.RequestModel((uint) hash);
            while (!API.HasModelLoaded((uint) hash))
                await Delay(1);
            return API.CreateObject(hash, pos.X, pos.Y, pos.Z, false, true, false);
        }

        private async Task StartPlacingObj()
        {
            string placementName = "prop_mp_placement";
            //string errPlacementName = "prop_mp_placement_red";
            string ringName = "prop_mp_pointer_ring";

            Vector3 playerPos = Game.PlayerPed.Position;
            placementHandle = await CreateLocalObject(placementName, playerPos);
            //errPlacementHandle = await CreateLocalObject(errPlacementName, playerPos);
            ringHandle = await CreateLocalObject(ringName, playerPos);
        }

        private void UpdateObjects(Vector3 pos, Vector3 rot)
        {
                API.SetEntityCoords(placementHandle, pos.X, pos.Y, pos.Z, true, false, false, true);
                API.SetEntityCoords(ringHandle, pos.X, pos.Y, pos.Z, true, false, false, true);
            
                API.SetEntityRotation(placementHandle, rot.X, rot.Y, rot.Z, 2, true);
                API.SetEntityRotation(ringHandle, rot.X, rot.Y, rot.Z, 2, true);
        }

        private void UpdatePlacingObj()
        {
            Camera currCam = new Camera(camHandle);
            Entity placementEntity = Entity.FromHandle(placementHandle);
           
            RaycastResult result = World.Raycast(currCam.Position, currCam.GetOffsetPosition(new Vector3(0.0f, 1000.0f, 0.0f)),
                IntersectOptions.Everything, placementEntity);
            Vector3 hitPos = result.HitPosition;
            
            UpdateObjects(hitPos, API.GetEntityRotation(placementHandle, 2));
            float height = API.GetEntityHeightAboveGround(placementHandle);

            if (API.PlaceObjectOnGroundProperly(placementHandle) &&
                API.PlaceObjectOnGroundProperly(ringHandle))
            {
                if (height >= 0.1f)
                    UpdateObjects(hitPos, new Vector3(0.0f,0.0f,0.0f));
            }

            if (API.IsInputDisabled(0))
            {  
                // Key "F"
                if (API.IsDisabledControlJustReleased(1, 23))
                {
                    creatorCheckpointPos.Add(new Vector3(hitPos.X, hitPos.Y, hitPos.Z));
                    int cp = API.CreateCheckpoint(42, hitPos.X, hitPos.Y, hitPos.Z, hitPos.X, hitPos.Y, hitPos.Z - 1.0f, 8.0f,
                        204, 204, 1, 100, creatorIndex);
                    creatorCheckpoints.Add(cp);
                    creatorIndex++;
                    
                    int blip = API.AddBlipForCoord(hitPos.X, hitPos.Y, hitPos.Z);
                    // Sprite Circle
                    API.SetBlipSprite(blip, 1);
                    // Display on both mini map and main map (Not Selectable)
                    API.SetBlipDisplay(blip, 8);
                    // Normal size
                    API.SetBlipScale(blip, 1.0f);
                    // Dark Taxi Yellow
                    API.SetBlipColour(blip, 28);
                    // Long range Blip
                    API.SetBlipAsShortRange(blip, false);
                    creatorBlips.Add(blip);
                }
            }

        }

        private void CreatorCleanUp()
        {
            creatorBlips.ForEach(blip => API.RemoveBlip(ref blip));
            creatorCheckpointPos.Clear();
            creatorCheckpoints.ForEach(API.DeleteCheckpoint);
        }

        private void StopPlacingObj()
        {
            API.SetEntityAsMissionEntity(placementHandle, true, true);
            //API.SetEntityAsMissionEntity(errPlacementHandle, true, true);
            API.SetEntityAsMissionEntity(ringHandle, true, true);
            
            API.DeleteObject(ref placementHandle);
            //API.DeleteObject(ref errPlacementHandle);
            API.DeleteObject(ref ringHandle);
        }
        
        private void StopFreeCam()
        {
            API.ClearFocus();

            SetPlayerEnterCam(Game.Player.Handle, false);
            
            API.RenderScriptCams(false, false, 0, true, false);
            API.DestroyCam(camHandle, false);

            camHandle = -1;
        }

        private void UpdateFreeCam()
        {
            API.DisableFirstPersonCamThisFrame();
            API.BlockWeaponWheelThisFrame();
            
            int playerPedId = API.PlayerPedId();
            Vector3 camCoord = API.GetCamCoord(camHandle);

            Vector3 newPos = camProcessor.ProcessNewPosition(camCoord);
            API.SetFocusArea(newPos.X, newPos.Y, newPos.Z, 0.0f, 0.0f, 0.0f);
            API.SetCamCoord(camHandle, newPos.X, newPos.Y, newPos.Z);
            API.SetCamRot(camHandle, camProcessor.offsetRotX, 0.0f, camProcessor.offsetRotZ, 2);
        }

        private async Task CheckSave()
        {
            if (mainMenu.requestSave)
            {
                mainMenu.requestSave = false;
                TriggerServerEvent("rs:SaveRaceJson", mainMenu.raceName, creatorCheckpointPosJson);
                //TriggerServerEvent("rs:SaveRace", mainMenu.raceName, creatorCheckpointPos);
                while (!isReceived)
                {
                    await Delay(100);
                    timerLocal += 100;
                    
                    if(timerLocal >= 1000)
                        break;
                }
                
                // Notification above mini map
                API.SetNotificationTextEntry("STRING");
                API.AddTextComponentString(response == "OK" ? "Saved Successfully!" : "Save Failed!");
                API.DrawNotification(false, true);
            }
        }

        private void CheckCleanUp()
        {
            if (mainMenu.requestCleanUp)
            {
                mainMenu.requestCleanUp = false;
                CreatorCleanUp();
            }
        }

        private void SetPlayerEnterCam(int handle, bool flag)
        {
            API.SetEntityCollision(handle, !flag, !flag);
            API.SetEntityVisible(handle, !flag, false);
            API.SetPlayerControl(handle, !flag, 0);
            API.SetEntityInvincible(handle, flag);
            API.FreezeEntityPosition(handle, flag);
        }

        private async Task OnTick()
        {
            if (mainMenu.isPlacingCP)
            {
                if (camHandle == -1)
                {
                    creatorIndex = 0;
                    StartFreeCam();
                    await StartPlacingObj();
                }
                else
                {
                    UpdateFreeCam();
                    UpdatePlacingObj();
                    API.SetBigmapActive(true, false);
                }
            }
            else if (camHandle != 1)
            {
                creatorIndex = -1;
                StopFreeCam();
                StopPlacingObj();
                API.SetBigmapActive(false, false);
            }

            await CheckSave();
            await Delay(0);
        }
    }
}
