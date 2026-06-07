namespace HandballCompetitionManager.ViewModels;

public class DateTimePickerViewModel
{
    public string Name { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string? Value { get; set; }
    public string? Placeholder { get; set; }
    public string? DescribedBy { get; set; }
    public bool IncludeTime { get; set; }
    public bool Required { get; set; }
    public string? ValidationField { get; set; }
}
