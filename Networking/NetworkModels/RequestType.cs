namespace Networking.NetworkModels
{
    public enum RequestType
    {
        Login = 100,
        Logout = 101,
        DataChanged = 500,
        SearchContestTasks = 501,
        AddParticipant = 502,
        AddEntry,
        SearchParticipants,
        SearchParticipantsJoin
    }
}