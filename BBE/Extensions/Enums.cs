﻿using BBE.CustomClasses;
using BBE.Extensions;
using BBE.Creators;
using MTM101BaldAPI.Registers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BBE.Extensions
{
    public enum PatchesType
    {
        Prefix,
        Transpiler,
        Postfix
    }
    public enum Layer
    {
        Window,
        Billboard,
        IClickable,
        ClickableEntities,
        IgnoreRaycast,
        BlockRaycast,
        StandardEntities,
        Map,
        UI,
        GumCollision,
        EntityCollision,
        PrincipalLooker
    }
    public enum ModdedRandomEvent
    {
        TeleportationChaos,
        SoundEvent,
        HookChaos,
        ElectricityEvent,
        FireEvent,
        None
    }
    public enum ModdedGenericHallBuilders
    {
        StrawberryZestyMachine,
        NotebookDoor,
        YTPDoor,
        BeltBuilder,
        None
    }
    public enum ModdedCharacters
    {
        Kulak,
        Snail,
        Andrey,
        MrPaint,
        Stockfish,
        Tesseract,
        None
    }
    public enum ModdedItems
    {
        Calculator,
        GravityDevice,
        PotionOfSpeed,
        Shield,
        Glue,
        IceBomb,
        MagicRuby,
        StrawberryZestyBar,
        DSODA,
        PaintBucket,
        TimeRewinderElectronicWristwatch,
        Weight,
        XrayGoggles,
        RubyClock,
        BaldiTeleporter,
        NoSign,
        RoomTeleporter,
        FlipperZero,
        BaldiDashPad,
        ChessBook,
        UnoReverseCard,
        None
    }
}
