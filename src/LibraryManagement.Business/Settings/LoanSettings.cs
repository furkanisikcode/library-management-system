namespace LibraryManagement.Business.Settings;

public class LoanSettings
{
    public int MaxActiveLoanPerMember { get; set; }
    public int LoanDurationInDays { get; set; }
    public decimal DailyPenaltyAmount { get; set; }
}