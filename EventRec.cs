﻿
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

    public static bool operator ==(EventRec firstEvent, EventRec secondEvent)
    {
        
        if(firstEvent.Category == secondEvent.Category
            && firstEvent.Date == secondEvent.Date
            && firstEvent.Description == secondEvent.Description)
        {
            return true;
        }

        return false;
        
    }
    public static bool operator !=(EventRec firstEvent, EventRec secondEvent)
    {

        if (firstEvent.Category != secondEvent.Category
            || firstEvent.Date != secondEvent.Date
            || firstEvent.Description != secondEvent.Description)
        {
            return true;
        }

        return false;
    }

    public override bool Equals(Object? obj)
    {
        if (obj == null)
            return false;

        return this == (EventRec)obj;
    }

    public override int GetHashCode()
    {
        return 1;
    }
}
