using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DelayTpToHome
{
    public class DelayTpToHome : RocketPlugin
    {
        static public DelayTpToHome Instance;

        public Dictionary<CSteamID, DateTime> TeleportationPlayers;

        protected override void Load()
        {
            Instance = this;
            TeleportationPlayers = new Dictionary<CSteamID, DateTime>();
        }

        protected override void Unload()
        {
            
        }

        private void FixedUpdate()
        {
            foreach (var item in TeleportationPlayers)
            {
                UnturnedPlayer player = UnturnedPlayer.FromCSteamID(item.Key);
                if ((DateTime.Now - item.Value).TotalSeconds >= 10) 
                {
                    
                    if (BarricadeManager.tryGetBed(item.Key, out Vector3 position, out byte angle))
                    {

                        position.y += 0.5f;
                        player.Player.teleportToLocation(position, player.Player.look.yaw);
                        UnturnedChat.Say(player, Translate("command_home_successfully"), Color.green);
                    }
                    else
                    {
                        UnturnedChat.Say(player, Translate("command_home_not_found"),Color.red);
                    }
                    TeleportationPlayers.Remove(item.Key);
                    break;
                }
            }
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            {"command_home_successfully", "Вы успешно телепортированы на спальник." },
            {"command_home_cooldawn", "Вы будете телепортированы через 10 секунд." },
            {"command_home_not_found", "Спальник не найден." },
            {"command_home_already_tp", "Вы уже телепортируетесь." },
            {"command_home_error", "Не удалось телепортироваться." },
        };


        

    }
}
