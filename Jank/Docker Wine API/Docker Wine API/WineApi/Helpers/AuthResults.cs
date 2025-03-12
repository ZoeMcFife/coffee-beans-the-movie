using Microsoft.AspNetCore.Mvc;

namespace WineApi.Helpers
{
    public class AuthResults
    {
        public bool IsAuthenticated { get; set; }
        public Guid? UserId { get; set; }
        public ActionResult? ErrorResult {  get; set; }

        public AuthResults(bool isAuthenticated, Guid? userId, ActionResult? errorResult)
        {
            IsAuthenticated = isAuthenticated;
            UserId = userId;
            ErrorResult = errorResult;
        }
    }
}
