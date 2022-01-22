namespace Duel.Data
{
    public class BusyFunction
    {
        private readonly TrueWhenDone isDone;
        public string Name { get; }

        public BusyFunction(string name, TrueWhenDone isDone)
        {
            this.Name = name;
            this.isDone = isDone;
        }

        public bool IsBusy()
        {
            return !this.isDone();
        }

        public bool IsFree()
        {
            return !IsBusy();
        }

        public override string ToString()
        {
            var status = this.isDone() ? "Done" : "Busy";
            return $"{this.Name} [{status}]";
        }
    }
}
