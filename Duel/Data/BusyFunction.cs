namespace Duel.Data
{
    public class BusyFunction
    {
        private readonly TrueWhenDone function;
        public string Name { get; }

        public BusyFunction(string name, TrueWhenDone function)
        {
            this.Name = name;
            this.function = function;
        }

        public bool IsBusy()
        {
            return !this.function();
        }

        public override string ToString()
        {
            var status = this.function() ? "Busy" : "Done";
            return $"{this.Name} [{status}]";
        }
    }
}
