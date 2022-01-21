namespace Duel.Data
{
    public class BusyFunction
    {
        private readonly FuncBool function;
        public string Name { get; }

        public BusyFunction(string name, FuncBool function)
        {
            this.Name = name;
            this.function = function;
        }

        public bool IsBusy()
        {
            return this.function();
        }

        public override string ToString()
        {
            var status = this.function() ? "Busy" : "Done";
            return $"{this.Name} [{status}]";
        }
    }
}
