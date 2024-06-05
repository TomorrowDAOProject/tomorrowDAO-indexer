namespace TomorrowDAO.Indexer.Plugin;

public class TomorrowDAOConst
{
    public const string Asc = "asc";
    public const string Ascend = "ascend";

    public const string VotesAmount = "votesamount";
    public const string StakeAmount = "stakeamount";
    
    public const string VoteTime = "votetime";
    public const string Amount = "amount";
    
    public const int MinActiveTimePeriod = 7; // days
    public const int MaxActiveTimePeriod = 15; // days
    public const int MinPendingTimePeriod = 5; // days
    public const int MaxPendingTimePeriod = 7; // days
    public const int MinExecuteTimePeriod = 3; // days
    public const int MaxExecuteTimePeriod = 5; // days
    public const int MinVetoActiveTimePeriod = 3; // days
    public const int MaxVetoActiveTimePeriod = 5; // days
    public const int MinVetoExecuteTimePeriod = 1; // days
    public const int MaxVetoExecuteTimePeriod = 3; // days
}