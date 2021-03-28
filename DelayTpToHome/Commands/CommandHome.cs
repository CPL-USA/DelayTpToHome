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

            if (DelayTpToHome.Instance.PendingPlayersTeleport.ContainsKey(player.CSteamID))
            {
                return;
            }

            if (!BarricadeManager.tryGetBed(player.CSteamID, out Vector3 bed, out byte angle))
            {
                UnturnedChat.Say(player, DelayTpToHome.Instance.Translate("command_home_cancel_not_found"), Color.red);
                return;
            }

            ushort cooldown = DelayTpToHome.Instance.Configuration.Instance.AdminsCoolDown;
            if (!player.IsAdmin)
            {
                cooldown = DelayTpToHome.Instance.Configuration.Instance.GetPlayerTeleportCoolDown(player);
            }
            DelayTpToHome.Instance.PendingPlayersTeleport.Add(player.CSteamID, new PendingTeleport(cooldown, player.Position));

            UnturnedChat.Say(player, DelayTpToHome.Instance.Translate("command_home_bed_found_no_move", player.CharacterName, cooldown), Color.green);
            


        }
    }
}
