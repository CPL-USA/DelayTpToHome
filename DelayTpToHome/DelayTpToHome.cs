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

        public  List<CSteamID> TeleportationPlayers;

        protected override void Load()
        {
            Instance = this;
            TeleportationPlayers = new List<CSteamID>();
        }

        protected override void Unload()
        {
            
        }


        public override TranslationList DefaultTranslations => new TranslationList()
        {
            {"command_home_successfully", "Вы успешно телепортированы на спальник." },
            {"command_home_cooldawn", "Вы будете телепортированы через 10 секунд." },
            {"command_home_not_found", "Спальник не найден." },
            {"command_home_already_tp", "Вы уже телепортируетесь." },
        };


        public IEnumerator HomeTeleportationInterval(UnturnedPlayer player, Vector3 position )
        {
            UnturnedChat.Say(player, Translate("command_home_cooldawn"), Color.yellow);
            yield return new WaitForSeconds(10);
            if (player.Player.teleportToLocation(position, player.Player.look.yaw))
            {
                UnturnedChat.Say(player, Translate("command_home_successfully"), Color.green);

                TeleportationPlayers.Remove(player.CSteamID);
            }
        }

    }
}
