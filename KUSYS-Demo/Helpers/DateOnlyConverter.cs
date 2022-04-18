using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KUSYS_Demo.Helpers
{
    public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
    {
        /// <summary>
        /// Extension for Dateonly field.
        /// </summary>
        public DateOnlyConverter() : base(
                d => d.ToDateTime(TimeOnly.MinValue),
                d => DateOnly.FromDateTime(d))
        { }
    }
}
