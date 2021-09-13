namespace Elsa.Models
{
    public abstract class Argument
    {
        protected Argument(RegisterLocationReference locationReference) => LocationReference = locationReference;
        public RegisterLocationReference LocationReference { get; set; }
    }
}