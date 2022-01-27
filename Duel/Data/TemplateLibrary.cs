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
        }

        private ITemplate GetTemplate(string templateName)
        {
            if (this.lookupTable.ContainsKey(templateName))
            {
                return this.lookupTable[templateName];
            }

            throw new Exception($"Template not found: {templateName}");
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

            throw new Exception($"Template is not EntityTemplate: {templateName}");
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
            templateLibrary.AddTemplate("large_cactus", new PropTemplate("props_large_cactus"));
            templateLibrary.AddTemplate("house", new PropTemplate("props_house"));
            templateLibrary.AddTemplate("large_rock", new PropTemplate("props_large_rock"));
            templateLibrary.AddTemplate("sloon", new PropTemplate("props_sloon"));
            templateLibrary.AddTemplate("small_cactus", new PropTemplate("props_small_cactus"));
            templateLibrary.AddTemplate("small_rock", new PropTemplate("props_small_rock"));
            templateLibrary.AddTemplate("thistown_sign", new PropTemplate("props_thistown_sign"));

            // Tiles
            templateLibrary.AddTemplate("wall", new TileTemplate(new Solid(), new TileImageTag(TileImageTag.TileImage.Wall), new BlockProjectileTag()));
            templateLibrary.AddTemplate("water", new TileTemplate(new TileImageTag(TileImageTag.TileImage.Water), new UnfilledWater()));
            templateLibrary.AddTemplate("bridge", new TileTemplate(new TileImageTag(TileImageTag.TileImage.Bridge), new Collapses(templateLibrary.GetTileTemplate("water"))));
            templateLibrary.AddTemplate("ravine", new TileTemplate(new TileImageTag(TileImageTag.TileImage.Ravine), new Ravine()));
            templateLibrary.AddTemplate("bramble", new TileTemplate(new TileImageTag(TileImageTag.TileImage.Bramble), new Solid()));
            templateLibrary.AddTemplate("hook", new TileTemplate(new Grapplable(Grapplable.Type.Static), new TileImageTag(TileImageTag.TileImage.Hook)));

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
                    new Solid().PushOnBump(),
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