
using ChoETL;
using System.ComponentModel.DataAnnotations;

namespace Days;
[ChoCSVFileHeader]
[ChoCSVRecordObject(ObjectValidationMode = ChoObjectValidationMode.ObjectLevel)]
internal class EventRec // Event object that will be saved to csv
{
    [ChoCSVRecordField(1)]
    [Required]
    public string Date { get; set; } = "";

    [ChoCSVRecordField(2)]
    public string? Category
    {
        get;
        set;
    }
    [ChoCSVRecordField(3)]
    [Required]
    public string Description{ get; set; } = "";

    public override string ToString()
    {
        return String.Format("{0, -15}{1, -15}{2, -15}",Date, Category, Description);
    }
}
