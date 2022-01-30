namespace Duel.Data.Dialog
{
    public class Speaker
    {

        // static instances

        public static Speaker getSpeaker(string name)
        {
            switch (name)
            {
                case "SheriffNormal": return SheriffNormal;
                case "SheriffSpooked": return SheriffSpooked;
                case "SheriffDeepBow": return SheriffDeepBow;
                case "RenegadeLaugh": return RenegadeLaugh;
                case "RenegadeGun": return RenegadeGun;
                case "RenegadeSmirk": return RenegadeSmirk;
                case "RenegadeEw": return RenegadeEw;
                case "CowboyNormal": return CowboyNormal;
                case "CowboySkeptical": return CowboySkeptical;
                case "CowboyHope": return CowboyHope;
                case "CowboyDetermined": return CowboyDetermined;
                case "KnightGrin": return KnightGrin;
                case "KnightRascal": return KnightRascal;
                case "KnightSalute": return KnightSalute;
                case "KnightGasp": return KnightGasp;
                case "Sar": return Sar;
                case "Mom": return Mom;
                case "Farmer": return Farmer;
                case "King": return King;
                case "Sneakman": return Sneakman;
                case "Egg": return Egg;
                case "CoolHairguy": return CoolHairguy;
                case "Jackalope": return Jackalope;
                case "Hardy": return Hardy;
                case "Moleman": return Moleman;
                case "Mime": return Mime;
                case "Fourman": return Fourman;
                default: return null;
            }
        }

        public static Speaker SheriffNormal = new Speaker("Ernesto", 0, new DialogBlip("banjo_blip", 0.5f, 0));
        public static Speaker SheriffSpooked = new Speaker("Ernesto", 1, new DialogBlip("banjo_blip", 0.75f, 0.5f));
        public static Speaker SheriffDeepBow = new Speaker("Ernesto", 2, new DialogBlip("banjo_blip", 0.25f, -0.25f));
        public static Speaker RenegadeLaugh = new Speaker("Miranda", 6);
        public static Speaker RenegadeGun = new Speaker("Miranda", 7);
        public static Speaker RenegadeSmirk = new Speaker("Miranda", 8);
        public static Speaker RenegadeEw = new Speaker("Miranda", 8);
        public static Speaker CowboyNormal = new Speaker("Steven", 12, new DialogBlip("deep_blip", 0.5f, 0f));
        public static Speaker CowboySkeptical = new Speaker("Steven", 13, new DialogBlip("deep_blip", 0.5f, -0.25f));
        public static Speaker CowboyHope = new Speaker("Steven", 14, new DialogBlip("deep_blip", 0.5f, 0.25f));
        public static Speaker CowboyDetermined = new Speaker("Steven", 15, new DialogBlip("deep_blip", 0.75f, 0.5f));
        public static Speaker KnightGrin = new Speaker("Bennigan", 18, new DialogBlip("harpsichord_blip", 0.5f, 0f));
        public static Speaker KnightRascal = new Speaker("Bennigan", 19, new DialogBlip("harpsichord_blip", 0.5f, 0f));
        public static Speaker KnightSalute = new Speaker("Bennigan", 20, new DialogBlip("harpsichord_blip", 0.5f, 0f));
        public static Speaker KnightGasp = new Speaker("Bennigan", 21, new DialogBlip("harpsichord_blip", 0.5f, 0f));

        public static Speaker Sar = new Speaker("Sar Saparilla", 24);
        public static Speaker Mom = new Speaker("Mom", 25);
        public static Speaker Farmer = new Speaker("Abe", 26);
        public static Speaker King = new Speaker("His Majesty", 27);
        public static Speaker Sneakman = new Speaker("Sneakman", 28);
        public static Speaker Egg = new Speaker("Egg-Shaped Man", 29);
        public static Speaker CoolHairguy = new Speaker("Cool Hairguy", 30);
        public static Speaker Jackalope = new Speaker("Jackalope", 31);
        public static Speaker Hardy = new Speaker("Hardy", 32);
        public static Speaker Moleman = new Speaker("Moleman", 34);
        public static Speaker Mime = new Speaker("????", 34);
        public static Speaker Fourman = new Speaker("Gary", 35);

        // class def

        public string Name { get; }
        public int PortraitIndex { get; }
        public DialogBlip Blip { get; }

        private Speaker(string name, int portraitIndex, DialogBlip dialogBlip = null)
        {
            if (dialogBlip == null)
            {
                Blip = DialogBlip.Default;
            }
            else
            {
                Blip = dialogBlip;
            }

            this.Name = name;
            this.PortraitIndex = portraitIndex;
        }

    }
}