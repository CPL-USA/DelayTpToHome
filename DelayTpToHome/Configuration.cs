using Rocket.API;
using Rocket.API.Serialisation;
using Rocket.Core;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayTpToHome
{
    public class Configuration : IRocketPluginConfiguration
    {
        public ushort AdminsCoolDown;

        public CoolDown[] CoolDowns;

        public bool TeleportingWhenMove;

        public bool TeleportingWhenBleeding;

        public void LoadDefaults()
        {
            AdminsCoolDown = 0;
            CoolDowns = new CoolDown[]
            {
                new CoolDown("default", 10),
                new CoolDown("Premium", 8),
            };
            TeleportingWhenBleeding = true;
            TeleportingWhenMove = true;
        }

        public ushort GetPlayerTeleportCoolDown(UnturnedPlayer player)
        {
            List<RocketPermissionsGroup> groups = R.Permissions.GetGroups(player, true);

            ushort result = ushort.MaxValue;
            bool flag = false;

            foreach (RocketPermissionsGroup group in groups)
            {
                foreach (CoolDown cooldown in CoolDowns)
                {
                    if (group.Id == cooldown.GroupId)
                    {
                        flag = true;
                        if (result > cooldown.Kd)
                        {
                            result = cooldown.Kd;
                        }
                    }
                }
            }

            return flag ? result : (ushort)0;

        }
    }
}
