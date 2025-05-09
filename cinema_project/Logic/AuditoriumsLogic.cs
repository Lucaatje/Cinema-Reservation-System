public static class AuditoriumsLogic
{
    public static void ShowAllAuditoriums()
    {
        CinemaHalls cinemaHalls = AuditoriumsDataAccess.GetAllAuditoriums();
        AuditoriumsPresentation.DisplayAuditoriums(cinemaHalls);
    }
}