namespace Elsa.Models
{
    public class Output : Argument
    {
        public Output(RegisterLocationReference locationReference) : base(locationReference)
        {
        }
    }

    public class Output<T> : Output
    {
        public Output(RegisterLocationReference locationReference) : base(locationReference)
        {
        }
    }
}