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

        public static Speaker SheriffNormal = new Speaker(
            "Ernesto", 0
        );

        public static Speaker SheriffSpooked = new Speaker(
            "Ernesto", 1
        );

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