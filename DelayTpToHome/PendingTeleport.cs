using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DelayTpToHome
{
    public class PendingTeleport
    {
        public DateTime Date;
        public ushort CoolDown;

        public Vector3 LastPosition;


        public PendingTeleport(ushort coolDown, Vector3 lastPosition)
        {
            Date = DateTime.Now;
            CoolDown = coolDown;
            LastPosition = lastPosition;
        }

    }
}
