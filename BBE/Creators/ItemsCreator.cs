using BBE.CustomClasses;
using BBE.Extensions;
using BBE.Helpers;
using BBE.ModItems;
using HarmonyLib;
using MTM101BaldAPI.ObjectCreation;
using MTM101BaldAPI.Registers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBE.Creators
{
    class ItemsCreator
    {
        private static void MakeForced(ItemObject itemObject, params string[] floors) => 
            floors.Do(x => FloorData.Get(x)?.forcedItems.Add(itemObject));
        private static void AddToPartyEvent(ItemObject itemObject, int weight)
        {
            foreach (FloorData floorData in FloorData.All)
            {
                floorData.partyEventItems.Add(new WeightedItemObject() { selection = itemObject, weight = weight });
            }
        }
        private static void AddToFloorsAndShop(ItemObject itemObject, int F1, int F2, int F3, int F4, int F5, int END)
        {
            AddToFloors(itemObject, F1, F2, F3, F4, F5, END);
            AddToShop(itemObject, F1, F2, F3, F4, F5, END);
        }
        private static void AddToFloorsAndShop(ItemObject itemObject, int F1, int F2, int F3, int END) => AddToFloorsAndShop(itemObject, F1, F2, F3, F2, F3, END);

        private static void AddToShop(ItemObject itemObject, int F1, int F2, int F3, int F4, int F5, int END)
        {
            if (F1 > 0)
                FloorData.Get("F1").shopItems.Add(new WeightedItemObject() { selection = itemObject, weight = F1 });
            if (F2 > 0)
                FloorData.Get("F2").shopItems.Add(new WeightedItemObject() { selection = itemObject, weight = F2 });
            if (F3 > 0)
                FloorData.Get("F3").shopItems.Add(new WeightedItemObject() { selection = itemObject, weight = F3 });
            if (F4 > 0)
                FloorData.Get("F2").shopItems.Add(new WeightedItemObject() { selection = itemObject, weight = F4 });
            if (F5 > 0)
                FloorData.Get("F3").shopItems.Add(new WeightedItemObject() { selection = itemObject, weight = F5 });
            if (END > 0)
                FloorData.Get("END").shopItems.Add(new WeightedItemObject() { selection = itemObject, weight = END });
        }
        private static void AddToShop(ItemObject itemObject, int F1, int F2, int F3, int END) => AddToShop(itemObject, F1, F2, F3, F2, F3, END);
        private static void AddToFloors(ItemObject itemObject, int F1, int F2, int F3, int F4, int F5, int END)
        {
            if (F1 > 0)
                FloorData.Get("F1").potentialItems.Add(new WeightedItemObject() { selection = itemObject, weight = F1 });
            if (F2 > 0)
                FloorData.Get("F2").potentialItems.Add(new WeightedItemObject() { selection = itemObject, weight = F2 });
            if (F3 > 0)
                FloorData.Get("F3").potentialItems.Add(new WeightedItemObject() { selection = itemObject, weight = F3 });
            if (F4 > 0)
                FloorData.Get("F4").potentialItems.Add(new WeightedItemObject() { selection = itemObject, weight = F4 });
            if (F5 > 0)
                FloorData.Get("F5").potentialItems.Add(new WeightedItemObject() { selection = itemObject, weight = F5 });
            if (END > 0)
                FloorData.Get("END").potentialItems.Add(new WeightedItemObject() { selection = itemObject, weight = END });
        }
        private static void AddToFloors(ItemObject itemObject, int F1, int F2, int F3, int END) => AddToFloors(itemObject, F1, F2, F3, F2, F3, END);
        private static void AddToMysteryRoom(ItemObject itemObject, int weight)
        {
            AssetsHelper.FindAllOfType<MysteryRoom>().Do(x => x.items = x.items.AddToArray(new WeightedItemObject() { selection = itemObject, weight = weight }));
        }

        public static void CreateItems()
        {
            ItemObject item = new ItemBuilder(BasePlugin.Instance.Info)
                .SetItemComponent<ITM_Calculator>()
                .SetNameAndDescription("BBE_Item_Calculator", "BBE_Item_Calculator_Desc")
                .SetEnum(ModdedItems.Calculator)
                .SetSmallSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_CalculatorSmall.png").ToSprite(25f))
                .SetLargeSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_CalculatorLarge.png").ToSprite(50f))
                .SetGeneratorCost(60)
                .SetShopPrice(350)
                .BuildAndSetup();
            AddToFloors(item, 0, 60, 60, 60);
            AddToShop(item, 90, 100, 0, 60);


            item = new ItemBuilder(BasePlugin.Instance.Info)
                .SetItemComponent<ITM_GravityDevice>()
                .SetNameAndDescription("BBE_Item_GravityDevice", "BBE_Item_GravityDevice_Desc")
                .SetEnum(ModdedItems.GravityDevice)
                .SetSmallSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_GravityDeviceSmall.png").ToSprite(25f))
                .SetLargeSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_GravityDeviceLarge.png").ToSprite(50f))
                .SetGeneratorCost(60)
                .SetShopPrice(350)
                .SetMeta(ItemFlags.Persists, "technology")
                .BuildAndSetup();
            AddToFloorsAndShop(item, 100, 100, 100, 100);
            ((ITM_GravityDevice)item.item).gaugeIcon = item.itemSpriteSmall;


            item = new ItemBuilder(BasePlugin.Instance.Info)
                .SetItemComponent<ITM_SpeedPotion>()
                .SetNameAndDescription("BBE_Item_SpeedSerum", "BBE_Item_SpeedSerum_Desc")
                .SetEnum(ModdedItems.PotionOfSpeed)
                .SetSmallSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_PotionOfSpeedSmall.png").ToSprite(25f))
                .SetLargeSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_PotionOfSpeedLarge.png").ToSprite(50f))
                .SetGeneratorCost(100)
                .SetShopPrice(600)
                .SetMeta(ItemFlags.Persists, "drink", "food")
                .BuildAndSetup();
            item.GetMeta().tags.Add("BBE_StockfishReward2");
            AddToPartyEvent(item, 50);
            AddToFloorsAndShop(item, 80, 80, 80, 80);
            ((ITM_SpeedPotion)item.item).gaugeIcon = item.itemSpriteSmall;


            item = new ItemBuilder(BasePlugin.Instance.Info)
                .SetItemComponent<ITM_Shield>()
                .SetNameAndDescription("BBE_Item_Shield", "BBE_Item_Shield_Desc")
                .SetEnum(ModdedItems.Shield)
                .SetSmallSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_ShieldSmall.png").ToSprite(25f))
                .SetLargeSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_ShieldLarge.png").ToSprite(50f))
                .SetGeneratorCost(260)
                .SetShopPrice(750)
                .SetMeta(ItemFlags.Persists)
                .BuildAndSetup();
            AddToShop(item, 60, 60, 60, 60);
            ((ITM_Shield)item.item).gaugeIcon = item.itemSpriteSmall;


            //! Glue
            item = new ItemBuilder(BasePlugin.Instance.Info)
                .SetItemComponent<ITM_Glue>()
                .SetNameAndDescription("BBE_Item_Glue", "BBE_Item_Glue_Desc")
                .SetEnum(ModdedItems.Glue)
                .SetSmallSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_GlueSmall.png").ToSprite(25f))
                .SetLargeSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_GlueLarge.png").ToSprite(50f))
                .SetGeneratorCost(80)
                .SetShopPrice(350)
                .SetMeta(ItemFlags.Persists)
                .BuildAndSetup();
            AddToShop(item, 70, 80, 65, 55);
            AddToFloors(item, 75, 85, 70, 60);


            item = new ItemBuilder(BasePlugin.Instance.Info)
                .SetItemComponent<ITM_IceBomb>()
                .SetNameAndDescription("BBE_Item_IceBomb", "BBE_Item_IceBomb_Desc")
                .SetEnum(ModdedItems.IceBomb)
                .SetSmallSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_IceBombSmall.png").ToSprite(25f))
                .SetLargeSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_IceBombLarge.png").ToSprite(50f))
                .SetGeneratorCost(260)
                .SetShopPrice(500)
                .BuildAndSetup();
            AddToFloors(item, 30, 35, 30, 35);
            AddToShop(item, 40, 45, 40, 45);


            item = new ItemBuilder(BasePlugin.Instance.Info)
                .SetItemComponent<ITM_MagicRuby>()
                .SetNameAndDescription("BBE_Item_MagicRuby", "BBE_Item_MagicRuby_Desc")
                .SetEnum(ModdedItems.MagicRuby)
                .SetSmallSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_MagicRubySmall.png").ToSprite(25f))
                .SetLargeSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_MagicRubyLarge.png").ToSprite(50f))
                .SetGeneratorCost(160)
                .SetShopPrice(750)
                .BuildAndSetup();
            AddToFloors(item, 30, 35, 30, 35);
            AddToShop(item, 40, 45, 40, 45);

            
            item = new ItemBuilder(BasePlugin.Instance.Info)
                .SetItemComponent<ITM_StrawberryZestyBar>()
                .SetNameAndDescription("BBE_Item_StrawberryZestyBar", "BBE_Item_StrawberryZestyBar_Desc")
                .SetEnum(ModdedItems.StrawberryZestyBar)
                .SetSmallSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_StrawberryZestyBarSmall.png").ToSprite(25f))
                .SetLargeSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_StrawberryZestyBarLarge.png").ToSprite(50f))
                .SetGeneratorCost(100)
                .SetShopPrice(350)
                .SetMeta(ItemFlags.Persists, "food", "BBE_StockfishReward1")
                .BuildAndSetup();
            AddToFloors(item, 20, 50, 55, 50);
            AddToShop(item, 50, 50, 50, 60);


            ItemObject bsoda = ItemMetaStorage.Instance.FindByEnum(Items.Bsoda).value;
            ItemObject dsoda = bsoda.Copy("BBE_Item_DSODA");
            dsoda.item.gameObject.AddComponent<ITM_DSODA>();
            dsoda.descKey = "BBE_Item_DSODA_Desc";
            dsoda.price = int.MaxValue;
            dsoda.itemSpriteSmall = AssetsHelper.CreateTexture("Textures", "Items", "BBE_DSodaSmall.png").ToSprite(25f);
            dsoda.itemSpriteLarge = AssetsHelper.CreateTexture("Textures", "Items", "BBE_DSodaLarge.png").ToSprite(50f);
            dsoda.itemType = ModdedItems.DSODA.ToItemsEnum();
            ItemMetaData data = dsoda.AddMeta(BasePlugin.Instance, ItemFlags.CreatesEntity | ItemFlags.Persists);
            data.tags.AddRange(ItemMetaStorage.Instance.FindByEnum(Items.Bsoda).tags);
            data.tags.Add("BBE_IgnoreRandomItemsFunSetting");
            data.tags.Remove("adv_perfect");


            item = new ItemBuilder(BasePlugin.Instance.Info)
                .SetItemComponent<ITM_PaintBucket>()
                .SetNameAndDescription("BBE_Item_PaintBucket", "BBE_Item_PaintBucket_Desc")
                .SetEnum(ModdedItems.PaintBucket)
                .SetSmallSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_PaintBucketSmall.png").ToSprite())
                .SetLargeSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_PaintBucketLarge.png").ToSprite(40f))
                .SetGeneratorCost(80)
                .SetShopPrice(700)
                .SetMeta(ItemFlags.Persists)
                .BuildAndSetup();
            AddToShop(item, 30, 30, 30, 30);

            item = new ItemBuilder(BasePlugin.Instance.Info)
                .SetItemComponent<ITM_TimeRewindElectronicWristwatch>()
                .SetNameAndDescription("BBE_Item_TimeRewinderElectronicWristwatch", "BBE_Item_TimeRewinderElectronicWristwatch_Desc")
                .SetEnum(ModdedItems.TimeRewinderElectronicWristwatch)
                .SetSmallSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_TimeRewindWristwatchSmall.png").ToSprite())
                .SetLargeSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_TimeRewindWristwatchLarge.png").ToSprite(40f))
                .SetGeneratorCost(80)
                .SetShopPrice(800)
                .SetMeta(ItemFlags.Persists)
                .BuildAndSetup();
            AddToShop(item, 50, 45, 40, 40);
            AddToFloors(item, 55, 50, 50, 60);
            AddToMysteryRoom(item, 30);
            ((ITM_TimeRewindElectronicWristwatch)item.item).gaugeIcon = item.itemSpriteSmall;


            new ItemBuilder(BasePlugin.Instance.Info)
                .SetItemComponent<ITM_Weight>()
                .SetNameAndDescription("BBE_Item_Weight", "BBE_Item_Weight_Desc")
                .SetEnum(ModdedItems.Weight)
                .SetSmallSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_Weight.png").ToSprite())
                .SetLargeSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_Weight.png").ToSprite(40f))
                .SetGeneratorCost(0)
                .SetShopPrice()
                .BuildAndSetup();
            
            item = new ItemBuilder(BasePlugin.Instance.Info)
                .SetItemComponent<ITM_XRayGoggles>()
                .SetNameAndDescription("BBE_Item_XrayGoggles", "BBE_Item_XrayGoggles_Desc")
                .SetEnum(ModdedItems.XrayGoggles)
                .SetSmallSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_XrayGogglesSmall.png").ToSprite(25f))
                .SetLargeSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_XrayGogglesLarge.png").ToSprite(50f))
                .SetGeneratorCost(50)
                .SetShopPrice(500)
                .SetMeta(ItemFlags.Persists)
                .BuildAndSetup();/*
            AddToShop(item, 10, 15, 20, 30);
            AddToFloors(item, 15, 20, 20, 30);*/
            ((ITM_XRayGoggles)item.item).gaugeIcon = item.itemSpriteSmall;


            new ItemBuilder(BasePlugin.Instance.Info)
                .SetItemComponent<ITM_RubyClock>()
                .SetNameAndDescription("BBE_Item_RubyClock", "BBE_Item_RubyClock_Desc")
                .SetEnum(ModdedItems.RubyClock)
                .SetSmallSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_RubyClock.png").ToSprite())
                .SetLargeSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_RubyClock.png").ToSprite(40f))
                .SetGeneratorCost(50)
                .SetShopPrice(500)
                .SetMeta(ItemFlags.CreatesEntity | ItemFlags.Persists, "technology", "makes_noise")
                .BuildAndSetup();/*
            AddToShop(item, 10, 15, 20, 30);
            AddToFloors(item, 15, 20, 20, 30);*/

            item = new ItemBuilder(BasePlugin.Instance.Info)
                .SetItemComponent<ITM_NoSign>()
                .SetNameAndDescription("BBE_Item_NoSign", "BBE_Item_NoSign_Desc")
                .SetEnum(ModdedItems.NoSign)
                .SetSmallSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_NoSignSmall.png").ToSprite())
                .SetLargeSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_NoSignLarge.png").ToSprite(50f))
                .SetGeneratorCost(30)
                .SetShopPrice()
                .SetMeta(ItemFlags.Persists)
                .BuildAndSetup();/*
            AddToShop(item, 20, 20, 40, 50);
            AddToFloors(item, 10, 25, 40, 50);*/


            item = new ItemBuilder(BasePlugin.Instance.Info)
                .SetItemComponent<ITM_RoomTeleporter>()
                .SetNameAndDescription("BBE_Item_RoomTeleporter", "BBE_Item_RoomTeleporter_Desc")
                .SetEnum(ModdedItems.RoomTeleporter)
                .SetSmallSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_RoomTeleporterSmall.png").ToSprite())
                .SetLargeSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_RoomTeleporterLarge.png").ToSprite(55f))
                .SetGeneratorCost(300)
                .SetShopPrice(300)
                .BuildAndSetup();
            item.GetMeta().tags.Add("technology");
            AddToFloorsAndShop(item, 40, 45, 50, 70);
            AddToMysteryRoom(item, 30);

            item = new ItemBuilder(BasePlugin.Instance.Info)
                .SetItemComponent<ITM_FlipperZero>()
                .SetNameAndDescription("BBE_Item_FlipperZero", "BBE_Item_FlipperZero_Desc")
                .SetEnum(ModdedItems.FlipperZero)
                .SetSmallSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_FlipperZeroSmall.png").ToSprite())
                .SetLargeSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_FlipperZeroLarge.png").ToSprite(80f))
                .SetGeneratorCost(500)
                .SetShopPrice(1000)
                .SetMeta(ItemFlags.MultipleUse, "technology")
                .BuildAndSetup();
            AddToFloorsAndShop(item, 20, 25, 30, 50);


            new ItemBuilder(BasePlugin.Instance.Info)
                .SetItemComponent<ITM_DashPad>()
                .SetNameAndDescription("BBE_Item_BaldiDashPad", "BBE_Item_BaldiDashPad_Desc")
                .SetEnum(ModdedItems.BaldiDashPad)
                .SetSmallSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_DashPadSmall.png").ToSprite())
                .SetLargeSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_DashPadLarge.png").ToSprite(40f))
                .SetGeneratorCost(350)
                .SetShopPrice(700)
                .SetMeta(ItemFlags.Persists, "technology")
                .BuildAndSetup();/*
            AddToFloors(item, 5, 5, 5, 5);*/


            new ItemBuilder(BasePlugin.Instance.Info)
                .SetItemComponent<Item>()
                .SetNameAndDescription("BBE_Item_ChessBook", "BBE_Item_ChessBook_Desc")
                .SetEnum(ModdedItems.ChessBook)
                .SetSmallSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_ChessBookSmall.png").ToSprite())
                .SetLargeSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_ChessBookLarge.png").ToSprite(40f))
                .SetGeneratorCost(int.MaxValue)
                .SetShopPrice(100)
                .SetMeta(ItemFlags.NoUses)
                .BuildAndSetup();

            item = new ItemBuilder(BasePlugin.Instance.Info)
                .SetItemComponent<ITM_MagicSwapCard>()
                .SetNameAndDescription("BBE_Item_MagicSwapCard", "BBE_Item_MagicSwapCard_Desc")
                .SetEnum(ModdedItems.UnoReverseCard)
                .SetSmallSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_MagicCardSmall.png").ToSprite())
                .SetLargeSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_MagicCardLarge.png").ToSprite(40f))
                .SetGeneratorCost(300)
                .SetShopPrice(500)
                .SetMeta(ItemFlags.CreatesEntity)
                .BuildAndSetup();
            AddToFloorsAndShop(item, 50, 55, 60, 80);



            item = new ItemBuilder(BasePlugin.Instance.Info)
                .SetItemComponent<ITM_BaldiTeleporter>()
                .SetNameAndDescription("BBE_Item_BaldiTeleporter", "BBE_Item_BaldiTeleporter_Desc")
                .SetEnum(ModdedItems.BaldiTeleporter)
                .SetSmallSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_BaldiTeleporterSmall.png").ToSprite())
                .SetLargeSprite(AssetsHelper.CreateTexture("Textures", "Items", "BBE_BaldiTeleporterLarge.png").ToSprite(40f))
                .SetGeneratorCost(300)
                .SetShopPrice(500)
                .SetMeta(ItemFlags.CreatesEntity)
                .BuildAndSetup();
            AddToFloorsAndShop(item, 40, 45, 50, 70);
        }
    }
}
