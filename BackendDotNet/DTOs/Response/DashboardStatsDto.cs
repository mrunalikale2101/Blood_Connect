namespace BackendDotNet.DTOs.Response
{
    public class DashboardStatsDto
    {
        public int TotalDonors { get; set; }
        public int TotalHospitals { get; set; }
        public int PendingRequests { get; set; }
        public int TotalBloodUnits { get; set; }
    }
}
