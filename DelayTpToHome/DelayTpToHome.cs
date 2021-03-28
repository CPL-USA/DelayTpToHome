using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core;
using Rocket.Core.Commands;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DelayTpToHome
{
    public class DelayTpToHome : RocketPlugin<Configuration>
    {
        public static DelayTpToHome Instance;
        public Dictionary<CSteamID, PendingTeleport> PendingPlayersTeleport;

        protected override void Load()
        {
            Instance = this;
            PendingPlayersTeleport = new Dictionary<CSteamID, PendingTeleport>();
        }

        protected override void Unload()
        {
            
        }

        public void FixedUpdate()
        {
            foreach (KeyValuePair<CSteamID, PendingTeleport> pair in PendingPlayersTeleport)
            {
                UnturnedPlayer player = UnturnedPlayer.FromCSteamID(pair.Key);
                if (player != null)
                {
                    if ((DateTime.Now - pair.Value.Date).TotalSeconds >= pair.Value.CoolDown && Configuration.Instance.TeleportingWhenBleeding == false && Configuration.Instance.TeleportingWhenMove == false)
                    {
                        UnturnedChat.Say(player, Translate("command_home_bed_found_move", player.CharacterName, pair.Value.CoolDown), Color.green);
                        player.Player.teleportToBed();
                        UnturnedChat.Say(player, Translate("command_home_successfully"), Color.green);
                        PendingPlayersTeleport.Remove(player.CSteamID);
                        break;
                    }
                    else if (Configuration.Instance.TeleportingWhenBleeding == true && player.Bleeding)
                    {
                        UnturnedChat.Say(Translate("command_home_cancel_bleeding"), Color.red);
                        PendingPlayersTeleport.Remove(player.CSteamID);
                        RemovePlayerCommandCooldown(player, "home");
                        break;
                    }
                    else if (Configuration.Instance.TeleportingWhenMove == true && pair.Value.LastPosition != player.Position)
                    {
                        UnturnedChat.Say(Translate("command_home_cancel_when_move"), Color.red);
                        PendingPlayersTeleport.Remove(player.CSteamID);
                        RemovePlayerCommandCooldown(player, "home");
                        break;
                    }
                    else if (player.Stance == EPlayerStance.DRIVING || player.Stance == EPlayerStance.SITTING)
                    {
                        UnturnedChat.Say(Translate("command_home_cancel_vehicle"), Color.red);
                        PendingPlayersTeleport.Remove(player.CSteamID);
                        RemovePlayerCommandCooldown(player, "home");
                        break;
                    }
                    else if (player.Dead)
                    {
                        UnturnedChat.Say(player, Translate("command_home_cancel_dead"), Color.red);
                        PendingPlayersTeleport.Remove(player.CSteamID);
                        RemovePlayerCommandCooldown(player, "home");
                        break;
                    }
                    else if ((DateTime.Now - pair.Value.Date).TotalSeconds >= pair.Value.CoolDown)
                    {
                        player.Player.teleportToBed();
                        UnturnedChat.Say(player, Translate("command_home_successfully"), Color.green);
                        PendingPlayersTeleport.Remove(player.CSteamID);
                        break;
                    }
                }
            }
        }
        
        public void RemovePlayerCommandCooldown(UnturnedPlayer player, string command)
        {
            try
            {
                IList list = (IList)typeof(RocketCommandManager).GetField("cooldown", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(R.Commands);
                Type rocketCommandCooldownType = typeof(R).Assembly.GetType("Rocket.Core.Commands.RocketCommandCooldown", true);


                foreach (object obj in list)
                {
                    IRocketPlayer p = (IRocketPlayer)rocketCommandCooldownType.GetField("Player", BindingFlags.Public | BindingFlags.Instance).GetValue(obj);
                    IRocketCommand cmd = (IRocketCommand)rocketCommandCooldownType.GetField("Command", BindingFlags.Public | BindingFlags.Instance).GetValue(obj);

                    if (player.Id == p.Id && cmd.Name == command)
                    {
                        list.Remove(obj);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Rocket.Core.Logging.Logger.Log("ERROR:: " + ex.ToString());
            }
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            {"command_home_successfully", "Вы успешно телепортированы на спальник." }, 
            {"command_home_cancel_vehicle", "Телепортация прервана, Вы находитесь в транспорте." }, 
            {"command_home_bed_found_no_move", "Ваш спальник найден {0}, не двигайтесь {1} секунд, пока идет подготовка к телепортации." }, 
            {"command_home_bed_found_move", "Ваш спальник найден {0}, подождите {1}, чтобы телепортироваться." }, 

            {"command_home_movement_restricted_true", "Вы двигались, телепортация прервана." }, 
            {"command_home_cancel_dead", "Телепортация прервана, Вы погибли." }, 
            {"command_home_cancel_not_found", "Спальник не найден." }, 
            {"command_home_cancel_bleeding", "Телепортация отменена из-за кровотечения." }, 

            {"command_home_cancel_when_move", "Телепортация отменена, Вы двигались."}, 

        };
    }
}
