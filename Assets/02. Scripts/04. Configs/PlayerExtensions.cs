using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class extends Photon's PhotonPlayer object by custom properties.
/// Provides several methods for setting and getting variables out of them.
/// </summary>
public static class PlayerExtensions
{
    public const string bullet = "bullet";

    /// <summary>
    /// Offline: returns the bullet index of a bot stored in PlayerBot.
    /// Fallback to online mode for the master or in case offline mode was turned off.
    /// </summary>
    public static int GetBullet(this PhotonView player)
    {
        if (PhotonNetwork.offlineMode == true)
        {
        /*
            PlayerBot bot = player.GetComponent<PlayerBot>();
            if (bot != null)
            {
                return bot.currentBullet;
            }
        */
        }

        return player.owner.GetBullet();
    }

    /// <summary>
    /// Online: returns the networked bullet index of the player out of properties.
    /// </summary>
    public static int GetBullet(this PhotonPlayer player)
    {
        return System.Convert.ToInt32(player.CustomProperties[bullet]);
    }

    /// <summary>
    /// Offline: synchronizes the currently selected bullet of a PlayerBot locally.
    /// Fallback to online mode for the master or in case offline mode was turned off.
    /// </summary>
    public static void SetBullet(this PhotonView player, int value)
    {
        if (PhotonNetwork.offlineMode == true)
        {
        /*
            PlayerBot bot = player.GetComponent<PlayerBot>();
            if (bot != null)
            {
                bot.currentBullet = value;
                return;
            }
        */
        }

        player.owner.SetBullet(value);
    }

    /// <summary>
    /// Online: Synchronizes the currently selected bullet of the player for all players via properties.
    /// </summary>
    public static void SetBullet(this PhotonPlayer player, int value)
    {
        player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { bullet, (byte)value } });
    }

    /// <summary>
    /// Offline: clears all properties of a PlayerBot locally.
    /// Fallback to online mode for the master or in case offline mode was turned off.
    /// </summary>
    public static void Clear(this PhotonView player)
    {
        if (PhotonNetwork.offlineMode == true)
        {
/*
            PlayerBot bot = player.GetComponent<PlayerBot>();
            if (bot != null)
            {
                bot.currentBullet = 0;
                bot.health = 0;
                bot.shield = 0;
                return;
            }
*/
        }

        player.owner.Clear();
    }

    /// <summary>
    /// Online: Clears all networked variables of the player via properties in one instruction.
    /// </summary>
    public static void Clear(this PhotonPlayer player)
    {
        player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { PlayerExtensions.bullet, (byte)0 } });
        /*
            player.SetCustomProperties(new Hashtable() { { PlayerExtensions.bullet, (byte)0 },
                                                             { PlayerExtensions.health, (byte)0 },
                                                             { PlayerExtensions.shield, (byte)0 } });
        */
    }
}
