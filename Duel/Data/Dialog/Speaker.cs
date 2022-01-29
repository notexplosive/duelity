namespace Duel.Data.Dialog {
    public class Speaker
    {

        // static instances

        public static Speaker getSpeaker(string name) {
            switch(name) {
                case "SheriffNormal": return SheriffNormal;
                case "SheriffSpooked": return SheriffSpooked;
                default: return null;
            }
        }

        public static Speaker SheriffNormal = new Speaker("Ernesto", 0);
        public static Speaker SheriffSpooked = new Speaker("Ernesto", 1);
        public static Speaker SheriffDeepBow = new Speaker("Ernesto", 2);
        public static Speaker RenegadeLaugh = new Speaker("Miranda", 6);
        public static Speaker RenegadeGun = new Speaker("Miranda", 7);
        public static Speaker RenegadeSmirk = new Speaker("Miranda", 8);
        public static Speaker CowboyNormal = new Speaker("Steven", 12);
        public static Speaker CowboySkeptical = new Speaker("Steven", 13);
        public static Speaker KnightGrin = new Speaker("Bennigan", 18);
        public static Speaker KnightRascal = new Speaker("Bennigan", 19);

        public static Speaker Sar = new Speaker("Sar Saparilla", 24);
        public static Speaker Mom = new Speaker("Mom", 25);
        public static Speaker Farmer = new Speaker("Ernest", 26);
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
        

        public Speaker(string name, int portraitIndex)
        {
            this.Name = name;
            this.PortraitIndex = portraitIndex;
        }
    }
}