using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

namespace RTS.Networking
{

    /// <summary>
    ///  Room Player, used in lobby
    /// </summary>
    public class NetworkRoomPlayerRTS : NetworkRoomPlayer
    {
        public override void OnStartClient()
        {
            base.OnStartClient();
        }

        public override void OnClientEnterRoom()
        {

        }

        public override void OnClientExitRoom()
        {
           
        }
    }
}