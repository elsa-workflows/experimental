namespace Elsa.Models
{
    public class ExpressionExecutionContext
    {
        public Register Register { get; }

        public ExpressionExecutionContext(Register register)
        {
            Register = register;
        }
        
        public RegisterLocation GetLocation(RegisterLocationReference locationReference) => locationReference.GetLocation(Register);
        public object Get(RegisterLocationReference locationReference) => GetLocation(locationReference).Value!;
        public T Get<T>(RegisterLocationReference locationReference) => (T)Get(locationReference);
        public T? Get<T>(Input<T> input) => (T?)input.LocationReference.GetLocation(Register).Value;

        public void Set(RegisterLocationReference locationReference, object? value)
        {
            var location = locationReference.GetLocation(Register);
            location.Value = value;
        }

        public void Set(Output? output, object? value)
        {
            if (output?.LocationReference == null)
                return;

            var convertedValue = output.ValueConverter?.Invoke(value) ?? value;
            Set(output.LocationReference, convertedValue);
        }
    }
}