using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DelayTpToHome.Commands
{
    public class CommandHome : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "home";

        public string Help => string.Empty;

        public string Syntax => string.Empty;

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            bool result = false;

            foreach (CSteamID item in DelayTpToHome.Instance.TeleportationPlayers)
            {
                if (item == player.CSteamID)
                {
                    result = true;
                    break;
                }
            }
            if (result==false)
            {
                if (BarricadeManager.tryGetBed(player.CSteamID, out Vector3 point, out byte angle))
                {
                    point.y += 0.5f;
                    DelayTpToHome.Instance.TeleportationPlayers.Add(player.CSteamID);
                    DelayTpToHome.Instance.StartCoroutine(DelayTpToHome.Instance.HomeTeleportationInterval(player, point));
                }
                else
                {
                    UnturnedChat.Say(player, DelayTpToHome.Instance.Translate("command_home_not_found"), Color.red);
                }
            }
            else
            {
                UnturnedChat.Say(player, DelayTpToHome.Instance.Translate("command_home_already_tp"), Color.yellow);
            }
        }
    }
}
