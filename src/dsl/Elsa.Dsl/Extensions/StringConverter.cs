using System;
using System.ComponentModel;
using Elsa.Dsl.Exceptions;

namespace Elsa.Dsl.Extensions
{
    public static class StringConverter
    {
        public static object? ConvertFrom(string? source, Type targetType)
        {
            if (source == null)
                return default!;
            
            var underlyingTargetType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            if (underlyingTargetType.IsInstanceOfType(typeof(string)))
                return source;
            
            if (targetType == typeof(object))
                return source;
            
            var targetTypeConverter = TypeDescriptor.GetConverter(underlyingTargetType);

            if (targetTypeConverter.CanConvertFrom(typeof(string)))
                return targetTypeConverter.ConvertFrom(source);
            
            try
            {
                return Convert.ChangeType(source, underlyingTargetType);
            }
            catch (InvalidCastException e)
            {
                throw new TypeConversionException($"Failed to convert source {source} to {underlyingTargetType}", source, underlyingTargetType, e);
            }
        }
    }
}