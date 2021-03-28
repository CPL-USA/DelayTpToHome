using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DelayTpToHome
{
    [Serializable]
    public class CoolDown
    {
        [XmlAttribute("KD")] public ushort Kd;
        [XmlAttribute("GroupID")] public string GroupId;

        public CoolDown(string groupid, ushort kd)
        {
            GroupId = groupid;
            Kd = kd;
        }

        public CoolDown() { }
    }
}
