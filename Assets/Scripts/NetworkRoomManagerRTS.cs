using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

namespace RTS.Networking
{
    /// <summary>
    /// Manages scenes and connected clients
    /// </summary>
    public class NetworkRoomManagerRTS : NetworkRoomManager
    {
        bool showStartButton;

        /// <summary>
        /// This method is called after GamePlayer Object is instantiated and just before it replaces RoomPlayer Object.
        /// This is where you pass data from the RoomPlayer to the GamePlayer object as it is about to enter the online scene.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="roomPlayer"></param>
        /// <param name="gamePlayer"></param>
        /// <returns>true unless code makes it abort </returns>
        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer, GameObject gamePlayer)
        { 
            return true;
        }

        /*
           This code below is to demonstrate how to do a Start button that only appears for the Host player
           showStartButton is a local bool that's needed because OnRoomServerPlayersReady is only fired when
           all players are ready, but if a player cancels their ready state there's no callback to set it back to false
           Therefore, allPlayersReady is used in combination with showStartButton to show/hide the Start button correctly.
           Setting showStartButton false when the button is pressed hides it in the game scene since NetworkRoomManager
           is set as DontDestroyOnLoad = true.
   
           Note: This could maybe be fixed with having a callBack function on client Disconnected.

             */


        /// <summary>
        /// Think this is called when all players are ready.
        /// </summary>
        public override void OnRoomServerPlayersReady()
        {
            if(isHeadless)
            {
                base.OnRoomServerPlayersReady();
            }
            else
            {
                showStartButton = true;
            }
        }


        public override void OnGUI()
        {
            base.OnGUI();

            if (allPlayersReady && showStartButton && GUI.Button(new Rect(150, 300, 120, 20), "START GAME"))
            {
                // set to false to hide it in the game scene
                showStartButton = false;

                ServerChangeScene(GameplayScene);
            }
        }
    }

}

