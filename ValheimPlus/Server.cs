﻿using System;
using UnityEngine;
using HarmonyLib;
using Steamworks;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(ZNet), "Awake")]
    public static class ChangeGameServerVariables
    {
        private static void Postfix(ref ZNet __instance)
        {
            if (Settings.isEnabled("Server"))
            {
                int maxPlayers = Settings.getInt("Server", "maxPlayers");
                if (maxPlayers >= 1)
                {
                    // Set Server Instance Max Players
                    __instance.m_serverPlayerLimit = maxPlayers;
                }
            }

            if (Settings.getBool("Map", "playerPositionPublicOnJoin"));
            {
                // Set player position visibility to public by default on server join
                __instance.m_publicReferencePosition = true;
            }
        }
    }

    [HarmonyPatch(typeof(SteamGameServer), "SetMaxPlayerCount")]
    public static class ChangeSteamServerVariables
    {
        private static void Prefix(ref int cPlayersMax)
        {
            if (Settings.isEnabled("Server"))
            {
                int maxPlayers = Settings.getInt("Server", "maxPlayers");
                if (maxPlayers >= 1)
                {
                    cPlayersMax = maxPlayers;
                }
            }
        }
    }

    [HarmonyPatch(typeof(FejdStartup), "IsPublicPasswordValid")]
    public static class ChangeServerPasswordBehavior
    {
        private static void Postfix(ref Boolean __result) // Set after awake function
        {
            if (Settings.isEnabled("Server"))
            {
                if (Settings.getBool("Server", "disableServerPassword"))
                {
                    __result = true;
                }
            }
        }
    }
}
