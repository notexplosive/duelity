using Duel.Components;
using Machina.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Data
{
    public class TemplateLibrary
    {
        private readonly Dictionary<string, ITemplate> lookupTable = new Dictionary<string, ITemplate>();

        public void AddTemplate(string templateName, ITemplate template)
        {
            this.lookupTable.Add(templateName, template);
            template.NameInLibrary = templateName;
        }

        private ITemplate GetTemplate(string templateName)
        {
            if (this.lookupTable.ContainsKey(templateName))
            {
                return this.lookupTable[templateName];
            }

            throw new Exception($"Template not found: {templateName}");
        }

        public EntityTemplate GetPlayerTemplateForMoveType(PlayerTag.Type type)
        {
            switch (type)
            {
                case PlayerTag.Type.Sheriff:
                    return GetTemplate("sheriff") as EntityTemplate;
                case PlayerTag.Type.Renegade:
                    return GetTemplate("renegade") as EntityTemplate;
                case PlayerTag.Type.Cowboy:
                    return GetTemplate("cowboy") as EntityTemplate;
                case PlayerTag.Type.Knight:
                    return GetTemplate("knight") as EntityTemplate;
            }

            throw new Exception("flrooop");
        }

        public static TemplateLibrary BuildWithPlayers()
        {
            var library = Build();

            library.AddTemplate("sheriff", new EntityTemplate(new PlayerTag(PlayerTag.Type.Sheriff)));
            library.AddTemplate("renegade", new EntityTemplate(new PlayerTag(PlayerTag.Type.Renegade)));
            library.AddTemplate("cowboy", new EntityTemplate(new PlayerTag(PlayerTag.Type.Cowboy)));
            library.AddTemplate("knight", new EntityTemplate(new PlayerTag(PlayerTag.Type.Knight)));

            return library;
        }

        public EntityTemplate GetEntityTemplate(string templateName)
        {
            var template = GetTemplate(templateName);
            if (template is EntityTemplate entityTemplate)
            {
                return entityTemplate;
            }

            throw new Exception($"Template is not EntityTemplate: {templateName}");
        }

        public TileTemplate GetTileTemplate(string templateName)
        {
            var template = GetTemplate(templateName);
            if (template is TileTemplate tileTemplate)
            {
                return tileTemplate;
            }

            throw new Exception($"Template is not TileTemplate: {templateName}");
        }

        public PropTemplate GetPropTemplate(string templateName)
        {
            var template = GetTemplate(templateName);
            if (template is PropTemplate propTemplate)
            {
                return propTemplate;
            }

            throw new Exception($"Template is not PropTemplate: {templateName}");
        }

        public static TemplateLibrary Build()
        {
            var templateLibrary = new TemplateLibrary();

            // Props
            templateLibrary.AddTemplate("large_cactus", new PropTemplate("props_large_cactus", PropTemplate.PropLayeringRule.Front));
            templateLibrary.AddTemplate("house", new PropTemplate("props_house", PropTemplate.PropLayeringRule.Front));
            templateLibrary.AddTemplate("large_rock", new PropTemplate("props_large_rock", PropTemplate.PropLayeringRule.Front));
            templateLibrary.AddTemplate("sloon", new PropTemplate("props_sloon", PropTemplate.PropLayeringRule.Front));
            templateLibrary.AddTemplate("small_cactus", new PropTemplate("props_small_cactus", PropTemplate.PropLayeringRule.Front));
            templateLibrary.AddTemplate("small_rock", new PropTemplate("props_small_rock", PropTemplate.PropLayeringRule.Front));
            templateLibrary.AddTemplate("thistown_sign", new PropTemplate("props_thistown_sign", PropTemplate.PropLayeringRule.Front));
            templateLibrary.AddTemplate("aloe", new PropTemplate("props_aloe", PropTemplate.PropLayeringRule.Front));
            templateLibrary.AddTemplate("barn", new PropTemplate("props_barn", PropTemplate.PropLayeringRule.Front));
            templateLibrary.AddTemplate("conveyor", new PropTemplate("props_conveyor", PropTemplate.PropLayeringRule.Front));
            templateLibrary.AddTemplate("crates", new PropTemplate("props_crates", PropTemplate.PropLayeringRule.Front));
            templateLibrary.AddTemplate("horse", new PropTemplate("props_horse", PropTemplate.PropLayeringRule.Front));
            templateLibrary.AddTemplate("house_2", new PropTemplate("props_house_2", PropTemplate.PropLayeringRule.Front));
            templateLibrary.AddTemplate("machinery", new PropTemplate("props_machinery", PropTemplate.PropLayeringRule.Front));
            templateLibrary.AddTemplate("palm_tree", new PropTemplate("props_palm_tree", PropTemplate.PropLayeringRule.Front));
            templateLibrary.AddTemplate("cubepig", new PropTemplate("props_cubepig", PropTemplate.PropLayeringRule.Front));

            // Props that appear below entities
            templateLibrary.AddTemplate("city_limits", new PropTemplate("props_city_limits", PropTemplate.PropLayeringRule.Behind));
            templateLibrary.AddTemplate("wave1", new PropTemplate("props_wave1", PropTemplate.PropLayeringRule.Behind));
            templateLibrary.AddTemplate("wave2", new PropTemplate("props_wave2", PropTemplate.PropLayeringRule.Behind));
            templateLibrary.AddTemplate("wave3", new PropTemplate("props_wave3", PropTemplate.PropLayeringRule.Behind));
            templateLibrary.AddTemplate("bgscuff1", new PropTemplate("props_bgscuff1", PropTemplate.PropLayeringRule.Behind));
            templateLibrary.AddTemplate("bgscuff2", new PropTemplate("props_bgscuff2", PropTemplate.PropLayeringRule.Behind));
            templateLibrary.AddTemplate("bgscuff3", new PropTemplate("props_bgscuff3", PropTemplate.PropLayeringRule.Behind));
            templateLibrary.AddTemplate("bgrocks1", new PropTemplate("props_bgrocks1", PropTemplate.PropLayeringRule.Behind));
            templateLibrary.AddTemplate("bgrocks2", new PropTemplate("props_bgrocks2", PropTemplate.PropLayeringRule.Behind));

            // Tiles
            templateLibrary.AddTemplate("empty_tile", new TileTemplate());
            templateLibrary.AddTemplate("wall", new TileTemplate(new Solid(), new TileImageTag(TileImageTag.TileImage.Wall), new BlockProjectileTag()));
            templateLibrary.AddTemplate("invisible_wall", new TileTemplate(new Solid(), new BlockProjectileTag(), new EditorImage(EntityFrame.CrateBreak)));
            templateLibrary.AddTemplate("next_level_trigger", new TileTemplate(new EditorImage(EntityFrame.BlueKeyBroken), new ZoneTransitionTrigger()));
            templateLibrary.AddTemplate("water", new TileTemplate(new TileImageTag(TileImageTag.TileImage.Water), new UnfilledWater()));
            templateLibrary.AddTemplate("bridge", new TileTemplate(new TileImageTag(TileImageTag.TileImage.Bridge), new Collapses(templateLibrary.GetTileTemplate("water"))));
            templateLibrary.AddTemplate("ravine", new TileTemplate(new TileImageTag(TileImageTag.TileImage.Ravine), new Ravine()));
            templateLibrary.AddTemplate("ravine-bridge", new TileTemplate(new TileImageTag(TileImageTag.TileImage.BridgeOverRavine), new Collapses(templateLibrary.GetTileTemplate("ravine"))));
            templateLibrary.AddTemplate("bramble", new TileTemplate(new TileImageTag(TileImageTag.TileImage.Bramble), new Solid()));
            templateLibrary.AddTemplate("hook", new TileTemplate(new Grapplable(Grapplable.Type.Static), new TileImageTag(TileImageTag.TileImage.Hook)));

            // NPCs
            templateLibrary.AddNpc(new NpcTag(NpcSprite.Sar, "sar_sheriff", "sar_renegade", "sar_cowboy", "sar_knight"));
            templateLibrary.AddNpc(new NpcTag(NpcSprite.MirandaMom, "mom_sheriff", "mom_renegade", "mom_cowboy", "mom_knight"));
            templateLibrary.AddNpc(new NpcTag(NpcSprite.Farmer, "farmer_sheriff", "farmer_renegade", "farmer_cowboy", "farmer_knight"));
            templateLibrary.AddNpc(new NpcTag(NpcSprite.Cactus, "sneakman_sheriff", "sneakman_renegade", "sneakman_cowboy", "sneakman_knight"));
            templateLibrary.AddNpc(new NpcTag(NpcSprite.Jackalope, "jackalope_sheriff", "jackalope_renegade", "jackalope_cowboy", "jackalope_knight"));
            templateLibrary.AddNpc(new NpcTag(NpcSprite.EggMan, "egg_sheriff", "egg_renegade", "egg_cowboy", "egg_knight"));
            templateLibrary.AddNpc(new NpcTag(NpcSprite.CoolHair, "hair_sheriff", "hair_renegade", "hair_cowboy", "hair_knight"));
            templateLibrary.AddNpc(new NpcTag(NpcSprite.Mime, "mime_sheriff", "mime_renegade", "mime_cowboy", "mime_knight"));

            // Entities
            templateLibrary.AddTemplate("glass", new EntityTemplate(
                new DestroyOnHit(),
                new SimpleEntityImage(EntityFrameSet.GlassBottle),
                new Solid().PushOnBump(),
                new Grapplable(Grapplable.Type.PulledByLasso),
                new WaterFiller(WaterFiller.Type.Floats)
            ));

            templateLibrary.AddTemplate("anvil", new EntityTemplate(
                new Solid().PushOnHit(),
                new BlockProjectileTag(),
                new SimpleEntityImage(EntityFrameSet.Anvil),
                new WaterFiller(WaterFiller.Type.Sinks)
            ));

            templateLibrary.AddTemplate("crate", new EntityTemplate(
                new Solid().PushOnBump(),
                new BlockProjectileTag(),
                new Grapplable(Grapplable.Type.PulledByLasso),
                new SimpleEntityImage(EntityFrameSet.Crate),
                new DestroyOnHit(),
                new WaterFiller(WaterFiller.Type.Floats)
            ));

            templateLibrary.AddTemplate("barrel", new EntityTemplate(
                new Solid().PushOnBump().PushOnHit(),
                new BlockProjectileTag(),
                new SimpleEntityImage(EntityFrameSet.Barrel),
                new WaterFiller(WaterFiller.Type.Floats)
            ));

            templateLibrary.AddTemplate("miasma", new EntityTemplate(
                new BlockProjectileTag(),
                new MiasmaImageTag(),
                new EditorImage(EntityFrame.Miasma)
            ));

            templateLibrary.AddTemplate("spawn-ernesto", new EntityTemplate(
                new PlayerSpawn(PlayerTag.Type.Sheriff)
            ));

            templateLibrary.AddTemplate("spawn-miranda", new EntityTemplate(
                new PlayerSpawn(PlayerTag.Type.Renegade)
            ));

            templateLibrary.AddTemplate("spawn-steven", new EntityTemplate(
                new PlayerSpawn(PlayerTag.Type.Cowboy)
            ));

            templateLibrary.AddTemplate("spawn-bennigan", new EntityTemplate(
                new PlayerSpawn(PlayerTag.Type.Knight)
            ));

            foreach (SignalColor color in Enum.GetValues(typeof(SignalColor)))
            {
                var colorName = "unknown_color";
                switch (color)
                {
                    case SignalColor.Red:
                        colorName = "red";
                        break;
                    case SignalColor.Blue:
                        colorName = "blue";
                        break;
                    case SignalColor.Yellow:
                        colorName = "yellow";
                        break;
                }

                // all 3 levers
                templateLibrary.AddTemplate($"{colorName}Lever", new EntityTemplate(
                    new BlockProjectileTag(),
                    new Solid(),
                    new ToggleSignal(color).OnBump().OnGrapple().OnHit(),
                    new LeverImageTag(color)
                ));

                // all 3 plates
                templateLibrary.AddTemplate($"{colorName}PressurePlate", new EntityTemplate(
                    new EnableSignalWhenSteppedOn(color),
                    new PressurePlateImageTag(color)
                ));

                // all 3 open doors
                templateLibrary.AddTemplate($"{colorName}OpenDoor", new EntityTemplate(
                    new SignalDoor(color, true)
                ));

                // all 3 closed doors
                templateLibrary.AddTemplate($"{colorName}ClosedDoor", new EntityTemplate(
                    new SignalDoor(color, false)
                ));

                // all 3 keys
                templateLibrary.AddTemplate($"{colorName}Key", new EntityTemplate(
                    new Solid().PushOnBump().PushOnHit(),
                    new BlockProjectileTag(),
                    new WaterFiller(WaterFiller.Type.Sinks),
                    new Key(color),
                    new Grapplable(Grapplable.Type.PulledByLasso)
                ));

                // all 3 keyDoors
                templateLibrary.AddTemplate($"{colorName}KeyDoor", new EntityTemplate(
                    new Solid(),
                    new BlockProjectileTag(),
                    new KeyDoor(color)
                ));
            }

            return templateLibrary;
        }

        private void AddNpc(NpcTag npcTag)
        {
            AddTemplate(npcTag.Sprite.ToString().ToLower(), new EntityTemplate(npcTag, new Solid()));
        }

        public IEnumerable<ITemplate> GetAllTemplates()
        {
            foreach (var template in this.lookupTable.Values)
            {
                if (template is TileTemplate tileTemplate)
                {
                    yield return tileTemplate;
                }
            }

            foreach (var template in this.lookupTable.Values)
            {
                if (template is EntityTemplate tileTemplate)
                {
                    yield return tileTemplate;
                }
            }

            foreach (var template in this.lookupTable.Values)
            {
                if (template is PropTemplate propTemplate)
                {
                    yield return propTemplate;
                }
            }
        }
    }
}