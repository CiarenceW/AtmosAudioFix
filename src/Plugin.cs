using BepInEx;
using Receiver2;
using UnityEngine.Advertisements;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using FMOD.Studio;
using FMODUnity;
using System.Reflection;
using BepInEx.Configuration;
using System;

namespace AtmosAudioFix
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private ConfigEntry<float> configAmbientVolume;
        //private ConfigEntry<float> configAmbientHeight; As it turns out, this whole thing is even more fucked than I thought. Probably happened when they updated FMOD.
        // the only way to hear them is to basically be ON the emitter. It's fucked.
        private Transform ambient_emitter;
        private int rank;

        ReceiverCoreScript RCS;
        string[] atmos_names =
        {
            "0-1 Rank Ambient",
            "0-1 Rank Ambient",
            "2 Sleeper Ambient",
            "3 SleepWalker Ambient",
            "4 Dawned Ambient",
            "5 Awake Ambient"
        };
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            configAmbientVolume = Config.Bind("Volume",
                "AmbientVolume",
                0.5f, //is 1 supposed to be max volume? because even at 0.5f it's pretty damn fucking loud.
                new ConfigDescription("The volume of the emitter.", new AcceptableValueRange<float>(0f, 1f)));

            configAmbientVolume.SettingChanged += (object sender, EventArgs args) =>
            {
                ambient_emitter.GetComponent<StudioEventEmitter>().EventInstance.setVolume(configAmbientVolume.Value);
            };

            /*configAmbientHeight = Config.Bind("Position",
                "AmbientPosition",
                210f,
                "The height of the emitters, don't change unless you know what you're doing.");
            */

            ReceiverEvents.StartListening(ReceiverEventTypeVoid.PlayerInitialized, new UnityAction<ReceiverEventTypeVoid>(PlayerInitialized));
        }
        private void PlayerInitialized(ReceiverEventTypeVoid ev)
        {
            RCS = ReceiverCoreScript.Instance();

            if (RCS.game_mode.GetGameMode() != GameMode.RankingCampaign) return;

            rank = ((RankingProgressionGameMode)RCS.game_mode).progression_data.receiver_rank;
            var ambient_parent = transform.Find(string.Format("/{0}", atmos_names[rank]));

            if (ambient_parent != null)
            {
                /*foreach (Transform atmos_transform in ambient_parent.transform)
                {
                    StudioEventEmitter fmod_event;
                    if (atmos_transform.TryGetComponent<FMODUnity.StudioEventEmitter>(out fmod_event))
                    {
                        var atmos_pos = atmos_transform.position;
                        atmos_pos.y = configAmbientHeight.Value;
                        fmod_event.EventInstance.setVolume(configAmbientVolume.Value);
                    }
                    else
                    {
                        Debug.LogError("wrong transform, asshole.");
                        Debug.LogError(rank);
                        Debug.LogError(ambient_parent);
                    }
                }*/
                ambient_emitter = ambient_parent.GetChild(0);
                ambient_emitter.GetComponent<StudioEventEmitter>().EventInstance.setVolume(configAmbientVolume.Value);
                ambient_emitter.parent = RCS.player.player_object.transform;
                ambient_emitter.localPosition = new Vector3(0f, -5f, 0f);
                ambient_emitter.localRotation = Quaternion.identity;
            }
            else
            {
                Debug.LogError("the fucking thing is null, fuckface");
            }
        }
    }
}
