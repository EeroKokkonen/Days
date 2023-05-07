using McMaster.Extensions.CommandLineUtils;


namespace Days;
[Command("Days")]
[Subcommand(
        typeof(ListCommand),
        typeof(AddCommand),
        typeof(DeleteCommand))]
internal class Days : DaysCommandBase
{
    protected override int OnExecute(CommandLineApplication app)
    {
        app.ShowHelp();
        return 1;
    }
}
[Command(Description = "List events according user choices")]
internal class ListCommand : DaysCommandBase
{
    // Subcommands for list-command
    [Option("-t|--today", CommandOptionType.NoValue)]
    public bool Today { get; set; }
    [Option("-b|--before-date", CommandOptionType.SingleValue)]
    public (bool hasValue, string value) BeforeDate { get; set; }
    [Option("-f|--after-date", CommandOptionType.SingleValue)]
    public (bool hasValue, string value) AfterDate { get; set; }
    [Option("-d|--date", CommandOptionType.SingleValue)]
    public (bool hasValue, string value) Date { get; set; }
    [Option("-c|--categories", CommandOptionType.SingleValue)]
    public (bool hasValue, string value) Categories { get; set; }
    [Option("-e|--exclude", CommandOptionType.NoValue)]
    public bool Exclude { get; set; }
    [Option("-n|--no-category", CommandOptionType.NoValue)]
    public bool NoCategory { get; set; }


    protected override int OnExecute(CommandLineApplication app)
    {
        List<EventRec> events = new();
        
        bool exclude = Exclude;
        string?[] emptyCategories = { null };

        // Gets all saved events
        events = DataHandler.GetEvents();

        // Starts extracting non-wanted events by given arguments
        if (Today)
            DataHandler.ExtractByDate(ref events, DateTime.Now.Date);

        if(Date.hasValue)
            DataHandler.ExtractByDate(ref events, DateTime.Parse(Date.value));

        if(BeforeDate.hasValue || AfterDate.hasValue)
        {
            DateTime afterDate;
            DateTime beforeDate;
            if (AfterDate.hasValue)
                afterDate = DateTime.Parse(AfterDate.value);
            else
                afterDate = DateTime.MaxValue;

            if (BeforeDate.hasValue)
                beforeDate = DateTime.Parse(BeforeDate.value);
            else
                beforeDate = DateTime.MinValue;

            DataHandler.ExtractByDates(ref events, afterDate, beforeDate);
        }

        if (Categories.hasValue)
            DataHandler.ExtractByCategories(ref events, Categories.value.Split(","), exclude);

        if(NoCategory)
            DataHandler.ExtractByCategories(ref events, emptyCategories);

        PrintEvents(events);

        return base.OnExecute(app);
    }

}

[Command(Description = "Add event according user choices")]
internal class AddCommand : DaysCommandBase
{
    // Subcommands for add-command
    [Option("-d|--date", CommandOptionType.SingleValue)]
    public (bool hasValue, string value) Date { get; set; }

    [Option("-c|--category", CommandOptionType.SingleValue)]
    public (bool hasValue, string value) Category { get; set; }

    [Option("-e|--description", CommandOptionType.SingleValue)]
    public (bool hasValue, string value) Description { get; set; }

    protected override int OnExecute(CommandLineApplication app)
    {
        // Creates new event object
        EventRec eventRec = new();

        // Starts adding user given information for event object
        if (Date.hasValue)
            eventRec.Date = Date.value;
        else
            eventRec.Date = DateTime.Now.Date.ToString("yyyy-MM-dd");

        if (Category.hasValue)
            eventRec.Category = Category.value;
        else
            eventRec.Category = "";

        // Description is required
        if (Description.hasValue)
            eventRec.Description = Description.value;
        else
        {
            System.Console.WriteLine("Description must have value");
            return -1;
        }
            

        DataHandler.AddEvent(eventRec.Date, eventRec.Category, eventRec.Description);
        return base.OnExecute(app);
    }
}
[Command(Description = "Delete events according user choices")]
internal class DeleteCommand : DaysCommandBase
{
    [Option("-b|--before-date", CommandOptionType.SingleValue)]
    public (bool hasValue, string value) BeforeDate { get; set; }
    [Option("-f|--after-date", CommandOptionType.SingleValue)]
    public (bool hasValue, string value) AfterDate { get; set; }

    [Option("-d|--date", CommandOptionType.SingleValue)]
    public (bool hasValue, string value) Date { get; set; }

    [Option("-c|--category", CommandOptionType.SingleValue)]
    public (bool hasValue, string value) Category { get; set; }

    [Option("-e|--description", CommandOptionType.SingleValue)]
    public (bool hasValue, string value) Description { get; set; }

    [Option("-r|--dry-run", CommandOptionType.NoValue)]
    public bool DryRun { get; set; }

    [Option("-a|--all", CommandOptionType.NoValue)]
    public bool All { get; set; }

    protected override int OnExecute(CommandLineApplication app)
    {
        List<EventRec> events = new();
        events = DataHandler.GetEvents();

        if (Date.hasValue)
            DataHandler.ExtractByDate(ref events, DateTime.Parse(Date.value));

        if (BeforeDate.hasValue || AfterDate.hasValue)
        {
            DateTime afterDate;
            DateTime beforeDate;
            if (AfterDate.hasValue)
                afterDate = DateTime.Parse(AfterDate.value);
            else
                afterDate = DateTime.MaxValue;

            if (BeforeDate.hasValue)
                beforeDate = DateTime.Parse(BeforeDate.value);
            else
                beforeDate = DateTime.MinValue;

            DataHandler.ExtractByDates(ref events, afterDate, beforeDate);
        }

        if (Category.hasValue)
            DataHandler.ExtractByCategories(ref events, Category.value.Split(","));

        if (Description.hasValue)
            DataHandler.ExtractByPartialDescription(ref events, Description.value);

        if(DryRun)
        {
            System.Console.WriteLine("You are about to delete following events:\n");
            PrintEvents(events);
            return base.OnExecute(app);
        }
            
        if (All)
        {
            DataHandler.AddEvents(new List<EventRec>());
            return base.OnExecute(app);
        }

        DataHandler.DeleteEvents(events);

        return base.OnExecute(app);
    }
}
[HelpOption("--help")]
abstract class DaysCommandBase
{
    protected static void PrintEvents(List<EventRec> events)
    {
        System.Console.WriteLine(String.Format("{0, -15}{1, -15}{2, -15}", "Date", "Category", "Description"));
        foreach (var _event in events)
        {
            System.Console.WriteLine(_event);
        }
    }
    protected virtual int OnExecute(CommandLineApplication app)
    {
        return 0;
    }
}
