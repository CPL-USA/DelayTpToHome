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
            

            if (!BarricadeManager.tryGetBed(player.CSteamID, out Vector3 position, out byte angle))
            {
                UnturnedChat.Say(player, DelayTpToHome.Instance.Translate("command_home_not_found"), Color.red);
                return;
            }
            bool result = false;
            foreach (var item in DelayTpToHome.Instance.TeleportationPlayers)
            {
                if (item.Key==player.CSteamID)
                {
                    result = true;
                    break;
                }
               
            }
            if (result==false)
            {
                DelayTpToHome.Instance.TeleportationPlayers.Add(player.CSteamID, DateTime.Now);
                UnturnedChat.Say(player, DelayTpToHome.Instance.Translate("command_home_cooldawn"), Color.green);
            }
            else
            {
                UnturnedChat.Say(player, DelayTpToHome.Instance.Translate("command_home_already_tp"), Color.yellow);
            }
            

           
        }
    }
}
