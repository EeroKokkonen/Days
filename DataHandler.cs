using ChoETL;
using System.Diagnostics;

namespace Days;

internal class DataHandler
{
    public DataHandler()
    {
        ChoETLFrxBootstrap.TraceLevel = TraceLevel.Error; 
    }
    public static void CreateCsvFile()
    {
        List<EventRec> events = new();
        EventRec rec = new()
        {
            Date = "Date",
            Description = "Description",
            Category = "Category"
        };

        using var parser = new ChoCSVWriter(path);
        
        parser.Write(rec);
    }
    public static void ExtractByDate(ref List<EventRec> events, DateTime date)
    {
        List <EventRec> events2 = new();
        try
        {
            foreach (EventRec e in events)
            {
                if (DateTime.Parse(e.Date) == date)
                    events2.Add(e);
            }
            events = events2;
        } catch(Exception ex)
        { 
            Console.WriteLine(ex);
        }
        
    }
    public static void ExtractByCategories(ref List<EventRec> events, string?[] categories, bool exclude = false)
    {
        
        List <EventRec> events2 = new();
        if (exclude)
        {
            foreach (EventRec e in events)
            {
                if (!categories.Contains(events[0].Category))
                    events2.Add(e);
            }
            events = events2;
            return;
        }

        foreach (EventRec e in events)
        {
            if (categories.Contains(e.Category))
                events2.Add(e);
        }
        events = events2;
    }
    public static void ExtractByDates(ref List<EventRec> events, DateTime afterDate, DateTime beforeDate)
    {
        List<EventRec> events2 = new();
        foreach (EventRec e in events)
        {
            if(afterDate <= DateTime.Parse(e.Date) || beforeDate > DateTime.Parse(e.Date))
                events2.Add(e);
        }
        events = events2;
    }

    public static void ExtractByDescription(ref List<EventRec> events, string description)
    {
        List<EventRec> cpEvents = new();
        foreach (EventRec e in events)
        {
            if (e.Description != description)
                cpEvents.Add(e);
        }
        events = cpEvents;
    }
    public static void ExtractByPartialDescription(ref List<EventRec> events, string partDes)
    {
        List<EventRec> cpEvents = new();
        foreach (EventRec e in events)
        {
            if (e.Description.StartsWith(partDes))
                cpEvents.Add(e);
        }
        events = cpEvents;
    }
    public static List<EventRec> GetEvents()
    {
        List<EventRec> events = new();

        foreach (var rec in new ChoCSVReader<EventRec>(path).WithFirstLineHeader())
        {
            events.Add(rec);
        }
 
        return events;
    }

    public static bool AddEvent(string _date, string _category, string _description)
    {
        try
        {
            List<EventRec> events;
            events = GetEvents();

            // To test if date is in correct format
            DateTime.Parse(_date);

            var rec = new EventRec
            {
                Date = _date,
                Category = _category,
                Description = _description
            };

            events.Add(rec);

            using var parser = new ChoCSVWriter(path).WithFirstLineHeader();

            parser.Write(events);

            return true;
        } catch(Exception e)
        {
            System.Console.WriteLine(e);
            return false;
        }
    }
    public static bool AddEvents(List<EventRec> events)
    {
        try
        {
            using var parser = new ChoCSVWriter(path).WithFirstLineHeader();

            parser.Write(events);

            return true;
        }
        catch (Exception e)
        {
            System.Console.WriteLine(e);
            return false;
        }
    }
    public static bool DeleteEvents(List<EventRec> events)
    {
        try
        {
            List<EventRec> savedEvents = GetEvents();
            foreach(var rec in events)
            {
                savedEvents.Remove(rec);
            }
            using var parser = new ChoCSVWriter(path).WithFirstLineHeader();

            parser.Write(savedEvents);

            return true;
        }
        catch (Exception e)
        {
            System.Console.WriteLine(e);
            return false;
        }
    }
    private static readonly string path = "./days.csv";

}
